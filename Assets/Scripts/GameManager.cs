using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gm; // �̱��� ����

    public GameState gState; // ���� ���� ���� ����

    public GameObject gameLabel; // ���� ���� UI ������Ʈ ����

    public Player player; // �÷��̾� ����

    Text gameText; // ���� ���� UI �ؽ�Ʈ ������Ʈ ����

    public enum GameState
    {
        Ready, 
        Run,
        GameOver
    };

    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
    }

    void Start()
    {
        gState = GameState.Ready;
        gameText = gameLabel.GetComponent<Text>();

        // ���� �� �ؽ�Ʈ = Ready...
        gameText.color = new Color32(255, 185, 0, 255);
        gameText.text = "Ready...";

        // �ؽ�Ʈ ����, Ready -> Run ���·� ����
        StartCoroutine(readyToStart());
    }

    void Update()
    {
        checkPlayerHP();
    }

    IEnumerator readyToStart()
    {
        // 2�� ���
        yield return new WaitForSeconds(2f);

        // �ؽ�Ʈ "GO!"�� ����
        gameText.text = "GO!";

        // 0.5�� ���
        yield return new WaitForSeconds(0.5f);

        // �ؽ�Ʈ ��Ȱ��ȭ
        gameLabel.SetActive(false);

        // ���¸� Run ���� ����
        gState = GameState.Run;
    }

    // �÷��̾��� ü���� 0 �������� üũ�ϴ� �Լ�
    void checkPlayerHP()
    {
        if (player.hp <= 0)
        {
            // ���� �ؽ�Ʈ Ȱ��ȭ
            gameLabel.SetActive(true);

            // �ؽ�Ʈ ������ "Game Over"�� ����
            gameText.text = "Game Over";

            // �ؽ�Ʈ ���� ���������� ����
            gameText.color = new Color32(255, 0, 0, 255);

            // ������ ���¸� GameOver�� ����
            gState = GameState.GameOver;
        }
    }
}
