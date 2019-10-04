using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using System;
using Newtonsoft.Json;

public class SaveAndLoad : MonoBehaviour
{

    public List<GameObject> obj;
    public Jsondatas jsondatas;
    public Jsondata jsondata;
    List<Jsondata> jsonList = new List<Jsondata>();


    GameObject prefab;
    private List<GameObject> functions = new List<GameObject>();
    private bool isSaved = false;
    
    private GameObject tmpButton;
    private GameObject parentbutton;
    

    

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
            functions.Clear();
            jsonList.Clear();
        }


        GameObject code = GameObject.FindGameObjectWithTag("codePanel");
        GameObject oldcode = null;
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

            code = codesQueue[i];

            while (true)
            {

                //code.name = code.name.Substring(0, code.name.IndexOf("("));
                
                if (code == oldcode)
                {
                    break;
                }
                functions.Add(code);
                oldcode = code;
                for (int j = 0; j < code.transform.childCount; j++)
                {
                    //bool contains = code.name.Contains("clone");
                    if (code.transform.GetChild(j).name.Contains("Clone"))
                    {
                        if (code.transform.GetChild(j).name == "BtnVariable==(Clone)")
                        {
                            //code.transform.GetChild(j).name = code.transform.GetChild(j).name.Substring(0, code.transform.GetChild(j).name.IndexOf("("));
                            functions.Add(code.transform.GetChild(j).gameObject);
                        }
                        else
                        {
                            code = code.transform.GetChild(j).gameObject;
                            break;
                        }

                    }
                }
            }
        }
        //String savedata = "";

        for(int i = 0; i < functions.Count; i++)
        {
            Debug.Log(functions[i].name);
            jsonList.Add(new Jsondata(functions[i]));
        }
        jsondatas.datas = jsonList.ToArray();
       
        String savedata = JsonUtility.ToJson(jsondatas);
/*        for(int i = 0; i < functions[0].transform.childCount; i++)
        {
           Debug.Log(functions[0].transform.GetChild(i).name);
        }
        Jsondata data = new Jsondata(functions[0]);
        savedata = JsonUtility.ToJson(data);*/
        //savedata = JsonConvert.SerializeObject(data);
        Debug.Log(savedata);
        CreateJsonFile(Application.dataPath, "savetest", savedata);

        isSaved = true;
    }

    public void load()
    {

        Debug.Log("load");
        Jsondatas loaddatas = LoadJsonFile(Application.dataPath, "savetest");
        parentbutton = GameObject.FindGameObjectWithTag("codePanel");
        //Debug.Log(loaddatas.datas);
        for (int i = 0; i < loaddatas.datas.Length; i++)
        {
            Debug.Log(loaddatas.datas[i].name);
            loaddatas.datas[i].name = loaddatas.datas[i].name.Substring(0, loaddatas.datas[i].name.IndexOf("("));
            for (int j = 0; j < obj.Count; j++)
            {
                if (obj[j].name == loaddatas.datas[i].name)
                {
                    prefab = obj[j];
                }
            }
            
            //tmpButton = Instantiate(prefab, functions[i].transform.position, Quaternion.identity, GameObject.FindGameObjectWithTag("codePanel").transform).gameObject;
            tmpButton = Instantiate(prefab, loaddatas.datas[i].v, Quaternion.identity, GameObject.FindGameObjectWithTag("codePanel").transform).gameObject;
            Debug.Log(tmpButton.name);
            tmpButton.tag = "clone";
            tmpButton.GetComponent<Image>().raycastTarget = true;
            tmpButton.transform.SetParent(parentbutton.transform);
            if(loaddatas.datas[i].name == "BtnDelay(Clone)")
            {
                tmpButton.transform.GetChild(0).GetComponent<InputField>().text = loaddatas.datas[i].text;
            }
            else if(loaddatas.datas[i].name == "BtnVariable=(Clone)")
            {
                tmpButton.transform.GetChild(0).GetComponent<InputField>().text = loaddatas.datas[i].text1;
                tmpButton.transform.GetChild(1).GetComponent<InputField>().text = loaddatas.datas[i].text;
            }
            else if (loaddatas.datas[i].name == "BtnVariable++(Clone)")
            {
                tmpButton.transform.GetChild(0).GetComponent<InputField>().text = loaddatas.datas[i].text;
            }
            else if (loaddatas.datas[i].name == "BtnVariable==(Clone)")
            {
                tmpButton.transform.GetChild(0).GetComponent<InputField>().text = loaddatas.datas[i].text1;
                tmpButton.transform.GetChild(1).GetComponent<InputField>().text = loaddatas.datas[i].text;
            }
            if(tmpButton.name != "BtnVariable==(Clone)")
            {
                parentbutton = tmpButton;
   
            }

        }
    }

    void CreateJsonFile(string createPath, string fileName, string jsonData)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }
    Jsondatas LoadJsonFile(string loadPath, string fileName)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", loadPath, fileName), FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string readdata = Encoding.UTF8.GetString(data);
        //Debug.Log(readdata);
        return JsonUtility.FromJson<Jsondatas>(readdata);
    }

}
[System.Serializable]
public class Jsondata
{
    public String name;
    public Vector3 v;
    public String parent;
    public String text = "2";
    public String text1 = "a";
   
    public Jsondata(GameObject objt)
    {
        name = objt.name;
        v = objt.transform.position;
        parent = objt.transform.parent.gameObject.name;

        if (objt.transform.GetChild(0).name.Contains("InputField"))
        {
            if (objt.transform.GetChild(0).name.Contains("(1)"))
            {
                if (!string.IsNullOrEmpty(objt.transform.GetChild(0).GetComponent<InputField>().text))
                {
                    text1 = objt.transform.GetChild(0).GetComponent<InputField>().text;
                }
                else
                {
                    text1 = "a";
                }
                if (!string.IsNullOrEmpty(objt.transform.GetChild(1).GetComponent<InputField>().text))
                {
                    text = objt.transform.GetChild(1).GetComponent<InputField>().text;
                }
                else
                {
                    text = "2";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(objt.transform.GetChild(0).GetComponent<InputField>().text))
                {
                    text = objt.transform.GetChild(0).GetComponent<InputField>().text;
                }
                else
                {
                    if(objt.name.Contains("Delay"))
                    {
                        text = "2";
                    }
                    else
                    {
                        text = "a";
                    }
                }
            }


        }

        /*for (int i = 0; i < objt.transform.childCount; i++)
        {
            if (objt.transform.GetChild(i).name.Contains("Clone"))
            {
                Debug.Log(objt.transform.GetChild(i).name);
                Jsondata childjson = new Jsondata(objt.transform.GetChild(i).gameObject);

                child = JsonUtility.ToJson(childjson);
            }
        }*/
    }

}

[System.Serializable]
public class Jsondatas
{
    public Jsondata[] datas;
}