using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public int x;
    private int y = 10;

    void Start()
    {
        
    }

    void Update()
    {
        // Q 키 누르는 동안
        if (Input.GetKey(KeyCode.Q))
        {
            Debug.Log("Q pressed");
        }

        // W 키 누른 순간
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("W pressed");
        }

        // E 키 누르고 뗀 순간
        if (Input.GetKeyUp(KeyCode.E))
        {
            Debug.Log("E pressed");
        }

        // 마우스 왼쪽 버튼 누른 순간
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse Left Button Pressed");
        }

        // 마우스 오른쪽 버튼 누른 순간
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Mouse Right Button Pressed");
        }

    }
}
