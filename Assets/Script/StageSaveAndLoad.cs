using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSaveAndLoad : MonoBehaviour
{
    Scene scene;
    public int curScene;

    void Awake()
    {
    //    PlayerPrefs.SetInt("Num", 0);    //0으로 초기화
        Load();
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
            curScene = PlayerPrefs.GetInt("Num");
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
