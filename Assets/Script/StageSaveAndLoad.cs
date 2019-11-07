using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSaveAndLoad : MonoBehaviour
{    // Start is called before the first frame update
    Scene scene;
    private int curScene;
  //  private Text countText;

    void Awake()
    {
        Load();
    //    countText = GameObject.FindGameObjectWithTag("coinText").GetComponent<Text>();///////////////////
    //    countText.text = curScene.ToString();
    }

    void Start()
    {
        scene = SceneManager.GetActiveScene();        //현재 씬 가져옴
        curScene = scene.buildIndex;       //현재 씬의 빌드넘버
        
    }

    public void Save()
    {
        if(curScene+1 > PlayerPrefs.GetInt("Num"))
        PlayerPrefs.SetInt("Num", curScene + 1);
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("Num"))
        {
            curScene = PlayerPrefs.GetInt("Num");
            Debug.Log(curScene);
        //    PlayerPrefs.Save();
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
