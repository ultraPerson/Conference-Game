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

        
        [SerializeField] public ScoreboardEntryData playerData;

        public SaveScore allScores;
        private string savePath = "https://solar-power-tech.com/game/GameTest/PHP/";  

        public void AddEntry(ScoreboardEntryData data)
        {
            SaveScore savedScore = new SaveScore();
            savedScore.scores = new List<ScoreboardEntryData>() { data };
            StartCoroutine(UpdateScores(savedScore));
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
        private IEnumerator GetSavedScores()
        {
            string errorMessage = "";
            SaveScore output = new SaveScore();
            using (UnityWebRequest www = UnityWebRequest.Get(savePath + "getscore.php"))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError)
                {
                    errorMessage = www.error;
                }
                else
                {
                    string responseText = www.downloadHandler.text;
                    Debug.LogError(responseText);
                    string[] allRows;
                    if(!responseText.StartsWith("Success"))
                    {
                        errorMessage = responseText;
                    }
                    else
                    {
                        allRows = responseText.Split("|".ToCharArray());
                        foreach(string row in allRows)
                        {
                            if(row.StartsWith("Success"))
                            {
                                continue;
                            }
                            string[] columns = row.Split(",".ToCharArray());
                            ScoreboardEntryData newEntry = new ScoreboardEntryData() { name = columns[0], score = int.Parse(columns[1]) };
                            output.scores.Add(newEntry);
                        }
                        allScores = output;
                    }
                }
            }
            UpdateUI(allScores);
        }

        private IEnumerator UpdateScores(SaveScore saveScore)
        {
            foreach (ScoreboardEntryData data in saveScore.scores)
            {
                string errorMessage = "";

                WWWForm form = new WWWForm();
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
                        if (!responseText.StartsWith("Success"))
                        {
                            errorMessage = responseText;
                        }
                    }
                    if (errorMessage != "")
                    {
                        Debug.LogError(errorMessage);
                    }
                }
            }
            StartCoroutine(GetSavedScores());
        }

    }

}

