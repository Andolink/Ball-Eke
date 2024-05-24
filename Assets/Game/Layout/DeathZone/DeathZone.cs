using System.Collections;
using System.Collections.Generic;
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
            LevelManager.Instance.gameOver = true;
            LevelManager.Instance.LevelEnd();
        }

        if (_collider.TryGetComponent(out Grabable _grab))
        {
            Meter.Instance.AddNewMeterText("Skill Issue", 0);
            _grab.Death();
        }
    }
}
