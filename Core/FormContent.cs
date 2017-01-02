﻿using System;
using NpgsqlTypes;

namespace Greatbone.Core
{
    /// 
    /// To generate a urlencoded byte or char string.
    /// 
    public class FormContent : DynamicContent, ISink<FormContent>
    {
        public FormContent(bool sendable, bool pooled, int capacity = 4092) : base(sendable, pooled, capacity)
        {
        }

        public override string MimeType => "application/x-www-form-urlencoded";

        void AddEsc(string v)
        {
            if (v != null)
            {
                for (int i = 0; i < v.Length; i++)
                {
                    char c = v[i];
                    if (c == '\"')
                    {
                        Add('\\');
                        Add('"');
                    }
                    else if (c == '\\')
                    {
                        Add('\\');
                        Add('\\');
                    }
                    else if (c == '\n')
                    {
                        Add('\\');
                        Add('n');
                    }
                    else if (c == '\r')
                    {
                        Add('\\');
                        Add('r');
                    }
                    else if (c == '\t')
                    {
                        Add('\\');
                        Add('t');
                    }
                    else
                    {
                        Add(c);
                    }
                }
            }
        }

        //
        // SINK
        //

        public FormContent PutNull(string name)
        {
            if (name != null)
            {
                Add('"');
                Add(name);
                Add('"');
                Add(':');
            }

            Add("null");

            return this;
        }

        public FormContent Put(string name, bool v)
        {
            if (name != null)
            {
                Add('"');
                Add(name);
                Add('"');
                Add(':');
            }
            Add(v ? "true" : "false");
            return this;
        }

        public FormContent Put(string name, short v)
        {
            if (name != null)
            {
                Add('"');
                Add(name);
                Add('"');
                Add(':');
            }
            Add(v);
            return this;
        }

        public FormContent Put(string name, int v)
        {
            if (name != null)
            {
                Add('"');
                Add(name);
                Add('"');
                Add(':');
            }
            Add(v);
            return this;
        }

        public FormContent Put(string name, long v)
        {
            if (name != null)
            {
                Add('"');
                Add(name);
                Add('"');
                Add(':');
            }
            Add(v);
            return this;
        }

        public FormContent Put(string name, double v)
        {
            if (name != null)
            {
                Add('"');
                Add(name);
                Add('"');
                Add(':');
            }
            Add(v);
            return this;
        }

        public FormContent Put(string name, decimal v)
        {
            if (name != null)
            {
                Add('"');
                Add(name);
                Add('"');
                Add(':');
            }
            Add(v);
            return this;
        }

        public FormContent Put(string name, JNum v)
        {
            if (name != null)
            {
                Add('"');
                Add(name);
                Add('"');
                Add(':');
            }

            Add(v.bigint);
            if (v.Pt)
            {
                Add('.');
                Add(v.fract);
            }
            return this;
        }

        public FormContent Put(string name, DateTime v)
        {
            if (name != null)
            {
                Add('"');
                Add(name);
                Add('"');
                Add(':');
            }

            Add('"');
            Add(v);
            Add('"');

            return this;
        }

        public FormContent Put(string name, NpgsqlPoint v)
        {
            if (name != null)
            {
                Add('"');
                Add(name);
                Add('"');
                Add(':');
            }

            Add('"');
            Add(v.X);
            Add(':');
            Add(v.Y);
            Add('"');

            return this;
        }

        public FormContent Put(string name, char[] v)
        {
            if (name != null)
            {
                Add('"');
                Add(name);
                Add('"');
                Add(':');
            }

            if (v == null)
            {
                Add("null");
            }
            else
            {
                Add('"');
                Add(v);
                Add('"');
            }

            return this;
        }

        public FormContent Put(string name, string v, bool? anylen = null)
        {
            if (name != null)
            {
                Add('"');
                Add(name);
                Add('"');
                Add(':');
            }

            if (v == null)
            {
                Add("null");
            }
            else
            {
                Add('"');
                AddEsc(v);
                Add('"');
            }

            return this;
        }

        public virtual FormContent Put(string name, byte[] v)
        {
            return this; // ignore ir
        }

        public virtual FormContent Put(string name, ArraySegment<byte> v)
        {
            return this; // ignore ir
        }

        public FormContent Put<B>(string name, B v, byte bits = 0) where B : IData
        {
            if (name != null)
            {
                Add('"');
                Add(name);
                Add('"');
                Add(':');
            }

            if (v == null)
            {
                Add("null");
            }
            else
            {
                Add('{');
                v.Dump(this, bits);
                Add('}');
            }

            return this;
        }

        public FormContent Put(string name, JObj v)
        {
            if (name != null)
            {
                Add('"');
                Add(name);
                Add('"');
                Add(':');
            }

            if (v == null)
            {
                Add("null");
            }
            else
            {
                Add('{');
                v.Dump(this);
                Add('}');
            }

            return this;
        }

        public FormContent Put(string name, JArr v)
        {
            if (name != null)
            {
                Add('"');
                Add(name);
                Add('"');
                Add(':');
            }

            if (v == null)
            {
                Add("null");
            }
            else
            {
                Add('[');
                v.Dump(this);
                Add(']');
            }
            return this;
        }

        public FormContent Put(string name, short[] v)
        {
            if (name != null)
            {
                Add('"');
                Add(name);
                Add('"');
                Add(':');
            }

            if (v == null)
            {
                Add("null");
            }
            else
            {
                Add('[');
                for (int i = 0; i < v.Length; i++)
                {
                    if (i > 0) Add(',');
                    Add(v[i]);
                }
                Add(']');
            }

            return this;
        }

        public FormContent Put(string name, int[] v)
        {
            if (name != null)
            {
                Add('"');
                Add(name);
                Add('"');
                Add(':');
            }

            if (v == null)
            {
                Add("null");
            }
            else
            {
                Add('[');
                for (int i = 0; i < v.Length; i++)
                {
                    if (i > 0) Add(',');
                    Add(v[i]);
                }
                Add(']');
            }

            return this;
        }

        public FormContent Put(string name, long[] v)
        {
            if (name != null)
            {
                Add('"');
                Add(name);
                Add('"');
                Add(':');
            }

            if (v == null)
            {
                Add("null");
            }
            else
            {
                Add('[');
                for (int i = 0; i < v.Length; i++)
                {
                    if (i > 0) Add(',');
                    Add(v[i]);
                }
                Add(']');
            }

            return this;
        }

        public FormContent Put(string name, string[] v)
        {
            if (name != null)
            {
                Add('"');
                Add(name);
                Add('"');
                Add(':');
            }

            if (v == null)
            {
                Add("null");
            }
            else
            {
                Add('[');
                for (int i = 0; i < v.Length; i++)
                {
                    if (i > 0) Add(',');
                    string str = v[i];
                    if (str == null)
                    {
                        Add("null");
                    }
                    else
                    {
                        Add('"');
                        AddEsc(str);
                        Add('"');
                    }
                }
                Add(']');
            }

            return this;
        }


        public FormContent Put<B>(string name, B[] v, byte bits = 0) where B : IData
        {
            if (name != null)
            {
                Add('"');
                Add(name);
                Add('"');
                Add(':');
            }

            if (v == null)
            {
                Add("null");
            }
            else
            {
                Add('[');
                for (int i = 0; i < v.Length; i++)
                {
                    Put(null, v[i], bits);
                }
                Add(']');
            }
            return this;
        }
    }
}