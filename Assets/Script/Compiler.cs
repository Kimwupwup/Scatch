using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compiler : MonoBehaviour {
    private List<GameObject> functions = new List<GameObject>();
    private List<int> loopSet = new List<int>();
    private List<int> loopIndex = new List<int>();
    private List<int> endLoopIndex = new List<int>();
    private List<int> ifIndex = new List<int>();
    private List<int> endIfIndex = new List<int>();
    private List<string> varName = new List<string>();
    private List<int> varValue = new List<int>();
    private GameObject[] subFlags;

    private bool isCompiled = false;
    private Rigidbody2D playerRig;
    private SpriteRenderer playerSprite;
    private Transform playerTransform;
    private Animator playerAn;
    private slaim_move jumpController;
    private GameObject failPanel;

    private bool playerFlip = false;
    private Vector3 playerOrginPos;
    private float targetPos;
    private bool isMoving = false;
    private int frameCount = 0;
    private int delayTime = 1;
    private int currentIndex = 0;

    private int cnt = -1;
    private int conditionCnt = -1;

    private int loopCnt = 0;
    public bool isResetView = false;
    public float moveSpeed = 1;
    private float jumpPower = 4.5f;
    private float timer = 0f;

    private void Start() {
        playerFlip = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().flipX;
        playerOrginPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        playerRig = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerSprite = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
        playerAn = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        jumpController = GameObject.FindGameObjectWithTag("Player").GetComponent<slaim_move>();
        failPanel = GameObject.FindGameObjectWithTag("canvas").transform.Find("fail").gameObject;
        subFlags = GameObject.FindGameObjectsWithTag("SubFlag");
    }

    // Update is called once per frame
    void Update() {
        if (isMoving) {
            timer += Time.deltaTime;
            if (timer > 3f) {
                failPanel.SetActive(true);
                Time.timeScale = 0;
            }
            run();
            return;
        }
        timer = 0f;
        frameCount++;
        if (frameCount % delayTime == 0) {
            if (isCompiled == true && currentIndex < functions.Count) {
                if (functions[currentIndex].name == "BtnMove(Clone)") {
                    int cnt = 0;
                    for (int i = currentIndex; i < functions.Count; i++) {
                        if (functions[i].name == "BtnMove(Clone)") {
                            FunctionMove(++cnt);
                            currentIndex++;
                        }
                        else {
                            break;
                        }
                    }
                    currentIndex--;
                }
                else if (functions[currentIndex].name == "BtnJump(Clone)") {
                    if (jumpController.getIsJump() || playerRig.velocity.y != 0) return;
                    int cnt = 0;
                    for (int i = currentIndex; i < functions.Count; i++) {
                        if (functions[i].name == "BtnJump(Clone)") {
                            cnt++;
                            currentIndex++;
                        }
                        else {
                            break;
                        }
                    }
                    FunctionJump(cnt);
                    currentIndex--;
                }
                else if (functions[currentIndex].name == "BtnRotate(Clone)") {
                    FunctionRotate();
                }
                else if (functions[currentIndex].name == "BtnLoop(Clone)") {
                    if (jumpController.getIsJump() || playerRig.velocity.y != 0) return;
                    FunctionLoop();
                }
                else if (functions[currentIndex].name == "BtnEndLoop(Clone)") {
                    FunctionEndLoop();
                }
                else if (functions[currentIndex].name == "BtnDelay(Clone)") {
                    FunctionDelay();
                }
                else if (functions[currentIndex].name == "BtnIf(Clone)") {
                    FunctionIf();
                }
                else if (functions[currentIndex].name == "BtnEndIf(Clone)") {
                    FunctionEndIf();
                }
                else if (functions[currentIndex].name == "BtnCnt=(Clone)") {
                    FunctionSetCnt();
                }
                else if (functions[currentIndex].name == "BtnCnt++(Clone)") {
                    FunctionIncreaseCnt();
                }
                else if (functions[currentIndex].name == "BtnBreak(Clone)") {
                    FunctionBreak();
                }
                else if (functions[currentIndex].name == "BtnVariable=(Clone)") {
                    FunctionSetVariable();
                }
                else if (functions[currentIndex].name == "BtnVariable++(Clone)") {
                    FunctionIncreaseValue();
                }
                currentIndex++;
                frameCount = 0;
            }
            else {
                isMoving = false;
                playerAn.SetBool("isMoving", false);
                currentIndex = 0;
                delayTime = 1;
                cnt = -1;
                conditionCnt = -1;
                isCompiled = false;
                loopIndex.Clear();
                endLoopIndex.Clear();
                ifIndex.Clear();
                endIfIndex.Clear();
                varName.Clear();
                varValue.Clear();
                functions.Clear();
                loopSet.Clear();
            }
        }

    }

    public void ResetView() {
        isMoving = false;
        playerAn.SetBool("isMoving", false);
        currentIndex = 0;
        delayTime = 1;
        cnt = -1;
        conditionCnt = -1;
        isCompiled = false;
        functions.Clear();
        loopIndex.Clear();
        endLoopIndex.Clear();
        ifIndex.Clear();
        endIfIndex.Clear();
        varName.Clear();
        varValue.Clear();
        loopSet.Clear();
        playerRig.velocity = Vector2.zero;
        playerRig.transform.position = playerOrginPos;
        playerRig.GetComponent<SpriteRenderer>().flipX = playerFlip;
        GameObject.Find("Canvas").transform.Find("fail").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("clear").gameObject.SetActive(false);
        foreach (GameObject temp in subFlags) {
            temp.GetComponent<SubFlagLoading>().SetZeroSliderValue();
        }
    }

    public void Compiling() {
        if (isCompiled == true) {
            isCompiled = false;
            return;
        }
        GameObject code = GameObject.FindGameObjectWithTag("codePanel").transform.GetChild(1).gameObject;
        List<GameObject> codesQueue = new List<GameObject>();
        GameObject temp;
        for (int i = 0; i < code.transform.childCount; i++) {
            codesQueue.Add(code.transform.GetChild(i).gameObject);
        }

        // Sort multiple code lines
        for (int i = 0; i < codesQueue.Count; i++) {
            for (int j = i + 1; j < codesQueue.Count; j++) {
                if (codesQueue[i].transform.position.x > codesQueue[j].transform.position.x) {
                    temp = codesQueue[i];
                    codesQueue[i] = codesQueue[j];
                    codesQueue[j] = temp;
                }
            }
        }

        loopCnt = 0;

        for (int i = 0; i < codesQueue.Count; i++) {
            code = codesQueue[i];
            while (true) {

                functions.Add(code);
                loopCnt++;

                if (code.name == "BtnLoop(Clone)") {
                    loopIndex.Add(functions.Count - 1);
                }
                if (code.name == "BtnEndLoop(Clone)") {
                    endLoopIndex.Add(functions.Count - 1);
                }
                if (code.name == "BtnIf(Clone)") {
                    ifIndex.Add(functions.Count - 1);
                    if (code.transform.childCount < 5) {
                        AlertError(2);
                        ResetView();
                        return;
                    }
                    string tempName = null;
                    bool isExist = false;

                    for (int j = 0; j < code.transform.childCount; j++) {
                        if (code.transform.GetChild(j).name.Contains("BtnVariable")) {
                            GameObject ifTemp = code.transform.GetChild(j).gameObject;
                            for (int k = 0; k < 2; k++) {
                                InputField inputTemp = ifTemp.transform.GetChild(k).GetComponent<InputField>();
                                if (string.IsNullOrEmpty(inputTemp.text))
                                    inputTemp.text = inputTemp.placeholder.GetComponent<Text>().text;
                                if (ifTemp.transform.GetChild(k).name.Contains("(1)"))
                                    tempName = inputTemp.text;
                            }
                        }
                    }
                    
                    if (tempName != null) {
                        for (int k = 0; k < varName.Count; k++) {
                            if (tempName == varName[k]) {
                                isExist = true;
                                break;
                            }
                        }
                        if (isExist == false) {
                            AlertError(0);
                            codesQueue.Clear();
                            ResetView();
                            return;
                        }
                    }
                    else {
                        if (cnt == -1) {
                            AlertError(0);
                            codesQueue.Clear();
                            ResetView();
                            return;
                        }
                    }
                }

                if (code.name == "BtnEndIf(Clone)") {
                    endIfIndex.Add(functions.Count - 1);
                }

                if (code.name == "BtnVariable=(Clone)") {
                    bool isExist = false;

                    if (string.IsNullOrEmpty(code.transform.GetChild(0).GetComponent<InputField>().text)) {
                        code.transform.GetChild(0).GetComponent<InputField>().text = "a";
                    }
                    if (string.IsNullOrEmpty(code.transform.GetChild(1).GetComponent<InputField>().text)) {
                        code.transform.GetChild(1).GetComponent<InputField>().text = "2";
                    }

                    for (int j = 0; j < varName.Count; j++) {
                        if (varName[j] == code.transform.GetChild(0).GetComponent<InputField>().text) {
                            isExist = true;
                            break;
                        }
                    }
                    if (isExist == false) {
                        varName.Add(code.transform.GetChild(0).GetComponent<InputField>().text);
                        varValue.Add(int.Parse(code.transform.GetChild(1).GetComponent<InputField>().text));
                    }
                }
                if (code.name == "BtnVariable++(Clone)") {
                    if (string.IsNullOrEmpty(code.transform.GetChild(0).GetComponent<InputField>().text)) {
                        code.transform.GetChild(0).GetComponent<InputField>().text = "a";
                    }

                    bool isExist = false;
                    for (int j = 0; j < varName.Count; j++) {
                        if (varName[j] == code.transform.GetChild(0).GetComponent<InputField>().text) {
                            isExist = true;
                            break;
                        }
                    }
                    if (isExist == false) {
                        AlertError(0);
                        codesQueue.Clear();
                        ResetView();
                        return;
                    }
                }
                if (code.name == "BtnCnt=(Clone)") {
                    if (string.IsNullOrEmpty(code.transform.GetChild(0).GetComponent<InputField>().text)) {
                        code.transform.GetChild(0).GetComponent<InputField>().text = "2";
                    }
                    cnt = int.Parse(code.transform.GetChild(0).GetComponent<InputField>().text);
                }
                if (code.name == "BtnCnt++(Clone)") {
                    if (cnt == -1) {
                        AlertError(0);
                        codesQueue.Clear();
                        ResetView();
                        return;
                    }
                }
                if (code.name == "BtnDelay(Clone)") {
                    if (string.IsNullOrEmpty(code.transform.GetChild(0).GetComponent<InputField>().text)) {
                        code.transform.GetChild(0).GetComponent<InputField>().text = "2";
                    }
                }

                if (code.transform.childCount < 2 &&
                    (code.name != "BtnDelay(Clone)" &&
                    code.name != "BtnIf(Clone)" &&
                    code.name != "BtnCnt=(Clone)")) {
                    break;
                }
                else if (code.transform.childCount < 3 &&
                  (code.name == "BtnDelay(Clone)" ||
                  code.name == "BtnCnt=(Clone)" ||
                  code.name == "BtnVariable++(Clone)")) {
                    break;
                }
                else if (code.transform.childCount < 4 &&
                  (code.name == "BtnIf(Clone)" ||
                  code.name == "BtnVariable=(Clone)")) {
                    break;
                }

                if (code.name == "BtnVariable=(Clone)") {
                    code = code.transform.GetChild(3).gameObject;
                }
                else if (code.name == "BtnIf(Clone)") {
                    if (code.transform.GetChild(3).name == "BtnCount(Clone)" ||
                        code.transform.GetChild(3).name == "BtnVariable==(Clone)") {
                        code = code.transform.GetChild(4).gameObject;
                    }
                    else {
                        code = code.transform.GetChild(3).gameObject;
                    }
                }
                else if (code.name == "BtnDelay(Clone)" ||
                  code.name == "BtnCnt=(Clone)" ||
                  code.name == "BtnVariable++(Clone)") {
                    code = code.transform.GetChild(2).gameObject;
                }
                else if (code.name == "BtnLoop(Clone)") {
                    if (code.transform.childCount < 3) {
                        AlertError(1);
                        ResetView();
                        codesQueue.Clear();
                        isCompiled = false;
                        return;
                    }
                    code = code.transform.GetChild(2).gameObject;
                }
                else {
                    code = code.transform.GetChild(1).gameObject;
                }
            }
        }

        if (loopIndex.Count != endLoopIndex.Count) {
            AlertError(1);
            ResetView();
            codesQueue.Clear();
            isCompiled = false;
        }
        else if (ifIndex.Count != endIfIndex.Count) {
            AlertError(2);
            ResetView();
            codesQueue.Clear();
            isCompiled = false;
        }
        else {
            codesQueue.Clear();
            isCompiled = true;
        }
    }
    //---------------------compiling-----------------------------------------------------

    public void run() {
        
        if (playerSprite.flipX && targetPos > playerTransform.position.x) {
            playerAn.SetBool("isMoving", true);
            playerTransform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
        else if (!playerSprite.flipX && targetPos < playerTransform.position.x) {
            playerAn.SetBool("isMoving", true);
            playerTransform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
        else {
            playerAn.SetBool("isMoving", false);
            playerTransform.position = new Vector3(targetPos, playerTransform.position.y, playerTransform.position.z);
            isMoving = false;
        }
    }
    public void FunctionMove(int cnt) {
        isMoving = true;

        if (playerSprite.flipX) {
            targetPos = playerRig.transform.position.x + (1f * cnt);
        }
        else {
            targetPos = playerRig.transform.position.x - (1f * cnt);
        }
        delayTime = 10;
    }

    public void FunctionJump(int cnt) {
        //playerAn.SetBool("isJumping", true);
        playerRig.AddForce(Vector2.up * (jumpPower + (cnt * 1.5f)), ForceMode2D.Impulse);
        delayTime = 1;
    }

    public void FunctionRotate() {
        if (playerRig.GetComponent<SpriteRenderer>().flipX == false) {
            playerRig.GetComponent<SpriteRenderer>().flipX = true;
        }
        else {
            playerRig.GetComponent<SpriteRenderer>().flipX = false;
        }
        delayTime = 1;
    }

    public void FunctionLoop() {
        loopSet.Add(currentIndex - 1);
        delayTime = 1;
    }

    public void FunctionEndLoop() {
        if (loopSet.Count == 0) {
            currentIndex++;
            delayTime = 1;
            return;
        }
        currentIndex = loopSet[loopSet.Count - 1];
        loopSet.RemoveAt(loopSet.Count - 1);
        delayTime = 1;
    }

    public void FunctionDelay() {
        string n = functions[currentIndex].transform.GetChild(0).GetComponent<InputField>().text;
        float temp;
        if (string.IsNullOrEmpty(n)) {
            functions[currentIndex].transform.GetChild(0).GetComponent<InputField>().text = "2";
            temp = 2;
        }
        else {
            temp = float.Parse(n);
        }

        if (temp == 0) {
            delayTime = 1;
        }
        else {
            delayTime = (int)(60 * temp);
        }
    }

    public void FunctionIf() {
        string tempName = null;
        bool conditionFalse = false;

        //if (functions[currentIndex].transform.GetChild(2).name == "BtnCount(Clone)") {
        //    conditionCnt = int.Parse(functions[currentIndex].transform.GetChild(2).GetChild(0).GetComponent<InputField>().text);

        //}
        //else if (functions[currentIndex].transform.GetChild(3).name == "BtnCount(Clone)") {
        //    conditionCnt = int.Parse(functions[currentIndex].transform.GetChild(3).GetChild(0).GetComponent<InputField>().text);
        //}

        for (int i = 0; i < functions[currentIndex].transform.childCount; i++) {
            GameObject temp = functions[currentIndex].transform.GetChild(i).gameObject;
            if (temp.name.Contains("==")) {
                tempName = temp.transform.GetChild(0).GetComponent<InputField>().text;
                conditionCnt = int.Parse(temp.transform.GetChild(1).GetComponent<InputField>().text);
            }
        }

        if (tempName != null) {
            for (int i = 0; i < varName.Count; i++) {
                if (tempName == varName[i]) {
                    if (varValue[i] != conditionCnt) {
                        conditionFalse = true;
                    }
                    break;
                }
            }
        }
        else {
            if (cnt != conditionCnt) {
                conditionFalse = true;
            }
        }

        if (conditionFalse == true) {
            for (int i = 0; i < ifIndex.Count; i++) {
                if (currentIndex == ifIndex[i]) {
                    currentIndex = endIfIndex[0 + i] - 1;
                    break;
                }
            }
        }

        delayTime = 1;
    }

    public void FunctionEndIf() {
        delayTime = 1;
    }

    public void FunctionSetCnt() {
        cnt = int.Parse(functions[currentIndex].transform.GetChild(0).GetComponent<InputField>().text);
        delayTime = 1;
    }

    public void FunctionIncreaseCnt() {
        cnt++;
        delayTime = 1;
    }

    public void FunctionBreak() {
        for (int i = 0; i < endLoopIndex.Count; i++) {
            if (currentIndex < endLoopIndex[i]) {
                currentIndex = endLoopIndex[i];
                loopSet.RemoveAt(loopSet.Count - 1);
                break;
            }
        }
        delayTime = 1;
    }

    public void FunctionSetVariable() {
        for (int i = 0; i < varName.Count; i++) {
            if (functions[currentIndex].transform.GetChild(0).GetComponent<InputField>().text == varName[i]) {
                varValue[i] = int.Parse(functions[currentIndex].transform.GetChild(1).GetComponent<InputField>().text);
                Debug.Log($"Info: varName: {varName[i]}, varValue: {varValue[i]}");
                break;
            }
        }
        delayTime = 1;
    }

    public void FunctionIncreaseValue() {
        for (int i = 0; i < varName.Count; i++) {
            if (functions[currentIndex].transform.GetChild(0).GetComponent<InputField>().text == varName[i]) {
                varValue[i]++;
                Debug.Log($"Info: varName: {varName[i]}, varValue: {varValue[i]}");
                break;
            }
        }
        delayTime = 1;
    }

    public void AlertError(int error) {
        GameObject errorPanel = GameObject.FindGameObjectWithTag("errorPanel");
        if (error == 0) {
            Debug.Log("VAR is not defined!");
            errorPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            errorPanel.transform.GetChild(1).localEulerAngles = new Vector3(0, 90, 0);
            errorPanel.transform.GetChild(2).localEulerAngles = new Vector3(0, 90, 0);
        }
        else if (error == 1) {
            Debug.Log("LOOP is incorrected!");
            errorPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            errorPanel.transform.GetChild(1).localEulerAngles = new Vector3(0, 0, 0);
            errorPanel.transform.GetChild(2).localEulerAngles = new Vector3(0, 90, 0);
        }
        else if (error == 2) {
            Debug.Log("IF is incorrected!");
            errorPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            errorPanel.transform.GetChild(1).localEulerAngles = new Vector3(0, 90, 0);
            errorPanel.transform.GetChild(2).localEulerAngles = new Vector3(0, 0, 0);
        }
    }
}
