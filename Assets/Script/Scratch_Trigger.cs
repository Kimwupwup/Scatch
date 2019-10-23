using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scratch_Trigger : MonoBehaviour
{
    private int count;
    private BtnDisable btnDisable;
    public Text countText;

    // Start is called before the first frame update
    void Start()
    {
        btnDisable = GameObject.FindGameObjectWithTag("compiler").GetComponent<BtnDisable>();
        count = 0;
        countText.text = "Count : " + count.ToString();
    }

    public void SetCount()
    {
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        countText.text = "Count : " + count.ToString();

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Fire"))
        {
            btnDisable.ClickBtnReset();
            Debug.Log("Fire");
            Time.timeScale = 0;
            GameObject.Find("Canvas").transform.Find("fail").gameObject.SetActive(true);
        }
        else if (other.gameObject.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
        }
        else if (other.gameObject.CompareTag("Flag"))
        {
            btnDisable.ClickBtnReset();
            Debug.Log(count);
            Debug.Log("Flag");
            Time.timeScale = 0;
            GameObject.Find("Canvas").transform.Find("clear").gameObject.SetActive(true);
        }
    }
}
