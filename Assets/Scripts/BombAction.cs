using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    // ���� ȿ�� ������ ����
    public GameObject bombEffect;

    private void OnCollisionEnter(Collision collision)
    {
        // ����Ʈ ����
        GameObject eff = Instantiate(bombEffect);

        // ����Ʈ�� ��ġ�� ��ź ������Ʈ �ڽ��� ��ġ�� ����
        eff.transform.position = transform.position;

        Destroy(gameObject); // �ڱ� �ڽ� �ı�
    }
}
