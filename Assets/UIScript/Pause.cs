using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public bool isPause = false;
    [SerializeField] GameObject MainMenu;
    [SerializeField] TMP_Text score;
    [SerializeField] GameObject ballsUI;
    public void PauseStateUpdate()
    {
        score.text = "Score : " + LevelManager.Instance.currentLevelScore;

        List<Grabable> _balls = LevelManager.Instance.currentLoadedLevel.balls;
        Transform _ballDefault = ballsUI.transform.Find("1");
        Transform _ballLolita = ballsUI.transform.Find("2");
        Transform _ballEmo = ballsUI.transform.Find("3");

        ResetBallUI();

        foreach (Grabable _ball in _balls)
        {
            if (_ball.name == "Ball") {
                _ballDefault.GetComponent<Image>().color = _ball.isEnding ? Color.white : Color.grey;
            }
            else if (_ball.name == "BallLolita") {
                _ballLolita.GetComponent<Image>().color = _ball.isEnding ? Color.white : Color.grey;
            }
            else if(_ball.name == "BallEmo"){
                _ballEmo.GetComponent<Image>().color = _ball.isEnding ? Color.white : Color.grey;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;
            
            if (isPause)
            {
                GameGlobalManager.Instance.ChangeUI(GameGlobalManager.UIState.Pause);
                GameGlobalManager.Instance.ChangeCursorStat(true);
            }
            else
            {
                GameGlobalManager.Instance.ChangeUI(GameGlobalManager.UIState.Game);
                GameGlobalManager.Instance.ChangeCursorStat(false);
            }
        }
    }

    public void ResetBallUI()
    {
        ballsUI.transform.Find("1").GetComponent<Image>().color = Color.black;
        ballsUI.transform.Find("2").GetComponent<Image>().color = Color.black;
        ballsUI.transform.Find("3").GetComponent<Image>().color = Color.black;
    }

    public void ClosePause()
    {
        score.text = "Score : " + LevelManager.Instance.currentLevelScore;
        isPause = false;

        GameGlobalManager.Instance.ChangeUI(GameGlobalManager.UIState.Game);
        GameGlobalManager.Instance.ChangeCursorStat(false);
    }
}
