using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float rotSpeed = 200f; // ȸ�� �ӵ�
    public float moveSpeed = 7f; // �̵� �ӵ�
    public float jumpPower = 10f; // ������ �������� ��
    public float throwPower = 15f; // ��ź ���� �� �������� ��

    public bool isJumping = false; // ���� ���ΰ�?

    private float mx = 0; // ȸ�� ��
    private float gravity = -20f; // �߷� ��
    private float yVelocity = 0f; // ���� �ӷ� ����

    public GameObject firePosition; // �߻� ��ġ
    public GameObject bombFactory; // ��ź ������Ʈ
    public GameObject bulletEffect; // �Ѿ� �ǰ� ����Ʈ

    public ParticleSystem ps; // �ǰ� ����Ʈ ��ƼŬ �ý���

    private CharacterController cc; // ĳ���� ��Ʈ�ѷ�

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        doRotate(); // ���콺�� �÷��̾� ȸ��
        doMove(); // WASD�� �÷��̾� �̵�
        doJump(); // Spacebar�� ����
        doFire(); // ���콺 �Է����� ���� ���
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
            // ��ź ������Ʈ ����
            GameObject bomb = Instantiate(bombFactory);

            // ��ź ���� ��ġ�� firePosition�̴�
            bomb.transform.position = firePosition.transform.position;

            // ��ź ������Ʈ�� Rigidbody�� ������
            Rigidbody rbBomb = bomb.GetComponent<Rigidbody>();

            // rigidbody�� ī�޶��� ���� �������� �������� ���������ش�
            rbBomb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
        }

        // ���콺 ���� Ŭ��
        if (Input.GetMouseButtonDown(0))
        {
            // ray ����, �߻� ��ġ, ���� ���� ����
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        
            // ray�� �ε��� ��� ������ ������ ���� ����
            RaycastHit hitInfo = new RaycastHit();

            // ray�� �ε��� ��ü�� �ִٸ�
            if (Physics.Raycast(ray, out hitInfo))
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
