using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gm; // 싱글턴 변수

    public GameState gState; // 현재 게임 상태 변수

    public GameObject gameLabel; // 게임 상태 UI 오브젝트 변수

    Text gameText; // 게임 상태 UI 텍스트 컴포넌트 변수

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

        // 시작 시 텍스트 = Ready...
        gameText.color = new Color32(255, 185, 0, 255);
        gameText.text = "Ready...";
    }

    void Update()
    {
        
    }
}
