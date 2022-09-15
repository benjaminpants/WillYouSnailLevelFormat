using System;
using System.Numerics;
using System.Text;

namespace WillYouSnailLevelFormat
{

    /// <summary>
    /// The base level object, this can be serialized and deserialized into valid Will You Snail levels.
    /// </summary>
    public class BaseLevel
    {
        public string GameVersion = "1.5";
        public Vector2 LevelBounds = new Vector2(1920, 1080);

        /// <summary>
        /// ToolData is all the properties for every object in the toolbar, this is where global object settings would be defined.
        /// There should be no duplicate IDs.
        /// </summary>
        public List<Element> ToolData = new List<Element>();

        /// <summary>
        /// All the elements that are in the level.
        /// Adding elements anywhere but the end of the list may break connections.
        /// </summary>
        public List<LevelElement> Elements = new List<LevelElement>();

        /// <summary>
        /// The connections between objects.
        /// </summary>
        public List<Wire> Connections = new List<Wire>();

        /// <summary>
        /// The slots in the hotbar.
        /// </summary>
        public string[] QuickSlots = new string[10];

        public override int GetHashCode()
        {
            return HashCode.Combine(GameVersion, LevelBounds.X, LevelBounds.Y, QuickSlots, Elements, ToolData);
        }

        public override string ToString()
        {
            return GameVersion + " | " + Elements.Count + " | " + (LevelBounds.X / 60f) + "x" + (LevelBounds.Y / 60f);
        }

        public static BaseLevel FromText(string LevelData)
        {

            BaseLevel level = new BaseLevel();

            string[] Lines = LevelData.Split(Environment.NewLine, StringSplitOptions.None);



            level.GameVersion = Lines[0];

            level.LevelBounds = new Vector2(float.Parse(Lines[3]), float.Parse(Lines[4]));

            int CurrentLine = 8; //Start at line 8 instead of 7 because line 7 just shows how many elements are in the list which we can ignore
            while (Lines[CurrentLine] != "")
            {
                Element el = new Element();
                el.ID = Lines[CurrentLine];
                CurrentLine++;
                el.Angle = float.Parse(Lines[CurrentLine]);
                CurrentLine++;
                el.XScale = float.Parse(Lines[CurrentLine]);
                CurrentLine++;
                el.YScale = float.Parse(Lines[CurrentLine]);
                CurrentLine++;
                int PropertiesToCopy = int.Parse(Lines[CurrentLine]);
                CurrentLine++;
                for (int i = 0; i < PropertiesToCopy; i++)
                {
                    el.Properties.Add(Lines[CurrentLine], float.Parse(Lines[CurrentLine + 1]));
                    CurrentLine += 2;
                }
                level.ToolData.Add(el);
            }

            CurrentLine++;

            for (int i = 0; i < 10; i++)
            {
                CurrentLine++;

                level.QuickSlots[i] = Lines[CurrentLine];
            }

            CurrentLine += 4;

            string CurrentObjectType = Lines[CurrentLine];

            while (CurrentObjectType != "") //Again we can ignore the count of how many total elements since we can just keep going until we encounter a ""
            {
                //Console.WriteLine((CurrentLine + 1) + ":" + CurrentObjectType);
                CurrentLine++;
                int CurrentObjectCount = int.Parse(Lines[CurrentLine]);
                CurrentLine++;
                //throw new Exception(CurrentLine + ":" + CurrentObjectCount);
                for (int i = 0; i < CurrentObjectCount; i++)
                {
                    LevelElement el = new LevelElement();
                    el.ID = CurrentObjectType;
                    el.Position = new Vector2(float.Parse(Lines[CurrentLine]), float.Parse(Lines[CurrentLine + 1]));
                    CurrentLine += 2;
                    el.Angle = float.Parse(Lines[CurrentLine]);
                    CurrentLine++;
                    el.XScale = float.Parse(Lines[CurrentLine]);
                    CurrentLine++;
                    el.YScale = float.Parse(Lines[CurrentLine]);
                    CurrentLine++;
                    int PropertiesToCopy = int.Parse(Lines[CurrentLine]);
                    CurrentLine++;
                    for (int j = 0; j < PropertiesToCopy; j++)
                    {
                        el.Properties.Add(Lines[CurrentLine], float.Parse(Lines[CurrentLine + 1]));
                        CurrentLine += 2;
                    }
                    level.Elements.Add(el);
                }
                CurrentObjectType = Lines[CurrentLine];

            }

            CurrentLine += 2;

            if (CurrentLine > (Lines.Length - 1))
            {
                return level;
            }

            int WireCount = int.Parse(Lines[CurrentLine]);

            for (int i = 0; i < WireCount; i++)
            {
                CurrentLine++;
                string ID1 = Lines[CurrentLine];
                int Index1 = int.Parse(Lines[CurrentLine + 1]);
                string ID2 = Lines[CurrentLine + 2];
                int Index2 = int.Parse(Lines[CurrentLine + 3]);
                level.Connections.Add(new Wire(new ElementReference(Index1, ID1), new ElementReference(Index2, ID2)));
                CurrentLine += 4;
            }





            return level;
        }

        public static BaseLevel FromFile(string Path)
        {
            return FromText(File.ReadAllText(Path));
        }

        /// <summary>
        /// Serializes this level into the WYS Level Format.
        /// Setting optimize to true will cut unnecessary entries from the Level Data list when serializing to reduce filesize.
        /// </summary>
        public virtual string Serialize(bool Optimize = false)
        {
            StringBuilder strb = new StringBuilder();

            strb.AppendLine(GameVersion);

            strb.AppendLine();

            strb.AppendLine("LEVEL DIMENSIONS:");

            strb.AppendLine(LevelBounds.X.ToString());
            strb.AppendLine(LevelBounds.Y.ToString());

            strb.AppendLine();

            strb.AppendLine("TOOL DATA:");

            strb.AppendLine(ToolData.Count.ToString());

            foreach (Element el in ToolData)
            {
                strb.AppendLine(el.Serialize());
            }

            strb.AppendLine();
            
            strb.AppendLine("QUICK SLOTS:");

            for (int i = 0; i < 10; i++)
            {
                strb.AppendLine(QuickSlots[i]);
            }

            strb.AppendLine();

            strb.AppendLine("PLACED OBJECTS:");

            Dictionary<string, List<LevelElement>> OrganizedElements = new Dictionary<string, List<LevelElement>>();

            if (!Optimize)
            {
                strb.AppendLine(ToolData.Count.ToString());

                foreach (Element e in ToolData)
                {
                    OrganizedElements.Add(e.ID, new List<LevelElement>());
                }
                foreach (LevelElement e in Elements)
                {
                    OrganizedElements[e.ID].Add(e);
                }

            }
            else
            {
                foreach (LevelElement e in Elements)
                {
                    if (!OrganizedElements.ContainsKey(e.ID))
                    {
                        OrganizedElements.Add(e.ID, new List<LevelElement>());
                    }
                    OrganizedElements[e.ID].Add(e);
                }
                strb.AppendLine(OrganizedElements.Count.ToString());
            }

            foreach (KeyValuePair<string, List<LevelElement>> kvp in OrganizedElements)
            {
                strb.AppendLine(kvp.Key);
                strb.AppendLine(kvp.Value.Count.ToString());
                foreach (LevelElement v in kvp.Value)
                {
                    strb.AppendLine(v.Serialize());
                }
            }

            strb.AppendLine();

            strb.AppendLine("WIRES:");

            strb.AppendLine(Connections.Count.ToString());

            foreach (Wire w in Connections)
            {
                strb.AppendLine(w.From.ID);
                strb.AppendLine(w.From.Index.ToString());
                strb.AppendLine(w.To.ID);
                strb.AppendLine(w.To.Index.ToString());
            }

            return strb.ToString();
        }


        /// <summary>
        /// Get all LevelElements with a matching ID.
        /// </summary>
        public virtual List<LevelElement> GetLevelElementsOfID(string ID)
        {
            return Elements.FindAll(s => s.ID == ID);
        }

        /// <summary>
        /// Get the LevelElements of the corresponding connection.
        /// Returns true if succesful.
        /// </summary>
        public bool GetConnection(Wire wire, out LevelElement? from, out LevelElement? to)
        {
            try
            {
                from = GetLevelElementsOfID(wire.From.ID)[wire.From.Index];
                to = GetLevelElementsOfID(wire.To.ID)[wire.To.Index];
            }
            catch (Exception)
            {
                from = null;
                to = null;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Wire two LevelElements together.
        /// Returns true if succesful.
        /// </summary>
        public bool Connect(LevelElement from, LevelElement to)
        {
            if (Elements.IndexOf(from) == -1 || Elements.IndexOf(to) == -1)
                return false;

            Connections.Add(new Wire(this, from, to));
            return true;
        }

    }


}