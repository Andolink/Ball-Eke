using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private Camera titleScreenCamemra;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var mouseWorldPos = Input.mousePosition;

        if (mouseWorldPos.x >= 0 && mouseWorldPos.x <= Screen.width && mouseWorldPos.y >= 0 && mouseWorldPos.y <= Screen.height)
        {
            mouseWorldPos.z = 0f; // zero z
            mouseWorldPos.x = (0.5f - (mouseWorldPos.x / 1080f) * 0.65f);
            mouseWorldPos.y = (0.5f - (mouseWorldPos.y / 620f) * 0.2f);


            titleScreenCamemra.transform.position = mouseWorldPos;
        }
    }
}