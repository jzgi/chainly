using System;

namespace Greatbone.Core
{

    ///
    /// <summary>
    /// A JSON object model.
    /// </summary>
    ///
    public class JObj : ISource
    {
        const int InitialCapacity = 16;

        readonly Roll<JMember> pairs;

        public JObj(int capacity = InitialCapacity)
        {
            pairs = new Roll<JMember>(16);
        }

        // add null
        internal void Add(string name)
        {
            pairs.Add(new JMember() { Key = name });
        }

        internal void Add(string name, JObj v)
        {
            pairs.Add(new JMember(v) { Key = name });
        }

        internal void Add(string name, JArr v)
        {
            pairs.Add(new JMember(v) { Key = name });
        }

        internal void Add(string name, string v)
        {
            pairs.Add(new JMember(v) { Key = name });
        }

        internal void Add(string name, byte[] v)
        {
            pairs.Add(new JMember(v) { Key = name });
        }

        internal void Add(string name, bool v)
        {
            pairs.Add(new JMember(v) { Key = name });
        }

        internal void Add(string name, Number v)
        {
            pairs.Add(new JMember(v) { Key = name });
        }

        public int Count => pairs.Count;

        public JMember this[int index] => pairs[index];

        public JMember this[string name] => pairs[name];

        //
        // SOURCE
        //

        public bool Get(string name, ref bool v)
        {
            JMember pair;
            if (pairs.TryGet(name, out pair))
            {
                v = (bool)pair;
                return true;
            }
            return false;
        }

        public bool Get(string name, ref short v)
        {
            JMember pair;
            if (pairs.TryGet(name, out pair))
            {
                v = (short)pair;
                return true;
            }
            return false;
        }

        public bool Get(string name, ref int v)
        {
            JMember pair;
            if (pairs.TryGet(name, out pair))
            {
                v = (int)pair;
                return true;
            }
            return false;
        }

        public bool Get(string name, ref long v)
        {
            JMember pair;
            if (pairs.TryGet(name, out pair))
            {
                v = (short)pair;
                return true;
            }
            return false;
        }

        public bool Get(string name, ref decimal v)
        {
            JMember pair;
            if (pairs.TryGet(name, out pair))
            {
                v = (decimal)pair;
                return true;
            }
            return false;
        }

        public bool Get(string name, ref Number v)
        {
            JMember pair;
            if (pairs.TryGet(name, out pair))
            {
                v = (Number)pair;
                return true;
            }
            return false;
        }

        public bool Get(string name, ref DateTime v)
        {
            JMember pair;
            if (pairs.TryGet(name, out pair))
            {
                v = (DateTime)pair;
                return true;
            }
            return false;
        }

        public bool Get(string name, ref char[] v)
        {
            JMember pair;
            if (pairs.TryGet(name, out pair))
            {
                v = (char[])pair;
                return true;
            }
            return false;
        }

        public bool Get(string name, ref string v)
        {
            JMember pair;
            if (pairs.TryGet(name, out pair))
            {
                v = (string)pair;
                return true;
            }
            return false;
        }

        public bool Get(string name, ref byte[] v)
        {
            JMember pair;
            if (pairs.TryGet(name, out pair))
            {
                v = (byte[])pair;
                return true;
            }
            return false;
        }

        public bool Get(string name, ref ArraySegment<byte>? v)
        {
            JMember pair;
            if (pairs.TryGet(name, out pair))
            {
                byte[] bv = (byte[])pair;
                v = new ArraySegment<byte>(bv);
                return true;
            }
            return false;
        }

        public bool Get<P>(string name, ref P v, byte x = 0xff) where P : IPersist, new()
        {
            JMember pair;
            if (pairs.TryGet(name, out pair))
            {
                JObj jo = (JObj)pair;
                if (jo != null)
                {
                    v = new P();
                    v.Load(jo);
                }
                return true;
            }
            return false;
        }

        public bool Get(string name, ref JObj v)
        {
            JMember pair;
            if (pairs.TryGet(name, out pair))
            {
                v = (JObj)pair;
                return true;
            }
            return false;
        }

        public bool Get(string name, ref JArr v)
        {
            JMember pair;
            if (pairs.TryGet(name, out pair))
            {
                v = (JArr)pair;
                return true;
            }
            return false;
        }

        public bool Get(string name, ref short[] v)
        {
            JMember pair;
            if (pairs.TryGet(name, out pair))
            {
                JArr ja = pair;
                if (ja != null)
                {
                    v = new short[ja.Count];
                    for (int i = 0; i < ja.Count; i++)
                    {
                        v[i] = ja[i];
                    }
                }
                return true;
            }
            return false;
        }

        public bool Get(string name, ref int[] v)
        {
            JMember pair;
            if (pairs.TryGet(name, out pair))
            {
                JArr ja = pair;
                if (ja != null)
                {
                    v = new int[ja.Count];
                    for (int i = 0; i < ja.Count; i++)
                    {
                        v[i] = ja[i];
                    }
                }
                return true;
            }
            return false;
        }

        public bool Get(string name, ref long[] v)
        {
            JMember pair;
            if (pairs.TryGet(name, out pair))
            {
                JArr ja = pair;
                if (ja != null)
                {
                    v = new long[ja.Count];
                    for (int i = 0; i < ja.Count; i++)
                    {
                        v[i] = ja[i];
                    }
                }
                return true;
            }
            return false;
        }

        public bool Get(string name, ref string[] v)
        {
            JMember pair;
            if (pairs.TryGet(name, out pair))
            {
                JArr ja = pair;
                if (ja != null)
                {
                    v = new string[ja.Count];
                    for (int i = 0; i < ja.Count; i++)
                    {
                        v[i] = ja[i];
                    }
                }
                return true;
            }
            return false;
        }

        public bool Get<P>(string name, ref P[] v, byte x = 0xff) where P : IPersist, new()
        {
            JMember pair;
            if (pairs.TryGet(name, out pair))
            {
                JArr ja = pair;
                if (ja != null)
                {
                    v = new P[ja.Count];
                    for (int i = 0; i < ja.Count; i++)
                    {
                        JObj jo = ja[i];
                        P obj = new P(); obj.Load(jo);
                        v[i] = obj;
                    }
                }
                return true;
            }
            return false;
        }


        internal void Dump<R>(ISink<R> snk) where R : ISink<R>
        {
            for (int i = 0; i < pairs.Count; i++)
            {
                JMember mbr = pairs[i];
                JType typ = mbr.type;
                if (typ == JType.Array)
                {
                    snk.Put(mbr.Key, (JArr)mbr);
                }
                else if (typ == JType.Object)
                {
                    snk.Put(mbr.Key, (JObj)mbr);
                }
                else if (typ == JType.String)
                {
                    snk.Put(mbr.Key, (string)mbr);
                }
                else if (typ == JType.Number)
                {
                    snk.Put(mbr.Key, (Number)mbr);
                }
                else if (typ == JType.True)
                {
                    snk.Put(mbr.Key, true);
                }
                else if (typ == JType.False)
                {
                    snk.Put(mbr.Key, false);
                }
                else if (typ == JType.Null)
                {
                    snk.PutNull(mbr.Key);
                }
            }
        }

    }

}