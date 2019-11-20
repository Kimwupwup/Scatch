using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChat3 : MonoBehaviour {
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

        dialog.Add("깃발이 하나 더 있어요..");
        dialog.Add("노란색 깃발은 2초 동안 가만히 있으면 점령할 수 있어요!");
        dialog.Add("노란색 깃발을 점령하지 않으면 빨간색 깃발이 활성화되지 않아요");
        dialog.Add("[DELAY □] 블럭을 사용하면 그 시간동안 가만히 있을 수 있어요!");
        dialog.Add("[DELAY □] 블럭 안에 있는 숫자는 바꿀 수 있어요");
        dialog.Add("좌측 상단에 있는 메뉴 버튼을 눌러 위에 보이는 코드와 같이 만들어 주세요!");
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