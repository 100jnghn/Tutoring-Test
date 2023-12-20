using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    private float speed = 1000f;
    
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // rigidbody 컴포넌트 가져옴

        Debug.Log("Blue Sphere Instantiated"); // 이 오브젝트가 생성되면 해당 문구 출력

        moveForward();
    }

    private void moveForward()
    {
        // 강체의 앞 방향으로 speed만큼 힘을 가함
        rb.AddForce(transform.forward * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 만약 충돌한 collision의 태그가 Wall 이라면
        if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject); // 이 오브젝트는 파괴
        }
    }
}
