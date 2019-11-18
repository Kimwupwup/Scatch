using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsTab : MonoBehaviour {
    public Button btnDisplay;
    public Button btnSound;
    private GameObject displayPanel;
    private GameObject soundPanel;
    private Vector3 ninety = new Vector3(0f, 90f, 0f);
    private Vector3 zero = new Vector3(0f, 0f, 0f);
    // Start is called before the first frame update
    void Start() {
        btnDisplay.onClick.AddListener(BtnDisplayOnClick);
        btnSound.onClick.AddListener(BtnSoundOnClick);

        displayPanel = transform.GetChild(0).Find("Display page").gameObject;
        soundPanel = transform.GetChild(0).Find("Sound page").gameObject;
    }

    void BtnDisplayOnClick() {
        displayPanel.transform.eulerAngles = zero;
        soundPanel.transform.eulerAngles = ninety;
        
    }

    void BtnSoundOnClick() {
        displayPanel.transform.eulerAngles = ninety;
        soundPanel.transform.eulerAngles = zero;
    }
}
