using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SetParams : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Slider sliderVolume;
    [SerializeField] UnityEngine.UI.Slider sliderSensi;
    [SerializeField] UnityEngine.UI.Slider sliderFOV;
    [SerializeField] GameCamera goCamera;
    [SerializeField] Camera playerCamera;

    private void Start()
    {
        SetMouseSpeed();
    }

    public void SetVolume()
    {
        AudioListener.volume = sliderVolume.value;
    }

    public void SetFOV()
    {
        playerCamera.fieldOfView = 50 + sliderFOV.value;
    }

    public void SetMouseSpeed()
    {
        Debug.Log("=====================" + sliderSensi.value * 100);
        goCamera.SetSensibility(sliderSensi.value * 100);
    }
}
