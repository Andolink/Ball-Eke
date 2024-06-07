using UnityEngine;

public class DeathZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider _collider)
    {
        if (_collider.TryGetComponent(out Player _player))
        {
            LevelManager.Instance.PlayerResetPosition();
            Meter.Instance.AddNewMeterText("Big Skill Issue", -100);
            SFXManager.Instance.SfxPlay(SFXManager.Instance.sfxSkillIssue);

        }

        if (_collider.TryGetComponent(out Grabable _grab))
        {
            Meter.Instance.AddNewMeterText("Skill Issue", -25);
            SFXManager.Instance.SfxPlay(SFXManager.Instance.sfxDjScratch);
            _grab.Death();
        }
    }
}
