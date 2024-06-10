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
                SFXManager.Instance.SfxPlay(SFXManager.Instance.sfxScore);
                ball.Take(transform, true);

                LevelManager.Instance.gameOver = false;
                LevelManager.Instance.currentCompletedGoals++;

                float _bonusScore = ball.rb.velocity.magnitude * ball.rb.velocity.magnitude;
                float _distanceBonus = (transform.position - ball.lastTrow).magnitude ;
                _distanceBonus *= _distanceBonus * 0.25f;

                //if (_bonusScore >= 100f) Meter.Instance.AddNewMeterText("VeloCity's PREMIUM", (int)_bonusScore);
                if (_distanceBonus >= 30f) Meter.Instance.AddNewMeterText("Sniper", (int)_distanceBonus);


                if (ball.rebond == 0)
                { Meter.Instance.AddNewMeterText("BALEK!!", 300); }
                else
                { Meter.Instance.AddNewMeterText("Ball-Eke!", 100); }
                isCompleted = true;
            }
            else
            {
                Vector3 _attraction = ((transform.position+Vector3.down) - ball.transform.position).normalized * 4f;
                ball.rb.velocity = Vector3.Lerp(ball.rb.velocity, _attraction, Time.deltaTime * ball.rb.velocity.magnitude * 2f);
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
