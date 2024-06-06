using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] private GameObject soundEffect;

    public AudioClip sfxAirHorn;
    public AudioClip sfxBass;
    public AudioClip sfxChadMove;
    public AudioClip sfxDash;
    public AudioClip sfxDjScratch;
    public AudioClip sfxLoading;
    public AudioClip sfxScore;
    public AudioClip sfxSkillIssue;

    public static SFXManager Instance { get; private set; }

    private void OnEnable()
    {
        Instance = this;
    }

    public void SfxPlay(AudioClip _audipClip)
    {
        GameObject _sfx = Instantiate(soundEffect);
        _sfx.GetComponent<SoundEffect>().audioSource.clip = _audipClip;
    }

}
