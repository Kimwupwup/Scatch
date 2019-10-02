using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public float speed = 1;

    private GameObject menuPanel;
    private GameObject codePanel;
    private GameObject pausePanel;
    private GameObject errorPanel;
    private GameObject settingPanel;
    private GameObject exitPanel;

    private float movePosition;
    private float prevPosition;

    private Vector3 posPausePanel;
    private Vector3 posErrorPanel;
    private Vector3 posMenuPanel;

    private float menuPanelWidth;
    private float codePanelHeight;

    private bool isSetMenu = false;
    private bool isSetCodePanel = false;
    private bool isSetViewPanel = false;
    private bool isTyping = false;

    void Awake() {
        errorPanel = GameObject.FindGameObjectWithTag("errorPanel");
        pausePanel = GameObject.FindGameObjectWithTag("pausePanel");
        settingPanel = GameObject.FindGameObjectWithTag("settingPanel");
        exitPanel = GameObject.FindGameObjectWithTag("exitPanel");
        menuPanel = GameObject.FindGameObjectWithTag("menuPanel");
        codePanel = GameObject.FindGameObjectWithTag("codePanel");

        posMenuPanel = menuPanel.transform.position;
        posPausePanel = pausePanel.transform.position;
        posErrorPanel = errorPanel.transform.position;

        //pausePanel.SetActive(false);
    }

    void Start() {
        
        menuPanelWidth = menuPanel.GetComponent<RectTransform>().sizeDelta.x;
        codePanelHeight = codePanel.GetComponent<RectTransform>().sizeDelta.y;
    }
    void Update() {

        // 메뉴창 켜기
        if (isSetMenu == true) {
            menuPanel.transform.position = Vector2.Lerp(menuPanel.transform.position, new Vector2((menuPanelWidth / 2) * Screen.height / 2960f, menuPanel.transform.position.y), Time.deltaTime * speed);

            //menuPanel.transform.position = Vector2.Lerp(menuPanel.transform.position, new Vector2(menuPanelWidth / 2, menuPanel.transform.position.y), Time.deltaTime * speed);
        }

        // 메뉴창 끄기
        if (isSetMenu == false) {
            menuPanel.transform.position = Vector2.Lerp(menuPanel.transform.position, new Vector2(-(menuPanelWidth / 2/* + 60*/) * Screen.height / 2960f, menuPanel.transform.position.y), Time.deltaTime * speed);

            //menuPanel.transform.position = Vector2.Lerp(menuPanel.transform.position, new Vector2(-menuPanelWidth / 2, menuPanel.transform.position.y), Time.deltaTime * speed);
        }

        // 코드창 키우기(타이핑중일때)
        if (isSetCodePanel == true && isTyping == true) {
            codePanel.transform.position = Vector3.Lerp(codePanel.transform.position, 
                new Vector2(codePanel.transform.position.x, Screen.height/2 - movePosition), 
                Time.deltaTime * speed);

            //GameObject.FindGameObjectWithTag("ground").transform.position =
            //    Vector3.Lerp(GameObject.FindGameObjectWithTag("ground").transform.position,
            //    new Vector3(0, -5 + (10 * ((Screen.height / 2 - movePosition) / Screen.height)), 0), Time.deltaTime * speed);
        }

        // 코드창 키우기
        if (isSetCodePanel == true && isTyping == false) {
            codePanel.transform.position = Vector3.Lerp(codePanel.transform.position, 
                new Vector2(codePanel.transform.position.x, Screen.height / 3), 
                Time.deltaTime * speed);

            //GameObject.FindGameObjectWithTag("ground").transform.position =
            //    Vector3.Lerp(GameObject.FindGameObjectWithTag("ground").transform.position,
            //    new Vector3(0, -1.67f, 0), Time.deltaTime * speed);
        }

        // 초기 상태
        if (isSetCodePanel == false && isSetViewPanel == false) {
            codePanel.transform.position = Vector3.Lerp(codePanel.transform.position, new Vector2(codePanel.transform.position.x, 0), Time.deltaTime * speed);
            //GameObject.FindGameObjectWithTag("ground").transform.position =
            //    Vector3.Lerp(GameObject.FindGameObjectWithTag("ground").transform.position,
            //    new Vector3(0, -5, 0), Time.deltaTime * speed);
        }

        // 뷰창 키우기
        if (isSetViewPanel == true) {
            codePanel.transform.position = Vector3.Lerp(codePanel.transform.position, new Vector2(codePanel.transform.position.x, -Screen.height / 3), Time.deltaTime * speed);
            //GameObject.FindGameObjectWithTag("ground").transform.position =
            //    Vector3.Lerp(GameObject.FindGameObjectWithTag("ground").transform.position,
            //    new Vector3(0, -8.33f, 0), Time.deltaTime * speed);
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (pausePanel.GetComponent<RectTransform>().anchoredPosition == Vector2.zero) {
                pausePanel.GetComponent<RectTransform>().position = posPausePanel;
            } else {
                pausePanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                settingPanel.GetComponent<RectTransform>().position = posPausePanel;
                exitPanel.GetComponent<RectTransform>().position = posPausePanel;
                GameObject.FindGameObjectWithTag("viewColor").GetComponent<SetColor>().ResetValue();
                GameObject.FindGameObjectWithTag("codeColor").GetComponent<SetColor>().ResetValue();

            }

            //pausePanel.SetActive(true);
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
        isSetMenu = false;
        isSetCodePanel = true;
        isTyping = true;
        movePosition = btn.transform.position.y - (2960 / 2);
        prevPosition = codePanel.transform.position.y;
        if (codePanel.transform.position.y < 10) {
            movePosition += 980;
        }
        //Debug.Log(codePanel.transform.position.y);
    }

    public void TurnOnMenu() {

        // 메뉴창, 코드창 키우기(뷰창 줄이기)
        isSetMenu = true;
        isSetCodePanel = true;
        isSetViewPanel = false;
    }

    public void TurnOffMenu() {

        // 이미 메뉴창이 꺼져있을 경우
        if (isSetMenu == false) {
            
            // 뷰창 키우기(코드창 줄이기)
            if (isSetViewPanel == false) {
                isSetViewPanel = true;
                isSetCodePanel = false;
            } else {    // 원래 상태로
                isSetViewPanel = false;
                isSetCodePanel = false;
            }
        } else {    // 원래 상태로 (메뉴창 끄기)
            isSetMenu = false;
            isSetCodePanel = false;
        }
    }

    public void SizeUpDownCodePanel() {

        // 코드창 키우기(뷰창 줄이기)
        if (isSetCodePanel == false) {
            isSetCodePanel = true;
            isSetViewPanel = false;
        } else {    // 원래 상태로
            isSetCodePanel = false;
            isSetViewPanel = false;
        }
    }

    public void IsExit() {
        exitPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        pausePanel.GetComponent<RectTransform>().position = posPausePanel;
    }

    public void CancelExit() {
        exitPanel.GetComponent<RectTransform>().position = posPausePanel;
    }

    public void IsSetting() {
        settingPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        pausePanel.GetComponent<RectTransform>().position = posPausePanel;
    }

    public void CancelSetting() {
        settingPanel.GetComponent<RectTransform>().position = posPausePanel;
        GameObject.FindGameObjectWithTag("viewColor").GetComponent<SetColor>().ResetValue();
        GameObject.FindGameObjectWithTag("codeColor").GetComponent<SetColor>().ResetValue();
    }

    public void IsPause() {
        pausePanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        //pausePanel.SetActive(true);
    }

    public void ConfirmExit() {
        Application.Quit();
    }

    public void CancelPause() {
        pausePanel.GetComponent<RectTransform>().position = posPausePanel;
        //pausePanel.SetActive(false);
    }

    public void ResetErrorMessage() {
        errorPanel.GetComponent<RectTransform>().position = posErrorPanel;
    }

    public void BtnBlue() {
        GameObject.FindGameObjectWithTag("menuPanel1").transform.localEulerAngles = new Vector3(0, 90, 0);
        GameObject.FindGameObjectWithTag("menuPanel2").transform.localEulerAngles = new Vector3(0, 90, 0);
    }

    public void BtnRed() {
        GameObject.FindGameObjectWithTag("menuPanel1").transform.localEulerAngles = new Vector3(0, 90, 0);
        GameObject.FindGameObjectWithTag("menuPanel2").transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    public void BtnYellow() {
        GameObject.FindGameObjectWithTag("menuPanel1").transform.localEulerAngles = new Vector3(0, 0, 0);
        GameObject.FindGameObjectWithTag("menuPanel2").transform.localEulerAngles = new Vector3(0, 90, 0);
    }
}
