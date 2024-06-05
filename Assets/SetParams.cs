using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SetParams : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Slider sliderVolume;
    [SerializeField] UnityEngine.UI.Slider sliderSensi;
    [SerializeField] GameCamera goCamera;

    private void Start()
    {
        SetMouseSpeed();
    }

    public void SetVolume()
    {
        AudioListener.volume = sliderVolume.value;
    }

    public void SetMouseSpeed()
    {
        Debug.Log("=====================" + sliderSensi.value * 100);
        goCamera.SetSensibility(sliderSensi.value * 100);
    }
}
