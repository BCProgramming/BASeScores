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
        private String _FileName = null;

        public String[] GetKeys()
        {
            return LoadedScoreSets.Keys.ToArray();
        }
        public Dictionary<String, XMLHighScores<T>> GetAllScoreSets()
        {
            return LoadedScoreSets;
        }

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
        public static XMLScoreManager<T> FromFile(String sFileName)
        {
            if (String.IsNullOrEmpty(sFileName) || !File.Exists(sFileName)) return null;
            return new XMLScoreManager<T>(sFileName);
        }
        ~XMLScoreManager()
        {
            Save(_FileName);
        }
        public void Save()
        {
            Save(_FileName);
        }
        public void Save(String pTargetFile)
        {
            if (String.IsNullOrEmpty(pTargetFile)) return;
            XDocument buildDocument = new XDocument();
            buildDocument.Add(GetXmlData("HighScores",null));
            buildDocument.Save(pTargetFile);
        }
        public XMLScoreManager(String pFileName)
        {
            _FileName = pFileName;
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
        public IEnumerable<XMLHighScores<T>> AllScores()
        {
            return LoadedScoreSets.Values;
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
