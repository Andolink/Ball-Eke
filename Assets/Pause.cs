using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public bool isPause = false;
    [SerializeField] GameObject MainMenu;

    public void PauseStateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;
            transform.GetChild(0).gameObject.SetActive(isPause);

            if (isPause)
            {
                GameGlobalManager.Instance.ChangeCursorStat(false);
            }
        }
    }

    public void ClosePause()
    {
        isPause = false;
        transform.GetChild(0).gameObject.SetActive(isPause);

        GameGlobalManager.Instance.ChangeCursorStat(false);
    }
}
