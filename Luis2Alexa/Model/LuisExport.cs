using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luis2Alexa.Model
{
    public class LuisExport
    {
        public string luis_schema_version { get; set; }
        public string versionId { get; set; }
        public string name { get; set; }
        public string desc { get; set; }
        public string culture { get; set; }
        public Intent[] intents { get; set; }
        public Entity[] entities { get; set; }
        public object[] composites { get; set; }
        public Closedlist[] closedLists { get; set; }
        public string[] bing_entities { get; set; }
        public object[] actions { get; set; }
        public Model_Features[] model_features { get; set; }
        public Regex_Features[] regex_features { get; set; }
        public Utterance[] utterances { get; set; }
    }

    public class Intent
    {
        public string name { get; set; }
    }

    public class Entity
    {
        public string name { get; set; }
    }

    public class Closedlist
    {
        public string name { get; set; }
        public Sublist[] subLists { get; set; }
    }

    public class Sublist
    {
        public string canonicalForm { get; set; }
        public object[] list { get; set; }
    }

    public class Model_Features
    {
        public string name { get; set; }
        public bool mode { get; set; }
        public string words { get; set; }
        public bool activated { get; set; }
    }

    public class Regex_Features
    {
        public string name { get; set; }
        public string pattern { get; set; }
        public bool activated { get; set; }
    }

    public class Utterance
    {
        public string text { get; set; }
        public string intent { get; set; }
        public Entity1[] entities { get; set; }
    }

    public class Entity1
    {
        public string entity { get; set; }
        public int startPos { get; set; }
        public int endPos { get; set; }
    }
}
