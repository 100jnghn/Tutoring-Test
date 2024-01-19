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

    public int hp = 100; // �÷��̾� ü��
    public int maxHP = 100; // �÷��̾� �ִ� ü��
    public int weaponPower = 5; // �߻� ���� ���ݷ�

    public float rotSpeed = 200f; // ȸ�� �ӵ�
    public float moveSpeed = 7f; // �̵� �ӵ�
    public float jumpPower = 10f; // ������ �������� ��
    public float throwPower = 15f; // ��ź ���� �� �������� ��

    public bool isJumping = false; // ���� ���ΰ�?

    private float mx = 0; // ȸ�� ��
    private float gravity = -20f; // �߷� ��
    private float yVelocity = 0f; // ���� �ӷ� ����
    private bool zoomMode = false;

    public GameObject firePosition; // �߻� ��ġ
    public GameObject bombFactory; // ��ź ������Ʈ
    public GameObject bulletEffect; // �Ѿ� �ǰ� ����Ʈ
    public GameObject hitEffect; // �ǰ� ȿ�� UI ������Ʈ

    public Slider hpSlider; // hp �����̴� ����
    public Text wModeText; // ���� ��� �ؽ�Ʈ

    public ParticleSystem ps; // �ǰ� ����Ʈ ��ƼŬ �ý���

    private CharacterController cc; // ĳ���� ��Ʈ�ѷ�
    private Animator anim; // �ִϸ����� 

    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        wMode = WeaponMode.Normal;
    }

    void Update()
    {
        // ������ Run ���°� �ƴϸ� �÷��̾� ���� ����
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        doRotate(); // ���콺�� �÷��̾� ȸ��
        doMove(); // WASD�� �÷��̾� �̵�
        doJump(); // Spacebar�� ����
        doFire(); // ���콺 �Է����� ���� ���
        doSetHP(); // hp UI ����
        doSetMode(); // ��� ����
    }

    // sniper, normal ��� ����
    void doSetMode()
    {
        // ����1 �Է� : normal���
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

    // UI - HP ����
    void doSetHP()
    {
        hpSlider.value = (float)hp / (float)maxHP;
    }

    // ���콺 ȸ���� ���� �÷��̾� ȸ��
    void doRotate()
    {
        // ���콺 �¿� ȸ�� �Է� �ޱ�
        float mouseX = Input.GetAxis("Mouse X");

        // ȸ�� �� ������ ���콺 ȸ�� �Է� �� ����
        mx += mouseX * rotSpeed * Time.deltaTime;

        // �÷��̾� ȸ��
        transform.eulerAngles = new Vector3(0, mx, 0);
    }

    // �÷��̾� �̵�
    void doMove()
    {
        // Ű �Է� ����
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // �̵� ���� ����
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized; // ����ȭ

        // �̵� ���� Ʈ�� ȣ���ϰ� ������ ũ�� �� ����
        anim.SetFloat("MoveMotion", dir.magnitude);

        // ���� ī�޶� �������� ������ ��ȯ
        dir = Camera.main.transform.TransformDirection(dir);

        // ĳ���� ���� �ӵ��� �߷� �� ����
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        // �̵�
        cc.Move(dir * moveSpeed * Time.deltaTime);
    }

    // �÷��̾� ����
    void doJump()
    {
        // �ٴڿ� �����ߴٸ� && ���� ���̾��ٸ�
        if (cc.collisionFlags == CollisionFlags.Below)
        {
            if (isJumping)
            {
                // ���� ���� �ƴϴ�
                isJumping = false;

                // ĳ���� ���� �ӵ� 0����
                yVelocity = 0f;
            }
        }

        // ���� Ű(spacebar) ������ && ���� ���� �ƴ϶��
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            // ĳ���� ���� �ӵ��� ������ ����
            yVelocity = jumpPower;

            // ������ ���� ���·� ����
            isJumping = true;
        }
    }

    // ���� ���
    void doFire()
    {
        // ���콺 ������ Ŭ��
        if (Input.GetMouseButtonDown(1))
        {
            switch(wMode)
            {
                case WeaponMode.Normal:
                    // ��ź ������Ʈ ����
                    GameObject bomb = Instantiate(bombFactory);

                    // ��ź ���� ��ġ�� firePosition�̴�
                    bomb.transform.position = firePosition.transform.position;

                    // ��ź ������Ʈ�� Rigidbody�� ������
                    Rigidbody rbBomb = bomb.GetComponent<Rigidbody>();

                    // rigidbody�� ī�޶��� ���� �������� �������� ���������ش�
                    rbBomb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);

                    break;

                case WeaponMode.Sniper:
                    if (!zoomMode) // ���� ��尡 �ƴ϶�� ���� ���� ����
                    {
                        Camera.main.fieldOfView = 15f;
                        zoomMode = true;
                    }
                    else // ���� ����� �Ϲ� ���� ����
                    {
                        Camera.main.fieldOfView = 60f;
                        zoomMode = false;
                    }

                    break;
            }  
        }

        // ���콺 ���� Ŭ��
        if (Input.GetMouseButtonDown(0))
        {
            // �̵� Blend Tree �Ķ���� ���� 0�̸� ���� �ִϸ��̼� ����
            if (anim.GetFloat("MoveMotion") == 0f)
            {
                anim.SetTrigger("Attack");
            }

            // ray ����, �߻� ��ġ, ���� ���� ����
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        
            // ray�� �ε��� ��� ������ ������ ���� ����
            RaycastHit hitInfo = new RaycastHit();

            // ray�� �ε��� ��ü�� �ִٸ�
            if (Physics.Raycast(ray, out hitInfo))
            {
                // ���̿� �ε��� ����� Layer�� 'Enemy'
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    // ���ʹ� �ǰ� ����
                    Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
                    enemy.hitEnemy(weaponPower);
                }

                else
                {
                    // �ǰ� ����Ʈ�� ��ġ�� ray�� �ε��� ��ġ�� ����
                    bulletEffect.transform.position = hitInfo.point;

                    // �ǰ� ����Ʈ�� forward ������ ray�� �ε��� ������ ���� ���Ϳ� ��ġ��Ŵ
                    // ����Ʈ ���� ������ �����ش�
                    bulletEffect.transform.forward = hitInfo.normal;

                    // �ǰ� ����Ʈ play
                    ps.Play();
                }
            }
        }
    }

    // �÷��̾� �ǰ� �Լ�
    public void damagedAction(int damage)
    {
        hp -= damage;
        
        // ü���� ���� ���������� �ǰ� ȿ�� ����
        if (hp > 0)
        {
            StartCoroutine(playerHitEffect());
        }
    }

    // �ǰ� ȿ�� �ڷ�ƾ
    IEnumerator playerHitEffect()
    {
        // �ǰ� UI Ȱ��ȭ
        hitEffect.SetActive(true);

        // 0.3�ʰ� ���
        yield return new WaitForSeconds(0.3f);

        // �ǰ� UI ��Ȱ��ȭ
        hitEffect.SetActive(false);
    }
}
