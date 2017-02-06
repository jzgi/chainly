namespace Greatbone.Core
{
    ///
    /// An XML parsing structure that deals with well-formed XML documents.
    ///
    public struct XmlParse
    {
        static readonly ParseException ParseEx = new ParseException("error parsing xml");

        // byte content to parse
        readonly byte[] bytebuf;

        // char content to parse
        readonly string strbuf;

        readonly int offset;

        readonly int length;

        // UTF-8 string builder
        readonly Text str;

        public XmlParse(byte[] bytebuf, int offset, int length)
        {
            this.bytebuf = bytebuf;
            this.strbuf = null;
            this.offset = offset;
            this.length = length;
            this.str = new Text(1024);
        }

        public XmlParse(string strbuf)
        {
            this.bytebuf = null;
            this.strbuf = strbuf;
            this.offset = 0;
            this.length = strbuf.Length;
            this.str = new Text(1024);
        }

        int this[int index] => bytebuf?[index] ?? (int) strbuf[index];

        public XElem Parse()
        {
            int p = offset;

            // seek to a less-than (<)
            int b;
            while (IsWs(b = this[p]))
            {
                p++;
            } // skip ws
            if (b != '<') throw ParseEx;

            // the first char
            b = this[++p];

            if (b == '?') // skip the prolog line
            {
                while (this[++p] != '>')
                {
                }

                // seek to a <
                for (;;)
                {
                    b = this[++p];
                    if (IsWs(b)) continue; // skip ws
                    if (b == '<') break;
                    throw ParseEx;
                }
            }

            if (IsNameStartChar(b))
            {
                return ParseElem(ref p, b);
            }
            throw ParseEx;
        }

        static bool IsWs(int c)
        {
            return c == ' ' || c == '\t' || c == '\n' || c == '\r';
        }

        static bool IsNameStartChar(int c)
        {
            return c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z' || c == '_';
        }

        static bool IsNameChar(int c)
        {
            return c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z' || c == '_' || c == '-' || c >= '0' && c <= '9';
        }

        XElem ParseElem(ref int pos, int startchar)
        {
            int p = pos;
            int b;

            // parse element tag name
            str.Clear();
            str.Accept(startchar);
            while (IsNameChar(b = this[++p]))
            {
                str.Accept(b); // to comprise start tag
            }
            string tag = str.ToString();
            XElem elem = new XElem(tag);

            // optionally parse attributes
            while (IsWs(b))
            {
                while (IsWs(b = this[++p]))
                {
                } // skip ws

                if (IsNameStartChar(b))
                {
                    // attribute name
                    str.Clear();
                    str.Accept(b);
                    while ((b = this[++p]) != '=')
                    {
                        str.Accept(b);
                    }
                    string name = str.ToString();

                    // attribute value
                    if (this[++p] != '"') throw ParseEx; // left quote
                    str.Clear();
                    while ((b = this[++p]) != '"') // till right quote
                    {
                        str.Accept(b);
                    }
                    string value = str.ToString();

                    elem.AddAttr(name, value);

                    b = this[++p]; // step
                }
            } // end of attributes

            if (b == '>') // a start tag just finished, expecting the ending-tag
            {
                for (;;) // child nodes iteration
                {
                    while (IsWs(b = this[++p]))
                    {
                    } // skip ws

                    if (b == '<')
                    {
                        b = this[++p];
                        if (b == '/') // the ending tag
                        {
                            // consume
                            str.Clear();
                            while ((b = this[++p]) != '>')
                            {
                                str.Accept(b);
                            }
                            if (!str.Equals(tag)) throw ParseEx;

                            pos = p; // adjust current position
                            return elem;
                        }

                        if (IsNameStartChar(b))
                        {
                            elem.AddSub(ParseElem(ref p, b));
                        }
                    }
                    else // text node
                    {
                        str.Clear();
                        str.Accept(b);
                        while ((b = this[++p]) != '<')
                        {
                            str.Accept(b);
                        }
                        if (str.Count > 0)
                        {
                            elem.Text = str.ToString();
                        }
                        // decrease in position to behave as other child nodes
                        --p;
                    }
                } // child nodes iteration
            }
            if (b == '/' && this[++p] == '>') // empty-element
            {
                pos = p; // adjust current position
                return elem;
            }
            throw ParseEx;
        }
    }
}