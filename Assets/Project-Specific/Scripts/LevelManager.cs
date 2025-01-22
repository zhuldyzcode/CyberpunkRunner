using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class LevelManager : MonoBehaviour
{
    private CurrencyManager currencyManager;
    private int scores = 0;
    private string playerName;

    [SerializeField] private TextMeshProUGUI scoresText;
    [SerializeField] private TextMeshProUGUI scoresTextEND;

    [SerializeField] private RoadGenerator environment;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private PlayerCollision player; // OnObstacleHit
    [SerializeField] GameManager gameManager;
    [SerializeField] private GameObject nameInputPanel;
    public List<Implant> boughtImplants;
    [SerializeField] private TextMeshProUGUI lastLoserScoresInLeaderboard;

    private void Awake()
    {
        RetrieveDataFromPlayersPrefs();
        currencyManager = gameObject.GetComponent<CurrencyManager>();
    }

    #region SaveData
    private void RetrieveDataFromPlayersPrefs()
    {
        playerName = PlayerPrefs.GetString("PlayerName", "");
        LoadBoughtImplants();
    }

    public void SaveDataToPlayerPrefs()
    {
        PlayerPrefs.SetString("PlayerName", playerName);
        SaveBoughtImplants();
        PlayerPrefs.Save();
    }

    private void AskForName()
    {
        nameInputPanel.SetActive(true);
    }

    public void ReadStringFromInput(string s)
    {
        playerName = s;
        SaveDataToPlayerPrefs();
        nameInputPanel.SetActive(false);
        HighScores.AddNewHighscore(playerName, scores);
    }

    #endregion

    private void Start()
    {
        player.OnCoinCollected += currencyManager.HandleCoinCollected;
        player.OnHeadOnObstacleHit += HandleHeadOnCollision;
        environment.OnSegmentDestroyedEvent += IncreaseAndDisplayMetres;

    }

    private void OnDestroy()
    {
        player.OnCoinCollected -= currencyManager.HandleCoinCollected;
        player.OnHeadOnObstacleHit -= HandleHeadOnCollision;
        environment.OnSegmentDestroyedEvent -= IncreaseAndDisplayMetres;
    }

    public void Restart()
    {
        scores = 0;
        scoresText.text = scores.ToString();
        environment.ResetLevel();
    }

    #region Metres
    public void IncreaseAndDisplayMetres()
    {
        scores++;
        scoresText.text = scores.ToString();
    }
    #endregion

 
    #region Implants
    public void BuyImplant(Implant implant)
    {
        boughtImplants.Add(implant);
        SaveBoughtImplants();
    }

    public void UseImplant(Implant implant)
    {
        StartCoroutine(implant.ApplyWithDuration(player.gameObject, implant.ApplyEffect, implant.RemoveEffect));
        boughtImplants.Remove(implant);
        Debug.Log("PlayerPrefs.GetString(BoughtImplants, implantsString); " + PlayerPrefs.GetString("BoughtImplants"));

    }

    private void SaveBoughtImplants()
    {
        List<string> implantNames = new List<string>();
        foreach (Implant implant in boughtImplants)
        {
            implantNames.Add(implant.name);
        }

        string implantsString = string.Join(",", implantNames);
        PlayerPrefs.SetString("BoughtImplants", implantsString);
        Debug.Log("PlayerPrefs.GetString(BoughtImplants, implantsString); " + PlayerPrefs.GetString("BoughtImplants"));
        PlayerPrefs.Save();
    }

    private void LoadBoughtImplants()
    {
        if (PlayerPrefs.HasKey("BoughtImplants"))
        {
            string implantsString = PlayerPrefs.GetString("BoughtImplants");
            string[] implantNames = implantsString.Split(',');
            UIManager uIManager = gameObject.GetComponent<UIManager>();
            foreach (string implantName in implantNames)
            {
                Implant implant = uIManager.availableImplants.Find(i => i.name == implantName);
                if (implant != null)
                {
                    boughtImplants.Add(implant);
                }
            }
        }
    }
    #endregion

    #region Obstacles Collisions
    private void HandleHeadOnCollision()
    {
        {
            gameManager.ChangeStateToLose();
            scoresTextEND.text = scores.ToString();
        }
        StartCoroutine(CheckLeaderboardAndHandleCollision());
    }

    private IEnumerator CheckLeaderboardAndHandleCollision()
    {
        string url = "http://dreamlo.com/lb/66a92ee28f40bb11085043ee/json";
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error fetching leaderboard: " + www.error);
        }
        else
        {
            string jsonText = www.downloadHandler.text;
            JObject json = JObject.Parse(jsonText);
            JArray entries = (JArray)json["dreamlo"]["leaderboard"]["entry"];

            if (string.IsNullOrEmpty(playerName))
            {
                AskForName();
            }
            else
            {
                bool isTop25 = false;
                int playerRank = -1;
                for (int i = 0; i < entries.Count; i++)
                {
                    string entryName = entries[i]["name"].ToString();
                    int entryScore = int.Parse(entries[i]["score"].ToString());
                    if (entryName == playerName && entryScore == scores)
                    {
                        isTop25 = true;
                        playerRank = i + 1;
                        break;
                    }
                }

                if (isTop25)
                {
                    audioManager.PlayEffect("Win");
                    HighlightPlayerNameOnLeaderboard(playerRank);
                    lastLoserScoresInLeaderboard.text = "";
                }
                else
                {
                    audioManager.PlayEffect("Lose");
                    AddLastPlayerToLeaderboard();
                }
            }
        }
    }

    private void HighlightPlayerNameOnLeaderboard(int playerRank)
    {
        // Implementation to highlight player's name on the leaderboard
    }

    private void AddLastPlayerToLeaderboard()
    {
        lastLoserScoresInLeaderboard.text = "25+. " + playerName + " - " + scores;
    }
    #endregion
}