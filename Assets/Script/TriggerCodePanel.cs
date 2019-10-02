using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TriggerCodePanel : MonoBehaviour {

    private GameObject objController;
    private Controller controller;

    public void Awake() {
        objController = GameObject.FindGameObjectWithTag("controller");
        controller = objController.GetComponent<Controller>();
    }

    public void setTrue() {
        controller.SetIsCodePanel(true);
    }

    public void setFalse() {
        controller.SetIsCodePanel(false);
    }
}
