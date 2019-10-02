using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveAndLoad : MonoBehaviour
{
    private List<GameObject> functions = new List<GameObject>();
    private bool isSaved = false;
    private int loopCnt = 0;

    private GameObject tmpButton;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Save()
    {
        Debug.Log("save");
        if (isSaved == true)
        {
            isSaved = false;
            functions.Clear();
        }


        GameObject code = GameObject.FindGameObjectWithTag("codePanel");
        List<GameObject> codesQueue = new List<GameObject>();
        GameObject temp;
        for (int i = 1; i < code.transform.childCount; i++)
        {
            codesQueue.Add(code.transform.GetChild(i).gameObject);
        }

        // Sort multiple code lines
        for (int i = 0; i < codesQueue.Count; i++)
        {
            for (int j = i + 1; j < codesQueue.Count; j++)
            {
                if (codesQueue[i].transform.position.x > codesQueue[j].transform.position.x)
                {
                    temp = codesQueue[i];
                    codesQueue[i] = codesQueue[j];
                    codesQueue[j] = temp;
                }
            }
        }

        loopCnt = 0;
        for (int i = 0; i < codesQueue.Count; i++)
        {

            code = codesQueue[i];

            while (true)
            {
                code.name = code.name.Substring(0, code.name.IndexOf("("));
                functions.Add(code);
                if (code.transform.childCount < 2)
                {
                    break;
                }
                loopCnt++;

                for (int j = 0; j < code.transform.childCount; j++)
                {
                    //bool contains = code.name.Contains("clone");
                    if (code.transform.GetChild(j).name.Contains("Clone"))
                    {
                        if (code.transform.GetChild(j).name == "BtnVariable==(Clone)")
                        {
                            code.transform.GetChild(j).name = code.transform.GetChild(j).name.Substring(0, code.transform.GetChild(j).name.IndexOf("("));
                            functions.Add(code.transform.GetChild(j).gameObject);
                        }
                        else
                        {
                            code = code.transform.GetChild(j).gameObject;
                        }

                    }
                }
            }
        }

        for (int i = 0; i < functions.Count; i++)
        {
            Debug.Log(functions[i].name);
        }
        isSaved = true;
    }

    public void load()
    {
        Debug.Log("load");
        for(int i = 0; i < functions.Count; i++)
        {
            tmpButton = Instantiate(functions[i], functions[i].transform.position, Quaternion.identity);

            Debug.Log(functions[i].name);
        }
    }


}
