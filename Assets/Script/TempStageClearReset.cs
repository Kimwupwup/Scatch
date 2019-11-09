using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TempStageClearReset : MonoBehaviour
{
    public GameObject canvas;
    private Button btn;

    // Start is called before the first frame update
    void Start()
    {
        btn = this.GetComponent<Button>();
        btn.onClick.AddListener(BtnOnClick);
    }

    public void BtnOnClick() {
        canvas.GetComponent<StageSaveAndLoad>().StageClearReset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
