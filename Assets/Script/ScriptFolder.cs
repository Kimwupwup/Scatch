using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptFolder : MonoBehaviour {
    private List<GameObject> insideObj = new List<GameObject>();
    private Toggle toggle = null;
    private bool isFolding = false;
    private GameObject thisObj;

    // Start is called before the first frame update
    void Start() {
        thisObj = this.transform.parent.gameObject;
        toggle = this.GetComponent<Toggle>();
    }

    void Update() {
        if (insideObj.Count != 0) {
            if (isFolding) {
                foreach (GameObject temp in insideObj) {
                    temp.GetComponent<Image>().color = Color.Lerp(temp.GetComponent<Image>().color,
                        new Color(255, 255, 255, 0), Time.deltaTime * 20f);
                    if (temp.name.Contains("If") || temp.name.Contains("Loop"))
                        if (!temp.name.Contains("End"))
                            temp.transform.Find("Toggle").gameObject.SetActive(false);
                    temp.GetComponent<Image>().raycastTarget = false;
                    temp.transform.position = Vector2.Lerp(temp.transform.position,
                        new Vector2(temp.transform.position.x, thisObj.transform.position.y), Time.deltaTime * 2f);
                    if (temp.transform.position.y >= thisObj.transform.position.y) {
                        foreach (GameObject ttemp in insideObj) {
                            ttemp.GetComponent<Image>().color = new Color(255, 255, 255, 0);
                            ttemp.transform.position = new Vector2(ttemp.transform.position.x, thisObj.transform.position.y);
                        }
                        Debug.Log("Good");
                        insideObj.Clear();
                        break;
                    }
                }
            }
            else {
                foreach (GameObject temp in insideObj) {
                    GameObject parentChildObj = temp.transform.parent.gameObject;
                    for (int i = 0; i < parentChildObj.transform.childCount; i++) {
                        if (parentChildObj.transform.GetChild(i).name.Equals("BtnChild")) {
                            parentChildObj = parentChildObj.transform.GetChild(i).gameObject;
                            break;
                        }
                    }
                    temp.GetComponent<Image>().color = Color.Lerp(temp.GetComponent<Image>().color,
                        Color.white, Time.deltaTime * 20f);

                    temp.transform.position = Vector2.Lerp(temp.transform.position,
                        new Vector2(temp.transform.position.x, parentChildObj.transform.position.y), Time.deltaTime * 5f);
                    if (temp.transform.position.y <= parentChildObj.transform.position.y + 1) {
                        foreach (GameObject ttemp in insideObj) {
                            GameObject parentChildObj2 = ttemp.transform.parent.gameObject;
                            for (int i = 0; i < parentChildObj2.transform.childCount; i++) {
                                if (parentChildObj2.transform.GetChild(i).name.Equals("BtnChild")) {
                                    parentChildObj2 = parentChildObj2.transform.GetChild(i).gameObject;
                                    break;
                                }
                            }
                            if (ttemp.name.Contains("If") || ttemp.name.Contains("Loop"))
                                if (!ttemp.name.Contains("End")) {
                                    ttemp.transform.Find("Toggle").GetComponent<Toggle>().isOn = false;
                                    ttemp.transform.Find("Toggle").gameObject.SetActive(true);
                                }

                            ttemp.GetComponent<Image>().raycastTarget = true;
                            ttemp.GetComponent<Image>().color = Color.white;
                            ttemp.transform.position = new Vector2(ttemp.transform.position.x, parentChildObj2.transform.position.y);
                        }
                        Debug.Log("Good");
                        insideObj.Clear();
                        break;
                    }
                }
            }
        }
    }

    public void Folding() {

        insideObj.Clear();
        GameObject parentObj = thisObj;
        if (thisObj == null) {
            return;
        }
        int stackCnt = 1;

        bool isFound = true;
        while (isFound) {
            isFound = false;
            for (int i = 0; i < parentObj.transform.childCount; i++) {
                GameObject temp = parentObj.transform.GetChild(i).gameObject;

                if (temp.name.Contains("=="))
                    continue;

                // stackCnt를 통해, if & loop의 짝을 맞춰 folding한다.
                if (temp.name.Contains("If") || temp.name.Contains("Loop"))
                    if (!temp.name.Contains("End"))
                        stackCnt++;

                if (temp.name.Contains("End"))
                    stackCnt--;

                if (stackCnt == 0)
                    break;

                if (temp.name.Contains("Clone")) {
                    insideObj.Add(temp);
                    isFound = true;
                    parentObj = temp;
                    break;
                }
            }
        }
        if (stackCnt != 0) {
            insideObj.Clear();
            return;
        }

        if (toggle.isOn)
            isFolding = true;
        else
            isFolding = false;
    }
}
