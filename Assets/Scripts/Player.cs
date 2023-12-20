using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject firePos;
    public GameObject bullet;

    void Start()
    {
    
    }

    void Update()
    {
        // �Ĵٺ� ���� ������ ���� �� Vector �� ����
        Vector3 dir = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.forward * 2.0f * Time.deltaTime, Space.World);
            dir += Vector3.forward; // ���� ���� ���� ������ ����
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.back * 2.0f * Time.deltaTime, Space.World);
            dir += Vector3.back; // ���� ���� ���� ������ ����
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * 2.0f * Time.deltaTime, Space.World);
            dir += Vector3.right; // ���� ���� ������ ������ ����
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * 2.0f * Time.deltaTime, Space.World);
            dir += Vector3.left; // ���� ���� ���� ������ ����
        }

        dir = dir.normalized;

        if (dir.magnitude > 0.5f)
        {
            transform.LookAt(transform.position + dir);
        }




        // �����̽��� ������
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bullet, firePos.transform);
        }
    }

}
