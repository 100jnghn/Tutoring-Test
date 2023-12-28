using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gm; // �̱��� ����

    public GameState gState; // ���� ���� ���� ����

    public GameObject gameLabel; // ���� ���� UI ������Ʈ ����

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
    }

    void Update()
    {
        
    }
}
