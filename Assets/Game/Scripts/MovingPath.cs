using UnityEngine;

public class MovingPath : MonoBehaviour
{
    public Transform GetPath(int index)
    {
        return transform.GetChild(index);
    }

    public int GetNextIndex(int index)
    {
        int nextIndex = index + 1;

        if (nextIndex == transform.childCount)
        {
            nextIndex = 0;
        }

        return nextIndex;
    }
}
