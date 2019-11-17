using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapStageUpdate : MonoBehaviour {
    public Button btnLeft;
    public Button btnRight;
    public Button btnJump;
    public Text content;
    public GameObject stageSet;
    public GameObject stageCheckMessage;

    private GameObject player;
    private GameObject currentStage;
    private Camera came;
    private Button btnYes;
    private Button btnNo;

    private Vector3 targetPos;

    private int clearIdx = 0;
    private int currentIdx = 0;
    private int maxIdx = 0;

    // Start is called before the first frame update
    void Start() {
        // 이벤트 콜백
        btnLeft.onClick.AddListener(BtnLeftOnClick);
        btnRight.onClick.AddListener(BtnRightOnClick);
        btnJump.onClick.AddListener(BtnJumpOnClick);

        // 버튼 이름이 바뀔 수 있음.
        btnYes = stageCheckMessage.transform.GetChild(0).Find("BtnConfirm").GetComponent<Button>();
        btnNo = stageCheckMessage.transform.GetChild(0).Find("BtnCancel").GetComponent<Button>();

        // 이벤트 콜백
        btnYes.onClick.AddListener(BtnYesOnClick);
        btnNo.onClick.AddListener(BtnNoOnClick);

        came = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player");

        maxIdx = stageSet.transform.childCount;

        // 현재까지 깬 Stage 가져옴
        currentIdx = GameObject.Find("Canvas").GetComponent<StageSaveAndLoad>().quitScene;
        clearIdx = GameObject.Find("Canvas").GetComponent<StageSaveAndLoad>().curScene;

        // 마지막 라운드 클리어시 오류 예외처리
        if (clearIdx >= maxIdx) clearIdx -= 1;

        stageSet.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        stageSet.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;

        // Lock 기능(깬 스테이지 까지만 열림)
        for (int i = 1; i <= clearIdx; i++) {
            stageSet.transform.GetChild(i).GetChild(2).gameObject.SetActive(false);
            stageSet.transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = true;
        }

        
        // 초기화면을 해당 스테이지로 이동
        currentStage = stageSet.transform.GetChild(currentIdx).gameObject;
        targetPos.y = came.transform.position.y;
        targetPos.x = currentStage.transform.position.x;
        targetPos.z = -10f;
        came.transform.position = targetPos;

        // 플레이어도 같이 이동
        targetPos.y = player.transform.position.y;
        player.transform.position = targetPos;
    }

    // Update is called once per frame
    void Update() {
        // 카메라 이동(현재 인덱스에 따라 이동함)
        currentStage = stageSet.transform.GetChild(currentIdx).gameObject;
        targetPos.y = came.transform.position.y;
        targetPos.x = currentStage.transform.position.x;
        targetPos.z = -10f;
        came.transform.position = Vector3.Lerp(came.transform.position, targetPos, Time.deltaTime * 4f);
    }

    /// <summary>
    /// 왼쪽 버튼 이벤트 콜백(클릭)
    /// 플레이어 스프라이트를 flipX
    /// </summary>
    public void BtnLeftOnClick() {
        player.GetComponent<SpriteRenderer>().flipX = false;
        if (currentIdx > 0)
            currentIdx--;
        else {
            player.GetComponent<SpriteRenderer>().flipX = true;
            currentIdx = maxIdx - 1;    // First -> Last  
        }
    }

    /// <summary>
    /// 오른쪽 버튼 이벤트 콜백(클릭)
    /// 플레이어 스프라이트르 flipX
    /// </summary>
    public void BtnRightOnClick() {
        player.GetComponent<SpriteRenderer>().flipX = true;
        if (currentIdx < maxIdx - 1)
            currentIdx++;
        else {
            player.GetComponent<SpriteRenderer>().flipX = false;
            currentIdx = 0;        //  Last -> First
        }
    }

    /// <summary>
    /// 스테이지를 감싸고 있는 버튼
    /// 메시지창에 있는 스테이지 번호 이미지를 변경(추후 변경 하여야함 - 한자리 수에만 대응)
    /// 메시지창 활성화
    /// </summary>
    public void BtnJumpOnClick() {
        if (currentIdx != 0)
            content.text = $"[스테이지{currentIdx}] 을(를) 플레이 하시겠습니까?";
        else
            content.text = $"튜토리얼을 플레이 하시겠습니까?";
        stageCheckMessage.SetActive(true);
    }

    /// <summary>
    /// 메시지창에서 확인(예) 버튼을 눌렀을 때, 플레이어가 점프하도록 함
    /// </summary>
    public void BtnYesOnClick() {
        stageCheckMessage.SetActive(false);
        Rigidbody2D playerRig = player.GetComponent<Rigidbody2D>();
        playerRig.AddForce(Vector2.up * 7f, ForceMode2D.Impulse);
    }

    /// <summary>
    /// 메시지창에서 취소(아니요) 버튼을 눌렀을 때, 메시지창을 비활성화 함
    /// </summary>
    public void BtnNoOnClick() {
        stageCheckMessage.SetActive(false);
    }
}
