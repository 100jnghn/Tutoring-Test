using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    // ���� ȿ�� ������ ����
    public GameObject bombEffect;

    // ��ź ������
    public int attackPower = 10;

    // ���� ȿ�� �ݰ�
    public float explosionRadius = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        // ���� �ݰ� ������ layer�� 'Enemy'�� ������Ʈ ã��
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8);

        // cols �迭�� �ִ� ������Ʈ�� ������ ����
        for (int i=0; i<cols.Length; i++)
        {
            cols[i].GetComponent<Enemy>().hitEnemy(attackPower);
        }


        // ����Ʈ ����
        GameObject eff = Instantiate(bombEffect);

        // ����Ʈ�� ��ġ�� ��ź ������Ʈ �ڽ��� ��ġ�� ����
        eff.transform.position = transform.position;

        Destroy(gameObject); // �ڱ� �ڽ� �ı�
    }
}
