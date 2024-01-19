using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    enum WeaponMode
    {
        Normal,
        Sniper
    }
    WeaponMode wMode;

    public int hp = 100; // 플레이어 체력
    public int maxHP = 100; // 플레이어 최대 체력
    public int weaponPower = 5; // 발사 무기 공격력

    public float rotSpeed = 200f; // 회전 속도
    public float moveSpeed = 7f; // 이동 속도
    public float jumpPower = 10f; // 점프에 가해지는 힘
    public float throwPower = 15f; // 폭탄 던질 때 가해지는 힘

    public bool isJumping = false; // 점프 중인가?

    private float mx = 0; // 회전 값
    private float gravity = -20f; // 중력 값
    private float yVelocity = 0f; // 수직 속력 변수
    private bool zoomMode = false;

    public GameObject firePosition; // 발사 위치
    public GameObject bombFactory; // 폭탄 오브젝트
    public GameObject bulletEffect; // 총알 피격 이펙트
    public GameObject hitEffect; // 피격 효과 UI 오브젝트

    public Slider hpSlider; // hp 슬라이더 변수
    public Text wModeText; // 무기 모드 텍스트

    public ParticleSystem ps; // 피격 이펙트 파티클 시스템

    private CharacterController cc; // 캐릭터 컨트롤러
    private Animator anim; // 애니메이터 

    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        wMode = WeaponMode.Normal;
    }

    void Update()
    {
        // 게임이 Run 상태가 아니면 플레이어 조작 제한
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        doRotate(); // 마우스로 플레이어 회전
        doMove(); // WASD로 플레이어 이동
        doJump(); // Spacebar로 점프
        doFire(); // 마우스 입력으로 무기 사용
        doSetHP(); // hp UI 관리
        doSetMode(); // 모드 변경
    }

    // sniper, normal 모드 변경
    void doSetMode()
    {
        // 숫자1 입력 : normal모드
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            wMode = WeaponMode.Normal;
            Camera.main.fieldOfView = 60f;

            wModeText.text = "Normal Mode";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            wMode = WeaponMode.Sniper;

            wModeText.text = "Sniper Mode";
        }
    }

    // UI - HP 관리
    void doSetHP()
    {
        hpSlider.value = (float)hp / (float)maxHP;
    }

    // 마우스 회전에 따른 플레이어 회전
    void doRotate()
    {
        // 마우스 좌우 회전 입력 받기
        float mouseX = Input.GetAxis("Mouse X");

        // 회전 값 변수에 마우스 회전 입력 값 누적
        mx += mouseX * rotSpeed * Time.deltaTime;

        // 플레이어 회전
        transform.eulerAngles = new Vector3(0, mx, 0);
    }

    // 플레이어 이동
    void doMove()
    {
        // 키 입력 받음
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 이동 방향 설정
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized; // 정규화

        // 이동 블렌딩 트리 호출하고 벡터의 크기 값 전달
        anim.SetFloat("MoveMotion", dir.magnitude);

        // 메인 카메라를 기준으로 방향을 변환
        dir = Camera.main.transform.TransformDirection(dir);

        // 캐릭터 수직 속도에 중력 값 적용
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        // 이동
        cc.Move(dir * moveSpeed * Time.deltaTime);
    }

    // 플레이어 점프
    void doJump()
    {
        // 바닥에 착지했다면 && 점프 중이었다면
        if (cc.collisionFlags == CollisionFlags.Below)
        {
            if (isJumping)
            {
                // 점프 중이 아니다
                isJumping = false;

                // 캐릭터 수직 속도 0으로
                yVelocity = 0f;
            }
        }

        // 점프 키(spacebar) 누르면 && 점프 중이 아니라면
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            // 캐릭터 수직 속도에 점프력 적용
            yVelocity = jumpPower;

            // 변수를 점프 상태로 변경
            isJumping = true;
        }
    }

    // 무기 사용
    void doFire()
    {
        // 마우스 오른쪽 클릭
        if (Input.GetMouseButtonDown(1))
        {
            switch(wMode)
            {
                case WeaponMode.Normal:
                    // 폭탄 오브젝트 생성
                    GameObject bomb = Instantiate(bombFactory);

                    // 폭탄 생성 위치는 firePosition이다
                    bomb.transform.position = firePosition.transform.position;

                    // 폭탄 오브젝트의 Rigidbody를 가져옴
                    Rigidbody rbBomb = bomb.GetComponent<Rigidbody>();

                    // rigidbody에 카메라의 정면 방향으로 물리적인 힘을가해준다
                    rbBomb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);

                    break;

                case WeaponMode.Sniper:
                    if (!zoomMode) // 저격 모드가 아니라면 저격 모드로 변경
                    {
                        Camera.main.fieldOfView = 15f;
                        zoomMode = true;
                    }
                    else // 저격 모드라면 일반 모드로 변경
                    {
                        Camera.main.fieldOfView = 60f;
                        zoomMode = false;
                    }

                    break;
            }  
        }

        // 마우스 왼쪽 클릭
        if (Input.GetMouseButtonDown(0))
        {
            // 이동 Blend Tree 파라미터 값이 0이면 공격 애니메이션 실행
            if (anim.GetFloat("MoveMotion") == 0f)
            {
                anim.SetTrigger("Attack");
            }

            // ray 생성, 발사 위치, 진행 방향 설정
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        
            // ray가 부딪힌 대상 정보를 저장할 변수 생성
            RaycastHit hitInfo = new RaycastHit();

            // ray가 부딪힌 물체가 있다면
            if (Physics.Raycast(ray, out hitInfo))
            {
                // 레이에 부딪힌 대상의 Layer가 'Enemy'
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    // 에너미 피격 실행
                    Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
                    enemy.hitEnemy(weaponPower);
                }

                else
                {
                    // 피격 이펙트의 위치를 ray가 부딪힌 위치로 설정
                    bulletEffect.transform.position = hitInfo.point;

                    // 피격 이펙트의 forward 방향을 ray가 부딪힌 지점의 법선 벡터와 일치시킴
                    // 이펙트 생성 방향을 맞춰준다
                    bulletEffect.transform.forward = hitInfo.normal;

                    // 피격 이펙트 play
                    ps.Play();
                }
            }
        }
    }

    // 플레이어 피격 함수
    public void damagedAction(int damage)
    {
        hp -= damage;
        
        // 체력이 아직 남아있으면 피격 효과 실행
        if (hp > 0)
        {
            StartCoroutine(playerHitEffect());
        }
    }

    // 피격 효과 코루틴
    IEnumerator playerHitEffect()
    {
        // 피격 UI 활성화
        hitEffect.SetActive(true);

        // 0.3초간 대기
        yield return new WaitForSeconds(0.3f);

        // 피격 UI 비활성화
        hitEffect.SetActive(false);
    }
}
