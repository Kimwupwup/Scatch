using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvent : MonoBehaviour
{
    public GameObject button;

    public void Reset()
    {
        GameObject.Find("View Control").GetComponent<Compiler>().ResetView();
    }

}
