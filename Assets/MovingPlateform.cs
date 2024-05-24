using System.Collections.Generic;
using UnityEngine;

public class MovingPlateform : MonoBehaviour
{
    public float Ratio = 1;
    public Vector3 Axes;
    public bool cyclicle = true;
    public float cycleSpeed = 1;

    private bool goingForward = true;
    private float timer;
    private List<GameObject> Riders = new();

    private void Start()
    {
        timer = cycleSpeed;
    }

    void Update()
    {
        Vector3 vect = Axes * Ratio;

        // Cyclicle
        if (cyclicle)
        {
            // Timer Cycle
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                goingForward = !goingForward;
                timer = cycleSpeed;
            } //

            // gestion State Cycle
            if (goingForward)
                Move(vect);
            else
                Move(-vect); //

            return;
        }

        // Not Cyclicle
        Move(vect);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Riders.Add(collision.gameObject);
        collision.gameObject.transform.SetParent(transform);
    }

    private void OnCollisionExit(Collision collision)
    {
        Riders.Remove(collision.gameObject);
    }

    private void Move(Vector3 vect)
    {
        // Déplacer plateforme
        transform.position += vect;
    }
}