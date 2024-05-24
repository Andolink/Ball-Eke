using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationVFX : MonoBehaviour
{
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Disable()
    {
        anim.gameObject.SetActive(false);
    }

    public void Pause()
    {
        anim.enabled = false;
    }
}
