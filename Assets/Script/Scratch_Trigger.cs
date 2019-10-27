﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scratch_Trigger : MonoBehaviour
{
    private int count;
    private BtnDisable btnDisable;
    public Text countText;
    private GameObject failPanel;
    private GameObject clearPanel;

    // Start is called before the first frame update
    void Start()
    {
        failPanel = GameObject.FindGameObjectWithTag("canvas").transform.Find("clear").gameObject;
        clearPanel = GameObject.FindGameObjectWithTag("canvas").transform.Find("fail").gameObject;
        btnDisable = GameObject.FindGameObjectWithTag("compiler").GetComponent<BtnDisable>();
        count = 0;
        countText.text = count.ToString();
    }

    public void SetCount()
    {
        count = 0;
        countText.text = count.ToString();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Fire"))
        {
            btnDisable.ClickBtnReset();
            Time.timeScale = 0;
            failPanel.SetActive(true);
        }
        else if (other.gameObject.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            countText.text = count.ToString();
        }
        else if (other.gameObject.CompareTag("Flag"))
        {
            btnDisable.ClickBtnReset();
            Time.timeScale = 0;
            clearPanel.SetActive(true);
        }
    }
}
