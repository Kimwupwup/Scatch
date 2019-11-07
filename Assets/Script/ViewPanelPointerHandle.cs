using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ViewPanelPointerHandle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int cnt = 0;
    public void OnPointerEnter(PointerEventData eventData) {
        cnt++;
    }

    public void OnPointerExit(PointerEventData eventData) {
        cnt--;
    }
}
