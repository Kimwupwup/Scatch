using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapStageUpdate : MonoBehaviour
{
    public Button btnLeft;
    public Button btnRight;
    public Button btnJump;
    public GameObject stageSet;
    public GameObject stageCheckMessage;
    public Sprite[] numbers;

    private GameObject player;
    private Camera came;

    private Button btnYes;
    private Button btnNo;

    private Vector3 targetPos;

    private int currentIdx = 0;
    private int maxIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        btnLeft.onClick.AddListener(BtnLeftOnClick);
        btnRight.onClick.AddListener(BtnRightOnClick);
        btnJump.onClick.AddListener(BtnJumpOnClick);

        // 버튼 이름이 바뀔 수 있음.
        btnYes = stageCheckMessage.transform.Find("BtnConfirm").GetComponent<Button>();
        btnNo = stageCheckMessage.transform.Find("BtnCancel").GetComponent<Button>();
        btnYes.onClick.AddListener(BtnYesOnClick);
        btnNo.onClick.AddListener(BtnNoOnClick);

        came = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player");

        maxIdx = stageSet.transform.childCount;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject currentStage = stageSet.transform.GetChild(currentIdx).gameObject;
        targetPos.y = came.transform.position.y;
        targetPos.x = currentStage.transform.position.x;
        targetPos.z = -10f;
        came.transform.position = Vector3.Lerp(came.transform.position, targetPos, Time.deltaTime * 4f);
    }

    public void BtnLeftOnClick() {
        player.GetComponent<SpriteRenderer>().flipX = false;
        if (currentIdx > 0)
            currentIdx--;
    }

    public void BtnRightOnClick() {
        player.GetComponent<SpriteRenderer>().flipX = true;
        if (currentIdx < maxIdx - 1)
            currentIdx++;
    }

    public void BtnJumpOnClick() {
        stageCheckMessage.transform.Find("stageNumber").GetComponent<Image>().sprite = numbers[currentIdx + 1];
        stageCheckMessage.SetActive(true);
    }

    public void BtnYesOnClick() {
        stageCheckMessage.SetActive(false);
        Rigidbody2D playerRig = player.GetComponent<Rigidbody2D>();
        playerRig.AddForce(Vector2.up * 7f, ForceMode2D.Impulse);
    }

    public void BtnNoOnClick() {
        stageCheckMessage.SetActive(false);
    }
}
