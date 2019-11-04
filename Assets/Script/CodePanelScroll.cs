using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CodePanelScroll : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {
    private GameObject codePanel;
    private GameObject realCodePanel;
    private Vector2 originPanelPos;
    private Vector2 originPos;
    private Vector2 currentPos;

    // Start is called before the first frame update
    void Start()
    {
        codePanel = GameObject.FindGameObjectWithTag("codePanel");
        realCodePanel = GameObject.FindGameObjectWithTag("codePanel").transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData) {
        originPos = eventData.position;
        originPanelPos = realCodePanel.transform.position;
    }

    public void OnEndDrag(PointerEventData eventData) {
    }

    public void OnDrag(PointerEventData eventData) {
        currentPos = eventData.position - originPos;
        currentPos.x = 0;

        Debug.Log(currentPos);
        if (originPanelPos.y + currentPos.y < codePanel.transform.position.y + 2000 && originPanelPos.y + currentPos.y > codePanel.transform.position.y - 2000)
            realCodePanel.transform.position = originPanelPos + currentPos;
    }
}
