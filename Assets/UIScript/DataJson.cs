using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class DataJson : MonoBehaviour
{
    private string filePath;
    [SerializeField] GameObject RegisterUI;
    private string pseudo;
    private int playerPos;
    [SerializeField] CharacterList defaultStats = new CharacterList();
    GameObject pseudoArea;

    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "CharacterList.json");
        Debug.Log(filePath);
        LoadData();
    }

    public void Register(GameObject _pseudoArea)
    {
        LevelManager.Instance.ScoreBoardScreen();
        pseudoArea = _pseudoArea;
        pseudo = _pseudoArea.GetComponent<TMP_Text>().text;
        pseudoArea.GetComponent<TMP_Text>().text = "";
    }

    private void LoadData()
    {
        if (!File.Exists(filePath))
        {
            Debug.Log("Le fichier est po là");
            CreateDefaultJson();
        }
        
        SaveData();

        // CODE POUR AFFICHER LES SCORES
        if(playerPos > 2)
        {
            GameObject ISBAD = gameObject.transform.Find("Top").Find("top4").gameObject;
            ISBAD.SetActive(true);
            ISBAD.GetComponent<TMP_Text>().text = playerPos++ + "   " + LevelManager.Instance.globalScore + "   " + pseudo;
        }

        for (int i = 0; i < 3; i++)
        {
            if (defaultStats.characterList.Count > i)
            {
                gameObject.transform.Find("Top").transform.Find("top" + (i+1)).gameObject.GetComponent<TMP_Text>().text = (i+1) + "   " + defaultStats.characterList[i].score + "   " + defaultStats.characterList[i].pseudo;
            }
        }
    }

    private void SaveData()
    {
        string json = File.ReadAllText(filePath);
        Debug.Log(json);
        defaultStats = JsonUtility.FromJson<CharacterList>(json);

        playerPos = 0;
        foreach (CharacterStats players in defaultStats.characterList)
        {
            Debug.Log(players.pseudo);
            if (LevelManager.Instance.globalScore < players.score)
            {
                playerPos++;
            }
        }
        CharacterStats player = new CharacterStats
        {
            pseudo = pseudo,
            score = LevelManager.Instance.globalScore
        }; 

        if (defaultStats.characterList.Count == 0)
        {
            defaultStats.characterList.Add(player);
        }
        else
        {
            defaultStats.characterList.Insert(playerPos, player);
        }

        //CODE POUR ENREGISTRER LE JOUEUR
        Debug.Log(defaultStats.characterList.Count);
        json = JsonUtility.ToJson(defaultStats, true);
        Debug.Log(json);
        File.WriteAllText(filePath, json);

        Debug.Log("Saved");
        
    }

    private void CreateDefaultJson()
    {
        CharacterList defaultStats = new CharacterList();
        string json = JsonUtility.ToJson(defaultStats);
        File.WriteAllText(filePath, json);
    }
}

[System.Serializable]
public class CharacterList
{
    public List<CharacterStats> characterList = new List<CharacterStats>();
}

[System.Serializable]
public class CharacterStats
{
    public string pseudo;
    public float score;
}

