using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoIndex : MonoBehaviour
{
    private int allContext;
    private int curContext;
    public Text pageNumber;

    // Start is called before the first frame update
    void Start()
    {
        curContext = 0;
        allContext = gameObject.transform.childCount - 3;  //0~curContext-1까지 접근하면 됌.
        pageNumber.text = $"{curContext + 1}/{allContext}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoNextPage()
    {
        gameObject.transform.GetChild(curContext).gameObject.SetActive(false);
        curContext = (curContext + 1) % allContext;
        gameObject.transform.GetChild(curContext).gameObject.SetActive(true);
        pageNumber.text = $"{curContext + 1}/{allContext}";
    }

    public void GoPrevPage()
    {
        gameObject.transform.GetChild(curContext).gameObject.SetActive(false);
        curContext -= 1;
        if (curContext < 0) curContext = allContext - 1;
        gameObject.transform.GetChild(curContext).gameObject.SetActive(true);
        pageNumber.text = $"{curContext + 1}/{allContext}";
    }
}
