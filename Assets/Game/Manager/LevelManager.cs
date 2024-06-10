using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    static public int LevelNumber = 0;
    static public int LvlIndex = 0;
    static public LevelManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI textTimer;
    [SerializeField] private Player player;
    [SerializeField] private GameCamera gameCamera;
    [SerializeField] private TextMeshProUGUI resultScore;
    [SerializeField] private TextMeshProUGUI resultSentence;
    [SerializeField] private GameObject resultContinue;
    [SerializeField] private Pause pause;


    [SerializeField] private float levelTimer = 45f;
    [SerializeField] private float minLevelTimer = 20.0f;
    private float deltaTimer = 10.0f;

    public int currentCompletedGoals = 0;
    private bool levelEnd = false;
    public float score = 0;
    [HideInInspector] public bool gameOver = false;
    [HideInInspector] public float currentLevelScore = 0;
    [HideInInspector] public float globalScore = 0;

    [SerializeField] public Level currentLoadedLevel;
    [SerializeField] List<GameObject> levelToInstanciate = new List<GameObject>();

    static private bool tutorielPassed = false;

    [SerializeField] private Light DirLight;
    private float colorChangeSpeed = 0.08f;
    public float cycleTime = 0f;
    private List<Color> colors = new();

    public Textbox textbox;

    private void OnEnable()
    {
        Instance = this;
    }

    void Start()
    {
        colors.Add(Color.green);
        colors.Add(Color.cyan);
        colors.Add(Color.blue);
        colors.Add(Color.magenta);
        colors.Add(Color.red);
        colors.Add(new Color(0,0,0.1f));
        colors.Add(Color.green);
    }

    private void CycleSkyColor(bool force = false)
    {
        if (!force && (levelEnd || !(GameGlobalManager.Instance.currentState == GameGlobalManager.GameStates.Gameplay)))
            return;
        if (!RenderSettings.skybox || !RenderSettings.skybox.HasProperty("_SkyTint") || !RenderSettings.skybox.HasProperty("_GroundColor"))
            return;

        cycleTime += colorChangeSpeed * Time.deltaTime;
        if (cycleTime >= colors.Count-2)
        {
            cycleTime -= colors.Count-2;
        }

        Color targetColor = Color.Lerp(colors[(int)cycleTime], colors[(int)cycleTime+1], cycleTime - (int)cycleTime);

        RenderSettings.skybox.SetColor("_SkyTint", targetColor);
        RenderSettings.skybox.SetColor("_GroundColor", Color.white - targetColor);
        if (DirLight)
        {
            DirLight.color = targetColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CycleSkyColor();

        if (!levelEnd && GameGlobalManager.Instance.currentState == GameGlobalManager.GameStates.Gameplay)
        {
            LevelTimerUpdate();
            pause.PauseStateUpdate();

            if (currentCompletedGoals >= currentLoadedLevel.balls.Count)
            {
                if (player.transform.position.y <= 3f)
                {
                    Meter.Instance.AddNewMeterText("Death Surfer", 100);
                    if (levelTimer <= 1f)
                    {
                        Meter.Instance.AddNewMeterText("SNCF!", 100);
                    }
                }

                gameOver = false;
                LevelEnd();
            }
        }
    }

    public void ResetLevelDifficulty()
    {
        LvlIndex = 0;
    }

    public void LevelLoad()
    {
        levelEnd = true;
        CycleSkyColor(true);

        
        GameGlobalManager.Instance.ChangeUI(GameGlobalManager.UIState.Game);
        TimeManager.Instance.SetTimePause(true);

        if (currentLoadedLevel)
        {
            player.transform.SetParent(transform);
            Destroy(currentLoadedLevel.gameObject);
        }

        if (LvlIndex >= levelToInstanciate.Count)
        {
            LvlIndex = 0;
            LevelNumber += 1;
        }

        GameObject _lvl = Instantiate(levelToInstanciate[LvlIndex], transform);
        currentLoadedLevel = _lvl.GetComponent<Level>();
        _lvl.transform.position = Vector3.zero;
        PlayerResetPosition();
        player.ResetVar();
        player.transform.position = currentLoadedLevel.playerSpawn.position;
        levelTimer = currentLoadedLevel.timer;
        currentLevelScore = 0f;

        // Difficulty
        levelTimer = Mathf.Max(levelTimer - LevelNumber * deltaTimer, minLevelTimer);

        currentLoadedLevel.ResetBallSpawn();
        foreach (Grabable ball in currentLoadedLevel.balls)
        {
            ball.defaultPosition = currentLoadedLevel.RandomEmptyBallSpawn();
            ball.Respawn();
        }
    }

    public void LevelUnload()
    {
        if (currentLoadedLevel)
        {
            player.transform.SetParent(transform);
            Destroy(currentLoadedLevel.gameObject);
        }
    }

    public void PlayerResetPosition()
    {
        player.transform.position = currentLoadedLevel.playerSpawn.position;
    }

    public void LevelStart()
    {
        GameGlobalManager.Instance.ChangeCursorStat(false);
        
        currentCompletedGoals = 0;
        currentLevelScore = 0;
        levelEnd = false;
        TimeManager.Instance.SetTimePause(false);
        player.transform.position = currentLoadedLevel.playerSpawn.position;
    }

    void LevelTimerUpdate()
    {
        if (levelTimer > 0)
        {
            levelTimer -= Time.deltaTime;
            textTimer.text = ((int)levelTimer+1).ToString();
           
            if (levelTimer <= 0f)
            {
                gameOver = true;
                LevelEnd();
            }
        }
    }

    public void LevelEnd()
    {
        TextboxEnable(false);
        CameraShake(0f,0f,0f);
        string _text = "*Personal Thought : ";
        
        if (gameOver)
        {
            switch ((int)Random.Range(0, 10))
            {
                default :  _text += "A NIGHTMARE WORSE THAT CAPITALISM HAS COME!"; break;
                case 0 : _text += "TOO BALLED..."; break;
                case 1: _text += "I GUESS YOU'RE NOT AN EPIC G4M3R LIKE ME."; break;
                case 2: _text += "YA BETTER GO BACK PLAY ADIDOU RN"; break;
                case 3: _text += "PRESSING RANDOM INPUT ISN'T A GOOD STRAT I GUESS."; break;
                case 4: _text += "STAY DETERMINE... BLABLABLA..."; break;
                case 5: _text += "IF YOU WOULD BE AN ANIMAL, YOU WOULD BE A SLOTH."; break;
                case 6: _text += "EKEKEKEKEKEKEKEKEKEKEKEKE!!"; break;
                case 7: _text += "NICE TRY. NAH JUST KIDDING IT WAS AWFUL."; break;
                case 8: _text += "AN ENDLESS SPIRAL OF WASTED TIME."; break;
                case 9: _text += "SO SKILLS? NAH TOO MUCH TO ASK 4."; break;
            }
        }
        else
        {
            switch ((int)Random.Range(0, 5))
            {
                default: _text += "BALL-EKE!"; break;
                case 0: _text += "EKE YEAH!"; break;
                case 1: _text += "WP"; break;
                case 2: _text += "YA GOT SOME BALLS"; break;
                case 3: _text += "ADDICTING RIGHT?"; break;
                case 4: _text += "LOL < YOU"; break;
            }

            LvlIndex += 1;
            if (!tutorielPassed)
            {
                tutorielPassed = true;
                levelToInstanciate.RemoveAt(0);
            }
        }

        
        textTimer.text = ""; 
        TimeManager.Instance.SetTimePause(true);
        

        Meter.Instance.ClearMeter();

        GameGlobalManager.Instance.ChangeUI(GameGlobalManager.UIState.Result);
        GameGlobalManager.Instance.ChangeCursorStat(true);

        resultScore.text = currentLevelScore.ToString();
        resultSentence.text = _text;
        resultContinue.SetActive(!gameOver);

        levelEnd = true;
        gameOver = false;
    }

    public void Continue()
    {
        GameGlobalManager.Instance.StartTransition();
    }
     
    public void Quit()
    {
        GameGlobalManager.Instance.GoToMainMenu();
        GameGlobalManager.Instance.StartTransition();
    }

    public void RegisterScreen()
    {
        GameGlobalManager.Instance.ChangeUI(GameGlobalManager.UIState.Register);
    }

    public void ScoreBoardScreen()
    {
        ResetLevelDifficulty();
        GameGlobalManager.Instance.ChangeUI(GameGlobalManager.UIState.ScoreBoard);
    }

    public void CameraShake(float _magnitude = 0.1f, float _loss = 2f, float _time = 0.1f)
    {
        gameCamera.Shake(_magnitude, _loss, _time);
    }

    public void TextboxEnable(bool _val)
    {
        textbox.isActivated = _val;
    }

    public void TextboxText(string _text)
    {
        if (textbox.textToDisplay != _text)
        {
            textbox.size = textbox.size * 0.85f;
        }
        textbox.textToDisplay = _text;   
    }

}
