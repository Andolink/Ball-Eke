using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public bool isPause = false;
    [SerializeField] GameObject MainMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && MainMenu.activeSelf == false)
        {
            isPause = !isPause;
            transform.GetChild(0).gameObject.SetActive(isPause);
            Cursor.visible = isPause;
            if (isPause)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void ClosePause()
    {
        isPause = false;
        transform.GetChild(0).gameObject.SetActive(isPause);

        Cursor.visible = isPause;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
