using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    [SerializeField] private PathCreator pathCreator;
    [SerializeField] private float targetSpeed = 20f;
    private Player player;
    private float slideSpeed = 10f;
    private float playerPosOnSlide = 0f;
    private float speedMult = 1f;
    private float waitEndMovement = 0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        waitEndMovement -= Time.deltaTime;

        if ( player)
        {
            if (player.isMovedOutside)
            {
                slideSpeed = Mathf.Lerp(slideSpeed, targetSpeed, Time.deltaTime * 10f);
                float _speed = slideSpeed;

                playerPosOnSlide += Time.deltaTime * _speed * speedMult;

                int point0 = (int)playerPosOnSlide;
                int point1 = (int)playerPosOnSlide+1;
                float factor = playerPosOnSlide - point0;
             
                if (point1 >= pathCreator.path.NumPoints - 1 || point0 <= 0)
                {
                    player.isMovedOutside = false;
                }
                else
                {
                    
                    player.transform.position = Vector3.Lerp(pathCreator.path.GetPoint(point0), pathCreator.path.GetPoint(point1), factor);
                    Vector3 _roadOrientation = Vector3.Lerp(pathCreator.path.GetNormal(point0), pathCreator.path.GetNormal(point1), factor);
                    Vector3 _roadOrientation0 = Quaternion.Euler(90f, 0f, 0f) * _roadOrientation;
                    Vector3 _roadOrientation1 = Quaternion.Euler(-90f, 0f, 0f) * _roadOrientation;

                    if (_roadOrientation0.y > _roadOrientation1.y)
                    {
                        _roadOrientation = _roadOrientation0;
                    }
                    else
                    {
                        _roadOrientation = _roadOrientation1;
                    }

                    player.transform.position += _roadOrientation ;
                }
            }
            else
            {  
                waitEndMovement = 1f;
                TimeManager.Instance.TimeStop(0.1f);
                player.StopMovedInside();

                if (speedMult == -1)
                { player.ResetMovement(player.rb.velocity + (pathCreator.path.GetPoint(0) - pathCreator.path.GetPoint(1)).normalized * slideSpeed); }
                else
                { player.ResetMovement(player.rb.velocity + (pathCreator.path.GetPoint(pathCreator.path.NumPoints - 1) - pathCreator.path.GetPoint(pathCreator.path.NumPoints - 2)).normalized * slideSpeed); }


                player = null;
            }
        }
        
    }

    private void OnCollisionStay(Collision _collision)
    {
        if (waitEndMovement <= 0 && _collision.transform.TryGetComponent(out Player _player) && _player.grounded)
        {
            player = _player;

            slideSpeed = player.rb.velocity.magnitude;
           
            player.rb.velocity = Vector3.zero;
            player.rb.isKinematic = true;
            player.isMovedOutside = true;
            Meter.Instance.AddNewMeterText("Sliide!!", 10);

            
            TimeManager.Instance.TimeStop(0.1f);

            float _magnitudeMax = 999f;
            int _index = 0;
            for (int i = 0; i < pathCreator.path.NumPoints; i++)
            {
                Vector3 _dist = pathCreator.path.GetPoint(i) - player.transform.position;
                if (_dist.magnitude < _magnitudeMax)
                { 
                    _index = i; 
                    _magnitudeMax = _dist.magnitude; 
                }
            }
            playerPosOnSlide = _index;
            speedMult = (_index <= pathCreator.path.NumPoints/2) ? speedMult = 1f : speedMult = -1f;
        }
    } 
}
