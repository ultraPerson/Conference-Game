using System.IO;
using UnityEngine;
using System;
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
            private string savePath;

            private void Start()
            {
                savePath = "/highscores.json";

                SaveScore savedScores = GetSavedScores();
                UpdateScores(savedScores);
                UpdateUI(savedScores);
            }

            public void AddEntry(ScoreboardEntryData scoreboardEntryData)
            {
                if(savePath == null){
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

            


            private SaveScore GetSavedScores()
            {
                if (!File.Exists(savePath))
                {
                    File.Create(savePath).Dispose();

                    return new SaveScore();
                }

                using (StreamReader stream = new StreamReader(savePath))
                {
                    string json = stream.ReadToEnd();

                    return JsonUtility.FromJson<SaveScore>(json);
                }

                return new SaveScore();

            }

            private void UpdateScores(SaveScore saveScore)
            {
                using (StreamWriter stream = new StreamWriter(savePath))
                {
                    string json = JsonUtility.ToJson(saveScore, true);
                    stream.Write(json);
                }
            }

            void Update()
            {

            }

        }

    }

