using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    [SerializeField] public AudioSource audioSource;
    public float audioPitch = 1f;
    public bool isDeltaTimeScaled = true;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
        else
        {
            audioSource.pitch = (isDeltaTimeScaled ? Time.timeScale : 1f) * audioPitch;
        }
    }
}
