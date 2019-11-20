using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubFlagLoading : MonoBehaviour {
    public SubFlagLoading otherFlag = null;
    private Slider slider;
    private Vector2 headPos;
    private Vector2 targetScale;
    private Color targetColor;
    private Color originColor;
    private Image fill;
    private bool isTriggerOn = false;
    private bool conquerStart = false;

    // Start is called before the first frame update
    void Start() {
        slider = this.transform.GetChild(0).GetChild(0).GetComponent<Slider>();
        fill = slider.transform.Find("Fill Area").GetChild(0).GetComponent<Image>();
        originColor = fill.color;
        headPos = this.transform.position;
        headPos.y += 0.8f;
        slider.transform.position = headPos;
        targetScale = Vector2.zero;
        targetColor = new Color(0, 170, 0);
    }

    // Update is called once per frame
    void Update() {
        if (otherFlag != null) {
            if (otherFlag.GetIsTriggerOn() && isTriggerOn)
                conquerStart = true;
            else
                conquerStart = false;
        } else {
            conquerStart = isTriggerOn;
        }

        if (conquerStart) {
            targetScale.x = 0.01f;
            targetScale.y = 0.01f;

            slider.transform.localScale = Vector2.Lerp(slider.transform.localScale, targetScale, Time.deltaTime * 6f);

            if (slider.transform.localScale.y > 0.009f) {
                slider.transform.localScale = targetScale;
            }

            slider.value = Mathf.Lerp(slider.value, 1f, Time.deltaTime * 3f);
            if (slider.value > 0.99f)
                slider.value = 1;
        } else {
            if (slider.value != 1) {
                slider.value = 0;
            }
            targetScale.x = 0.01f;
            targetScale.y = 0f;

            if (slider.value == 0)
                slider.transform.localScale = Vector2.Lerp(slider.transform.localScale, targetScale, Time.deltaTime * 6f);
            else
                slider.transform.localScale = Vector2.Lerp(slider.transform.localScale, targetScale, Time.deltaTime * 2f);

            if (slider.transform.localScale.y < 0.001f) {
                slider.transform.localScale = targetScale;
            }
        }
        if (slider.value == 1) {
            fill.color = Color.Lerp(fill.color, targetColor, Time.deltaTime);
        }
        if (slider.value == 0) {
            fill.color = originColor;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Contains("Player")) {
            isTriggerOn = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag.Contains("Player")) {
            isTriggerOn = false;
        }
    }

    public bool GetIsTriggerOn() {
        return isTriggerOn;
    }
    public float GetSliderValue() {
        return slider.value;
    }

    public void SetZeroSliderValue() {
        slider.value = 0f;
    }
}