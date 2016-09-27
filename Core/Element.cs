using System;

namespace Greatbone.Core
{

    public enum VT
    {
        String, Array, Object, Null, Bool
    }

    /// <summary>
    /// An element represents either a value or a name/value pair.
    /// </summary>
    public struct Element : IMember
    {
        // type of value
        VT vt;

        Number numv;

        DateTime dtv;

        // boolean value
        bool boolv;

        // Obj, Arr, string, byte[]
        internal object refv;

        string name;

        public string Key => name;

        public bool IsPair => name != null;

        public static implicit operator Obj(Element v)
        {
            return (Obj)v.refv;
        }

        public static implicit operator Arr(Element v)
        {
            return (Arr)v.refv;
        }

        public static implicit operator int(Element v)
        {
            return 0;
        }

        public static implicit operator string(Element v)
        {
            return (string)v.refv;
        }
    }
}