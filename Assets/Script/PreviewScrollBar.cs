using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewScrollBar : MonoBehaviour
{
    private GameObject realCodePanel;
    private GameObject thisObj;
    private GameObject cur;
    // Start is called before the first frame update
    void Start()
    {
        thisObj = this.gameObject;
        realCodePanel = GameObject.FindGameObjectWithTag("codePanel").transform.GetChild(1).gameObject;
    }

    private void Update() {
        DrawPreview();
    }

    public void DrawPreview() {
        if (cur != null)
            Destroy(cur);
        cur = Instantiate(realCodePanel, thisObj.transform) as GameObject;
        cur.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
}
