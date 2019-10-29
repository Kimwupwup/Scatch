using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_trigger : MonoBehaviour
{
    private GameObject trap;
    private Vector3 trapOriginPos;
    private Vector3 trapOnPos;
    private bool isTrapOn = false;

    void Start() {
        trap = this.transform.parent.gameObject;
        trapOriginPos = trap.transform.position;
        trapOnPos = trapOriginPos + new Vector3(0, 0.5f, 0);
    }
    // Update is called once per frame
    void Update()
    {
        if (isTrapOn) {
            trap.transform.position =
            Vector3.Lerp(trap.transform.position, trapOnPos,
            Time.deltaTime * 5f);
        } else {
            trap.transform.position =
            Vector3.Lerp(trap.transform.position, trapOriginPos,
            Time.deltaTime * 5f);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            Debug.Log("!");
            isTrapOn = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            isTrapOn = false;
        }
    }
}
