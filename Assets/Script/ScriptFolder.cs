using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptFolder : MonoBehaviour {

    private List<GameObject> insideObjs = new List<GameObject>();   // Folder기능을 가지고 있는 코드 블럭 안에 들어있는 코드 블럭들
    private Toggle toggle = null;           // Toggle component
    private bool isFolding = false;         // 애니메이션 효과 작동을 위한 Boolean
    private GameObject thisObj;             // 이 Folder를 가지고 있는 코드 블럭 저장

    void Update() {

        // 이 애니메이션 효과는 List가 채워져 있을 경우에만, 작동한다.
        if (insideObjs.Count != 0) {
            if (isFolding) {
                foreach (GameObject temp in insideObjs) {

                    // Condition 블럭을 찾아서 투명화 애니메이션 효과 적용, raycast 비활성화(클릭 비활성화), inputFieldObj 비활성화
                    if (temp.name.Contains("If")) {
                        if (!temp.name.Contains("End")) {
                            for (int i = 0; i < temp.transform.childCount; i++) {
                                GameObject condition = temp.transform.GetChild(i).gameObject;
                                if (condition.name.Contains("==")) {

                                    // 투명화 애니메이션
                                    condition.GetComponent<Image>().color = Color.Lerp(condition.GetComponent<Image>().color,
                                        new Color(255, 255, 255, 0), Time.deltaTime * 20f);

                                    // raycast 비활성화
                                    condition.GetComponent<Image>().raycastTarget = false;

                                    // Condition 하위 오브젝트들 전부 비활성화(inputFieldObj만 있음)
                                    for (int j = 0; j < condition.transform.childCount; j++)
                                        condition.transform.GetChild(j).gameObject.SetActive(false);
                                }
                            }
                        }
                    }

                    // 코드 블럭 투명화 애니메이션(Lerp)
                    temp.GetComponent<Image>().color = Color.Lerp(temp.GetComponent<Image>().color,
                        new Color(255, 255, 255, 0), Time.deltaTime * 20f);

                    // IF, LOOP에 있는 Toggle 비활성화
                    if (temp.name.Contains("If") || temp.name.Contains("Loop"))
                        if (!temp.name.Contains("End"))
                            temp.transform.Find("Toggle").gameObject.SetActive(false);

                    // 코드가 접히는 동안 클릭이 되지 않도록 raycast 비활성화
                    temp.GetComponent<Image>().raycastTarget = false;

                    // 입력이 들어가는 블럭의 경우 inputFieldObj 비활성화(Variable++, Variable=, Variable==, Delay)
                    if (temp.name.Contains("Variable") || temp.name.Contains("Delay")) {
                        for (int i = 0; i < temp.transform.childCount; i++) {
                            GameObject inputFieldObj = temp.transform.GetChild(i).gameObject;
                            if (inputFieldObj.name.Contains("InputField"))
                                inputFieldObj.SetActive(false);
                        }
                    }

                    // 코드 접히는 애니메이션
                    temp.transform.position = Vector2.Lerp(temp.transform.position,
                        new Vector2(temp.transform.position.x, thisObj.transform.position.y), Time.deltaTime * 2f);
                }

                // Lerp가 끝난 경우(코드들이 전부 다 접힌 경우)
                foreach (GameObject temp in insideObjs) {
                    if (temp.transform.position.y >= thisObj.transform.position.y) {
                        // 코드 블럭들을 전부 투명화, 위치값 고정
                        foreach (GameObject ttemp in insideObjs) {
                            ttemp.GetComponent<Image>().color = new Color(255, 255, 255, 0);
                            ttemp.transform.position = new Vector2(ttemp.transform.position.x, thisObj.transform.position.y);

                            // Condition 블럭을 찾아서 투명화 적용
                            if (ttemp.name.Contains("If")) {
                                if (!ttemp.name.Contains("End")) {
                                    for (int i = 0; i < ttemp.transform.childCount; i++) {
                                        GameObject condition = ttemp.transform.GetChild(i).gameObject;
                                        if (condition.name.Contains("==")) {
                                            condition.GetComponent<Image>().color = new Color(255, 255, 255, 0);
                                        }
                                    }
                                }
                            }
                        }

                        // 코드가 들어있는 List를 초기화하여 애니메이션 효과가 작동하지 않도록 함
                        insideObjs.Clear();
                        break;
                    }
                }
            }
            else {
                foreach (GameObject temp in insideObjs) {

                    // Condition 블럭을 찾아서 불투명화 애니메이션 효과 적용
                    if (temp.name.Contains("If")) {
                        if (!temp.name.Contains("End")) {
                            for (int i = 0; i < temp.transform.childCount; i++) {
                                GameObject condition = temp.transform.GetChild(i).gameObject;
                                if (condition.name.Contains("==")) {

                                    // 투명화 애니메이션
                                    condition.GetComponent<Image>().color = Color.Lerp(condition.GetComponent<Image>().color,
                                        Color.white, Time.deltaTime * 20f);
                                }
                            }
                        }
                    }


                    // 각 코드 블럭은 부모 코드 블럭의 BtnChild 오브젝트의 위치값과 동일해야 하므로
                    // 찾아서 저장
                    GameObject parentChildObj = temp.transform.parent.gameObject;
                    for (int i = 0; i < parentChildObj.transform.childCount; i++) {
                        if (parentChildObj.transform.GetChild(i).name.Equals("BtnChild")) {
                            parentChildObj = parentChildObj.transform.GetChild(i).gameObject;
                            break;
                        }
                    }

                    // 코드 블럭 불투명화 애니메이셔 효과(Lerp)
                    temp.GetComponent<Image>().color = Color.Lerp(temp.GetComponent<Image>().color,
                        Color.white, Time.deltaTime * 20f);

                    // 코드 펼치는 애니메이션
                    // 위에서 구한 부모 코드블럭의 BtnChild 오브젝트의 위치 값으로 이동
                    temp.transform.position = Vector2.Lerp(temp.transform.position,
                        new Vector2(temp.transform.position.x, parentChildObj.transform.position.y), Time.deltaTime * 5f);
                }

                // 위치 이동 Lerp가 끝난 경우
                foreach (GameObject temp in insideObjs) {
                    GameObject parentChildObj = temp.transform.parent.gameObject;
                    for (int i = 0; i < parentChildObj.transform.childCount; i++) {
                        if (parentChildObj.transform.GetChild(i).name.Equals("BtnChild")) {
                            parentChildObj = parentChildObj.transform.GetChild(i).gameObject;
                            break;
                        }
                    }

                    if (temp.transform.position.y <= parentChildObj.transform.position.y + 1) {
                        // 해당 코드 블럭들의 부모 오브젝트의 BtnChild를 찾음
                        foreach (GameObject ttemp in insideObjs) {

                            // Condition 블럭을 찾아서 raycast 비활성화(클릭 활성화), inputFieldObj 활성화
                            if (ttemp.name.Contains("If")) {
                                if (!ttemp.name.Contains("End")) {
                                    for (int i = 0; i < ttemp.transform.childCount; i++) {
                                        GameObject condition = ttemp.transform.GetChild(i).gameObject;
                                        if (condition.name.Contains("==")) {

                                            // raycast 활성화
                                            condition.GetComponent<Image>().raycastTarget = true;

                                            // Condition 하위 오브젝트들 전부 활성화(inputFieldObj만 있음)
                                            for (int j = 0; j < condition.transform.childCount; j++)
                                                condition.transform.GetChild(j).gameObject.SetActive(true);
                                        }
                                    }
                                }
                            }

                            GameObject parentChildObj2 = ttemp.transform.parent.gameObject;
                            for (int i = 0; i < parentChildObj2.transform.childCount; i++) {
                                if (parentChildObj2.transform.GetChild(i).name.Equals("BtnChild")) {
                                    parentChildObj2 = parentChildObj2.transform.GetChild(i).gameObject;
                                    break;
                                }
                            }

                            // 해당 코드 블럭이 IF, LOOP 일 경우, Toggle을 펼친상태로 만들고 활성화 시킴
                            // Toggle을 펼친상태 (ex. IF안에 IF가 있는 경우, 가장 밖에 있는 IF를 펼칠 경우
                            // 안에 있는 IF도 펼친 상태로 만드는 것)
                            if (ttemp.name.Contains("If") || ttemp.name.Contains("Loop"))
                                if (!ttemp.name.Contains("End")) {
                                    ttemp.transform.Find("Toggle").GetComponent<Toggle>().isOn = false;
                                    ttemp.transform.Find("Toggle").gameObject.SetActive(true);
                                }

                            // 해당 코드 블럭을 클릭 가능한 상태로 만듬
                            ttemp.GetComponent<Image>().raycastTarget = true;

                            // 입력이 들어가는 블럭의 경우 inputFieldObj 비활성화(Variable++, Variable=, Variable==, Delay)
                            if (ttemp.name.Contains("Variable") || ttemp.name.Contains("Delay")) {
                                for (int i = 0; i < ttemp.transform.childCount; i++) {
                                    GameObject inputFieldObj = ttemp.transform.GetChild(i).gameObject;
                                    if (inputFieldObj.name.Contains("InputField"))
                                        inputFieldObj.SetActive(true);
                                }
                            }

                            // 해당 코드 블럭을 완전 불투명 상태로 만듬
                            ttemp.GetComponent<Image>().color = Color.white;

                            // 해당 코드 블럭을 위에서 구한 부모 코드 블럭의 BtnChild로 강제 이동시킴
                            ttemp.transform.position = new Vector2(ttemp.transform.position.x, parentChildObj2.transform.position.y);
                        }

                        // 위 애니메이션 효과가 작동하지 않도록 List를 초기화 시킴
                        insideObjs.Clear();
                        break;
                    }
                }
            }
        }
    }

    public void Folding() {
        // List를 초기화
        insideObjs.Clear();

        // Folding 기능을 가지고 있는 오브젝트를 저장
        thisObj = this.transform.parent.gameObject;
        toggle = this.GetComponent<Toggle>();

        GameObject parentObj = thisObj;
        if (thisObj == null) {
            return;
        }

        // Folder의 짝을 맞추기 위한 Count
        int stackCnt = 1;

        // 더 이상 하위 코드 블러기 없을 경우 False
        // 짝을 찾았을 경우 False
        bool isFound = true;

        while (isFound) {
            isFound = false;

            // 현재 코드 블럭에 하위 코드 블럭이 있는 지 찾아서 처리
            for (int i = 0; i < parentObj.transform.childCount; i++) {
                GameObject temp = parentObj.transform.GetChild(i).gameObject;

                // Condition 블럭은 처리하지 않는다
                if (temp.name.Contains("=="))
                    continue;

                // stackCnt를 통해, if & loop의 짝을 맞춰 folding한다.
                if (temp.name.Contains("If") || temp.name.Contains("Loop"))
                    if (!temp.name.Contains("End"))
                        stackCnt++;
                if (temp.name.Contains("End"))
                    stackCnt--;

                // stackCnt가 0이라는 것은 짝을 찾았다는 의미이므로 List에 추가하는 것을 멈추고
                // Folding 애니메이션을 작동시킨다.
                if (stackCnt == 0) {

                    // 짝을 이루었을 경우는 stackCnt == 0
                    if (thisObj.name.Contains("If")) {
                        if (temp.name.Contains("If"))
                            break;
                    }
                    else if (thisObj.name.Contains("Loop")) {
                        if (temp.name.Contains("Loop"))
                            break;
                    }

                    // 짝이 이루어지지 않았을 경우 stackCnt == -1 로 종료
                    stackCnt = -1;
                    break;
                }

                // 하위 코드 블럭이 있을 경우
                // isFound를 true로 하고 List에 추가한다.
                // 그리고 현재 오브젝트를 갱신한다.
                if (temp.name.Contains("Clone")) {
                    insideObjs.Add(temp);
                    isFound = true;
                    parentObj = temp;
                    break;
                }
            }
        }

        // 탐색이 끝난경우, stackCnt가 0이 아니면 짝을 이루지 못하였다는 것이므로
        // List를 초기화하고 애니메이션 효과를 작동시키지 않는다.
        if (stackCnt != 0) {
            insideObjs.Clear();
            return;
        }

        // Toggle의 상태에 따라, 애니메이션 효과를 달리 작동시킨다.
        if (toggle.isOn)
            isFolding = true;
        else
            isFolding = false;
    }
}
