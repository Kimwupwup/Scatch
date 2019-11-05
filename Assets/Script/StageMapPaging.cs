using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMapPaging : MonoBehaviour
{
    private GameObject stagesSet;
    private RectTransform stagesSetRectT;
    private int currentIdx = 0;
    private int fullPage;
    private Vector2 targetPos;

    private void Start() {
        stagesSet = GameObject.FindGameObjectWithTag("Stage");
        fullPage = stagesSet.transform.childCount;
        targetPos = stagesSet.GetComponent<RectTransform>().anchoredPosition;
        stagesSetRectT = stagesSet.GetComponent<RectTransform>();
        currentIdx = 0;
    }

    private void Update() {
        
        if (stagesSet != null) {
            stagesSetRectT.anchoredPosition = 
                Vector2.Lerp(
                    stagesSetRectT.anchoredPosition, 
                    targetPos, 
                    Time.deltaTime * 4f);
        }
    }

    public void MoveNextStagesSet() {
        if (currentIdx < fullPage - 1) {
            targetPos.x -= 1440;
            currentIdx++;
        }
    }

    public void MovePrevStagesSet() {
        if (currentIdx > 0) {
            targetPos.x += 1440;
            currentIdx--;
        }
    }
}
