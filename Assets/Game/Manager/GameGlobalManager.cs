using System.Collections.Generic;
using UnityEngine;

public class GameGlobalManager : MonoBehaviour
{
    static public GameGlobalManager Instance { get; private set; }

    
    [SerializeField] public GameObject MainMenuRoot;
    [SerializeField] public GameObject PlayerPackage;

    [Header("UI")]

    [SerializeField] public GameObject UIMainMenu;
    [SerializeField] public GameObject UIGame;
    [SerializeField] public GameObject UIPause;
    [SerializeField] public GameObject UIOption;
    [SerializeField] public GameObject UIResult;
    [SerializeField] public GameObject UIRegister;
    [SerializeField] public GameObject UIScoreBoard;
   
    [Header("Icon Transition")]

    [SerializeField] private GameObject UIRoot;
    [SerializeField] private GameObject prefabEndIcon;
    [SerializeField] private int iconToSpawn = 200;
    private bool iconTrasition = false;
    private float iconTransitionEndTimer = 0f;
    private float timeIconSpawn = 0;
    private float currentUIScale = 1f;
    private List<GameObject> endIconList = new List<GameObject>();


    public enum UIState
    {
        TitleScreen,
        Game,
        Pause,
        Option,
        Result,
        Register,
        ScoreBoard
    }
    public enum GameStates
    { 
        TitleScreen,
        Gameplay,
        Exit,
    }

    public GameStates currentState  = GameStates.TitleScreen;
    public GameStates nextState     = GameStates.TitleScreen;

    public UIState currentUI = UIState.TitleScreen;
    public UIState previousUI = UIState.Game;

    private void OnEnable()
    {
        UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false;
        Instance = this;
    }
    void Start()
    {
       
    }

    void Update()
    {
        IconTransition();
        UpdateUI();
    }

    public void GoToPlayMode()
    {
        StartTransition();
        nextState = GameStates.Gameplay;
    }
    public void GoToMainMenu()
    {
        StartTransition();
        nextState = GameStates.TitleScreen;
    }
    public void Exit()
    {
        StartTransition();
        nextState = GameStates.Exit;
    }

    public void InitializeMode()
    {
        if (iconTrasition)
        {
            iconTrasition = false;
            currentState = nextState;
        }

        switch (currentState)
        {
            case GameStates.TitleScreen:

                UIMainMenu.SetActive(true);
                UIGame.SetActive(false);
                MainMenuRoot.SetActive(true);
                PlayerPackage.SetActive(false);
                LevelManager.Instance.LevelUnload();

                break;
            case GameStates.Gameplay:

                UIMainMenu.SetActive(false);
                UIGame.SetActive(true);
                MainMenuRoot.SetActive(false);
                PlayerPackage.SetActive(true);

                LevelManager.Instance.LevelLoad();

                break;
            case GameStates.Exit:
                Application.Quit();
                break;
        }
    }
    public void StartMode()
    {
        switch (currentState)
        {
            case GameStates.TitleScreen:

                ChangeUI(UIState.TitleScreen);

                break;
            case GameStates.Gameplay:

                LevelManager.Instance.LevelStart();

                break;
        }
    }

    public void ChangeCursorStat(bool _active)
    {
        Cursor.lockState = _active ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = _active;
    }

    public void StartTransition()
    {
        iconTrasition = true;
        SFXManager.Instance.SfxPlay(SFXManager.Instance.sfxLoading,false,false);
    }
    private void IconTransition()
    {
        timeIconSpawn += Time.unscaledDeltaTime * iconToSpawn;
        if (timeIconSpawn > 1)
        {
            if (iconTrasition)
            {
                if (endIconList.Count < iconToSpawn)
                {
                    endIconList.Add(Instantiate(prefabEndIcon, UIRoot.transform));
                    timeIconSpawn = 0f;
                }
                else
                {
                    iconTransitionEndTimer += Time.unscaledDeltaTime;
                    if (iconTransitionEndTimer >= 1)
                    {
                        iconTransitionEndTimer = 0;

                        InitializeMode();
                    }
                }
            }
            else
            {
                if (endIconList.Count > 0)
                {
                    Destroy(endIconList[endIconList.Count - 1]);
                    endIconList.RemoveAt(endIconList.Count - 1);
                    timeIconSpawn = 0f;

                    if (endIconList.Count == 0)
                    {
                        StartMode();
                    }
                }
            }

        }
    }

    public void ChangeUI(UIState _ui)
    {
        previousUI = currentUI;
        currentUI = _ui;
        currentUIScale = 2f;
        UpdateUI();
    }

    private void UpdateUI()
    {
        GetUIGameObject(previousUI).SetActive(false);
        GetUIGameObject(currentUI).SetActive(true);
        GetUIGameObject(currentUI).GetComponent<RectTransform>().localScale = Vector3.one * currentUIScale;

        currentUIScale = Mathf.Lerp(currentUIScale, 1f, Time.unscaledDeltaTime * 12f);
    }

    private GameObject GetUIGameObject(UIState _uiState)
    {
        GameObject _menu = null;
        switch ( _uiState )
        {
            case UIState.TitleScreen :  _menu = UIMainMenu; break;
            case UIState.Game:          _menu = UIGame; break;
            case UIState.Pause:         _menu = UIPause; break;
            case UIState.Option:        _menu = UIOption; break;
            case UIState.Result:        _menu = UIResult; break;
            case UIState.Register:      _menu = UIRegister; break;
            case UIState.ScoreBoard:    _menu = UIScoreBoard; break;
        }

        return _menu;
    }

    //-------------------------------Options----------------------------//
    public void OpenOptions()
    {
        ChangeUI(UIState.Option);
    }
    public void CloseOptions()
    {
        if (previousUI == UIState.TitleScreen)
        {
            ChangeUI(UIState.TitleScreen);
        }
        else
        {
            ChangeUI(UIState.Pause);
        }
    }
}
