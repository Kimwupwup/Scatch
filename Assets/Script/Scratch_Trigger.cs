using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scratch_Trigger : MonoBehaviour
{
    private int count;
    private Text countText;
    private GameObject failPanel;
    private GameObject clearPanel;
    private PlayToggle playToggle;

    // Start is called before the first frame update
    void Start()
    {
        playToggle = GameObject.FindGameObjectWithTag("compiler").transform.GetChild(0).GetComponent<PlayToggle>();
        countText = GameObject.FindGameObjectWithTag("coinText").GetComponent<Text>();
        failPanel = GameObject.FindGameObjectWithTag("canvas").transform.Find("fail").gameObject;
        clearPanel = GameObject.FindGameObjectWithTag("canvas").transform.Find("clear").gameObject;
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
        if (other.gameObject.CompareTag("Fire") || other.gameObject.CompareTag("wall"))
        {
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
<<<<<<< HEAD
=======
            playToggle.OnMouseDown();
            GameObject.Find("Canvas").GetComponent<StageSaveAndLoad>().Save();
>>>>>>> nEw_BrAnCh_nO01
            Time.timeScale = 0;
            clearPanel.SetActive(true);
        }
    }
}
