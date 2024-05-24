using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ending : MonoBehaviour
{
    private bool isBallInside = false;
    private Grabable ball = null;
    private bool isCompleted = false;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isBallInside)
        {
            if (ball.transform.position.y <= transform.position.y && !isCompleted)
            {
                LevelManager.Instance.gameOver = false;
                LevelManager.Instance.currentCompletedGoals++;

                float _bonusScore = ball.rb.velocity.magnitude * ball.rb.velocity.magnitude;

                if (_bonusScore >= 100f) Meter.Instance.AddNewMeterText("VeloCity's PREMIUM", (int)_bonusScore);

                if (ball.rebond == 0)
                { Meter.Instance.AddNewMeterText("BALEK!!", 300); }
                else
                { Meter.Instance.AddNewMeterText("Ball-Eke!", 100); }
                isCompleted = true;

            }
            else
            {
                Vector3 _attraction = ((transform.position+Vector3.down) - ball.transform.position).normalized * 4f;
                ball.rb.velocity = Vector3.Lerp(ball.rb.velocity, _attraction, Time.deltaTime * 2.5f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Grabable _ball) && _ball.transform.position.y > transform.position.y)
        {
            isBallInside = true;
            ball = _ball;
            Meter.Instance.AddNewMeterText("???", 1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Grabable _ball))
        {
            isBallInside = false;
        }
    }
}
