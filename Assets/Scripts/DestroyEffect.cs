using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    // ����Ʈ�� ���� �� ���ŵ� �ð� ����
    public float destroyTime = 1.5f;

    // ��� �ð� ������ ����
    private float currentTime = 0f;

    void Update()
    {
        // ���� ��� �ð��� ���ŵ� �ð��� �ʰ��ϸ�
        if (currentTime > destroyTime)
        {
            Destroy(gameObject); // �ڱ� �ڽ� ����
        }

        // ��� �ð� ����
        currentTime += Time.deltaTime;
    }
}
