using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    // 이펙트가 생성 후 제거될 시간 변수
    public float destroyTime = 1.5f;

    // 경과 시간 측정용 변수
    private float currentTime = 0f;

    void Update()
    {
        // 만약 경과 시간이 제거된 시간을 초과하면
        if (currentTime > destroyTime)
        {
            Destroy(gameObject); // 자기 자신 제거
        }

        // 경과 시간 누적
        currentTime += Time.deltaTime;
    }
}
