using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gm; // 싱글턴 변수

    public GameState gState; // 현재 게임 상태 변수

    public GameObject gameLabel; // 게임 상태 UI 오브젝트 변수

    public Player player; // 플레이어 변수

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

        // 텍스트 변경, Ready -> Run 상태로 변경
        StartCoroutine(readyToStart());
    }

    void Update()
    {
        checkPlayerHP();
    }

    IEnumerator readyToStart()
    {
        // 2초 대기
        yield return new WaitForSeconds(2f);

        // 텍스트 "GO!"로 변경
        gameText.text = "GO!";

        // 0.5초 대기
        yield return new WaitForSeconds(0.5f);

        // 텍스트 비활성화
        gameLabel.SetActive(false);

        // 상태를 Run 으로 변경
        gState = GameState.Run;
    }

    // 플레이어의 체력이 0 이하인지 체크하는 함수
    void checkPlayerHP()
    {
        if (player.hp <= 0)
        {
            // 상태 텍스트 활성화
            gameLabel.SetActive(true);

            // 텍스트 내용을 "Game Over"로 변경
            gameText.text = "Game Over";

            // 텍스트 색을 빨간색으로 변경
            gameText.color = new Color32(255, 0, 0, 255);

            // 게임의 상태를 GameOver로 변경
            gState = GameState.GameOver;
        }
    }
}
