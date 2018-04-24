using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BASeCamp.Elementizer;


namespace BASeCamp.BASeScores
{
    public class XMLScoreManager<T> where T:IHighScoreEntryCustomData
    {
        Dictionary<String, XMLHighScores<T>> LoadedScoreSets = new Dictionary<string, XMLHighScores<T>>();
        public Func<string, XMLHighScores<T>> NewScoreListGenerator = GenerateNewScoreList;
        public XMLScoreManager(XElement Source,Object context)
        {
            LoadFromXElement(Source,context);
        }
        private static XMLHighScores<T> GenerateNewScoreList(String pName)
        {
            XMLHighScores<T> buildList = new XMLHighScores<T>(pName,25000);
            return buildList;
        }
        public void LoadFromXElement(XElement Source,Object pContext)
        {
            foreach(var loadelement in Source.Elements("ScoreSet"))
            {
                XMLHighScores<T> buildscoreset = new XMLHighScores<T>(loadelement, pContext);
                LoadedScoreSets.Add(buildscoreset.Name,buildscoreset);
            }
        }
        public XMLScoreManager(String pFileName)
        {
            if (File.Exists(pFileName))
            {
                XDocument loadDocument = XDocument.Load(pFileName);
                LoadFromXElement(loadDocument.Root, null);
            }
        }
        public XElement GetXmlData(String pNodeName,Object context)
        {
            XElement SavedRoot = new XElement(pNodeName,null);
            foreach(var iteratekey in LoadedScoreSets)
            {
                XElement SaveNode = iteratekey.Value.GetXmlData("ScoreSet", context);
                SavedRoot.Add(SaveNode);
            }


            return SavedRoot;
        }

        public XMLHighScores<T> this[String ScoreSet]
        {
            get
            {
                if(!LoadedScoreSets.ContainsKey(ScoreSet))
                {
                    LoadedScoreSets.Add(ScoreSet,NewScoreListGenerator(ScoreSet));
                }
                return LoadedScoreSets[ScoreSet];
            }
            set => LoadedScoreSets[ScoreSet] = value;
        }

    }
}
