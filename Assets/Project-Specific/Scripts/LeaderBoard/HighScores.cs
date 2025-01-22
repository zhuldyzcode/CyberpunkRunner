using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HighScores : MonoBehaviour
{

	const string privateCode = "6oALGI8RckeFpcyNjBdjxgIzfWlR7eUEiUwJrwGTPeBQ";
	const string publicCode = "66a92ee28f40bb11085043ee";
	const string webURL = "http://dreamlo.com/lb/";

	public Highscore[] highscoresList;
	static HighScores instance;
	DisplayHighscores highScoresDisplay;

	// new part
	private static List<Highscore> entries = new List<Highscore>();


	void Awake()
	{
		highScoresDisplay = GetComponent<DisplayHighscores>();
		instance = this;
	}

	public static void AddNewHighscore(string username, int score)
	{
		instance.StartCoroutine(instance.UploadNewHighscore(username, score));
	}

	IEnumerator UploadNewHighscore(string username, int score)
	{
		WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(username) + "/" + score);
		yield return www;

		if (string.IsNullOrEmpty(www.error))
        {
			DownloadHighscores();
		}
		else
		{
			print("Error uploading: " + www.error);
		}
	}

	public void DownloadHighscores()
	{
		StartCoroutine("DownloadHighscoresFromDatabase");
	}

	IEnumerator DownloadHighscoresFromDatabase()
	{
		WWW www = new WWW(webURL + publicCode + "/pipe/");
		yield return www;

		if (string.IsNullOrEmpty(www.error))
        {
			FormatHighscores(www.text);
			highScoresDisplay.OnHighscoresDownloaded(highscoresList);
		}
		else
		{
			print("Error Downloading: " + www.error);
		}
	}

	void FormatHighscores(string textStream)
	{
		string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
		highscoresList = new Highscore[entries.Length];

		for (int i = 0; i < entries.Length; i++)
		{
			string[] entryInfo = entries[i].Split(new char[] { '|' });
			string username = entryInfo[0];
			int score;
			if (!int.TryParse(entryInfo[1], out score))
			{
				Debug.LogWarning($"Failed to parse score for entry {i}: {entryInfo[1]}");
				score = 0; // or handle the error as needed, e.g., skip this entry
			}
			highscoresList[i] = new Highscore(username, score);
		}
	}
	
}

public struct Highscore
{
	public string username;
	public int score;

	public Highscore(string _username, int _score)
	{
		username = _username;
		score = _score;
	}

}