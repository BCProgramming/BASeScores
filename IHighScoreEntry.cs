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
        public HighScoreNullCustomData(XElement Source)
        {

        }
        public XElement GetXmlData(String pNodeName,object context)
        {
            return new XElement(pNodeName);
        }
    }
    public interface IHighScoreEntry<T> : IComparable<IHighScoreEntry<T>>,IXmlPersistable where T:IHighScoreEntryCustomData
    {
        T CustomData { get; set; }
        String Name { get; set; }
        int Score { get; set; }


    }
}
