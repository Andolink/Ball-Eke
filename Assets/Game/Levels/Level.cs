using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] public Transform playerSpawn;
    [SerializeField] public float timer = 45f;
    [SerializeField] public List<Grabable> balls;
    [SerializeField] private List<Transform> GrabableSpawns = new();

    private List<Transform> EmptyGrabableSpawns = new();

    private void Start()
    {
        EmptyGrabableSpawns = new List<Transform>(GrabableSpawns);
    }

    public void ResetBallSpawn()
    {
        EmptyGrabableSpawns = new List<Transform>(GrabableSpawns);
    }

    public Vector3 RandomEmptyBallSpawn()
    {
        // Exception
        if (EmptyGrabableSpawns.Count == 0)
        {
            if (GrabableSpawns.Count == 0)
            {
                Debug.LogError("Pas de spawn de ball dans le level");
                return Vector3.zero;
            }
            Debug.LogWarning("Moins de spawn que de ball dans le level.");
            return GrabableSpawns[0].position;
        }

        int index = Random.Range(0, EmptyGrabableSpawns.Count);

        Vector3 spawn = EmptyGrabableSpawns[index].position;
        EmptyGrabableSpawns.RemoveAt(index);

        return spawn;
    }
}
