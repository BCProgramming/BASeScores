using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BASeCamp.Elementizer;

namespace BASeCamp.BASeScores
{
    public class NoSpecialInfo:IHighScoreEntryCustomData
    {
        public NoSpecialInfo(XElement Source,Object pSpecialData)
        {

        }
        public  NoSpecialInfo()
        {

        }
        public XElement GetXmlData(string pNodeName, object PersistenceData)
        {
            return new XElement(pNodeName);
        }
    }
    public class XMLHighScores<T> : IHighScoreList<T> where T:IHighScoreEntryCustomData
    {
        class DescComparer<T> : IComparer<T>
        {
            public int Compare(T x, T y)
            {
                return Comparer<T>.Default.Compare(y, x);
            }
        }
        public String Name { get; set; }
        public int MaximumSize {  get { return 10; } }
        SortedList<int,XMLScoreEntry<T>> ScoreEntries = new SortedList<int, XMLScoreEntry<T>>(new DescComparer<int>());
        public XElement GetXmlData(string pNodeName, object PersistenceData)
        {
           XElement buildnode = new XElement(pNodeName);
            buildnode.Add(new XAttribute("Name",Name));
            foreach(var iterate in ScoreEntries)
            {
                var createnode = iterate.Value.GetXmlData("Score", PersistenceData);
                buildnode.Add(createnode);
            }
            return buildnode;

        }
        public XMLHighScores(String pName,int MaximumGeneratedScore,Func<XMLScoreEntry<T>,T> GenerateCustomDataFunc=null)
        {
            Name = pName;
            if (MaximumGeneratedScore>0) InitializeRandom(GenerateCustomDataFunc);
        }
        private static Random rgen = new Random();
        private int MaximumScore = 35000;
        private static String[] RandomNames = new string[]{"Slash","Jam","Nancy","Tommy","Linda","Wendy","Mario","Luigi","Jezebel","Clownman","Jok4r","Scarycat","Dictionary attack","Clojure Lover","M$ Shill","Lintard","RMS Troll","Max",
            "Bearsky","Toad","h4x0r","Jim","Sir Slush","Marle","Terra","random pidgeon","unrandom pidgeon","barfly","Chuck","Jok4r","Israphel","MelonMan","Simon","Timmy","Paul the Musician","Carl the Yellow","FlowerFace","Nibbles.bas","Paddler","RAZAR","BABAR","SNUFFLEOPAGUS","Big Bird","One Fish","Two Fish","Red Fish","Blue Fish","Santa","Oogie","Flashlight","pickleJar","Blanket Jackson","Screwdriver face","Fuddlenuts","Stewart Cheifet","Bears","SAMSINGER"};
        private void InitializeRandom(Func<XMLScoreEntry<T>, T> CustomDataFunc)
        {
            for(int i=0;i<MaximumSize;i++)
            {
                int NameIndex = rgen.Next(RandomNames.Length);
                String sUseName = RandomNames[NameIndex];
                XMLScoreEntry<T> buildentry = new XMLScoreEntry<T>(sUseName, rgen.Next(MaximumScore), default(T));
                if(CustomDataFunc!=null) buildentry.CustomData = CustomDataFunc(buildentry);
                ScoreEntries.Add(buildentry.Score,buildentry);
            }
        }
        public XMLHighScores(XElement SourceNode,Object pPersistenceData)
        {
            Name = SourceNode.GetAttributeString("Name", "Default");
            foreach(var ScoreElement in SourceNode.Elements("Score"))
            {
                XMLScoreEntry<T> newEntry = new XMLScoreEntry<T>(ScoreElement, pPersistenceData);
                ScoreEntries.Add(newEntry.Score,newEntry);
            }
        }
        public IEnumerable<IHighScoreEntry<T>> GetScoresEx()
        {
            foreach(var iterate in ScoreEntries)
            {
                yield return iterate.Value;
            }
        }
        public IEnumerable<IHighScoreEntry> GetScores()
        {
            return GetScoresEx();
        }

        public int IsEligible(int Score)
        {
            if (ScoreEntries.Count < MaximumSize) return ScoreEntries.Count;
            else
            {
                int i = 0;
                foreach(var loopentry in ScoreEntries)
                {
                    if (loopentry.Value.Score < Score) return i+1;
                    i++;
                }
                return -1;
            }
        }
        public IHighScoreEntry Submit(String pName,int Score)
        {
            return Submit(pName, Score, default(T));
        }
        public IHighScoreEntry Submit(IHighScoreEntry newEntry)
        {
            
            IHighScoreEntry<T> casted = newEntry as IHighScoreEntry<T>;
            if (casted != null)
            {
                return Submit(casted.Name, casted.Score, casted.CustomData);
            }
            return Submit(newEntry.Name, newEntry.Score);
            
        }
        public IHighScoreEntry<T> Submit(string pName, int Score, T ScoreData)
        {
            XMLScoreEntry<T> buildentry = new XMLScoreEntry<T>(pName, Score, ScoreData);
            int eligibleresult = IsEligible(Score);
            if (eligibleresult == -1) return null;

            ScoreEntries.Add(Score,buildentry);

            //until we have MaximumSize items, remove the item with the smallest score.
            while(ScoreEntries.Count > MaximumSize)
            {
                ScoreEntries.Remove(ScoreEntries.Last().Key);
            }

            return buildentry;
        }
    }
}
