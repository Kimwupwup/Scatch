using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
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

            if (tmpButton.name == "BtnCount(Clone)") {
                if (tmpButton.transform.parent.GetChild(1).CompareTag("condition")) {
                    tmpButton.transform.parent.GetChild(1).gameObject.SetActive(true);
                }
            } else {
                if (!tmpButton.transform.parent.CompareTag("codePanel")) {
                    if (tmpButton.transform.parent.GetChild(0).CompareTag("child")) {
                        tmpButton.transform.parent.GetChild(0).gameObject.SetActive(true);
                    } else if (tmpButton.transform.parent.GetChild(1).CompareTag("child")) {
                        tmpButton.transform.parent.GetChild(1).gameObject.SetActive(true);
                    } else if (tmpButton.transform.parent.name == "BtnVariable=(Clone)") {
                        if (tmpButton.transform.parent.GetChild(2).CompareTag("child")) {
                            tmpButton.transform.parent.GetChild(2).gameObject.SetActive(true);
                        }
                    }
                }
            }
            tmpButton.transform.SetParent(GameObject.FindGameObjectWithTag("canvas").transform);
            tmpButton.GetComponent<Image>().raycastTarget = false;
        }        
    }

    public void OnDrag(PointerEventData eventData) {
        tmpButton.transform.position = Input.mousePosition;
        
        bool isTrue = controller.GetIsCodePanel();

        if (isTrue) {
            menuButton.SetMenuPanel(false);
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (isSetMenu)
            menuButton.SetMenuPanel(true);
        
        for (int i = 0; i < tmpButton.transform.childCount; i++) {
            tmpButton.transform.GetChild(i).gameObject.SetActive(true);
        }

        bool isTrue = controller.GetIsCodePanel();

        if (!isTrue) {
            Destroy(tmpButton);
            Debug.Log("Destroy");
        } else {
            tmpButton.transform.SetParent(GameObject.FindGameObjectWithTag("codePanel").transform);
            tmpButton.GetComponent<Image>().raycastTarget = true;
        }

        bool isChild = controller.GetIsCodeChild();

        if (isChild) {
            objTarget = controller.GetObjTarget();

            if (objTarget != null) {
                Debug.Log(objTarget.transform.parent.name);
            }

            if (objTarget.CompareTag("condition") && (tmpButton.name == "BtnCount(Clone)" || tmpButton.name == "BtnVariable==(Clone)")) {
                float temp = tmpButton.GetComponent<RectTransform>().rect.width;
                temp = objTarget.GetComponent<RectTransform>().rect.width - temp;

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

            if (objTarget.CompareTag("child") && (tmpButton.name != "BtnCount(Clone)" && tmpButton.name != "BtnVariable==(Clone)")) {
                float temp = tmpButton.GetComponent<RectTransform>().rect.width;
                temp = objTarget.GetComponent<RectTransform>().rect.width - temp;

                tmpButton.transform.position =
                    new Vector3(objTarget.transform.position.x - (temp / 2) * (Screen.height / 2960f) * 0.75f,
                    objTarget.transform.position.y, 0);

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
