using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSaveAndLoad : MonoBehaviour {
    Scene scene;
    public int curScene;
    public int quitScene;
    private GameObject codePanel;
    private Camera came;
    Color backgroundColor = new Color();

    void Awake() {
        codePanel = GameObject.FindGameObjectWithTag("codePanel");
        came = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Load();
    }

    void Start() {
        scene = SceneManager.GetActiveScene(); // 현재 씬 가져옴
        string number = Regex.Replace(scene.name, @"\D", ""); // stage1 에서 1만 남기고 없앰

        // 숫자가 없는 경우 아무것도 하지 않는다.
        if (int.TryParse(number, out int ret) == true) {
            curScene = int.Parse(number); // 현재 씬의 빌드넘버
            quitScene = int.Parse(number);
            if (SceneManager.GetActiveScene().name.Contains("tutorial"))
                quitScene = 0;
            PlayerPrefs.SetInt("Quit", quitScene);
        }
    }

    public void Save() {
        if (curScene + 1 > PlayerPrefs.GetInt("Num"))
            PlayerPrefs.SetInt("Num", curScene + 1);
    }

    public void Load() {

        if (PlayerPrefs.HasKey("Num") || PlayerPrefs.GetInt("Num") != 0) {
            curScene = PlayerPrefs.GetInt("Num");
        } else {
            PlayerPrefs.SetInt("Num", 1);
            curScene = 1;
        }

        if (PlayerPrefs.HasKey("Quit")) {
            quitScene = PlayerPrefs.GetInt("Quit");
        } else {
            PlayerPrefs.SetInt("Quit", 0);
            quitScene = 0;
        }

        if (PlayerPrefs.HasKey("VR")) {
            backgroundColor.r = PlayerPrefs.GetFloat("VR");
            backgroundColor.g = PlayerPrefs.GetFloat("VG");
            backgroundColor.b = PlayerPrefs.GetFloat("VB");
            backgroundColor.a = 255f;
            came.backgroundColor = backgroundColor;
        } else {
            PlayerPrefs.SetFloat("VR", came.backgroundColor.r);
            PlayerPrefs.SetFloat("VG", came.backgroundColor.g);
            PlayerPrefs.SetFloat("VB", came.backgroundColor.b);
        }

        if (codePanel != null) {
            Image codePanelColor = codePanel.GetComponent<Image>();
            if (PlayerPrefs.HasKey("CR")) {
                backgroundColor.r = PlayerPrefs.GetFloat("CR");
                backgroundColor.g = PlayerPrefs.GetFloat("CG");
                backgroundColor.b = PlayerPrefs.GetFloat("CB");
                backgroundColor.a = 180f / 256f;
                codePanelColor.color = backgroundColor;
            } else {
                PlayerPrefs.SetFloat("CR", codePanelColor.color.r);
                PlayerPrefs.SetFloat("CG", codePanelColor.color.g);
                PlayerPrefs.SetFloat("CB", codePanelColor.color.b);
            }
        }
    }

    public void StageClearReset() {
        PlayerPrefs.SetInt("Num", 1);
        PlayerPrefs.SetInt("Quit", 0);
    }

    public void StageCheat() {
        PlayerPrefs.SetInt("Num", 23);
    }
}