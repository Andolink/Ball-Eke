using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Meter : MonoBehaviour
{
    public static Meter Instance { get; private set; }
    [SerializeField] float meterTextGaps = 50f;

    private List<MeterText> meterTexts = new List<MeterText>();
    [SerializeField] private GameObject meterTextPrefab;
    [SerializeField] private Transform transformUI;

    private void OnEnable()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        UpdateMeterList();
        UpdateMeterTextsPosition();
    }
    public void UpdateMeterList()
    {
        for (int _i = 0; _i < meterTexts.Count; _i++)
        {
            if (!meterTexts[_i])
            {
                meterTexts.RemoveAt(_i);
                _i--;
            }
        }
     }

    public void UpdateMeterTextsPosition()
    {
        int _i = 0;
        foreach (var meterText in meterTexts)
        {
            meterText.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, _i * -meterTextGaps, 0f);
            _i++;
        }
    }

    public void AddNewMeterText(string _text = "undefined", int _score = 0)
    {
        GameObject _meterTextGO = Instantiate(meterTextPrefab, transformUI);
        MeterText _meterText = _meterTextGO.GetComponent<MeterText>();

        _meterText.text = _text;
        _meterText.score = _score;
        _meterText.transform.position = Vector3.zero;

        LevelManager.Instance.currentLevelScore += _score;

        meterTexts.Insert(0, _meterText);
    }
}
