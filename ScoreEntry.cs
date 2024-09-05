using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BASeCamp.BASeScores
{
    public abstract class ScoreEntry<T> : IHighScoreEntry<T> where T:IHighScoreEntryCustomData
    {
        public virtual int CompareTo(IHighScoreEntry other)
        {
            return Score.CompareTo(other.Score);
        }


        public string Name { get; set; }
        public int Score { get; set; }
        public virtual int CompareTo(IHighScoreEntry<T> other)
        {
            return Score.CompareTo(other.Score);
        }
        

        public T CustomData { get; set; }
        object IHighScoreEntry.CustomData { get => CustomData; }
    }
}
