using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compiler : MonoBehaviour
{
    private List<GameObject> functions = new List<GameObject>();
    private List<int> loopSet = new List<int>();
    private List<int> loopIndex = new List<int>();
    private List<int> endLoopIndex = new List<int>();
    private List<int> ifIndex = new List<int>();
    private List<int> endIfIndex = new List<int>();
    private List<string> varName = new List<string>();
    private List<int> varValue = new List<int>();

    private bool isCompiled = false;
    private Rigidbody2D player;
    
    private int frameCount = 0;
    private int delayTime = 1;
    private int currentIndex = 0;

    private int cnt = -1;
    private int conditionCnt = -1;

    private int loopCnt = 0;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        frameCount++;
        if(frameCount % delayTime == 0) {
            if (isCompiled == true && currentIndex < functions.Count) {
                if (functions[currentIndex].name == "BtnMove(Clone)") {
                    Debug.Log("Code index " + currentIndex + " : " + functions[currentIndex].name);
                    FunctionMove();
                } else if (functions[currentIndex].name == "BtnJump(Clone)") {
                    Debug.Log("Code index " + currentIndex + " : " + functions[currentIndex].name);
                    FunctionJump();
                } else if (functions[currentIndex].name == "BtnRotate(Clone)") {
                    Debug.Log("Code index " + currentIndex + " : " + functions[currentIndex].name);
                    FunctionRotate();
                } else if (functions[currentIndex].name == "BtnLoop(Clone)") {
                    Debug.Log("Code index " + currentIndex + " : " + functions[currentIndex].name);
                    FunctionLoop();
                } else if (functions[currentIndex].name == "BtnEndLoop(Clone)") {
                    Debug.Log("Code index " + currentIndex + " : " + functions[currentIndex].name);
                    FunctionEndLoop();
                } else if (functions[currentIndex].name == "BtnDelay(Clone)") {
                    Debug.Log("Code index " + currentIndex + " : " + functions[currentIndex].name);
                    FunctionDelay();
                } else if (functions[currentIndex].name == "BtnIf(Clone)") {
                    Debug.Log("Code index " + currentIndex + " : " + functions[currentIndex].name);
                    FunctionIf();
                } else if (functions[currentIndex].name == "BtnEndIf(Clone)") {
                    Debug.Log("Code index " + currentIndex + " : " + functions[currentIndex].name);
                    FunctionEndIf();
                } else if (functions[currentIndex].name == "BtnCnt=(Clone)") {
                    Debug.Log("Code index " + currentIndex + " : " + functions[currentIndex].name);
                    FunctionSetCnt();
                } else if (functions[currentIndex].name == "BtnCnt++(Clone)") {
                    Debug.Log("Code index " + currentIndex + " : " + functions[currentIndex].name);
                    FunctionIncreaseCnt();
                } else if (functions[currentIndex].name == "BtnBreak(Clone)") {
                    Debug.Log("Code index " + currentIndex + " : " + functions[currentIndex].name);
                    FunctionBreak();
                } else if (functions[currentIndex].name == "BtnVariable=(Clone)") {
                    Debug.Log("Code index " + currentIndex + " : " + functions[currentIndex].name);
                    FunctionSetVariable();
                } else if (functions[currentIndex].name == "BtnVariable++(Clone)") {
                    Debug.Log("Code index " + currentIndex + " : " + functions[currentIndex].name);
                    FunctionIncreaseValue();
                }

                currentIndex++;
                frameCount = 0;
            } else {
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
        player.velocity = new Vector2(0, 0);
        player.transform.position = new Vector2(0, 2);
        player.GetComponent<SpriteRenderer>().flipX = false;
    }

    public void Compiling() {
        if (isCompiled == true) {
            isCompiled = false;
            return;
        }
        GameObject code = GameObject.FindGameObjectWithTag("codePanel");
        List<GameObject> codesQueue = new List<GameObject>();
        GameObject temp;
        for (int i = 1; i < code.transform.childCount; i++) {
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
                    if (code.transform.childCount < 4) {
                        AlertError(2);
                        ResetView();
                        return;
                    }
                    string tempName = null;
                    bool isExist = false;

                    if (code.transform.GetChild(2).name == "BtnCount(Clone)") {
                        if (string.IsNullOrEmpty(code.transform.GetChild(2).GetChild(0).GetComponent<InputField>().text)) {
                            code.transform.GetChild(2).GetChild(0).GetComponent<InputField>().text = "2";
                        }
                    } else if (code.transform.GetChild(3).name == "BtnCount(Clone)") {
                        if (string.IsNullOrEmpty(code.transform.GetChild(3).GetChild(0).GetComponent<InputField>().text)) {
                            code.transform.GetChild(3).GetChild(0).GetComponent<InputField>().text = "2";
                        }
                    }

                    if (code.transform.GetChild(2).name == "BtnVariable==(Clone)") {
                        if (string.IsNullOrEmpty(code.transform.GetChild(2).GetChild(1).GetComponent<InputField>().text)) {
                            code.transform.GetChild(2).GetChild(1).GetComponent<InputField>().text = "2";
                        }
                        if (string.IsNullOrEmpty(code.transform.GetChild(2).GetChild(0).GetComponent<InputField>().text)) {
                            code.transform.GetChild(2).GetChild(0).GetComponent<InputField>().text = "a";
                        }
                        tempName = code.transform.GetChild(2).GetChild(0).GetComponent<InputField>().text;
                    } else if (code.transform.GetChild(3).name == "BtnVariable==(Clone)") {
                        if (string.IsNullOrEmpty(code.transform.GetChild(3).GetChild(1).GetComponent<InputField>().text)) {
                            code.transform.GetChild(3).GetChild(1).GetComponent<InputField>().text = "2";
                        }
                        if (string.IsNullOrEmpty(code.transform.GetChild(3).GetChild(0).GetComponent<InputField>().text)) {
                            code.transform.GetChild(3).GetChild(0).GetComponent<InputField>().text = "a";
                            tempName = "a";
                        }
                        tempName = code.transform.GetChild(3).GetChild(0).GetComponent<InputField>().text;
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
                    } else {
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
                    if(string.IsNullOrEmpty(code.transform.GetChild(0).GetComponent<InputField>().text)) {
                        code.transform.GetChild(0).GetComponent<InputField>().text = "2";
                    }
                }

                if (code.transform.childCount < 2 && 
                    (code.name != "BtnDelay(Clone)" &&
                    code.name != "BtnIf(Clone)" &&
                    code.name != "BtnCnt=(Clone)")) {
                    break;
                } else if (code.transform.childCount < 3 &&
                    (code.name == "BtnDelay(Clone)" ||
                    code.name == "BtnCnt=(Clone)" ||
                    code.name == "BtnVariable++(Clone)")) {
                    break;
                } else if (code.transform.childCount < 4 &&
                    (code.name == "BtnIf(Clone)" ||
                    code.name == "BtnVariable=(Clone)")) {
                    break;
                }

                if (code.name == "BtnVariable=(Clone)") {
                    code = code.transform.GetChild(3).gameObject;
                } else if (code.name == "BtnIf(Clone)") {
                    if (code.transform.GetChild(2).name == "BtnCount(Clone)" ||
                        code.transform.GetChild(2).name == "BtnVariable==(Clone)") {
                        code = code.transform.GetChild(3).gameObject;
                    } else {
                        code = code.transform.GetChild(2).gameObject;
                    }
                } else if (code.name == "BtnDelay(Clone)" ||
                    code.name == "BtnCnt=(Clone)" ||
                    code.name == "BtnVariable++(Clone)") {
                    code = code.transform.GetChild(2).gameObject;
                } else {
                    code = code.transform.GetChild(1).gameObject;
                }
            }
        }

        if (loopIndex.Count != endLoopIndex.Count) {
            AlertError(1);
            ResetView();
            codesQueue.Clear();
            isCompiled = false;
        } else if (ifIndex.Count != endIfIndex.Count) {
            AlertError(2);
            ResetView();
            codesQueue.Clear();
            isCompiled = false;
        } else {
            codesQueue.Clear();
            isCompiled = true;
        }
    }

    public void FunctionMove() {
        if (player.GetComponent<SpriteRenderer>().flipX == false) {
            //player.transform.position += Vector3.right * 0.3f;
            player.AddForce(Vector2.right*2, ForceMode2D.Impulse);
        } else {
            //player.transform.position += Vector3.left * 0.3f;
            player.AddForce(Vector2.left*2, ForceMode2D.Impulse);
        }
        delayTime = 1;
    }

    public void FunctionJump() {
        player.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
        delayTime = 1;
    }

    public void FunctionRotate() {
        if (player.GetComponent<SpriteRenderer>().flipX == false) {
            player.GetComponent<SpriteRenderer>().flipX = true;
        } else {
            player.GetComponent<SpriteRenderer>().flipX = false;
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
        //for (int i = 0; i < endLoopIndex.Count; i++) {
        //    if (currentIndex == endLoopIndex[i]) {
        //        for (int j = 0; j < loopIndex.Count; j++) {
        //            if (loopIndex[j] > endLoopIndex[i]) {
        //                currentIndex = loopIndex[j - 1 - i] - 1;
        //            }
        //            break;
        //        }
        //        //currentIndex = loopIndex[loopIndex.Count - 1 - i] - 1;
        //        break;
        //    }
        //}
        delayTime = 1;
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

        if (temp == 0) {
            delayTime = 1;
        } else {
            delayTime = (int)(60 * temp);
        }
    }

    public void FunctionIf() {
        string tempName = null;
        bool conditionFalse = false;

        if (functions[currentIndex].transform.GetChild(2).name == "BtnCount(Clone)") {
            conditionCnt = int.Parse(functions[currentIndex].transform.GetChild(2).GetChild(0).GetComponent<InputField>().text);
            
        } else if (functions[currentIndex].transform.GetChild(3).name == "BtnCount(Clone)") {
            conditionCnt = int.Parse(functions[currentIndex].transform.GetChild(3).GetChild(0).GetComponent<InputField>().text);
        }

        if (functions[currentIndex].transform.GetChild(2).name == "BtnVariable==(Clone)") {
            tempName = functions[currentIndex].transform.GetChild(2).GetChild(0).GetComponent<InputField>().text;
            conditionCnt = int.Parse(functions[currentIndex].transform.GetChild(2).GetChild(1).GetComponent<InputField>().text);
            
        } else if (functions[currentIndex].transform.GetChild(3).name == "BtnVariable==(Clone)") {
            tempName = functions[currentIndex].transform.GetChild(3).GetChild(0).GetComponent<InputField>().text;
            conditionCnt = int.Parse(functions[currentIndex].transform.GetChild(3).GetChild(1).GetComponent<InputField>().text);
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
        } else {
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
                Debug.Log("Info : " + varName[i] + "=" + varValue[i]);
                break;
            }
        }
        delayTime = 1;
    }

    public void FunctionIncreaseValue() {
        for (int i = 0; i < varName.Count; i++) {
            if (functions[currentIndex].transform.GetChild(0).GetComponent<InputField>().text == varName[i]) {
                varValue[i]++;
                Debug.Log("Info : " + varName[i] + "=" + varValue[i]);
                break;
            }
        }
        delayTime = 1;
    }

    public void AlertError(int error) {
        GameObject errorPanel = GameObject.FindGameObjectWithTag("errorPanel");

        if (error == 0) {
            Debug.Log("CNT is not defined!");
            errorPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            errorPanel.transform.GetChild(1).localEulerAngles = new Vector3(0, 90, 0);
            errorPanel.transform.GetChild(2).localEulerAngles = new Vector3(0, 90, 0);
        } else if (error == 1) {
            Debug.Log("LOOP is incorrected!");
            errorPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            errorPanel.transform.GetChild(1).localEulerAngles = new Vector3(0, 0, 0);
            errorPanel.transform.GetChild(2).localEulerAngles = new Vector3(0, 90, 0);
        } else if (error == 2) {
            Debug.Log("IF is incorrected!");
            errorPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            errorPanel.transform.GetChild(1).localEulerAngles = new Vector3(0, 90, 0);
            errorPanel.transform.GetChild(2).localEulerAngles = new Vector3(0, 0, 0);
        }

    }
}
