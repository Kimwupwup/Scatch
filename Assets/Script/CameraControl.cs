using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour {
    public Vector3 offset;
    private Transform objTransform;
    private Camera came;
    private bool isFreeView = false;

    private ViewPanelPointerHandle viewPanel;

    // touch event values
    private Vector2 prevPos;
    private Vector2 curPos;
    private Vector2 movePos;
    private float speed = 0.3f;

    private float touchData;

    // Start is called before the first frame update
    void Start() {
        objTransform = GameObject.FindGameObjectWithTag("Player").transform;
        came = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        viewPanel = GameObject.FindGameObjectWithTag("viewPanel").GetComponent<ViewPanelPointerHandle>();
    }

    // Update is called once per frame
    void Update() {
        if (!isFreeView) {
            came.transform.position = Vector2.Lerp(came.transform.position, objTransform.position + offset, 2f * Time.deltaTime);
            came.transform.Translate(0, 0, -10);
        } else {
            if (viewPanel.cnt == 1) {
                Touch touch = Input.GetTouch(0); //먼저 터치가 된 녀석이 0번째 
                if (touch.phase == TouchPhase.Began) { //터치가 된 상태냐
                    prevPos = touch.position - touch.deltaPosition;
                } else if (touch.phase == TouchPhase.Moved) { //움직이고 있다면
                    curPos = touch.position - touch.deltaPosition;
                    movePos = (prevPos - curPos) * speed * Time.deltaTime;
                    if (movePos.x < 1 && movePos.x > -1 && movePos.y < 1 && movePos.y > -1) {
                        came.transform.Translate(movePos);
                        prevPos = touch.position - touch.deltaPosition;
                    }
                }
            } else if (viewPanel.cnt == 2) { //줌 인 아웃!
                curPos = Input.GetTouch(0).position - Input.GetTouch(1).position;
                prevPos = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) -
                    (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition)); //여기까지는 지금 포스와 전 포스의 거리 차를 구하는 걸로 이해된다.
                touchData = curPos.magnitude - prevPos.magnitude; //magnityude는 제곱근을 계산해주는 걸로 알고있다.
                came.orthographicSize -= touchData * 0.03f;
                came.orthographicSize = Mathf.Max(came.orthographicSize, 1f);
                came.orthographicSize = Mathf.Min(came.orthographicSize, 15f);
            } else {
                viewPanel.cnt = 0;
            }
        }
    }

    public void FreeViewButtonHandler() {
        isFreeView = this.GetComponent<Toggle>().isOn;
    }
}