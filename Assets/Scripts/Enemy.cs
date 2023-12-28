using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // 상태 변수
    public enum EnemyState
    {
        Idle, // 기본 상태
        Move, // 움직이는 중
        Attack, // 공격하는 중
        Return, // 자기 자리로 돌아가는 중
        Damaged, // 피격 중
        Die // 죽음
    }

    public EnemyState mState; // 상태를 저장할 변수

    private Vector3 originPos; // 초기 위치 저장 변수

    public CharacterController cc; // 캐릭터 컨트롤러 컴포넌트

    public GameObject player; // 플레이어 오브젝트

    public Slider hpSlider; // 에너미 hp 슬라이더 변수

    public int hp = 15; // 에너미 현재 체력
    public int maxHP = 15; // 에너미 최대 체력
    public int attackPower = 3; // 에너미 공격력

    public float findDistance = 8f; // 플레이어 발견 범위
    public float attackDistance = 2f; // 공격 가능 범위
    public float moveSpeed = 5f; // 이동 속도
    public float attackDelay = 2f; // 공격 딜레이 시간
    public float moveDistance = 20f; // 이동 가능 범위

    private float currentTime = 0f; // 누적 시간


    void Start()
    {
        mState = EnemyState.Idle;
        cc = GetComponent<CharacterController>();
        originPos = transform.position;
    }

    void Update()
    {
        doCheckState();
        doSetHP();
    }

    // 상태를 체크하고 해당 상태를 변수에 저장
    void doCheckState()
    {
        switch(mState)
        {
            case EnemyState.Idle:
                doIdle();
                break;
                
            case EnemyState.Move:
                doMove();
                break;

            case EnemyState.Attack:
                doAttack();
                break;

            case EnemyState.Return:
                doReturn();
                break;

            case EnemyState.Damaged:
                //doDamaged();
                break;

            case EnemyState.Die:
                //doDie();
                break;
        }
    }

    // 기본 상태
    void doIdle()
    {
        // 플레이어와의 거리가 발견 범위 안
        if (Vector3.Distance(transform.position, player.transform.position) < findDistance)
        {
            mState = EnemyState.Move; // 이동 상태로 전환
            Debug.Log("상태 전환 Idle -> " +  mState);
        }
    }

    // 이동 상태
    void doMove()
    {
        // 현재 위치가 초기 위치에서 이동 가능 범위를 넘어간다면
        if (Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            // 현재 상태를 복귀 상태로 전환
            mState = EnemyState.Return;
            Debug.Log("상태 전환 Move -> " + mState);
        }

        // 플레이어와의 거리가 공격 범위 밖 -> 플레이어를 향해 이동
        else if (Vector3.Distance(transform.position, player.transform.position) > attackDistance)
        {
            // 이동 방향 설정
            Vector3 dir = (player.transform.position - transform.position).normalized;

            // 플레이어에게 이동
            cc.Move(dir * moveSpeed * Time.deltaTime);
        }

        // 그렇지 않으면 현재 상태를 공격으로 전환
        else
        {
            mState = EnemyState.Attack;
            Debug.Log("상태 전환 Move -> " + mState);
        }
    }

    // 공격 상태
    void doAttack()
    {
        // 플레이어가 공격 범위 이내 -> 플레이어 공격
        if (Vector3.Distance(transform.position, player.transform.position) < attackDistance)
        {
            // attackDelay 마다 공격
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                Debug.Log("Enemy가 Player 공격");

                player.GetComponent<Player>().damagedAction(attackPower);
                currentTime = 0;
            }
        }

        // 그렇지 않으면 현재 상태를 Move로 전환 (재추격)
        else
        {
            mState = EnemyState.Move;
            Debug.Log("상태 전환 Attack -> " + mState);
            currentTime = 0;
        }
    }

    // 복귀 상태
    void doReturn()
    {
        // 초기 위치에서의 거리가 0.1 이상이면 초기 위치로 이동
        if (Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            cc.Move(dir * moveSpeed * Time.deltaTime);
        }

        // 현재 위치가 초기 위치라면 (복귀 완료)
        else
        {
            transform.position = originPos;
            hp = maxHP; // 체력 회복
            mState = EnemyState.Idle; // 기본 상태로 전환

            Debug.Log("상태 전환 Return -> " + mState);
        }
    }

    // 피격 상태
    void doDamaged()
    {
        StartCoroutine(damageProcess());
    }

    // 사망
    void doDie()
    {
        // 진행 중인 모든 코루틴 종료
        StopAllCoroutines();

        // 죽음 코루틴 시작
        StartCoroutine(dieProcess());
    }

    // 받은 데미지만큼 체력 감소
    public void hitEnemy(int damage)
    {
        // 피격 상태 || 사망 상태 || 복귀 상태 -> 아무 동작 하지 않고 함수 종료 
        if (mState == EnemyState.Damaged || mState == EnemyState.Die || mState == EnemyState.Return)
        {
            return;
        }

        hp -= damage;

        // 체력이 아직 남아있으면
        if (hp > 0)
        {
            mState = EnemyState.Damaged;
            Debug.Log("상태 전환 Any State -> " + mState);
            doDamaged();
        }

        // 체력 다 떨어졌으면
        else
        {
            mState = EnemyState.Die;
            Debug.Log("상태 전환 Any State -> " + mState);
            doDie();
        }
    }

    // 피격을 처리하는 코루틴
    IEnumerator damageProcess()
    {
        // 피격 모션만큼 기다린다
        yield return new WaitForSeconds(0.5f);

        // 현재 상태를 이동 상태로 전환
        mState = EnemyState.Move;
        Debug.Log("상태 전환 Damaged -> " + mState);
    }

    // 죽음을 처리하는 코루틴
    IEnumerator dieProcess()
    {
        // 캐릭터 컨트롤러 비활성화
        cc.enabled = false;

        // 2초 후 에너미 오브젝터 파괴
        yield return new WaitForSeconds(2f);

        Debug.Log("Enemy Die");
        Destroy(gameObject);
    }

    // UI - HP 관리
    void doSetHP()
    {
        hpSlider.value = (float)hp / (float)maxHP;
    }
}
