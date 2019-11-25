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
    private int currentIndex = 0;

    private int cnt = -1;
    private int conditionCnt = -1;
    private int loopCnt = 0;
    public bool isResetView = false;
    public float moveSpeed = 1;
    private float jumpPower = 4.5f;
    private float timer = 0f;
    private float timeOut = 0f;
    private float delayTimer = 0f;

    private int ifCnt = 0, endIfCnt = 0;
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
    private void FixedUpdate() {
        timer += Time.deltaTime;
        if (isMoving) {

            // 벽으로 이동시 타임아웃 (삭제)
            if (timer > timeOut) {
                float temp = targetPos - playerTransform.position.x;
                targetPos -= Mathf.Round(temp);
                playerTransform.position = new Vector3(targetPos, playerTransform.position.y, 0f);
                playerAn.SetBool("isMoving", false);
                isMoving = false;
                return;
            }

            run();
            return;
        }

        if (timer < delayTimer) {
            return;
        }

        delayTimer = 0f;
        timeOut = 0f;
        timer = 0f;

        if (isCompiled == true && currentIndex < functions.Count) {
            for (; currentIndex < functions.Count; currentIndex++) {

                if (functions[currentIndex].name == "BtnMove(Clone)") {
                    int moveCnt = 0;
                    for (int i = currentIndex; i < functions.Count; i++) {
                        if (functions[i].name == "BtnMove(Clone)") {
                            FunctionMove(++moveCnt);
                            currentIndex++;
                            timeOut = moveCnt;
                        } else {
                            break;
                        }
                    }
                    // currentIndex--;
                    break;
                } else if (functions[currentIndex].name == "BtnJump(Clone)") {
                    if (jumpController.getIsJump() || playerRig.velocity.y != 0)return;
                    int jumpCnt = 0;
                    for (int i = currentIndex; i < functions.Count; i++) {
                        if (functions[i].name == "BtnJump(Clone)") {
                            jumpCnt++;
                            currentIndex++;
                        } else {
                            break;
                        }
                    }
                    FunctionJump(jumpCnt);
                    // currentIndex--;
                    break;
                } else if (functions[currentIndex].name == "BtnRotate(Clone)") {
                    FunctionRotate();
                } else if (functions[currentIndex].name == "BtnLoop(Clone)") {
                    if (jumpController.getIsJump() || playerRig.velocity.y != 0)return;
                    FunctionLoop();

                    // 무한 루프를 빠져나가기 위함
                    loopCnt++;
                    if (loopCnt > 500) {
                        AlertError(3);
                        isCompiled = false;
                        break;
                    }

                } else if (functions[currentIndex].name == "BtnEndLoop(Clone)") {
                    FunctionEndLoop();
                } else if (functions[currentIndex].name == "BtnDelay(Clone)") {
                    FunctionDelay();
                    currentIndex++;
                    break;
                } else if (functions[currentIndex].name == "BtnIf(Clone)") {
                    FunctionIf();
                } else if (functions[currentIndex].name == "BtnEndIf(Clone)") {
                    FunctionEndIf();
                } else if (functions[currentIndex].name == "BtnCnt=(Clone)") {
                    FunctionSetCnt();
                } else if (functions[currentIndex].name == "BtnCnt++(Clone)") {
                    FunctionIncreaseCnt();
                } else if (functions[currentIndex].name == "BtnBreak(Clone)") {
                    FunctionBreak();
                } else if (functions[currentIndex].name == "BtnVariable=(Clone)") {
                    FunctionSetVariable();
                } else if (functions[currentIndex].name == "BtnVariable++(Clone)") {
                    FunctionIncreaseValue();
                }
            }
            // 컴파일러 구 버전
            // if (functions[currentIndex].name == "BtnMove(Clone)") {
            //     int moveCnt = 0;
            //     for (int i = currentIndex; i < functions.Count; i++) {
            //         if (functions[i].name == "BtnMove(Clone)") {
            //             FunctionMove(++moveCnt);
            //             currentIndex++;
            //             timeOut = moveCnt;
            //         } else {
            //             break;
            //         }
            //     }
            //     currentIndex--;
            // } else if (functions[currentIndex].name == "BtnJump(Clone)") {
            //     if (jumpController.getIsJump() || playerRig.velocity.y != 0)return;
            //     int jumpCnt = 0;
            //     for (int i = currentIndex; i < functions.Count; i++) {
            //         if (functions[i].name == "BtnJump(Clone)") {
            //             jumpCnt++;
            //             currentIndex++;
            //         } else {
            //             break;
            //         }
            //     }
            //     FunctionJump(jumpCnt);
            //     currentIndex--;
            // } else if (functions[currentIndex].name == "BtnRotate(Clone)") {
            //     FunctionRotate();
            // } else if (functions[currentIndex].name == "BtnLoop(Clone)") {
            //     if (jumpController.getIsJump() || playerRig.velocity.y != 0)return;
            //     FunctionLoop();
            // } else if (functions[currentIndex].name == "BtnEndLoop(Clone)") {
            //     FunctionEndLoop();
            // } else if (functions[currentIndex].name == "BtnDelay(Clone)") {
            //     FunctionDelay();
            // } else if (functions[currentIndex].name == "BtnIf(Clone)") {
            //     FunctionIf();
            // } else if (functions[currentIndex].name == "BtnEndIf(Clone)") {
            //     FunctionEndIf();
            // } else if (functions[currentIndex].name == "BtnCnt=(Clone)") {
            //     FunctionSetCnt();
            // } else if (functions[currentIndex].name == "BtnCnt++(Clone)") {
            //     FunctionIncreaseCnt();
            // } else if (functions[currentIndex].name == "BtnBreak(Clone)") {
            //     FunctionBreak();
            // } else if (functions[currentIndex].name == "BtnVariable=(Clone)") {
            //     FunctionSetVariable();
            // } else if (functions[currentIndex].name == "BtnVariable++(Clone)") {
            //     FunctionIncreaseValue();
            // }
            // currentIndex++;
        } else {
            isMoving = false;
            playerAn.SetBool("isMoving", false);
            currentIndex = 0;
            cnt = -1;
            loopCnt = 0;
            conditionCnt = -1;
            isCompiled = false;
            ifCnt = 0;
            endIfCnt = 0;
            loopIndex.Clear();
            endLoopIndex.Clear();
            ifIndex.Clear();
            varName.Clear();
            varValue.Clear();
            functions.Clear();
            loopSet.Clear();
        }

    }

    public void ResetView() {
        isMoving = false;
        playerAn.SetBool("isMoving", false);
        currentIndex = 0;
        cnt = -1;
        loopCnt = 0;
        conditionCnt = -1;
        isCompiled = false;
        ifCnt = 0;
        endIfCnt = 0;
        functions.Clear();
        loopIndex.Clear();
        endLoopIndex.Clear();
        ifIndex.Clear();
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
        GameObject nextCode;
        GameObject code = GameObject.FindGameObjectWithTag("codePanel").transform.GetChild(1).gameObject;
        List<GameObject> codesQueue = new List<GameObject>();
        GameObject temp;

        // codeQueue 에 codePanel에 있는 코드 블럭 오브젝트를 넣는다.
        // 여러 줄의 코드가 있을 때를 방지하기 위해서
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

        // codeQueue 에 있는 코드 블럭들을 기준으로 시작한다.
        // codeQueue 에 들어있는 코드 블럭은 해당 코드 라인에 가장 부모가 되는 코드 블럭이다.
        for (int i = 0; i < codesQueue.Count; i++) {
            code = codesQueue[i];

            // codeQueue 를 시작으로 계속 자식 오브젝트를 찾으며 functions 리스트에 추가한다.
            while (true) {

                functions.Add(code); // Move, jump, rotate 는 따로 작업하는 것이 없이 functions 에 추가한다.

                // Loop 는 해당 코드가 몇 번째에 있는 지 따로 저장한다.
                if (code.name == "BtnLoop(Clone)") {
                    loopIndex.Add(functions.Count - 1);

                    // Loop 는 자식이 총 3개 이여야 한다. 부족하다면 에러창을 띄운다.
                    // 자식 오브젝트(Child Check obj, Folding obj, Child Code Block obj): 총 3개가 와야 한다.
                    if (code.transform.childCount < 3) {
                        AlertError(1); // 1번 에러창을 띄운다. (Loop error)
                        codesQueue.Clear(); // 큐를 비운다.
                        isCompiled = false; // 컴파일을 취소하고 바로 리턴한다.
                        return;
                    }
                }

                // EndLoop 는 해당 코드가 몇 번째에 있는 지 따로 저장한다.
                if (code.name == "BtnEndLoop(Clone)") {
                    endLoopIndex.Add(functions.Count - 1);
                }

                // If 는 해당 코드가 몇 번째에 있는 지 따로 저장한다.
                // IfIndex 는 stack 으로 구현하여 If 와 EndIf 를 찾을 수 있다.
                if (code.name == "BtnIf(Clone)") {
                    ifCnt++; // ifCnt 를 통해 If 와 EndIf 의 갯수 차이를 확인하기 위함이다.
                    ifIndex.Add(functions.Count - 1); // ifIndex stack 에 현재 코드 index 를 추가한다.

                    // If 는 자식 오브젝트를 5개 가지고 있어야 한다.
                    // 자식 오브젝트(Child Check obj, Condition Check obj Folding obj, Condition Block obj, Child Block obj)
                    if (code.transform.childCount < 5) {
                        AlertError(2); // 2번 에러를 띄운다. (If error)
                        codesQueue.Clear(); // 큐를 비운다.
                        return;
                    }

                    string tempName = null; // 변수 이름 저장
                    bool isExist = false; // 해당 변수가 있는 지 확인하는 bool

                    // If 와 동시에 Condition 부분도 동시에 처리한다.
                    // 아래 부분에서는 비어있는 inputField를 채운다.
                    // 추가적으로 functions 에 추가하지 않고 게임이 실행되면서 확인절차를 거친다.
                    for (int j = 0; j < code.transform.childCount; j++) {

                        // Condition 은 "==", "!="이 있다.
                        if (code.transform.GetChild(j).name.Contains("BtnVariable==")) {
                            GameObject ifTemp = code.transform.GetChild(j).gameObject; // 해당 블럭을 임시로 저장한다.

                            // InputField 를 채운다.
                            // Condition 블럭에는 총 2개의 InputField 가 있다.(Variable name, value)
                            for (int k = 0; k < 2; k++) {
                                InputField inputTemp = ifTemp.transform.GetChild(k).GetComponent<InputField>();

                                // 해당 inputField 가 비어있을 경우, placeholder 값으로 대체한다.
                                if (string.IsNullOrEmpty(inputTemp.text))
                                    inputTemp.text = inputTemp.placeholder.GetComponent<Text>().text;
                                if (ifTemp.transform.GetChild(k).name.Contains("(1)"))
                                    tempName = inputTemp.text;
                            }
                        } else if (code.transform.GetChild(j).name.Contains("BtnVariable!=")) {
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

                    // tempName 을 통해 condition 에 사용된 변수가 있는 지 확인하고(선언하였는지 확인) 처리한다.
                    if (tempName != null) {

                        // 변수를 저장하는 리스트에서 해당 변수가 있는 지 확인한다.
                        for (int k = 0; k < varName.Count; k++) {
                            if (tempName == varName[k]) {
                                isExist = true;
                                break;
                            }
                        }

                        // 해당 변수가 없으면 에러를 출력한다.
                        if (isExist == false) {
                            AlertError(0); // 에러창을 띄운다.(Variable error)
                            codesQueue.Clear();
                            return;
                        }
                    } else {

                        // Cnt block 관련(현재는 삭제된 블럭)
                        if (cnt == -1) {
                            AlertError(0);
                            codesQueue.Clear();
                            return;
                        }
                    }
                }

                // EndIf 를 ifIndex stack 에 추가하고, endIfCnt 를 늘린다.
                // endIfCnt 는 If 블럭과 EndIf 블럭의 갯수 차이를 확인하기 위함이다.
                if (code.name == "BtnEndIf(Clone)") {
                    endIfCnt++;
                    ifIndex.Add(functions.Count - 1);
                }

                // Variable 선언 블럭 함수
                // Variable 선언 블럭과 "++" 블럭은 비슷한 방식이다.
                if (code.name == "BtnVariable=(Clone)") {
                    bool isExist = false;

                    // InputField 는 총 2개 있으며, 이는 variable name, value 이다.
                    // InputField 가 비어있으면, placeholder 로 대체한다.
                    if (string.IsNullOrEmpty(code.transform.GetChild(0).GetComponent<InputField>().text)) {
                        code.transform.GetChild(0).GetComponent<InputField>().text = "a";
                    }
                    if (string.IsNullOrEmpty(code.transform.GetChild(1).GetComponent<InputField>().text)) {
                        code.transform.GetChild(1).GetComponent<InputField>().text = "0";
                    }

                    // 해당 변수가 리스트에 이미 있으면, 아무것도 하지 않는다.
                    // 해당 변수가 리스트에 없으면, 해당 변수의 이름과 값을 저장한다. 
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

                // 변수의 값에 1을 더하는 블럭 처리
                if (code.name == "BtnVariable++(Clone)") {

                    // InputField 는 총 1개 있으며, 이는 variable name 이다.
                    // 이 값이 비어있으면, placeholder로 대체한다.
                    if (string.IsNullOrEmpty(code.transform.GetChild(0).GetComponent<InputField>().text)) {
                        code.transform.GetChild(0).GetComponent<InputField>().text = "a";
                    }

                    // 해당 변수가 리스트에 없으면(선언되지 않았으면), 에러창을 띄운다.
                    bool isExist = false;
                    for (int j = 0; j < varName.Count; j++) {
                        if (varName[j] == code.transform.GetChild(0).GetComponent<InputField>().text) {
                            isExist = true;
                            break;
                        }
                    }
                    if (isExist == false) {
                        AlertError(0); // 변수 에러와 동일하다.
                        codesQueue.Clear();
                        return;
                    }
                }

                // Cnt 블럭 관련 처리(현재는 사용되지 않는 블럭)
                if (code.name == "BtnCnt=(Clone)") {
                    if (string.IsNullOrEmpty(code.transform.GetChild(0).GetComponent<InputField>().text)) {
                        code.transform.GetChild(0).GetComponent<InputField>().text = "2";
                    }
                    cnt = int.Parse(code.transform.GetChild(0).GetComponent<InputField>().text);
                }

                // Cnt 블럭 관련 처리(현재는 사용되지 않는 블럭)
                if (code.name == "BtnCnt++(Clone)") {
                    if (cnt == -1) {
                        AlertError(0);
                        codesQueue.Clear();
                        return;
                    }
                }

                // 지연시간 처리
                // InputField 는 총 1개 있으며, 이는 time(s) 이다.
                // 해당 inputField 가 비어있으면, placeholder 로 대체한다.
                if (code.name == "BtnDelay(Clone)") {
                    if (string.IsNullOrEmpty(code.transform.GetChild(0).GetComponent<InputField>().text)) {
                        code.transform.GetChild(0).GetComponent<InputField>().text = "2";
                    }
                }

                // 현재 블럭의 자식 오브젝트를 탐색하여 다음 Child Block 이 있는 지 확인하고
                // code 를 대체한다.
                nextCode = null;
                for (int j = 0; j < code.transform.childCount; j++) {

                    // 다음 코드 블럭은 "Clone"을 포함하고 있어야 하며, Condition 블럭이면 안된다.
                    if (code.transform.GetChild(j).name.Contains("Clone")) {
                        if (!code.transform.GetChild(j).name.Contains("==") && !code.transform.GetChild(j).name.Contains("!="))
                            nextCode = code.transform.GetChild(j).gameObject;
                    }
                }

                // nextCode 블럭이 정해졌으면 code 에 넣고, 
                // 아니면 코드 라인이 끝났다는 말이므로 while 문을 탈출한다.
                if (nextCode != null)
                    code = nextCode;
                else
                    break;
            }
        }

        // Error 확인
        // Loop 와 EndLoop 의 갯수가 다르면 error
        // If 와 EndIf 의 갯수가 다르면 error
        // 둘다 아니면 큐를 비우고 컴파일 완료
        if (loopIndex.Count != endLoopIndex.Count) {
            AlertError(1);
            codesQueue.Clear();
            isCompiled = false;
        } else if (ifCnt != endIfCnt) {
            AlertError(2);
            codesQueue.Clear();
            isCompiled = false;
        } else {
            codesQueue.Clear();
            isCompiled = true;
        }
    }
    //---------------------compiling-----------------------------------------------------

    public void run() {

        if (playerSprite.flipX && targetPos > playerTransform.position.x) {
            playerAn.SetBool("isMoving", true);
            playerTransform.position += Vector3.right * moveSpeed * Time.deltaTime;
        } else if (!playerSprite.flipX && targetPos < playerTransform.position.x) {
            playerAn.SetBool("isMoving", true);
            playerTransform.position += Vector3.left * moveSpeed * Time.deltaTime;
        } else {
            playerAn.SetBool("isMoving", false);
            playerTransform.position = new Vector3(targetPos, playerTransform.position.y, playerTransform.position.z);
            isMoving = false;
        }
    }
    public void FunctionMove(int moveCnt) {
        isMoving = true;

        if (playerSprite.flipX) {
            targetPos = playerRig.transform.position.x + (1f * moveCnt);
        } else {
            targetPos = playerRig.transform.position.x - (1f * moveCnt);
        }
    }

    public void FunctionJump(int jumpCnt) {
        //playerAn.SetBool("isJumping", true);
        playerRig.AddForce(Vector2.up * (jumpPower + (jumpCnt * 1.5f)), ForceMode2D.Impulse);
    }

    public void FunctionRotate() {
        if (playerRig.GetComponent<SpriteRenderer>().flipX == false) {
            playerRig.GetComponent<SpriteRenderer>().flipX = true;
        } else {
            playerRig.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    public void FunctionLoop() {
        loopSet.Add(currentIndex - 1);
    }

    public void FunctionEndLoop() {
        if (loopSet.Count == 0) {
            currentIndex++;
            return;
        }
        currentIndex = loopSet[loopSet.Count - 1];
        loopSet.RemoveAt(loopSet.Count - 1);
    }

    public void FunctionDelay() {
        string n = functions[currentIndex].transform.GetChild(0).GetComponent<InputField>().text;
        float temp;
        if (string.IsNullOrEmpty(n)) {
            functions[currentIndex].transform.GetChild(0).GetComponent<InputField>().text = "2";
            temp = 2;
        } else {
            temp = float.Parse(n);
        }
        delayTimer = temp;
    }

    public void FunctionIf() {
        string tempName = null;
        bool conditionFalse = false;
        bool isNot = false;

        for (int i = 0; i < functions[currentIndex].transform.childCount; i++) {
            GameObject temp = functions[currentIndex].transform.GetChild(i).gameObject;
            if (temp.name.Contains("==")) {
                tempName = temp.transform.GetChild(0).GetComponent<InputField>().text;
                conditionCnt = int.Parse(temp.transform.GetChild(1).GetComponent<InputField>().text);
                isNot = false;
            } else if (temp.name.Contains("!=")) {
                tempName = temp.transform.GetChild(0).GetComponent<InputField>().text;
                conditionCnt = int.Parse(temp.transform.GetChild(1).GetComponent<InputField>().text);
                isNot = true;
            }
        }

        if (tempName != null) {
            for (int i = 0; i < varName.Count; i++) {
                if (tempName == varName[i]) {
                    if (isNot == false && varValue[i] != conditionCnt) {
                        conditionFalse = true;
                    } else if (isNot == true && varValue[i] == conditionCnt) {
                        conditionFalse = true;
                    }
                    break;
                }
            }
        }
        if (conditionFalse == true) {
            int brace;
            for (int i = 0; i < ifIndex.Count; i++) {
                if (currentIndex == ifIndex[i]) {
                    brace = 1;
                    for (int j = i + 1; j < ifIndex.Count; j++) {
                        if (functions[ifIndex[j]].name == "BtnIf(Clone)")brace++;
                        else if (functions[ifIndex[j]].name == "BtnEndIf(Clone)")brace--;

                        if (brace == 0) {
                            brace = ifIndex[j];
                            break;
                        }
                    }
                    currentIndex = brace - 1;
                    break;
                }
            }
        }
    }

    public void FunctionEndIf() {

    }

    public void FunctionSetCnt() {
        cnt = int.Parse(functions[currentIndex].transform.GetChild(0).GetComponent<InputField>().text);
    }

    public void FunctionIncreaseCnt() {
        cnt++;
    }

    public void FunctionBreak() {
        for (int i = 0; i < endLoopIndex.Count; i++) {
            if (currentIndex < endLoopIndex[i]) {
                currentIndex = endLoopIndex[i];
                loopSet.RemoveAt(loopSet.Count - 1);
                break;
            }
        }
    }

    public void FunctionSetVariable() {
        for (int i = 0; i < varName.Count; i++) {
            if (functions[currentIndex].transform.GetChild(0).GetComponent<InputField>().text == varName[i]) {
                varValue[i] = int.Parse(functions[currentIndex].transform.GetChild(1).GetComponent<InputField>().text);
                Debug.Log($"Info: varName: {varName[i]}, varValue: {varValue[i]}");
                break;
            }
        }
    }

    public void FunctionIncreaseValue() {
        for (int i = 0; i < varName.Count; i++) {
            if (functions[currentIndex].transform.GetChild(0).GetComponent<InputField>().text == varName[i]) {
                varValue[i]++;
                Debug.Log($"Info: varName: {varName[i]}, varValue: {varValue[i]}");
                break;
            }
        }
    }

    public void AlertError(int error) {
        GameObject errorPanel = GameObject.FindGameObjectWithTag("errorPanel");
        if (error == 0) {
            errorPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            errorPanel.transform.GetChild(1).localEulerAngles = new Vector3(0, 90, 0);
            errorPanel.transform.GetChild(2).localEulerAngles = new Vector3(0, 90, 0);
        } else if (error == 1) {
            errorPanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().text =
                "[LOOP] 와 [END LOOP] 의\n갯수가 맞지 않습니다.\n블럭의 연결상태를 확인해주세요.";
            errorPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            errorPanel.transform.GetChild(1).localEulerAngles = new Vector3(0, 0, 0);
            errorPanel.transform.GetChild(2).localEulerAngles = new Vector3(0, 90, 0);
        } else if (error == 2) {
            errorPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            errorPanel.transform.GetChild(1).localEulerAngles = new Vector3(0, 90, 0);
            errorPanel.transform.GetChild(2).localEulerAngles = new Vector3(0, 0, 0);
        } else if (error == 3) {
            errorPanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().text =
                "무한 루프입니다.\n코드 블럭을 확인해주세요";
            errorPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            errorPanel.transform.GetChild(1).localEulerAngles = new Vector3(0, 0, 0);
            errorPanel.transform.GetChild(2).localEulerAngles = new Vector3(0, 90, 0);
        }
    }

    public void SetFalseIsCompiled() {
        isCompiled = false;
        isMoving = false;
    }
}