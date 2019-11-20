using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayToggle : MonoBehaviour {
    public float switchSpeed = 5f;
    public Color offColor;
    public Color onColor;

    private Compiler2 compiler2;
    private Compiler compiler;
    private GameObject toggleBackground;
    private Image backgroundColor;
    private GameObject toggleHandle;
    private RectTransform handlePos;

    private Vector2 originPos;
    private Vector2 offPos;
    private Vector2 onPos;

    private bool isOn = false;

    // Start is called before the first frame update
    void Start() {
        compiler2 = GameObject.FindGameObjectWithTag("compiler").GetComponent<Compiler2>();
        compiler = GameObject.FindGameObjectWithTag("compiler").GetComponent<Compiler>();
        originPos = this.GetComponent<RectTransform>().anchoredPosition;
        offPos = new Vector2(originPos.x - 155, 0);
        onPos = new Vector2(originPos.x + 155, 0);
        toggleBackground = this.transform.Find("ToggleBackground").gameObject;
        backgroundColor = toggleBackground.GetComponent<Image>();
        toggleHandle = this.transform.Find("ToggleHandle").gameObject;
        handlePos = toggleHandle.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update() {
        if (isOn) {
            handlePos.anchoredPosition = Vector2.Lerp(handlePos.anchoredPosition, onPos, Time.deltaTime * switchSpeed);
            backgroundColor.color = Color.Lerp(backgroundColor.color, onColor, Time.deltaTime * switchSpeed);
        } else {
            handlePos.anchoredPosition = Vector2.Lerp(handlePos.anchoredPosition, offPos, Time.deltaTime * switchSpeed);
            backgroundColor.color = Color.Lerp(backgroundColor.color, offColor, Time.deltaTime * switchSpeed);
        }
    }

    public void OnMouseDown() {
        if (isOn) {
            compiler.ResetView();
            if (compiler2 != null)
                compiler2.ResetView();
            isOn = false;
        } else {
            compiler.Compiling();
            if (compiler2 != null)
                compiler2.Compiling();
            isOn = true;
        }
    }
}