using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageButton : MonoBehaviour
{
    private string btnString;
    private SceneFadeInOut sceneAnim;
    public GameObject animObj;

    private void Awake() {
        sceneAnim = animObj.GetComponent<SceneFadeInOut>();
    }
    // Start is called before the first frame update
    void Start()
    {
        btnString = this.name;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(btnString);
        Time.timeScale = 1;
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        sceneAnim.OutStartFadeAnim(btnString);
    }

    //public void OnTriggerExit2D(Collider2D collision) {
    //    SceneManager.LoadScene(btnString);
    //}
}
