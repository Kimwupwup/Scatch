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

    public void NextStage()
    {
        Scene scene = SceneManager.GetActiveScene();
        int curSecene = scene.buildIndex;
        int nextScene = curSecene + 1;
        SceneManager.LoadScene(nextScene);
        Compiler compiler = GameObject.FindGameObjectWithTag("compiler").GetComponent<Compiler>();
        compiler.ResetView();
        Time.timeScale = 1;
    }
}
