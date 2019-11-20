using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChat2 : MonoBehaviour {
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

        dialog.Add("앞에 불이 길을 막고 있어요...");
        dialog.Add("불을 점프해서 피해가면 될 것 같아요!");
        dialog.Add("[MOVE] 는 한칸 이동하니까.. 두 개 사용하면 깰 수 있어요!");
        dialog.Add("좌측 상단에 있는 메뉴 버튼을 눌러 위에 보이는 코드와 같이 만들어 주세요!");
        dialog.Add("앞에서 했던 것처럼 블럭을 아래로 연결하면 돼요!");
        dialog.Add("한번 해볼까요?");

        content.text = dialog[currentDialog];
    }

    public void NextDialog() {
        if (currentDialog < dialog.Count - 1)
            content.text = dialog[++currentDialog];
        else
            dialogPanel.SetActive(false);
    }
}