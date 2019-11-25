using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEvent : MonoBehaviour {
    private SceneFadeInOut currentSceneAnim;
    private Scratch_Trigger st;
    private Scratch_Trigger st2;
    private void Start() {
        currentSceneAnim = GameObject.FindGameObjectWithTag("Fader").GetComponent<SceneFadeInOut>();
        st = GameObject.FindGameObjectWithTag("Player").GetComponent<Scratch_Trigger>();
        if (GameObject.FindGameObjectWithTag("Player2") != null)
            st2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<Scratch_Trigger>();
    }

    public void Reset() {
        st.SetFailClear();
        if (st2 != null)
            st2.SetFailClear();
        PlayToggle toggle = GameObject.FindGameObjectWithTag("compiler").transform.GetChild(0).GetComponent<PlayToggle>();
        toggle.OnMouseDown();
        Time.timeScale = 1;
    }

    public void NextStage() //Scenes in Build 순서 상 다음 순서의 씬으로 이동(즉, 나중에 Stage 순서대로 정리 필요)
    {
        Scene scene = SceneManager.GetActiveScene();
        int curSecene = scene.buildIndex;
        int nextScene = curSecene + 1;

        currentSceneAnim.OutStartFadeAnim(nextScene);
        Time.timeScale = 1;
        // SceneManager.LoadScene(nextScene);
    }

    public void StageMap() // Stage Map1 창으로 이동(임시)
    {

        currentSceneAnim.OutStartFadeAnim("stage_map_update");
        Time.timeScale = 1;
        // SceneManager.LoadScene("stage_map_update");
    }

    public void GoStageMapLeft() //Stage Map 이전 창으로 이동
    {
        Scene scene = SceneManager.GetActiveScene();
        int curSecene = scene.buildIndex;
        int nextScene = curSecene - 1;
        SceneManager.LoadScene(nextScene);
    }

    public void GoStageMapRight() //Stage Map 다음 창으로 이동
    {
        Scene scene = SceneManager.GetActiveScene();
        int curSecene = scene.buildIndex;
        int nextScene = curSecene + 1;
        SceneManager.LoadScene(nextScene);
    }
}