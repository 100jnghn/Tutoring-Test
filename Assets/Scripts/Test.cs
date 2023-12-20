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
        // Q Ű ������ ����
        if (Input.GetKey(KeyCode.Q))
        {
            Debug.Log("Q pressed");
        }

        // W Ű ���� ����
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("W pressed");
        }

        // E Ű ������ �� ����
        if (Input.GetKeyUp(KeyCode.E))
        {
            Debug.Log("E pressed");
        }

        // ���콺 ���� ��ư ���� ����
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse Left Button Pressed");
        }

        // ���콺 ������ ��ư ���� ����
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Mouse Right Button Pressed");
        }

    }
}
