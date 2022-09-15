using System;
using System.Diagnostics.CodeAnalysis;

namespace WillYouSnailLevelFormat
{
    /// <summary>
    /// Represents a wire/connection between 2 objects.
    /// </summary>
    public struct Wire
    {
        public ElementReference From;
        public ElementReference To;

        public override string ToString()
        {
            return From.ToString() + ":" + To.ToString();
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null)
                return false;
            if (!(obj is Wire))
                return false;
            Wire other = (Wire)obj;
            return other.From.Equals(From) && other.To.Equals(To);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(From.GetHashCode(), To.GetHashCode());
        }

        /// <summary>
        /// Create a wire from two ElementReferences.
        /// </summary>
        public Wire(ElementReference from, ElementReference to)
        {
            To = to;
            From = from;
        }

        /// <summary>
        /// Create a wire from a Level and two LevelElements that are in the Level.
        /// </summary>
        public Wire(BaseLevel lvl, LevelElement from, LevelElement to)
        {
            ElementReference f = new ElementReference(lvl.GetLevelElementsOfID(from.ID).IndexOf(from),from.ID);
            ElementReference t = new ElementReference(lvl.GetLevelElementsOfID(to.ID).IndexOf(to), to.ID);
            if (f.Index == -1  || t.Index == -1)
            {
                throw new IndexOutOfRangeException();
            }
            To = t;
            From = f;
        }

    }

    /// <summary>
    /// Represents a reference to an Element, where Index is its index in a list of elements only containing Elements of the same ID.
    /// </summary>
    public struct ElementReference
    {
        public int Index;
        public string ID;

        public override string ToString()
        {
            return ID + "-" + Index;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Index, ID);
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null)
                return false;
            if (!(obj is ElementReference))
                return false;
            ElementReference other = (ElementReference)obj;
            return other.ID == this.ID && other.Index == this.Index;
        }

        public ElementReference(int i, string id)
        {
            Index = i;
            ID = id;
        }
    }
}
