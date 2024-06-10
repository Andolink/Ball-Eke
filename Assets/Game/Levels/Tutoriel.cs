using TMPro;
using UnityEngine;

public class Tutoriel : MonoBehaviour
{
    private bool WallJumpTriggered = false;

    private readonly string[] Objs =
    {
        "Welcome Lebran, to ekes's mysterious world! Move using WASD/ZQSD.",
        "Jump using Space!",
        "Do a Wall Jump!",
        "Dash using A.",
        "Slow down time using Shift.",
        "Grab me using R-Click.",
        "Trow me using L-Click."
    };

    private readonly string[] Success =
    {
        "MOVE MOVE MOVE!!",
        "Wow look at that height!",
        "JUMP JUMP JUMP!!",
        "ZOOOOOOOOOOO!!",
        "A  W  E  S  O  M  E ! !",
        "~hold me daddy~",
        "WAAAAAAAAAAA!!"
    };

    private int ObjIndex = 0;

    private bool success = false;
    private bool next = false;

    private float TimerBetweenObj = 1.8f;
    private float timer = 0;

    public Grabable ball;

    void Start()
    {
        LevelManager.Instance.TextboxEnable(true);
        LevelManager.Instance.TextboxText("Welcome, Lebron Jam, to Ball-EKE's mysterious world!");
        timer = 2f;
        FirstObj();
    }

    void Update()
    {
        if (success)
        {
            success = false;
            Sucess();
        }
        if (next)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                next = false;
                NextObj();
            }
        }
        else
        {
            CheckObj();
        }
    }

    private void CheckObj()
    {
        // Logique pour vérifier les objectifs
        switch (ObjIndex)
        {
            case 0:
                if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                {
                    success = true;
                    LevelManager.Instance.TextboxText("");
                }
                break;
            case 1:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    success = true;
                }
                break;
            case 2:
                if (WallJumpTriggered)
                {
                    success = true;
                }
                break;
            case 3:
                if (Input.GetKeyDown(KeyCode.A))
                {
                    success = true;
                }
                break;
            case 4:
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    success = true;
                }
                break;
            case 5:
                if (Input.GetMouseButtonDown(1) && ball.isGrabed) // clic droit
                {
                    success = true;
                }
                break;
            case 6:
                if (Input.GetMouseButtonDown(0) && !ball.isGrabed) // clic gauche
                {
                    success = true;
                }
                break;
        }
    }

    private void Sucess()
    {
        LevelManager.Instance.TextboxText(Success[ObjIndex]);
        timer = TimerBetweenObj;
        next = true;
    }

    private void NextObj()
    {
        ObjIndex++;
        if (ObjIndex < Objs.Length)
        {
            LevelManager.Instance.TextboxText(Objs[ObjIndex]);
        }
        else
        {
            LevelManager.Instance.TextboxText("...");
        }
    }

    private void FirstObj()
    {
        LevelManager.Instance.TextboxText(Objs[0]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ObjIndex == 2)
            WallJumpTriggered = true;
    }
}
