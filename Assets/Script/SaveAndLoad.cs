using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveAndLoad : MonoBehaviour
{
    private bool isSaved = false;
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
        Debug.Log("!!!!!!!");
        if(isSaved == false)
        {
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
            for (int i = 0; i < codesQueue.Count; i++)
            {
                Debug.Log(codesQueue[i].name);
            }
        }
    }
}
