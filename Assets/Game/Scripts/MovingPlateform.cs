using System.Collections.Generic;
using UnityEngine;

public class MovingPlateform : MonoBehaviour
{
    [SerializeField] private MovingPath path;

    [SerializeField] private float speed = 1.0f;

    private int pathIndex;

    private Transform nextPath;
    private Transform prevPath;

    private float travelTime;
    private float timer;

    private List<GameObject> Riders = new();

    private void Start()
    {
        GotoNextPath();
    }

    private void GotoNextPath()
    {
        prevPath = path.GetPath(pathIndex);
        pathIndex = path.GetNextIndex(pathIndex);
        nextPath = path.GetPath(pathIndex);
        timer = 0;

        float distToTravel = Vector3.Distance(prevPath.position, nextPath.position);
        travelTime = distToTravel / speed;
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;

        Move();
    }

    private void OnTriggerEnter(Collider collider)
    {
        Riders.Add(collider.gameObject);
        collider.gameObject.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider collider)
    {
        Riders.Remove(collider.gameObject);
        collider.gameObject.transform.SetParent(null);
    }

    private void Move()
    {
        float timerPercentage = timer / travelTime;
        timerPercentage = Mathf.SmoothStep(0, 1, timerPercentage);
        transform.position = Vector3.Lerp(prevPath.position, nextPath.position, timerPercentage);
        transform.rotation = Quaternion.Lerp(prevPath.rotation, nextPath.rotation, timerPercentage);

        if (timerPercentage >= 1)
            GotoNextPath();
    }
}