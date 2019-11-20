using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ViewPanelPointerHandle : MonoBehaviour, IPointerDownHandler, IPointerExitHandler {
    public int cnt = 0;

    public void OnPointerDown(PointerEventData eventData) {
        cnt++;
    }

    public void OnPointerExit(PointerEventData eventData) {
        cnt--;
    }
}