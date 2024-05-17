using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    static public LevelManager Instance { get; private set; }

    [SerializeField] private GameObject UIroot;
    [SerializeField] private GameObject prefabEndIcon;
    [SerializeField] private TextMeshProUGUI textTimer;
    [SerializeField] private Player player;

    [SerializeField] public float levelTimer = 45f;

    private bool iconTrasition = false;
    private float iconTransitionEndTimer = 0f;
    [SerializeField] private int iconToSpawn = 100;
    private float timeIconSpawn = 0;
    private List<GameObject> endIconList = new List<GameObject>();

    private bool levelEnd = false;

    [SerializeField] private Level currentLoadedLevel;
    [SerializeField] List<GameObject> levelToInstanciate = new List<GameObject>();
    

    private void OnEnable()
    {
        Instance = this;
    }

    void Start()
    {
        LevelStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (!levelEnd)
        {
            LevelTimerUpdate();
        }

        IconTransition();
    }

    void LevelStart()
    {
        TimeManager.Instance.SetTimePause(false);
        if (currentLoadedLevel)
        {
            player.transform.SetParent(transform);
            Destroy(currentLoadedLevel.gameObject);
        }

        GameObject _lvl = Instantiate(levelToInstanciate[0], transform);
        currentLoadedLevel = _lvl.GetComponent<Level>();
        _lvl.transform.position = Vector3.zero;

        player.transform.position = currentLoadedLevel.playerSpawn.position;
        levelTimer = currentLoadedLevel.timer;
        levelEnd = false;
    }

    void LevelTimerUpdate()
    {
        if (levelTimer > 0)
        {
            levelTimer -= Time.deltaTime;
            textTimer.text = ((int)levelTimer+1).ToString();
            if (levelTimer <= 0f)
            {
                textTimer.text = "TIME'S UP!";
                LevelEnd();
            }
        }
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
                    endIconList.Add(Instantiate(prefabEndIcon, UIroot.transform));
                    timeIconSpawn = 0f;
                }
                else
                {
                    iconTransitionEndTimer += Time.unscaledDeltaTime;
                    if (iconTransitionEndTimer >= 1)
                    {
                        iconTransitionEndTimer = 0;
                        iconTrasition = false;

                        LevelStart();
                    }
                }
            }
            else
            {
                if (endIconList.Count > 0)
                {
                    Destroy(endIconList[endIconList.Count-1]);
                    endIconList.RemoveAt(endIconList.Count-1);
                    timeIconSpawn = 0f;
                }
            }
            
        }
    }

    public void LevelEnd()
    {
        TimeManager.Instance.SetTimePause(true);
        levelEnd = true;
        iconTrasition = true;
    }
}
