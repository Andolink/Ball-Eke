using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public bool isPause = false;
    [SerializeField] GameObject MainMenu;
    [SerializeField] TMP_Text score;
    [SerializeField] TMP_Text ballText;

    public void PauseStateUpdate()
    {
        score.text = "Score : " + LevelManager.Instance.currentLevelScore;

        Transform lvl = LevelManager.Instance.currentLoadedLevel.GetComponent<Transform>();
        int ballLeft = 0;
        foreach (Transform enfant in lvl)
        {
            if (enfant.name == "Ball" || enfant.name == "BallLolita" || enfant.name == "BallEmo")
            {
                ballLeft++;
            }
        }


        ballText.text = "Eke(s) Left : " + ballLeft;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;
            transform.gameObject.SetActive(isPause);

            if (isPause)
            {
                GameGlobalManager.Instance.ChangeCursorStat(true);
            }
            else
            {
                GameGlobalManager.Instance.ChangeCursorStat(false);
            }
        }
    }

    public void ClosePause()
    {
        score.text = "Score : " + LevelManager.Instance.currentLevelScore;
        isPause = false;
        transform.gameObject.SetActive(isPause);

        GameGlobalManager.Instance.ChangeCursorStat(false);
    }
}
