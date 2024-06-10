using TMPro;
using UnityEngine;

public class Tutoriel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TutoText;

    [SerializeField] private TextMeshProUGUI Objtext;

    private bool WallJumpTriggered = false;

    private readonly string[] Objs =
    {
        "- [ ] Bouger avec ZQSD.",
        "- [ ] Sauter avec Espace.",
        "- [ ] Faire un Wall Jump",
        "- [ ] Dash avec A.",
        "- [ ] Ralentir le temps avec Shift.",
        "- [ ] Ramasser quelque chose (viser et utiliser clic droit).",
        "- [ ] Lancer avec clic gauche."
    };

    private readonly string[] Success =
    {
        "- [X] Bouger avec ZQSD.",
        "- [X] Sauter avec Espace.",
        "- [X] Faire un Wall Jump",
        "- [X] Dash avec A.",
        "- [X] Ralentir le temps avec Shift.",
        "- [X] Ramasser quelque chose (viser et utiliser clic droit).",
        "- [X] Lancer avec clic gauche."
    };

    private int ObjIndex = 0;

    private bool success = false;
    private bool next = false;

    private float TimerBetweenObj = 3;
    private float timer = 0;

    public Grabable ball;

    void Start()
    {
        TutoText.text = "Bienvenue, commençons par les bases :";
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
                    TutoText.text = "";
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
        Objtext.text = Success[ObjIndex];
        timer = TimerBetweenObj;
        next = true;
    }

    private void NextObj()
    {
        ObjIndex++;
        if (ObjIndex < Objs.Length)
        {
            Objtext.text = Objs[ObjIndex];
        }
        else
        {
            Objtext.text = "Tutoriel terminé ! Marquez un panier maintenant ! (Astuce : lancer dans l'anneau et gagner des points bonus !)";
        }
    }

    private void FirstObj()
    {
        Objtext.text = Objs[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ObjIndex == 2)
            WallJumpTriggered = true;
    }
}
