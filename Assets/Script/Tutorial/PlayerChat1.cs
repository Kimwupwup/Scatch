using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChat1 : MonoBehaviour {
    // Start is called before the first frame update
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

        dialog.Add("앞에 계단이 있어요!");
        dialog.Add("계단을 오르려면 점프해서 앞으로 가야겠죠?");
        dialog.Add("[JUMP] 블럭을 사용하면 한 칸 위로 점프할 수 있어요!");
        dialog.Add("좌측 상단에 있는 메뉴 버튼을 눌러 위에 보이는 코드와 같이 만들어주세요!");
        dialog.Add("블럭 아래에 다른 블럭을 올리면 자동으로 연결돼요!");
        dialog.Add("블럭을 만들었나요? 실행해서 확인해봐요!");

        content.text = dialog[currentDialog];
    }

    public void NextDialog() {
        if (currentDialog < dialog.Count - 1)
            content.text = dialog[++currentDialog];
        else
            dialogPanel.SetActive(false);
    }
}