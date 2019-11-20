using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalFlagLoading : MonoBehaviour {
    private Slider slider;
    private Vector2 headPos;
    private Vector2 targetScale;
    private Color targetColor;
    private Color originColor;
    private Image fill;
    private GameObject[] subFlags;
    private BoxCollider2D col;

    // Start is called before the first frame update
    void Start() {
        col = this.GetComponent<BoxCollider2D>();
        col.enabled = false;
        subFlags = GameObject.FindGameObjectsWithTag("SubFlag");
        slider = this.transform.GetChild(0).GetChild(0).GetComponent<Slider>();

        if (subFlags.Length == 0)
            slider.maxValue = 1;
        else
            slider.maxValue = subFlags.Length;

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
        if (subFlags.Length == 0) {
            slider.value = 1;
        } else {
            int cnt = 0;
            foreach (GameObject temp in subFlags) {
                if (temp.GetComponent<SubFlagLoading>().GetSliderValue() == 1) {
                    cnt++;
                }
            }
            slider.value = Mathf.Lerp(slider.value, cnt * 1f, Time.deltaTime * 8f);
            if (slider.value > cnt * 0.99f)
                slider.value = cnt * 1f;
        }
        if (slider.value == slider.maxValue) {
            fill.color = Color.Lerp(fill.color, targetColor, Time.deltaTime);
            col.enabled = true;
        } else {
            fill.color = originColor;
            col.enabled = false;
        }
    }
}