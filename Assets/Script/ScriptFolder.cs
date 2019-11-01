using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptFolder : MonoBehaviour {
    private List<GameObject> insideObj = new List<GameObject>();
    private Toggle toggle = null;
    private int functionType = 0;
    private bool isFolding = false;
    private GameObject thisObj;

    // Start is called before the first frame update
    void Start() {
        thisObj = this.transform.parent.gameObject;
        toggle = this.GetComponent<Toggle>();
        if (this.name.Equals("BtnIf(Clone)")) {
            functionType = 0;
        }
        else if (this.name.Equals("BtnLoop(Clone)")) {
            functionType = 1;
        }
    }

    void Update() {
        if (insideObj.Count != 0) {
            if (isFolding) {
                foreach (GameObject temp in insideObj) {
                    temp.GetComponent<Image>().color = Color.Lerp(temp.GetComponent<Image>().color,
                        new Color(255, 255, 255, 0), Time.deltaTime * 20f);
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
        bool isFound = true;
        while (isFound) {
            isFound = false;
            for (int i = 0; i < parentObj.transform.childCount; i++) {
                if (parentObj.transform.GetChild(i).gameObject.name.Contains("=="))
                    continue;
                if (parentObj.transform.GetChild(i).gameObject.name.Contains("End"))
                    continue;
                if (parentObj.transform.GetChild(i).gameObject.name.Contains("Clone")) {
                    insideObj.Add(parentObj.transform.GetChild(i).gameObject);
                    isFound = true;
                    parentObj = parentObj.transform.GetChild(i).gameObject;
                    break;
                }
            }
        }
        if (toggle.isOn)
            isFolding = true;
        else 
            isFolding = false;
    }
}
