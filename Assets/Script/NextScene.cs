﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NextScene : MonoBehaviour
{
    public void MoveMainScene() {
        SceneManager.LoadScene("stage_map_update");
        //SceneManager.LoadScene("test");
    }
}
