using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resolution : MonoBehaviour
{
    private int screenSize;
    private Toggle fhd;
    private Toggle fhdPlus;
    private Toggle wqhdPlus;

    private bool isAwake = true;

    void Awake() {
        isAwake = true;
    }

    void Start() {
        screenSize = Screen.width + Screen.height;

        fhd = transform.GetChild(0).GetChild(0).GetComponent<Toggle>();
        fhdPlus = transform.GetChild(0).GetChild(1).GetComponent<Toggle>();
        wqhdPlus = transform.GetChild(0).GetChild(2).GetComponent<Toggle>();

        if (screenSize == 3000) {
            fhd.isOn = true;
            Debug.Log(screenSize);
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
        Screen.SetResolution(1080, 1920, true);
        Debug.Log(Screen.width + " " + Screen.height);
    }
    public void SetFHDPlus() {
        if (isAwake == true) {
            return;
        }
        if (fhdPlus.isOn == false) {
            return;
        }
        Screen.SetResolution(1080, 2220, true);
        Debug.Log(Screen.width + " " + Screen.height);
    }
    public void SetWQHDPlus() {
        if (isAwake == true) {
            return;
        }
        if (wqhdPlus.isOn == false) {
            return;
        }
        Screen.SetResolution(1440, 2960, true);
        Debug.Log(Screen.width + " " + Screen.height);
    }
}
