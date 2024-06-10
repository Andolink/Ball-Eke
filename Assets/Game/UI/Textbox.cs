using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Textbox : MonoBehaviour
{
    public bool isActivated = false;
    public string textToDisplay = "";
    public TextMeshProUGUI textMesh;
    public Vector3 size = Vector3.zero;
    RectTransform rect;
    Vector3 defaultPos = Vector3.zero;
    void Start()
    {

        rect = GetComponent<RectTransform>();
        defaultPos = rect.position;
    }

    void Update()
    {
        Vector3 _rot = isActivated ? Vector3.one : Vector3.zero;

        size = Vector3.Lerp(size, _rot, Time.unscaledDeltaTime * 15f);

        rect.localScale = size;
        textMesh.text = textToDisplay;
    }
}
