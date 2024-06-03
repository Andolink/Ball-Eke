using System.Collections.Generic;
using UnityEngine;

public class MovingPlateform : MonoBehaviour
{
    public bool Up;
    public bool Forward;

    public Vector3 AxeToVector3()
    {
        return new Vector3(Forward ? Force : 0f, Up ? Force : 0f, 0f);
    }

    public float Ratio = 1;
    public bool cyclicle = true;
    public float cycleSpeed = 1;

    private Vector3 Axes;
    private readonly float Force = 0.005f;
    private bool goingForward = true;
    private float timer;
    private List<GameObject> Riders = new();

    private void Start()
    {
        timer = cycleSpeed;
        Axes = AxeToVector3();
    }

    void FixedUpdate()
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
        collision.gameObject.transform.SetParent(null);
    }

    private void Move(Vector3 vect)
    {
        // Déplacer plateforme
        transform.position += vect * Time.deltaTime;
    }
}