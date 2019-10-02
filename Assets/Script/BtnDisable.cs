using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BtnDisable : MonoBehaviour
{
    public GameObject start;
    public GameObject reset;

    private Button btnStart;
    private Button btnReset;
    // Start is called before the first frame update
    void Start()
    {
        btnStart = start.GetComponent<Button>();
        btnReset = reset.GetComponent<Button>();
    }

    public void ClickBtnStart() {
        btnStart.interactable = false;
        btnReset.interactable = true;
    }

    public void ClickBtnReset() {
        btnStart.interactable = true;
        btnReset.interactable = false;
    }
}
