using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChat4 : MonoBehaviour {
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

        dialog.Add("빨간색 깃발이 어디있는지 모르겠어..");
        dialog.Add("좌측 상단에 있는 눈 모양의 아이콘을 눌러봐요!");
        dialog.Add("눈 아이콘은 카메라를 조작하는 아이콘이에요");
        dialog.Add("비활성화 되어있으면 카메라가 고정되고, 활성화되면 카메라를 움직일 수 있어요!");
        dialog.Add("한 손가락으로는 움직이고, 두 손가락으로는 확대/축소 할 수 있어요!");
        dialog.Add("깃발이 멀리있어요.. [MOVE] 블럭을 쓰기에는 너무 많을 것 같아요");
        dialog.Add("[LOOP], [END LOOP] 블럭을 사용하면 그 안에 있는 블럭들을 계속 반복할 수 있어요!");
        dialog.Add("[LOOP] 블럭을 사용하면, 꼭! \n[END LOOP] 블럭으로 닫아줘야 해요!");
        dialog.Add("위에 있는 블럭처럼 만들어봐요!");

        content.text = dialog[currentDialog];
    }

    public void NextDialog() {
        if (currentDialog < dialog.Count - 1)
            content.text = dialog[++currentDialog];
        else
            dialogPanel.SetActive(false);
    }
}