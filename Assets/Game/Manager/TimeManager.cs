using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    private float pauseMult = 1;
    private float timeStop = 0;

    private float timeScale = 1f;
    private float targetTimeScale = 1f;
    private float factorTimeScale = 1f;

    [SerializeField] Pause pause;

    private void OnEnable()
    {
        Instance = this;
    }

    void Start()
    {
       
    }

    void Update()
    {
        if (timeStop > 0)
        {
            timeStop -= Time.unscaledDeltaTime;
            Time.timeScale = 0;
        }
        else
        {
            timeScale = Mathf.Lerp(timeScale, targetTimeScale, factorTimeScale * 10f * Time.unscaledDeltaTime);
            if (!pause.isPause)
                Time.timeScale = timeScale * pauseMult;
        }
    }

    public void TimeStop(float _time)
    {
        if (timeStop <= _time)
        {
            timeStop = _time;
        }
    }

    public void Slowmo(float _timeScale = -1, float _targetTimeScale = -1, float _factorTimeScale = -1)
    {
        if (_timeScale       != -1) timeScale       = _timeScale;
        if (_targetTimeScale != -1) targetTimeScale = _targetTimeScale;
        if (_factorTimeScale != -1) factorTimeScale = _factorTimeScale;
    }

    public void SetTimePause(bool _val)
    {
        pauseMult = _val ? 0f : 1f;
    }
}
