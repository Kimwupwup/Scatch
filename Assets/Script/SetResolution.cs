using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetResolution : MonoBehaviour
{
    private int screenSize;
    private Toggle fhd;
    private Toggle fhdPlus;
    private Toggle wqhdPlus;

    private int tempWidth;
    private int tempHeight;

    private bool isAwake = true;

    void Awake() {
        isAwake = true;
    }

    void Start() {
        screenSize = Screen.width + Screen.height;
        fhd = transform.GetChild(0).GetChild(0).GetComponent<Toggle>();
        fhdPlus = transform.GetChild(0).GetChild(1).GetComponent<Toggle>();
        wqhdPlus = transform.GetChild(0).GetChild(2).GetComponent<Toggle>();

        if (screenSize == 3000 || screenSize == 3120) {
            fhd.isOn = true;
        } else if (screenSize == 3300) {
            fhdPlus.isOn = true;
        } else if (screenSize == 4400) {
            wqhdPlus.isOn = true;
        }
        isAwake = false;
    }

    public void SetFHD() {
        if (isAwake == true) {
            return;
        }
        if (fhd.isOn == false) {
            return;
        }
        tempWidth = 1080;
        tempHeight = 1920;
    }

    public void SetFHDPlus() {
        if (isAwake == true) {
            return;
        }
        if (fhdPlus.isOn == false) {
            return;
        }
        tempWidth = 1080;
        tempHeight = 2220;
    }

    public void SetWQHDPlus() {
        if (isAwake == true) {
            return;
        }
        if (wqhdPlus.isOn == false) {
            return;
        }
        tempWidth = 1440;
        tempHeight = 2960;
    }

    public void ApplyValue() {
        Screen.SetResolution(tempWidth, tempHeight, true);
    }
}
