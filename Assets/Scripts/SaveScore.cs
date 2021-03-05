using System;
using System.Collections.Generic;


namespace Conference.Scoreboards
{
    [Serializable]
    public class SaveScore
    {
        public List<ScoreboardEntryData> scores = new List<ScoreboardEntryData>();

    }
}
