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
        // 쳐다볼 방향 저장을 위한 빈 Vector 값 생성
        Vector3 dir = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.forward * 2.0f * Time.deltaTime, Space.World);
            dir += Vector3.forward; // 벡터 값에 앞쪽 방향을 더함
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.back * 2.0f * Time.deltaTime, Space.World);
            dir += Vector3.back; // 벡터 값에 뒤쪽 방향을 더함
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * 2.0f * Time.deltaTime, Space.World);
            dir += Vector3.right; // 벡터 값에 오른쪽 방향을 더함
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * 2.0f * Time.deltaTime, Space.World);
            dir += Vector3.left; // 벡터 값에 왼쪽 방향을 더함
        }

        dir = dir.normalized;

        if (dir.magnitude > 0.5f)
        {
            transform.LookAt(transform.position + dir);
        }




        // 스페이스바 누르면
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bullet, firePos.transform);
        }
    }

}
