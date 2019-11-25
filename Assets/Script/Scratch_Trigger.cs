using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scratch_Trigger : MonoBehaviour {
    private GameObject failPanel;
    private GameObject clearPanel;
    private PlayToggle playToggle;
    private float timer = 0;
    private bool isFail = false;
    private bool isClear = false;

    // Start is called before the first frame update
    void Start() {
        playToggle = GameObject.FindGameObjectWithTag("compiler").transform.GetChild(0).GetComponent<PlayToggle>();
        failPanel = GameObject.FindGameObjectWithTag("canvas").transform.Find("fail").gameObject;
        clearPanel = GameObject.FindGameObjectWithTag("canvas").transform.Find("clear").gameObject;
    }

    void Update() {
        if (isFail) {
            timer += Time.deltaTime;
            if (timer >= 0.7f) {
                Time.timeScale = 0.1f;
                failPanel.SetActive(true);
                timer = 0;
            }
        }
        if (isClear) {
            timer += Time.deltaTime;
            if (timer >= 0.7f) {
                Time.timeScale = 0.1f;
                clearPanel.SetActive(true);
                timer = 0;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Fire") || other.gameObject.CompareTag("wall")) {
            if (clearPanel.activeSelf == false) {
                GameObject.FindGameObjectWithTag("compiler").GetComponent<Compiler>().SetFalseIsCompiled();
                if (GameObject.FindGameObjectWithTag("compiler").GetComponent<Compiler2>() != null)
                    GameObject.FindGameObjectWithTag("compiler").GetComponent<Compiler2>().SetFalseIsCompiled();
                transform.GetComponent<SpriteRenderer>().color = Color.red;
                transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                transform.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
                isFail = true;
                // Time.timeScale = 0.1f;
                // failPanel.SetActive(true);
            }
        } else if (other.gameObject.CompareTag("Flag")) {
            if (!SceneManager.GetActiveScene().name.Contains("tutorial"))
                GameObject.Find("Canvas").GetComponent<StageSaveAndLoad>().Save();
            if (failPanel.activeSelf == false) {
                isClear = true;
            }
        }
    }

    public void SetFailClear() {
        transform.GetComponent<SpriteRenderer>().color = Color.white;
        isFail = false;
        isClear = false;
        timer = 0;
    }
}