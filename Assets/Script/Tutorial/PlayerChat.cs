using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChat : MonoBehaviour {
    private Vector2 headPos;
    public GameObject dialogPanel;
    private List<string> dialog = new List<string>();
    private int currentDialog = 0;
    private Text content;

    void Start() {
        content = dialogPanel.transform.GetChild(0).GetComponent<Text>();
        headPos = this.transform.position;
        headPos.y += 1f;
        headPos.x -= 0.3f;
        dialogPanel.transform.position = headPos;

        dialog.Add("안녕하세요! 저는 널구리입니다!");
        dialog.Add("앞에 있는 깃발에 가면 다음 스테이지로 갈 수 있어요!");
        dialog.Add("좌측 상단에 있는 메뉴 버튼을 눌러 [MOVE] 버튼을 밑 CODE창에 올려주세요!");
        dialog.Add("[MOVE] 는 한 블럭당 앞으로 한 칸 이동해요!");
        dialog.Add("[MOVE] 를 올리고 중앙 상단에 있는 OFF토글을 누르면 실행됩니다!");

        content.text = dialog[currentDialog];
    }

    public void NextDialog() {
        if (currentDialog < dialog.Count - 1)
            content.text = dialog[++currentDialog];
        else
            dialogPanel.SetActive(false);
    }
}