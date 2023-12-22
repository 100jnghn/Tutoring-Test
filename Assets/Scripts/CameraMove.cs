using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float rotSpeed = 200f; // ȸ�� �ӵ� ����

    public Transform target; // ī�޶��� ��ǥ ��ġ

    // ȸ�� �� ����
    private float mx = 0;
    private float my = 0;

    void Start()
    {
        
    }

    void Update()
    {
        camfollow();
        camMove();
    }

    // ī�޶� ����� ����
    void camfollow()
    {
        transform.position = target.position;
    }

    // ī�޶� ���콺 ȸ���� ���� ȸ��
    void camMove()
    {
        // ���콺 �Է� ����
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // ȸ�� �� ������ ���콺 �Է� ����ŭ ����
        mx += mouseX * rotSpeed * Time.deltaTime;
        my += mouseY * rotSpeed * Time.deltaTime;

        // X�� ȸ�� ��(�� �Ʒ��� ȸ��) -90 ~ 90�� ���̷� ����
        my = Mathf.Clamp(my, -90f, 90f);

        // ��ü�� ȸ�� �������� ȸ��
        transform.eulerAngles = new Vector3(-my, mx, 0);
    }
}
