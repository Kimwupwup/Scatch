using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private bool isCodePanel;
    private bool isCodeChild;
    private GameObject objTarget;

    public bool GetIsCodePanel() {
        return isCodePanel;
    }

    public void SetIsCodePanel(bool a) {
        isCodePanel = a;
    }

    public bool GetIsCodeChild() {
        return isCodeChild;
    }

    public void SetIsCodeChild(bool a) {
        isCodeChild = a;
    }

    public GameObject GetObjTarget() {
        return objTarget;
    }

    public void SetObjTarget(GameObject a) {
        objTarget = a;
    }
}
