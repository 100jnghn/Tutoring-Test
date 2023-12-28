using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform target; // Ä«¸Þ¶ó

    void Update()
    {
        transform.forward = target.forward;
    }
}
