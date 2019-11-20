using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChat5 : MonoBehaviour {
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

        dialog.Add("이번이 튜토리얼 마지막 스테이지에요!");
        dialog.Add("뭔가 맵도 길고.. 코드 블럭도 길어요..");
        dialog.Add("우선 카메라를 조절해서 깃발이 어디있는지 찾아봐요!");
        dialog.Add("다섯 번은 앞으로 두 칸씩.. 마지막에는 한 칸 더 멀어요");
        dialog.Add("그냥 반복해서는 깰 수 없어요..");
        dialog.Add("변수와 [IF], [END IF] 블럭을 통해, 특정 조건에 맞으면 추가적인 행동을 할 수 있어요");
        dialog.Add("[□ = □] 블럭을 사용해서 먼저 변수를 선언해야 사용할 수 있어요");
        dialog.Add("[□ ++] 블럭은 해당 변수에 1을 더할 수 있어요");
        dialog.Add("[IF] 블럭 옆의\n[□ == □] 블럭이 조건 블럭이에요!\n변수가 그 숫자랑 같으면 내부의 코드 블럭이 실행돼요!");
        dialog.Add("[IF], [END IF] 블럭 안에 있는 [BREAK] 블럭은 반복문을 탈출하는 블럭이에요!");
        dialog.Add("[IF] 블럭을 사용하면, 꼭! \n[END IF] 블럭으로 닫아줘야 해요!");
        dialog.Add("옆에 있는 블럭처럼 만들어봐요!");

        content.text = dialog[currentDialog];
    }

    public void NextDialog() {
        if (currentDialog < dialog.Count - 1)
            content.text = dialog[++currentDialog];
        else
            dialogPanel.SetActive(false);
    }
}