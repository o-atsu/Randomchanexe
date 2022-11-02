using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundSlider : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audio_mixer;
    [SerializeField]
    private string mixer_param;

    private Slider slider;


    void Awake(){
        slider = gameObject.GetComponent<Slider>();
    }

    void OnEnable(){
        float volume;
        bool ret = audio_mixer.GetFloat(mixer_param, out volume);
        if(ret){
            float val = Mathf.Pow(10.0f, volume / 20.0f);// 0から1に正規化
            slider.value = val;
        }
    }


    public void SetValue(float val){
        
        float volume = Mathf.Clamp(Mathf.Log10(val) * 20.0f, -80.0f, 0.0f);// -80dbから0dbに正規化
        audio_mixer.SetFloat(mixer_param, volume);
        // Debug.Log(mixer_param + " : " + volume);
    }


}
