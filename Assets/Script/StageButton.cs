using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageButton : MonoBehaviour
{
    private string btnString;

    // Start is called before the first frame update
    void Start()
    {
        btnString = this.name;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(btnString);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
