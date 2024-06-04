using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusCircle : MonoBehaviour
{
    private bool isTraversed = false;
    private bool done = false;
    [SerializeField] private Collider colliderCircle;

    void Start()
    {
        
    }

    void Update()
    {
        if (done)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 30f);
            if (transform.localScale.magnitude <= 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Grabable _ball))
        {
            isTraversed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Grabable _ball))
        {
            if (isTraversed && !done)
            {
                done = true;
                colliderCircle.enabled = false;
                Meter.Instance.AddNewMeterText("Bonus", 20);
            }
        }
    }
}
