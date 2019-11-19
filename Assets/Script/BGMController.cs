using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMController : MonoBehaviour
{
    private AudioSource bgm;
    private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        bgm = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();
        slider = transform.GetComponent<Slider>();
        slider.value = bgm.volume;
    }

    public void VolumeChanged() {
        bgm.volume = slider.value;
    }
}
