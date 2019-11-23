using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMController : MonoBehaviour
{
    private AudioSource bgm;
    private Slider sliderVolume;
    private Toggle toggleMute;

    private float tempVolume;
    private bool tempMute;
    // Start is called before the first frame update
    void Start()
    {
        bgm = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();
        sliderVolume = transform.GetChild(0).GetComponent<Slider>();
        toggleMute = transform.GetChild(1).GetComponent<Toggle>();
        sliderVolume.onValueChanged.AddListener(VolumeChanged);
        toggleMute.onValueChanged.AddListener(VolumeMute);
        sliderVolume.value = bgm.volume;
        toggleMute.isOn = bgm.mute;
        if (toggleMute.isOn)
            sliderVolume.enabled = false;
        else
            sliderVolume.enabled = true;
    }

    public void VolumeChanged(float f) {
        bgm.volume = f;
    }

    public void VolumeMute(bool b) {
        bgm.mute = b;
        if (b) {
            sliderVolume.fillRect.gameObject.GetComponent<Image>().color = Color.gray;
            sliderVolume.enabled = false;
        }            
        else {
            sliderVolume.fillRect.gameObject.GetComponent<Image>().color = Color.green;
            sliderVolume.enabled = true;
        }
    }
}
