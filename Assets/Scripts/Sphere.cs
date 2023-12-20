using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    private float speed = 1000f;
    
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // rigidbody ������Ʈ ������

        Debug.Log("Blue Sphere Instantiated"); // �� ������Ʈ�� �����Ǹ� �ش� ���� ���

        moveForward();
    }

    private void moveForward()
    {
        // ��ü�� �� �������� speed��ŭ ���� ����
        rb.AddForce(transform.forward * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ���� �浹�� collision�� �±װ� Wall �̶��
        if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject); // �� ������Ʈ�� �ı�
        }
    }
}
