using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFadeInOut : MonoBehaviour {
    private float FadeTime = 0.5f; // Fade효과 재생시간
    Image fadeImg;
    float start;
    float end;
    float time = 0f;
    private string sceneStr;
    private int sceneInt;
    public bool isPlaying = false;

    void Awake() {
        Time.timeScale = 1;
        fadeImg = GetComponent<Image>();
        InStartFadeAnim();
    }

    public void OutStartFadeAnim(string sceneName) {
        //중복재생방지
        if (isPlaying == true) {
            return;
        }
        Time.timeScale = 1;

        start = 0f;
        end = 1f;
        sceneStr = sceneName;
        StartCoroutine("fadeOutAnim");    //코루틴 실행
    }

    public void OutStartFadeAnim(int sceneName) {
        //중복재생방지
        if (isPlaying == true) {
            return;
        }
        Time.timeScale = 1;

        start = 0f;
        end = 1f;
        sceneInt = sceneName;
        StartCoroutine("fadeOutAnim");    //코루틴 실행
    }

    public void InStartFadeAnim() {
        //중복재생방지
        if (isPlaying == true) {
            return;
        }
        start = 1f;
        end = 0f;
        StartCoroutine("fadeInAnim");
    }

    IEnumerator fadeOutAnim() {
        isPlaying = true;
        Color fadecolor = fadeImg.color;
        time = 0f;

        while (fadecolor.a < 1f) {
            Time.timeScale = 1;
            time += Time.deltaTime / FadeTime;
            fadecolor.a = Mathf.Lerp(start, end, time);
            fadeImg.color = fadecolor;
            yield return null;
        }

        if (!string.IsNullOrEmpty(sceneStr))
            SceneManager.LoadScene(sceneStr);
        else if (sceneInt != 0 || string.IsNullOrEmpty(sceneStr)) 
            SceneManager.LoadScene(sceneInt);

        sceneStr = "";
        sceneInt = 0;
        isPlaying = false;
    }

    IEnumerator fadeInAnim() {
        isPlaying = true;
        Color fadecolor = fadeImg.color;
        time = 0f;

        while (fadecolor.a > 0f) {
            Time.timeScale = 1;
            time += Time.deltaTime / FadeTime;
            fadecolor.a = Mathf.Lerp(start, end, time);
            fadeImg.color = fadecolor;
            yield return null;
        }
        isPlaying = false;
    }
}
