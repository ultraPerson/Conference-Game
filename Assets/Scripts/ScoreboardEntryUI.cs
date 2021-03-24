using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

namespace Conference.Characters
{
    namespace Conference.Scoreboards
    {


        public class ScoreboardEntryUI : MonoBehaviour
        {
            [SerializeField] public Text entryNameText = null;
            [SerializeField] public Text entryScoreText = null;

            public void Initialize(ScoreboardEntryData scoreboardEntryData)
            {
                entryNameText.text = scoreboardEntryData.name;
                entryScoreText.text = scoreboardEntryData.score.ToString();
            }
        }
    }
}