using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    static public LevelManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI textTimer;
    [SerializeField] private Player player;

    [SerializeField] public float levelTimer = 45f;

    public int currentCompletedGoals = 0;
    private bool levelEnd = false;
    [HideInInspector] public bool gameOver = false; 

    [SerializeField] private Level currentLoadedLevel;
    [SerializeField] List<GameObject> levelToInstanciate = new List<GameObject>();
    
    private void OnEnable()
    {
        Instance = this;
    }

    void Start()
    {
        //LevelStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (!levelEnd)
        {
            LevelTimerUpdate();
            
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

    public void LevelLoad()
    {
        TimeManager.Instance.SetTimePause(true);
        if (currentLoadedLevel)
        {
            player.transform.SetParent(transform);
            Destroy(currentLoadedLevel.gameObject);
        }

        levelEnd = true;
        GameObject _lvl = Instantiate(levelToInstanciate[0], transform);
        currentLoadedLevel = _lvl.GetComponent<Level>();
        _lvl.transform.position = Vector3.zero;

        player.ResetVar();
        player.transform.position = currentLoadedLevel.playerSpawn.position;
        levelTimer = currentLoadedLevel.timer;
    }

    public void LevelStart()
    {
        currentCompletedGoals = 0;
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
        string _text = "BALL-EKED!";
        if (gameOver)
        {
            switch ((int)Random.Range(0, 9))
            {
                default :  _text = "A NIGHTMARE WORSE THAT CAPITALISM HAS COME!"; break;
                case 0 : _text = "TOO BALLED..."; break;
                case 1: _text = "I GUESS YOU'RE NOT AN EPIC G4M3R LIKE ME."; break;
                case 2: _text = "YA BETTER GO BACK PLAY ADIDOU RN"; break;
                case 3: _text = "PRESSING RANDOM INPUT ISN'T A GOOD STRAT I GUESS."; break;
                case 4: _text = "STAY DETERMINE... BLABLABLA..."; break;
                case 5: _text = "IF YOU WOULD BE AN ANIMAL, YOU WOULD BE A SLOTH."; break;
                case 6: _text = "EKEKEKEKEKEKEKEKEKEKEKEKE!!"; break;
                case 7: _text = "NICE TRY. NAH JUST KIDDING IT WAS AWFUL."; break;
                case 8: _text = "AN ENDLESS SPIRAL OF WASTED TIME."; break;
                case 9: _text = "SO SKILLS? NAH TOO MUCH TO ASK 4."; break;
            }
        }

        gameOver = false;
        textTimer.text = _text; 
        TimeManager.Instance.SetTimePause(true);
        levelEnd = true;
        GameGlobalManager.Instance.StartTransition();
    }
}
