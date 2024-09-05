using BASeCamp.Elementizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASeCamp.BASeScores
{
    public interface IHighScoreEntryCustomData : IXmlPersistable
    {

    }
    public class HighScoreNullCustomData : IHighScoreEntryCustomData
    {
        public HighScoreNullCustomData(XElement Source,object pContext)
        {

        }
        public HighScoreNullCustomData()
        {

        }
        public XElement GetXmlData(String pNodeName,object context)
        {
            return new XElement(pNodeName);
        }
    }
    public interface IHighScoreEntry : IComparable<IHighScoreEntry>
    {
        String Name { get; set; }
        int Score { get; set; }

        Object CustomData { get; }

    }
    public interface IHighScoreEntry<T> : IHighScoreEntry, IComparable<IHighScoreEntry<T>> where T:IHighScoreEntryCustomData
    {
        new T CustomData { get; set; }
        

    }
}
