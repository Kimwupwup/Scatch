using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinActive : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (GameObject.Find("View Control").GetComponent<Compiler>().isResetView == true)
        {
            Debug.Log("CoinReset");
            GameObject.Find("Coin").SetActive(true);
        }
    }
}
