using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    // 폭발 효과 프리팹 변수
    public GameObject bombEffect;

    private void OnCollisionEnter(Collision collision)
    {
        // 이펙트 생성
        GameObject eff = Instantiate(bombEffect);

        // 이펙트의 위치는 폭탄 오브젝트 자신의 위치와 동일
        eff.transform.position = transform.position;

        Destroy(gameObject); // 자기 자신 파괴
    }
}
