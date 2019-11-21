using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {
    private GameObject tmpButton;
    private GameObject objController;
    private Controller controller;
    private GameObject objTarget;
    private MenuButton menuButton;
    private bool isSetMenu;

    public void Awake() {
        menuButton = GameObject.FindGameObjectWithTag("menu_controller").GetComponent<MenuButton>();
        objController = GameObject.FindGameObjectWithTag("controller");
        controller = objController.GetComponent<Controller>();
    }
    //------------------------------------------------------------------------------------------------------
    public void OnBeginDrag(PointerEventData eventData) {
        isSetMenu = menuButton.GetMenuPanel();
        if (this.CompareTag("button") == true) {
            tmpButton = Instantiate(this, Input.mousePosition, Quaternion.identity, GameObject.FindGameObjectWithTag("canvas").transform).gameObject;
            tmpButton.tag = "clone";
            tmpButton.GetComponent<Image>().raycastTarget = false;
        } else {
            tmpButton = this.gameObject;

            if (tmpButton.name == "BtnVariable=(Clone)") {
                tmpButton.transform.GetChild(0).gameObject.SetActive(false);
            }
            if (tmpButton.name == "BtnVariable++(Clone)") {
                tmpButton.transform.GetChild(0).gameObject.SetActive(false);
            }
            if (tmpButton.name == "BtnVariable==(Clone)") {
                tmpButton.transform.GetChild(0).gameObject.SetActive(false);
            }
            if (tmpButton.name == "BtnVariable!=(Clone)") {
                tmpButton.transform.GetChild(0).gameObject.SetActive(false);
            }

            if (tmpButton.name == "BtnVariable==(Clone)" || tmpButton.name == "BtnVariable!=(Clone)") {
                if (tmpButton.transform.parent.GetChild(1).CompareTag("condition")) {
                    tmpButton.transform.parent.GetChild(1).gameObject.SetActive(true);
                }
            } else {
                if (!tmpButton.transform.parent.parent.CompareTag("codePanel")) {
                    int size = tmpButton.transform.parent.childCount;
                    for (int i = 0; i < size; i++) {
                        if (tmpButton.transform.parent.GetChild(i).CompareTag("child")) {
                            tmpButton.transform.parent.GetChild(i).gameObject.SetActive(true);
                        }
                    }
                }
            }
            tmpButton.transform.SetParent(GameObject.FindGameObjectWithTag("canvas").transform);
            tmpButton.GetComponent<Image>().raycastTarget = false;
        }
    }
    //---------------------------------------------------------------------------------------------
    public void OnDrag(PointerEventData eventData) {
        tmpButton.transform.position = Input.mousePosition;

        bool isTrue = controller.GetIsCodePanel();

        if (isTrue) {
            menuButton.SetMenuPanel(false);
        }
    }
    //-----------------------------------------------------------------------------
    public void OnEndDrag(PointerEventData eventData) {
        if (isSetMenu)
            menuButton.SetMenuPanel(true);

        for (int i = 0; i < tmpButton.transform.childCount; i++) {
            if (tmpButton.transform.GetChild(i).name.Contains("Clone")) {
                if (tmpButton.transform.GetChild(i).name.Contains("==") || tmpButton.transform.GetChild(i).name.Contains("!=")) {
                    tmpButton.transform.Find("BtnCondition").gameObject.SetActive(false);
                } else {
                    tmpButton.transform.Find("BtnChild").gameObject.SetActive(false);
                }
            } else {
                tmpButton.transform.GetChild(i).gameObject.SetActive(true);
            }
        }

        // 우선 마우스가 코드 패널 위에 있는지 확인한다.
        bool isTrue = controller.GetIsCodePanel();

        // 코드 패널이 아닐 경우는 삭제하고, 코드 패널일 경우에는
        // 코드 패널 오브젝트 내부의 realCodePanel 오브젝트에 넣는다.
        // 넣게 되면 raycast를 켜줘야 클릭할 수 있는 상태가 된다.
        if (!isTrue) {
            Destroy(tmpButton);
            return;
        } else {
            tmpButton.transform.SetParent(GameObject.FindGameObjectWithTag("codePanel").transform.GetChild(1));
            tmpButton.GetComponent<Image>().raycastTarget = true;
        }

        // 마우스가 다른 코드 블럭의 자식으로 위치하는지 확인한다.
        bool isChild = controller.GetIsCodeChild();

        // 마우스가 다른 코드 블럭의 자식으로 위치한다면 해당 자식으로 추가한다.
        if (isChild) {
            objTarget = controller.GetObjTarget();

            if (objTarget != null) {
                Debug.Log(objTarget.transform.parent.name);
            }

            // 현 블럭이 condition 블럭일 경우
            if (objTarget.CompareTag("condition") && (tmpButton.name == "BtnVariable!=(Clone)" || tmpButton.name == "BtnVariable==(Clone)")) {
                float temp = tmpButton.GetComponent<RectTransform>().rect.width;
                temp = objTarget.GetComponent<RectTransform>().rect.width - temp;

                // 블럭의 위치를 잡아준다.
                tmpButton.transform.position =
                    new Vector3(objTarget.transform.position.x - (temp / 2) * (Screen.height / 2960f) * 0.75f,
                        objTarget.transform.position.y + 10, 0);

                tmpButton.transform.SetParent(objTarget.transform.parent);
                tmpButton.GetComponent<Image>().raycastTarget = true;
                objTarget.SetActive(false);
                controller.SetObjTarget(null);
                controller.SetIsCodeChild(false);
                return;
            }

            // 현 블럭이 condition 블럭이 아닐 경우
            if (objTarget.CompareTag("child") && (tmpButton.name != "BtnVariable!=(Clone)" && tmpButton.name != "BtnVariable==(Clone)")) {
                float temp = tmpButton.GetComponent<RectTransform>().rect.width;
                temp = objTarget.GetComponent<RectTransform>().rect.width - temp;

                // 블럭의 위치를 잡아준다.
                if (tmpButton.name.Contains("End")) {
                    tmpButton.transform.position =
                        new Vector3(objTarget.transform.position.x - 30f - (temp / 2) * (Screen.height / 2960f) * 0.75f,
                            objTarget.transform.position.y, 0);
                } else {
                    tmpButton.transform.position =
                        new Vector3(objTarget.transform.position.x - (temp / 2) * (Screen.height / 2960f) * 0.75f,
                            objTarget.transform.position.y, 0);
                }

                tmpButton.transform.SetParent(objTarget.transform.parent);
                tmpButton.GetComponent<Image>().raycastTarget = true;
                objTarget.SetActive(false);
                controller.SetObjTarget(null);
                controller.SetIsCodeChild(false);
                return;
            }
        }
    }
}