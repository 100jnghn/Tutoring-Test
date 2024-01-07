using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // ���� ����
    public enum EnemyState
    {
        Idle, // �⺻ ����
        Move, // �����̴� ��
        Attack, // �����ϴ� ��
        Return, // �ڱ� �ڸ��� ���ư��� ��
        Damaged, // �ǰ� ��
        Die // ����
    }

    public EnemyState mState; // ���¸� ������ ����

    private Vector3 originPos; // �ʱ� ��ġ ���� ����
    private Quaternion originRot; // �ʱ� ȸ���� ���� ����

    public CharacterController cc; // ĳ���� ��Ʈ�ѷ� ������Ʈ

    public GameObject player; // �÷��̾� ������Ʈ

    public Slider hpSlider; // ���ʹ� hp �����̴� ����

    private Animator anim; // �ִϸ�����

    private NavMeshAgent nav; // ������̼� ������Ʈ ����

    public int hp = 15; // ���ʹ� ���� ü��
    public int maxHP = 15; // ���ʹ� �ִ� ü��
    public int attackPower = 3; // ���ʹ� ���ݷ�

    public float findDistance = 8f; // �÷��̾� �߰� ����
    public float attackDistance = 2f; // ���� ���� ����
    public float moveSpeed = 5f; // �̵� �ӵ�
    public float attackDelay = 2f; // ���� ������ �ð�
    public float moveDistance = 20f; // �̵� ���� ����

    private float currentTime = 0f; // ���� �ð�


    void Start()
    {
        mState = EnemyState.Idle;
        cc = GetComponent<CharacterController>();
        originPos = transform.position;
        originRot = transform.rotation;
        anim = transform.GetComponentInChildren<Animator>();
        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        doCheckState();
        doSetHP();
    }

    // ���¸� üũ�ϰ� �ش� ���¸� ������ ����
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

    // �⺻ ����
    void doIdle()
    {
        // �÷��̾���� �Ÿ��� �߰� ���� ��
        if (Vector3.Distance(transform.position, player.transform.position) < findDistance)
        {
            mState = EnemyState.Move; // �̵� ���·� ��ȯ
            Debug.Log("���� ��ȯ Idle -> " +  mState);

            // �̵� �ִϸ��̼����� ��ȯ
            anim.SetTrigger("IdleToMove");
        }
    }

    // �̵� ����
    void doMove()
    {
        // ���� ��ġ�� �ʱ� ��ġ���� �̵� ���� ������ �Ѿ�ٸ�
        if (Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            // ���� ���¸� ���� ���·� ��ȯ
            mState = EnemyState.Return;
            Debug.Log("���� ��ȯ Move -> " + mState);
        }

        // �÷��̾���� �Ÿ��� ���� ���� �� -> �÷��̾ ���� �̵�
        else if (Vector3.Distance(transform.position, player.transform.position) > attackDistance)
        {
            /*
            // �̵� ���� ����
            Vector3 dir = (player.transform.position - transform.position).normalized;

            // �÷��̾�� �̵�
            cc.Move(dir * moveSpeed * Time.deltaTime);

            // �÷��̾ �ٶ󺻴�
            transform.forward = dir;
            */

            // ������̼� ������Ʈ�� �̵��� ���߰� ��θ� �ʱ�ȭ
            nav.isStopped = true;
            nav.ResetPath();

            // ������̼����� �����ϴ� �ּ� ���� ���� �Ÿ��� ����
            nav.stoppingDistance = attackDistance;

            // ������̼��� �������� �÷��̾��� ��ġ��
            nav.destination = player.transform.position;
        }

        // �׷��� ������ ���� ���¸� �������� ��ȯ
        else
        {
            mState = EnemyState.Attack;
            Debug.Log("���� ��ȯ Move -> " + mState);

            // ���� �ð��� ���� ������ �ð���ŭ ������� ���´�
            currentTime = attackDelay;

            // ���� ��� �ִϸ��̼� �÷���
            anim.SetTrigger("MoveToAttackDelay");
        }
    }

    // ���� ����
    void doAttack()
    {
        // �÷��̾ ���� ���� �̳� -> �÷��̾� ����
        if (Vector3.Distance(transform.position, player.transform.position) < attackDistance)
        {
            // attackDelay ���� ����
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                Debug.Log("Enemy�� Player ����");

                //player.GetComponent<Player>().damagedAction(attackPower);
                currentTime = 0;

                // ���� �ִϸ��̼� �÷���
                anim.SetTrigger("StartAttack");
            }
        }

        // �׷��� ������ ���� ���¸� Move�� ��ȯ (���߰�)
        else
        {
            mState = EnemyState.Move;
            Debug.Log("���� ��ȯ Attack -> " + mState);
            currentTime = 0;

            // �̵� �ִϸ��̼� �÷���
            anim.SetTrigger("AttackToMove");
        }
    }

    // �÷��̾� ��ũ��Ʈ�� ������ ó�� �Լ� �����ϴ� �Լ�
    public void attackAction()
    {
        player.GetComponent<Player>().damagedAction(attackPower);
    }

    // ���� ����
    void doReturn()
    {
        // �ʱ� ��ġ������ �Ÿ��� 0.1 �̻��̸� �ʱ� ��ġ�� �̵�
        if (Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            /*
            Vector3 dir = (originPos - transform.position).normalized;
            cc.Move(dir * moveSpeed * Time.deltaTime);

            // ������ ���� ��������
            transform.forward = dir;
            */

            // ������̼��� �������� �ʱ� ��ġ�� ����
            nav.destination = originPos;

            // ������̼����� �����ϴ� �ּ� �Ÿ��� 0���� ����
            nav.stoppingDistance = 0;
        }

        // ���� ��ġ�� �ʱ� ��ġ��� (���� �Ϸ�)
        else
        {
            // ������̼� �̵��� ���߰� ��� �ʱ�ȭ
            nav.isStopped = true;
            nav.ResetPath();

            transform.position = originPos;
            transform.rotation = originRot;

            hp = maxHP; // ü�� ȸ��
            mState = EnemyState.Idle; // �⺻ ���·� ��ȯ

            Debug.Log("���� ��ȯ Return -> " + mState);

            // ��� �ִϸ��̼����� ��ȯ
            anim.SetTrigger("MoveToIdle");
        }
    }

    // �ǰ� ����
    void doDamaged()
    {
        StartCoroutine(damageProcess());
    }

    // ���
    void doDie()
    {
        // ���� ���� ��� �ڷ�ƾ ����
        StopAllCoroutines();

        // ���� �ڷ�ƾ ����
        StartCoroutine(dieProcess());
    }

    // ���� ��������ŭ ü�� ����
    public void hitEnemy(int damage)
    {
        // �ǰ� ���� || ��� ���� || ���� ���� -> �ƹ� ���� ���� �ʰ� �Լ� ���� 
        if (mState == EnemyState.Damaged || mState == EnemyState.Die || mState == EnemyState.Return)
        {
            return;
        }

        hp -= damage;

        // ������̼� �̵� ���߰� ��� �ʱ�ȭ
        nav.isStopped = true;
        nav.ResetPath();

        // ü���� ���� ����������
        if (hp > 0)
        {
            mState = EnemyState.Damaged;
            Debug.Log("���� ��ȯ Any State -> " + mState);

            // �ǰ� �ִϸ��̼� ���
            anim.SetTrigger("Damaged");

            doDamaged();
        }

        // ü�� �� ����������
        else
        {
            mState = EnemyState.Die;
            Debug.Log("���� ��ȯ Any State -> " + mState);

            // Die �ִϸ��̼� ���
            anim.SetTrigger("Die");

            doDie();
        }
    }

    // �ǰ��� ó���ϴ� �ڷ�ƾ
    IEnumerator damageProcess()
    {
        // �ǰ� ��Ǹ�ŭ ��ٸ���
        yield return new WaitForSeconds(1f);

        // ���� ���¸� �̵� ���·� ��ȯ
        mState = EnemyState.Move;
        Debug.Log("���� ��ȯ Damaged -> " + mState);
    }

    // ������ ó���ϴ� �ڷ�ƾ
    IEnumerator dieProcess()
    {
        // ĳ���� ��Ʈ�ѷ� ��Ȱ��ȭ
        cc.enabled = false;

        // 2�� �� ���ʹ� �������� �ı�
        yield return new WaitForSeconds(2f);

        Debug.Log("Enemy Die");
        Destroy(gameObject);
    }

    // UI - HP ����
    void doSetHP()
    {
        hpSlider.value = (float)hp / (float)maxHP;
    }
}
