using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenResolution : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        switch (Screen.width + Screen.height) {
            case 3000:
                Screen.SetResolution(1080, 1920, true);
                break;
            case 3120:
                Screen.SetResolution(1200, 1920, true);
                break;
            case 3300:
                Screen.SetResolution(1080, 2220, true);
                break;
            case 4300:
                Screen.SetResolution(1440, 2960, true);
                break;
        }
    }
}
