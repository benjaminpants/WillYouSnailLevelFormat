using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WillYouSnailLevelFormat
{
    public class Element
    {
        public string ID = "invalid";
        public float Angle = (float)GameMakerAngles.Right;
        public float XScale = 1f;
        public float YScale = 1f;
        public Dictionary<string, float> Properties = new Dictionary<string, float>();

        public override string ToString()
        {
            return ID;
        }

        /// <summary>
        /// Serialize the data for this Element for the WYS Level Format.
        /// </summary>
        public virtual string Serialize()
        {
            StringBuilder strb = new StringBuilder();
            strb.AppendLine(ID);
            strb.AppendLine(Angle.ToString());
            strb.AppendLine(XScale.ToString());
            strb.AppendLine(YScale.ToString());
            strb.AppendLine(Properties.Count.ToString());
            foreach (KeyValuePair<string, float> kvp in Properties)
            {
                strb.AppendLine(kvp.Key);
                strb.AppendLine(kvp.Value.ToString());
            }
            strb.Remove(strb.Length - Environment.NewLine.Length, Environment.NewLine.Length);
            return strb.ToString();
        }

    }

    public class LevelElement : Element
    {
        public Vector2 Position = new Vector2(0f,0f);

        public override string ToString()
        {
            return base.ToString() + ":" + Position.ToString();
        }

        /// <summary>
        /// Serialize the data for this Element for the WYS Level Format.
        /// The ID is not included in the serialized data.
        /// </summary>
        public override string Serialize()
        {
            StringBuilder strb = new StringBuilder();
            strb.AppendLine(Position.X.ToString());
            strb.AppendLine(Position.Y.ToString());
            strb.AppendLine(Angle.ToString());
            strb.AppendLine(XScale.ToString());
            strb.AppendLine(YScale.ToString());
            strb.AppendLine(Properties.Count.ToString());
            foreach (KeyValuePair<string, float> kvp in Properties)
            {
                strb.AppendLine(kvp.Key);
                strb.AppendLine(kvp.Value.ToString());
            }
            strb.Remove(strb.Length - Environment.NewLine.Length, Environment.NewLine.Length);
            return strb.ToString();

        }
    }
}
