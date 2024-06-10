using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] public AudioSource audioSource;
    public AudioLowPassFilter lowPassFilter;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float _time = Time.timeScale;

        audioSource.pitch = _time;
        lowPassFilter.cutoffFrequency = Mathf.Lerp(lowPassFilter.cutoffFrequency, (GameGlobalManager.Instance.currentState == GameGlobalManager.GameStates.TitleScreen ? 2200f : 22000f ), Time.unscaledDeltaTime * 4f);
    }
}
