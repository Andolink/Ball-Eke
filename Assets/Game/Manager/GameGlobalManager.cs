using System.Collections.Generic; 
using UnityEngine;

public class GameGlobalManager : MonoBehaviour
{
    static public GameGlobalManager Instance { get; private set; }

    [SerializeField] public GameObject UIGame;
    [SerializeField] public GameObject UIMainMenu;
    [SerializeField] public GameObject MainMenuRoot;
    [SerializeField] public GameObject PlayerPackage;

    [SerializeField] private GameObject UIRoot;
    [SerializeField] private GameObject prefabEndIcon;
    [SerializeField] private int iconToSpawn = 200;
    private bool iconTrasition = false;
    private float iconTransitionEndTimer = 0f;
    private float timeIconSpawn = 0;
    private List<GameObject> endIconList = new List<GameObject>();

    public enum GameStates
    { 
        TitleScreen,
        Gameplay,
        Exit,
    }

    public GameStates currentState  = GameStates.TitleScreen;
    public GameStates nextState     = GameStates.TitleScreen;

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
    }

    public void GoToPlayMode()
    {
        Debug.Log("Mom");
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
        currentState = nextState;

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
        SFXManager.Instance.SfxPlay(SFXManager.Instance.sfxLoading);
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
                        iconTrasition = false;

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

    //-------------------------------Options----------------------------//
    public void OpenOptions()
    {
        UIMainMenu.transform.Find("Options").gameObject.SetActive(true);
    }
    public void CloseOptions()
    {
        UIMainMenu.transform.Find("Options").gameObject.SetActive(false);
    }
}
