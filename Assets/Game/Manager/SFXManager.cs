using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    [SerializeField] private GameObject soundEffect;

    public AudioClip sfxAirHorn;
    public AudioClip sfxBass;
    public AudioClip sfxChadMove;
    public AudioClip sfxDash;
    public AudioClip sfxDjScratch;
    public AudioClip sfxLoading;
    public AudioClip sfxScore;
    public AudioClip sfxSkillIssue;

 
    private void OnEnable()
    {
        Instance = this;
    }

    public void SfxPlay(AudioClip _audipClip, bool _addRandomPitch = true, bool _isDeltaTimeScaled = true)
    {
        if (_audipClip)
        {
            GameObject _sfx = Instantiate(soundEffect);
            SoundEffect _soundEffect = _sfx.GetComponent<SoundEffect>();

            _soundEffect.audioSource.clip = _audipClip;
            _soundEffect.audioSource.Play();

            if (_addRandomPitch)
            {
                _soundEffect.audioPitch = Random.Range(0.95f,1.05f);
            }
            _soundEffect.isDeltaTimeScaled = _isDeltaTimeScaled;

        }
    }

}
