using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BASeCamp.Elementizer;

namespace BASeCamp.BASeScores
{
    /// <summary>
    /// Represents an entry in the high score table. This includes any associated name, the score, and an instance of the custom data type parameter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class XMLScoreEntry<T> : ScoreEntry<T>,IXmlPersistable where T : IHighScoreEntryCustomData
    {
      
        public XMLScoreEntry(String pName,int pScore,T pCustomData)
        {
            Name = pName;
            Score = pScore;
            CustomData = pCustomData;
        }
        public XMLScoreEntry(XElement SourceNode,Object pPersistenceData)
        {
            String sName = SourceNode.GetAttributeString("Name", "");
            int iScore = SourceNode.GetAttributeInt("Score", 0);
            XElement ExtraNode = SourceNode.Element("Extra");
            if(ExtraNode!=null)
            {
                CustomData = (T)Activator.CreateInstance(typeof(T), (XElement)ExtraNode, (object)null);
            }
            Name = sName;
            Score = iScore;
        }
        public XElement GetXmlData(string pNodeName, object PersistenceData)
        {
            XElement BuildElement = new XElement(pNodeName, new XAttribute("Name", Name), new XAttribute("Score", Score));
            if(CustomData!=null)
            {
                XElement CustomElement = CustomData.GetXmlData("Extra", null);
                if(CustomElement!=null)
                    BuildElement.Add(CustomElement);
            }
            return BuildElement;
        }
    }
}
