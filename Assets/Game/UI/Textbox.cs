using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Textbox : MonoBehaviour
{
    public bool isActivated = false;
    public string textToDisplay = "";
    public TextMeshProUGUI textMesh;
    RectTransform rect;
    Vector3 defaultPos = Vector3.zero;
    void Start()
    {

        rect = GetComponent<RectTransform>();
        defaultPos = rect.position;
    }

    void Update()
    {
        Vector3 _pos = defaultPos;
        _pos += isActivated ? Vector3.zero : Vector3.right * 10000f;

        rect.position = Vector3.Lerp(rect.position, _pos, Time.unscaledDeltaTime);
        textMesh.text = textToDisplay;
    }
}
