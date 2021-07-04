using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using Characters;


namespace Scoreboards
{
    //[Serializable]
    public class Scoreboard : MonoBehaviour
    {

        [SerializeField] public int maxEntries = 5;
        [SerializeField] public RectTransform highscoresHolderTransform = null;
        [SerializeField] public GameObject scoreboardEntryObj = null;

        [Header("Test")]
        [SerializeField] public ScoreboardEntryData testData = new ScoreboardEntryData();

        private GameObject[] allScores;
        private string savePath = "https://solar-power-tech.com/game/GameTest/PHPTest/PHP/";  

        private void Start()
        {
            //SaveScore savedScores = GetSavedScores();
            //UpdateScores(savedScores);
            //UpdateUI(savedScores);
            Debug.Log("Initializing PHP");
            SaveScore savedScores = new SaveScore();
            savedScores.scores = new List<ScoreboardEntryData>() { testData };
            StartCoroutine(UpdateScores(savedScores));
        }

        public void AddEntry(ScoreboardEntryData scoreboardEntryData)
        {
            if (savePath == null)
            {
                savePath = "/highscores.json";
            }
            SaveScore savedScores = GetSavedScores();
            bool scoreAdded = false;



            //scoreboard cleaner
            for (int i = 0; i < savedScores.scores.Count; i++)
            {



                if (scoreboardEntryData.name == savedScores.scores[i].name && scoreboardEntryData.score > savedScores.scores[i].score)
                {
                    savedScores.scores.Remove(savedScores.scores[i]);

                    //savedScores.scores.Insert(i, scoreboardEntryData);
                    //scoreAdded = true;


                }
            }

            for (int i = 0; i < savedScores.scores.Count; i++)
            {
                if (scoreboardEntryData.score > savedScores.scores[i].score)
                {
                    savedScores.scores.Insert(i, scoreboardEntryData);
                    scoreAdded = true;
                    break;
                }
            }

            if (!scoreAdded && savedScores.scores.Count < maxEntries)
            {
                savedScores.scores.Add(scoreboardEntryData);
            }
            if (savedScores.scores.Count > maxEntries)
            {
                savedScores.scores.RemoveRange(maxEntries, savedScores.scores.Count - maxEntries);

            }



            UpdateUI(savedScores);

            UpdateScores(savedScores);
        }

        private void UpdateUI(SaveScore savedScores)
        {
            //Debug.Log("UpdateUI run");
            foreach (Transform child in highscoresHolderTransform)
            {
                Destroy(child.gameObject);

            }

            foreach (ScoreboardEntryData score in savedScores.scores)
            {


                Instantiate(scoreboardEntryObj, highscoresHolderTransform).GetComponent<ScoreboardEntryUI>().Initialize(score);
            }




        }

        // Stores the location of the PHP scripts to be used by this build
        private string phpDirectory = "https://solar-power-tech.com/game/GameTest/Build/PHP/";
        private SaveScore GetSavedScores()
        {
            //if (!File.Exists(savePath))
            //{
            //    File.Create(savePath).Dispose();

            //    return new SaveScore();
            //}

            //using (StreamReader stream = new StreamReader(savePath))
            //{
            //    string json = stream.ReadToEnd();



            //    return JsonUtility.FromJson<SaveScore>(json);
            //}
            // Do PHP instead
            // Return SaveScore (List of all ScoreBoardEntryData s)
            return new SaveScore();
        }

        private IEnumerator UpdateScores(SaveScore saveScore)
        {
            Debug.Log("Trying to write to database");
            //using (StreamWriter stream = new StreamWriter(savePath))
            //{
            //    string json = JsonUtility.ToJson(saveScore, true);
            //    stream.Write(json);
            //}
            // Do PHP instead
            // Saves new scores (Overwrites registers linked to same ScoreboardEntryData.user (UserID);
            foreach (ScoreboardEntryData data in saveScore.scores)
            {
                string errorMessage = "";

                WWWForm form = new WWWForm();
                form.AddField("user_id", data.user);
                form.AddField("username", data.name);
                form.AddField("score", data.score);
                // Tries to post a form using PHP
                using (UnityWebRequest www = UnityWebRequest.Post(savePath + "addscore.php", form))
                {
                    // Starts communicating with the server
                    yield return www.SendWebRequest();

                    // Checks for network errors
                    if (www.isNetworkError)
                    {
                        errorMessage = www.error;
                    }
                    else
                    {
                        // checks if form succesfully posted to the site
                        string responseText = www.downloadHandler.text;

                        // If not succesfully posted data
                        if(!responseText.StartsWith("Success"))
                        {
                            errorMessage = responseText;
                        }
                    }
                    if(errorMessage != "")
                    {
                        Debug.LogError(errorMessage);
                    }
                }
            }
        }


        void Update()
        {
            SaveScore savedScores = new SaveScore();
            savedScores.scores = new List<ScoreboardEntryData>() { testData };
            UpdateScores(savedScores);
        }

    }

}

