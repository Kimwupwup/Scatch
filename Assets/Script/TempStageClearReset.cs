using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TempStageClearReset : MonoBehaviour {
    public GameObject canvas;
    private Button btn;

    // Start is called before the first frame update
    void Start() {
        btn = this.GetComponent<Button>();
        if (btn.name.Contains("Reset"))
            btn.onClick.AddListener(BtnOnClick);
        else
            btn.onClick.AddListener(CheatClick);
    }

    public void BtnOnClick() {
        canvas.GetComponent<StageSaveAndLoad>().StageClearReset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CheatClick() {
        canvas.GetComponent<StageSaveAndLoad>().StageCheat();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}