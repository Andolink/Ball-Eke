using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MeterText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh;
    [SerializeField] TextMeshProUGUI textMeshScore;

    public string text = "";
    public int score = 0;
    private float lifeTime = 5f;

    void Start()
    {
        
    }

    void Update()
    {
        textMesh.text = text;
        textMeshScore.text = "+"+score.ToString();
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        { 
            Destroy(gameObject);
        }
    }
}
