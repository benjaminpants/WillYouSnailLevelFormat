using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WillYouSnailLevelFormat;

namespace WYSLevelFormatTests
{
    [TestClass]
    public class LevelTests
    {

        public BaseLevel CreateTestLevel()
        {
            BaseLevel level = new BaseLevel();

            LevelElement element = new LevelElement();

            Element elemente = new Element();

            LevelElement element2 = new LevelElement();

            Element elemente2 = new Element();

            elemente.ID = "test1";

            element.ID = "test1";

            elemente2.ID = "test2";

            element2.ID = "test2";

            element2.Properties.Add("testprop", 1.25f);
            elemente2.Properties.Add("testprop", 1.25f);
            elemente2.Properties.Add("nocopy", -1.25f);

            level.QuickSlots[0] = "TESTSLOT0";
            level.QuickSlots[1] = "TESTSLOT1";
            level.QuickSlots[2] = "TESTSLOT2";
            level.QuickSlots[3] = "TESTSLOT3";
            level.QuickSlots[4] = "TESTSLOT4";
            level.QuickSlots[5] = "TESTSLOT5";
            level.QuickSlots[6] = "TESTSLOT6";
            level.QuickSlots[7] = "TESTSLOT7";
            level.QuickSlots[8] = "TESTSLOT8";
            level.QuickSlots[9] = "TESTSLOT9";

            level.ToolData.Add(elemente);

            level.Elements.Add(element);

            level.ToolData.Add(elemente2);

            level.Elements.Add(element2);

            level.Connect(element,element2);

            return level;
        }

        [TestMethod]
        public void TestSerializationAndReserialization()
        {
            BaseLevel level = CreateTestLevel();

            string levelinitserialize = level.Serialize();

            BaseLevel SerializeAgain = BaseLevel.FromText(levelinitserialize);

            Assert.AreEqual(levelinitserialize, SerializeAgain.Serialize());

        }

        [TestMethod]
        public void TestOptimization()
        {
            BaseLevel level = CreateTestLevel();

            string levelinitserialize = level.Serialize(true);

            string levelinitee = level.Serialize(false);

            Assert.AreEqual(levelinitee, levelinitserialize);

            Element testE = new Element();

            testE.ID = "EEEE";

            level.ToolData.Add(testE);

            levelinitserialize = level.Serialize(true);

            levelinitee = level.Serialize(false);

            Assert.AreNotEqual(levelinitee, levelinitserialize);

        }


        [TestMethod]
        public void CheckConnections()
        {
            BaseLevel level = CreateTestLevel();

            level.GetConnection(level.Connections.First(), out LevelElement? from, out LevelElement? to);
            Assert.IsNotNull(from);
            Assert.IsNotNull(to);

            Wire? w = null;

            try
            {
                w = new Wire(level, new LevelElement(), new LevelElement());
            }
            catch
            {

            }

            //Should be null, since the wire SHOULD NOT BE SUCCESFULLY CREATED due to neither of the component elements existing.
            Assert.IsNull(w);

        }
    }
}