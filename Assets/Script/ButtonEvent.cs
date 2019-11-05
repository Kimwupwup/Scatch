using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEvent : MonoBehaviour
{
    public void Reset()
    {
        Compiler compiler = GameObject.FindGameObjectWithTag("compiler").GetComponent<Compiler>();
        compiler.ResetView();
        Time.timeScale = 1;
    }

    public void NextStage()     //Scenes in Build 순서 상 다음 순서의 씬으로 이동(즉, 나중에 Stage 순서대로 정리 필요)
    {
        Scene scene = SceneManager.GetActiveScene();
        int curSecene = scene.buildIndex;
        int nextScene = curSecene + 1;
        SceneManager.LoadScene(nextScene);
        Time.timeScale = 1;
    }

    public void StageMap()  // Stage Map1 창으로 이동(임시)
    {
        SceneManager.LoadScene("stage_map_01");
        Time.timeScale = 1;
    }

    public void GoStageMapLeft() //Stage Map 이전 창으로 이동
    {
        Scene scene = SceneManager.GetActiveScene();
        int curSecene = scene.buildIndex;
        int nextScene = curSecene - 1;
        SceneManager.LoadScene(nextScene);
    }

    public void GoStageMapRight()   //Stage Map 다음 창으로 이동
    {
        Scene scene = SceneManager.GetActiveScene();
        int curSecene = scene.buildIndex;
        int nextScene = curSecene + 1;
        SceneManager.LoadScene(nextScene);
    }
}
