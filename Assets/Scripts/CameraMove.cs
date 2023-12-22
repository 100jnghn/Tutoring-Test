using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float rotSpeed = 200f; // 회전 속도 변수

    public Transform target; // 카메라의 목표 위치

    // 회전 값 변수
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

    // 카메라가 대상을 따라감
    void camfollow()
    {
        transform.position = target.position;
    }

    // 카메라가 마우스 회전에 따라 회전
    void camMove()
    {
        // 마우스 입력 받음
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 회전 값 변수에 마우스 입력 값만큼 누적
        mx += mouseX * rotSpeed * Time.deltaTime;
        my += mouseY * rotSpeed * Time.deltaTime;

        // X축 회전 값(위 아래로 회전) -90 ~ 90도 사이로 제한
        my = Mathf.Clamp(my, -90f, 90f);

        // 물체를 회전 방향으로 회전
        transform.eulerAngles = new Vector3(-my, mx, 0);
    }
}
