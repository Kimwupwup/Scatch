using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour {
    public float speed = 1;

    private GameObject menuPanel;
    private GameObject codePanel;
    private GameObject pausePanel;
    private GameObject errorPanel;
    private GameObject settingPanel;
    private GameObject exitPanel;
    private GameObject InfoPanel;

    private float movePosition;
    private float prevPosition;

    private Vector3 posPausePanel;
    private Vector3 posErrorPanel;
    private Vector3 posMenuPanel;
    private Vector2 targetPos;

    private float menuPanelWidth;
    private float codePanelHeight;

    private bool isSetMenu = false;
    private bool isSetCodePanel = false;
    private bool isSetViewPanel = false;
    private bool isTyping = false;
    private bool isSlideClick = false;
    private bool isInfoMenu = false;

    void Awake() {
        pausePanel = GameObject.FindGameObjectWithTag("pausePanel");
        exitPanel = GameObject.FindGameObjectWithTag("exitPanel");
        posPausePanel = pausePanel.transform.position;
        InfoPanel = GameObject.Find("Canvas").transform.Find("Information Panel").gameObject;
        settingPanel = GameObject.FindGameObjectWithTag("settingPanel");

        if (SceneManager.GetActiveScene().name != "stage_map_update") {            
            errorPanel = GameObject.FindGameObjectWithTag("errorPanel");
            menuPanel = GameObject.FindGameObjectWithTag("menuPanel");
            codePanel = GameObject.FindGameObjectWithTag("codePanel");
            posMenuPanel = menuPanel.transform.position;
            posErrorPanel = errorPanel.transform.position;
        }

        //pausePanel.SetActive(false);
    }

    void Start() {
        if (SceneManager.GetActiveScene().name == "stage_map_update")return;
        menuPanelWidth = menuPanel.GetComponent<RectTransform>().sizeDelta.x;
        codePanelHeight = codePanel.GetComponent<RectTransform>().sizeDelta.y;
    }

    void FixedUpdate() {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (InfoPanel != null)
                isInfoMenu = InfoPanel.activeSelf;
            if (isInfoMenu) {
                InfoPanel.SetActive(false);
            } else {
                if (pausePanel.GetComponent<RectTransform>().anchoredPosition == Vector2.zero) {
                    pausePanel.GetComponent<RectTransform>().position = posPausePanel;
                } else {
                    targetPos.x = 0;
                    targetPos.y = 0;
                    pausePanel.GetComponent<RectTransform>().anchoredPosition = targetPos;
                    exitPanel.GetComponent<RectTransform>().position = posPausePanel;
                    settingPanel.GetComponent<RectTransform>().position = posPausePanel;
                    GameObject.FindGameObjectWithTag("viewColor").GetComponent<SetColor>().ResetValue();
                    if (SceneManager.GetActiveScene().name == "stage_map_update")return;
                    GameObject.FindGameObjectWithTag("codeColor").GetComponent<SetColor>().ResetValue();
                }
            }

            //pausePanel.SetActive(true);
        }

        if (SceneManager.GetActiveScene().name == "stage_map_update")return;

        // 메뉴창 켜기
        if (isSetMenu == true) {

            targetPos.x = (menuPanelWidth / 2) * Screen.height / 2960f;
            targetPos.y = menuPanel.transform.position.y;

            menuPanel.transform.position =
                Vector2.Lerp(menuPanel.transform.position,
                    targetPos,
                    Time.deltaTime * speed);

            if (menuPanel.transform.position.x >
                (menuPanelWidth / 2) * Screen.height / 2960f - 5f)
                menuPanel.transform.position = targetPos;
        }

        // 메뉴창 끄기
        if (isSetMenu == false) {

            targetPos.x = -(menuPanelWidth / 2) * Screen.height / 2960f;
            targetPos.y = menuPanel.transform.position.y;

            menuPanel.transform.position =
                Vector2.Lerp(menuPanel.transform.position,
                    targetPos,
                    Time.deltaTime * speed);

            if (menuPanel.transform.position.x < -(menuPanelWidth / 2) * Screen.height / 2960f + 5f)
                menuPanel.transform.position = targetPos;
        }

        // 코드창 키우기(타이핑중일때)
        if (isSetCodePanel == true && isTyping == true) {

            targetPos.x = codePanel.transform.position.x;
            targetPos.y = Screen.height / 2 - movePosition;

            codePanel.transform.position =
                Vector3.Lerp(codePanel.transform.position,
                    targetPos,
                    Time.deltaTime * speed);

            if (codePanel.transform.position.y > Screen.height / 2 - movePosition - 5f && codePanel.transform.position.y < Screen.height / 2 - movePosition + 5f)
                codePanel.transform.position = targetPos;
        }

        // 코드창 키우기
        if (isSetCodePanel == true && isTyping == false) {

            targetPos.x = codePanel.transform.position.x;
            targetPos.y = Screen.height / 3;

            codePanel.transform.position =
                Vector3.Lerp(codePanel.transform.position,
                    targetPos,
                    Time.deltaTime * speed);

            if (codePanel.transform.position.y > Screen.height / 3 - 5f && codePanel.transform.position.y < Screen.height / 3 + 5f)
                codePanel.transform.position = targetPos;
        }

        // 초기 상태
        if (isSetCodePanel == false && isSetViewPanel == false) {

            targetPos.x = codePanel.transform.position.x;
            targetPos.y = 0;

            codePanel.transform.position =
                Vector3.Lerp(codePanel.transform.position,
                    targetPos,
                    Time.deltaTime * speed);

            if (codePanel.transform.position.y < 0 + 5f && codePanel.transform.position.y > 0 - 5f)
                codePanel.transform.position = targetPos;
        }

        // 뷰창 키우기
        if (isSetViewPanel == true) {

            targetPos.x = codePanel.transform.position.x;
            targetPos.y = -Screen.height / 3;

            codePanel.transform.position =
                Vector3.Lerp(codePanel.transform.position,
                    targetPos,
                    Time.deltaTime * speed);

            if (codePanel.transform.position.y < -Screen.height / 3 + 5f)
                codePanel.transform.position = targetPos;
        }
    }

    public bool GetMenuPanel() {
        return isSetMenu;
    }

    public void SetMenuPanel(bool b) {
        isSetMenu = b;
    }

    public void ResetScreen() {
        if (prevPosition < 10) {
            isSetCodePanel = false;
        } else {
            isSetCodePanel = true;
        }

        isSetViewPanel = false;
        isTyping = false;
        movePosition = 0;
    }

    public void OnlyCodePanel(GameObject btn) {
        isSlideClick = false;
        isSetMenu = false;
        isSetCodePanel = true;
        isTyping = true;
        movePosition = btn.transform.position.y - (Screen.height / 2);
        Debug.Log(codePanel.name);
        prevPosition = codePanel.transform.position.y;
        if (codePanel.transform.position.y < 10) {
            movePosition += 980 - (2960 - Screen.height) / 3;
        }
        //Debug.Log(codePanel.transform.position.y);
    }

    public void TurnOnOffMenu() {
        if (!isSlideClick) {
            isSlideClick = true;

            // 메뉴창, 코드창 키우기(뷰창 줄이기)
            isSetMenu = true;
            isSetCodePanel = true;
            isSetViewPanel = false;
        } else {
            isSlideClick = false;

            // 이미 메뉴창이 꺼져있을 경우
            if (isSetMenu == false) {
                // 뷰창 키우기(코드창 줄이기)
                if (isSetViewPanel == false) {
                    isSetViewPanel = true;
                    isSetCodePanel = false;
                } else { // 원래 상태로
                    isSetViewPanel = false;
                    isSetCodePanel = false;
                }
            } else { // 원래 상태로 (메뉴창 끄기)
                isSetMenu = false;
                isSetCodePanel = false;
            }
        }
    }

    public void TurnOnMenu() {

        // 메뉴창, 코드창 키우기(뷰창 줄이기)
        isSetMenu = true;
        isSetCodePanel = true;
        isSetViewPanel = false;
    }

    public void TurnOffMenu() {
        isSlideClick = false;
        // 이미 메뉴창이 꺼져있을 경우
        if (isSetMenu == false) {

            // 뷰창 키우기(코드창 줄이기)
            if (isSetViewPanel == false) {
                isSetViewPanel = true;
                isSetCodePanel = false;
            } else { // 원래 상태로
                isSetViewPanel = false;
                isSetCodePanel = false;
            }
        } else { // 원래 상태로 (메뉴창 끄기)
            isSetMenu = false;
            isSetCodePanel = false;
        }
    }
    public void PlayButtonOnClick() {
        isSlideClick = false;
        isSetMenu = false;
        isSetCodePanel = false;
    }
    public void SizeUpDownCodePanel() {

        // 코드창 키우기(뷰창 줄이기)
        if (isSetCodePanel == false) {
            isSetCodePanel = true;
            isSetViewPanel = false;
        } else { // 원래 상태로
            isSetCodePanel = false;
            isSetViewPanel = false;
        }
    }

    public void IsExit() {
        targetPos.x = 0;
        targetPos.y = 0;
        exitPanel.GetComponent<RectTransform>().anchoredPosition = targetPos;
        pausePanel.GetComponent<RectTransform>().position = posPausePanel;
    }

    public void CancelExit() {
        exitPanel.GetComponent<RectTransform>().position = posPausePanel;
    }

    public void IsSetting() {
        targetPos.x = 0;
        targetPos.y = 0;
        settingPanel.GetComponent<RectTransform>().anchoredPosition = targetPos;
        pausePanel.GetComponent<RectTransform>().position = posPausePanel;
    }

    public void CancelSetting() {
        settingPanel.GetComponent<RectTransform>().position = posPausePanel;
        GameObject.FindGameObjectWithTag("viewColor").GetComponent<SetColor>().ResetValue();
        if (SceneManager.GetActiveScene().name == "stage_map_update")return;
        GameObject.FindGameObjectWithTag("codeColor").GetComponent<SetColor>().ResetValue();
    }

    public void IsPause() {
        targetPos.x = 0;
        targetPos.y = 0;
        pausePanel.GetComponent<RectTransform>().anchoredPosition = targetPos;
    }

    public void ConfirmExit() {
        Application.Quit();
    }

    public void CancelPause() {
        pausePanel.GetComponent<RectTransform>().position = posPausePanel;
    }

    public void ResetErrorMessage() {
        errorPanel.GetComponent<RectTransform>().position = posErrorPanel;
    }
}