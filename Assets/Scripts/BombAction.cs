using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    // 폭발 효과 프리팹 변수
    public GameObject bombEffect;

    // 폭탄 데미지
    public int attackPower = 10;

    // 폭발 효과 반경
    public float explosionRadius = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        // 폭발 반경 내에서 layer가 'Enemy'인 오브젝트 찾음
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8);

        // cols 배열에 있는 오브젝트에 데미지 적용
        for (int i=0; i<cols.Length; i++)
        {
            cols[i].GetComponent<Enemy>().hitEnemy(attackPower);
        }


        // 이펙트 생성
        GameObject eff = Instantiate(bombEffect);

        // 이펙트의 위치는 폭탄 오브젝트 자신의 위치와 동일
        eff.transform.position = transform.position;

        Destroy(gameObject); // 자기 자신 파괴
    }
}
