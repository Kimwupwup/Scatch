using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveAndLoad : MonoBehaviour {

    public List<GameObject> obj; // Function blocks preb

    public Jsondatas jsondatas;
    public Jsondata jsondata;
    List<Jsondata> jsonList = new List<Jsondata>();
    List<GameObject> ifLoopList = new List<GameObject>();

    private GameObject prefab;
    private List<GameObject> functions = new List<GameObject>();

    private GameObject tmpButton; // New function block object
    private GameObject parentbutton; // Parent object of current object

    /// <summary>
    /// Event : click on save button
    /// </summary>
    public void Save() {
        Debug.Log("save");

        // Clear functions list and json list.(Reset)
        functions.Clear();
        jsonList.Clear();

        GameObject code = GameObject.FindGameObjectWithTag("codePanel").transform.GetChild(1).gameObject;
        GameObject oldcode = null;
        List<GameObject> codesQueue = new List<GameObject>();

        // child[0] is "CODE" image.
        // For multiple code lines.
        for (int i = 0; i < code.transform.childCount; i++) {
            codesQueue.Add(code.transform.GetChild(i).gameObject);
        }

        // Sort multiple code lines
        // From left to right
        for (int i = 0; i < codesQueue.Count; i++) {
            for (int j = i + 1; j < codesQueue.Count; j++) {
                if (codesQueue[i].transform.position.x > codesQueue[j].transform.position.x) {
                    GameObject temp = codesQueue[i];
                    codesQueue[i] = codesQueue[j];
                    codesQueue[j] = temp;
                }
            }
        }

        for (int i = 0; i < codesQueue.Count; i++) {
            code = codesQueue[i];
            while (true) {
                //code.name = code.name.Substring(0, code.name.IndexOf("("));
                if (code == oldcode) {
                    break;
                }
                functions.Add(code);
                oldcode = code;
                for (int j = 0; j < code.transform.childCount; j++) {
                    //bool contains = code.name.Contains("clone");
                    if (code.transform.GetChild(j).name.Contains("Clone")) {
                        if (code.transform.GetChild(j).name == "BtnVariable==(Clone)") {
                            //code.transform.GetChild(j).name = code.transform.GetChild(j).name.Substring(0, code.transform.GetChild(j).name.IndexOf("("));
                            functions.Add(code.transform.GetChild(j).gameObject);
                        } else {
                            code = code.transform.GetChild(j).gameObject;
                            break;
                        }
                    }
                }
            }
        }

        for (int i = 0; i < functions.Count; i++) {
            Debug.Log(functions[i].name);
            jsonList.Add(new Jsondata(functions[i]));
        }
        jsondatas.datas = jsonList.ToArray();

        String savedata = JsonUtility.ToJson(jsondatas);

        Debug.Log(savedata);
        //   Debug.Log(Application.dataPath);
        //  Debug.Log(Application.persistentDataPath);

        //    CreateJsonFile(Application.dataPath + "\\savefiles", SceneManager.GetActiveScene().name + "_save", savedata);
        CreateJsonFile(Application.persistentDataPath, SceneManager.GetActiveScene().name + "_save", savedata);
    }

    public void load() {

        Debug.Log("load");
        //  Jsondatas loaddatas = LoadJsonFile(Application.dataPath + "\\savefiles", SceneManager.GetActiveScene().name + "_save");
        Jsondatas loaddatas = LoadJsonFile(Application.persistentDataPath, SceneManager.GetActiveScene().name + "_save");

        parentbutton = GameObject.FindGameObjectWithTag("codePanel").transform.GetChild(1).gameObject;
        //Debug.Log(loaddatas.datas);
        bool relocateFlag = false;
        if (loaddatas.datas[0].v.y > 980) {
            relocateFlag = true;
        }
        for (int i = 0; i < loaddatas.datas.Length; i++) {
            if (relocateFlag == true) {
                loaddatas.datas[i].v.y = loaddatas.datas[i].v.y - 987;
            }
            Debug.Log(loaddatas.datas[i].name);
            loaddatas.datas[i].name = loaddatas.datas[i].name.Substring(0, loaddatas.datas[i].name.IndexOf("("));
            for (int j = 0; j < obj.Count; j++) {
                if (obj[j].name == loaddatas.datas[i].name) {
                    prefab = obj[j];
                }
            }
            tmpButton = Instantiate(prefab, loaddatas.datas[i].v, Quaternion.identity, GameObject.FindGameObjectWithTag("codePanel").transform.GetChild(1)).gameObject;
            if (tmpButton.name.Contains("If") || tmpButton.name.Contains("Loop"))
                if (!tmpButton.name.Contains("End"))
                    ifLoopList.Add(tmpButton);

            Debug.Log(tmpButton.name);
            tmpButton.tag = "clone";

            // New button raycast on.
            tmpButton.GetComponent<Image>().raycastTarget = true;

            // New button turn on the child and condition.
            for (int j = 0; j < tmpButton.transform.childCount; j++) {
                tmpButton.transform.GetChild(j).gameObject.SetActive(true);
            }

            // Parent obj child and condition check.
            tmpButton.transform.SetParent(parentbutton.transform);
            for (int j = 0; j < parentbutton.transform.childCount; j++) {
                GameObject current = parentbutton.transform.GetChild(j).gameObject;
                if (current.tag == "child")current.SetActive(false);
                if (current.tag == "condition" && tmpButton.name.Contains("=="))current.SetActive(false);
            }

            // Insert the text.
            int idx = 0;
            for (int j = 0; j < tmpButton.transform.childCount; j++) {
                GameObject current = tmpButton.transform.GetChild(j).gameObject;
                if (current.name.Contains("InputField")) {
                    current.SetActive(true);
                    current.GetComponent<InputField>().text = loaddatas.datas[i].text[idx];
                    idx++;
                }
            }

            // Set parent.
            if (!tmpButton.name.Contains("==")) {
                parentbutton = tmpButton;
            }
        }

        // Unfolding
        for (int i = 0; i < ifLoopList.Count; i++) {
            GameObject parentTemp = ifLoopList[i];
            for (int j = 0; j < parentTemp.transform.childCount; j++) {
                GameObject childTemp = parentTemp.transform.GetChild(j).gameObject;
                if (childTemp.name.Contains("Clone") && !childTemp.name.Contains("==")) {
                    if (childTemp.transform.position.y + 1 >= parentTemp.transform.position.y) {
                        parentTemp.transform.Find("Toggle").GetComponent<Toggle>().isOn = true;
                        parentTemp.transform.Find("Toggle").GetComponent<ScriptFolder>().Folding();
                    } else {
                        parentTemp.transform.Find("Toggle").GetComponent<Toggle>().isOn = false;
                        parentTemp.transform.Find("Toggle").GetComponent<ScriptFolder>().Folding();
                    }
                    break;
                }
            }
        }

        ifLoopList.Clear();
    }

    void CreateJsonFile(string createPath, string fileName, string jsonData) {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }
    Jsondatas LoadJsonFile(string loadPath, string fileName) {
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
public class Jsondata {
    public String name;
    public Vector3 v;
    public String parent;
    public String[] text = new String[2];

    public Jsondata(GameObject objt) {
        name = objt.name;
        v = objt.transform.position;
        parent = objt.transform.parent.gameObject.name;

        if (objt.transform.GetChild(0).name.Contains("InputField")) {
            int idx = 0;
            for (int i = 0; i < objt.transform.childCount; i++) {
                GameObject current = objt.transform.GetChild(i).gameObject;
                if (current.name.Contains("InputField")) {
                    if (string.IsNullOrEmpty(current.GetComponent<InputField>().text)) {
                        text[idx] = current.GetComponent<InputField>().placeholder.GetComponent<Text>().text;
                        idx++;
                    } else {
                        text[idx] = current.GetComponent<InputField>().text;
                        idx++;
                    }
                }
            }

        }
    }
}

[System.Serializable]
public class Jsondatas {
    public Jsondata[] datas;
}