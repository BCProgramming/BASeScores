using BASeCamp.Elementizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASeCamp.BASeScores
{
    public interface IHighScoreList : IXmlPersistable
    {
        int MaximumSize { get; }
        IEnumerable<IHighScoreEntry> GetScores();

        /// <summary>
        /// Determines if the provided score is eligible for entry in the list.
        /// </summary>
        /// <param name="Score">Score to test.</param>
        /// <returns>the position that the score would place, or -1 if the score is not eligible.</returns>
        int IsEligible(int Score);

        IHighScoreEntry Submit(String pName, int Score);
        IHighScoreEntry Submit(IHighScoreEntry Item);

    }
    public interface IHighScoreList<T> : IHighScoreList, IXmlPersistable where T : IHighScoreEntryCustomData
    {
        int MaximumSize { get; }
    
        /// <summary>
        /// Retrieve all Scores present in this list.
        /// </summary>
        /// <returns>Enumeration of all HighScoreEntries, in descending order.</returns>
        IEnumerable<IHighScoreEntry<T>> GetScoresEx();

        /// <summary>
        /// Submits this high score entry into the list in the appropriate position, returning the constructed Entry instance.
        /// </summary>
        /// <param name="pName"></param>
        /// <param name="Score"></param>
        /// <param name="ScoreData"></param>
        /// <returns></returns>
        IHighScoreEntry<T> Submit(String pName, int Score, T ScoreData);

    }
}
