using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NextScene : MonoBehaviour
{
    private SceneFadeInOut sceneAnim;
    private void Start() {
        sceneAnim = GameObject.FindGameObjectWithTag("Fader").GetComponent<SceneFadeInOut>();
    }
    public void MoveMainScene() {
        sceneAnim.OutStartFadeAnim("stage_map_update");
        //SceneManager.LoadScene("stage_map_update");        
    }
}
