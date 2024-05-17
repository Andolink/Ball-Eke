using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndIcon : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image image;
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();
   
    void Start()
    {
        image.sprite = sprites[(int)Random.Range(0,3)];
        rectTransform.position = new(Random.Range(0, Screen.width), Random.Range(0, Screen.height), 0f);
        rectTransform.eulerAngles = new(0f, 0f, Random.Range(0f,360f));
        rectTransform.localScale = new Vector3(1f, 1f, 1f) * Random.Range(2.5f,5.5f);
    }

    void Update()
    {
        
    }
}
