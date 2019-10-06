using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvent : MonoBehaviour
{
    public void Reset()
    {
        Compiler compiler = GameObject.FindGameObjectWithTag("compiler").GetComponent<Compiler>();
        compiler.ResetView();
        Time.timeScale = 1;
    }
}
