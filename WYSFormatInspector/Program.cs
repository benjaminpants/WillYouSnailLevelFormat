using System.Numerics;
using WillYouSnailLevelFormat;


Level lvl = Level.FromFile("C:\\Users\\User1\\AppData\\Local\\Will_You_Snail\\Community Levels\\MyFirstCampaign\\Level 7.lvl");

LevelElement door = lvl.Elements.Find(s => s.ID == "door");

LevelElement doorcopy = new LevelElement();

doorcopy.ID = "door";

doorcopy.YScale = door.YScale;
doorcopy.XScale = door.XScale;


doorcopy.Position = door.Position + new Vector2(60f,0f);

lvl.Elements.Add(doorcopy);

lvl.Connect(door,doorcopy);

File.WriteAllText("C:\\Users\\User1\\AppData\\Local\\Will_You_Snail\\Community Levels\\MyFirstCampaign\\Trolled.lvl",lvl.Serialize(true));

