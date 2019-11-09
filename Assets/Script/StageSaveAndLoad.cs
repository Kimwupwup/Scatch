using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class StageSaveAndLoad : MonoBehaviour {
    Scene scene;
    public int curScene;
    public int quitScene;

    void Awake() {
        Load();
    }

    void Start() {
        scene = SceneManager.GetActiveScene();                  // 현재 씬 가져옴
        string number = Regex.Replace(scene.name, @"\D", "");   // stage1 에서 1만 남기고 없앰

        // 숫자가 없는 경우 아무것도 하지 않는다.
        if (int.TryParse(number, out int ret) == true) {
            curScene = int.Parse(number);                       // 현재 씬의 빌드넘버
            quitScene = int.Parse(number);
            PlayerPrefs.SetInt("Quit", quitScene);
        }
    }

    public void Save() {
        if (curScene + 1> PlayerPrefs.GetInt("Num"))
            PlayerPrefs.SetInt("Num", curScene + 1);
    }

    public void Load() {
        if (PlayerPrefs.HasKey("Num") || PlayerPrefs.GetInt("Num") != 0) {
            curScene = PlayerPrefs.GetInt("Num");
        } else {
            PlayerPrefs.SetInt("Num", 1);
            curScene = 1;
        }
            
        if (PlayerPrefs.HasKey("Quit") || PlayerPrefs.GetInt("Quit") != 0) {
            quitScene = PlayerPrefs.GetInt("Quit");
        } else {
            PlayerPrefs.SetInt("Quit", 1);
            quitScene = 1;
        }
    }

    public void StageClearReset() {
        PlayerPrefs.SetInt("Num", 1);
    }
}
