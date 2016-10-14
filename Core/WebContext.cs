﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;

namespace Greatbone.Core
{
    /// <summary>
    /// The encapsulation of a web request/response exchange context.
    /// </summary>
    public class WebContext : DefaultHttpContext, ISource, IDisposable
    {

        internal WebContext(IFeatureCollection features) : base(features)
        {
        }

        public WebSub Controller { get; }

        public WebAction Action { get; }

        public string Var { get; internal set; }

        public IToken Token { get; internal set; }


        //
        // REQUEST
        //

        // received body bytes
        ArraySegment<byte> bytesSeg;

        // parsed request entity, can be Doc or Form
        object entity;

        async void ReceiveAsync()
        {
            if (bytesSeg != null)
            {
                return;
            }
            HttpRequest req = Request;
            long? clen = req.ContentLength;
            if (clen > 0)
            {
                int len = (int)clen.Value;
                byte[] buffer = BufferPool.Borrow(len);
                int count = await req.Body.ReadAsync(buffer, 0, len);
                bytesSeg = new ArraySegment<byte>(buffer, 0, count);
            }
        }

        public ArraySegment<byte> BytesSeg
        {
            get
            {
                if (bytesSeg.Array == null) { ReceiveAsync(); }
                return bytesSeg;
            }
        }

        void ParseEntity()
        {
            if (entity != null) return;

            ArraySegment<byte> bseg = BytesSeg;
            if (bseg.Array != null)
            {
                string ctyp = Request.ContentType;
                if ("application/x-www-form-urlencoded".Equals(ctyp))
                {
                    FormParse parse = new FormParse(bseg);
                    entity = parse.Parse();
                }
                else
                {
                    JParse parse = new JParse(bseg);
                    entity = parse.Parse();
                }
            }
        }

        public Form Form
        {
            get
            {
                ParseEntity();
                return entity as Form;
            }
        }

        public JObj JObj
        {
            get
            {
                ParseEntity();
                return entity as JObj;
            }
        }

        public JArr JArr
        {
            get
            {
                ParseEntity();
                return entity as JArr;
            }
        }

        public T Obj<T>(ushort x = 0) where T : IPersist, new()
        {
            JObj jo = JObj;
            T obj = new T();
            obj.Load(jo, x);
            return obj;
        }

        public T[] Arr<T>(ushort x = 0) where T : IPersist, new()
        {
            JArr ja = JArr;
            T[] arr = new T[ja.Count];
            for (int i = 0; i < arr.Length; i++)
            {
                T obj = new T();
                obj.Load((JObj)ja[i], x);
                arr[i] = obj;
            }
            return arr;
        }


        //
        // SOURCE FOR QUERY STRING
        //

        public bool Got(string name, ref bool v)
        {
            StringValues values;
            if (Request.Query.TryGetValue(name, out values))
            {
                string str = values[0];
                if ("true".Equals(str) || "1".Equals(str))
                {
                    v = true;
                    return true;
                }
                if ("false".Equals(str) || "0".Equals(str))
                {
                    v = false;
                    return true;
                }
            }
            return false;
        }

        public bool Got(string name, ref short v)
        {
            StringValues values;
            if (Request.Query.TryGetValue(name, out values))
            {
                string str = values[0];
                short num;
                if (short.TryParse(str, out num))
                {
                    v = num;
                    return true;
                }
            }
            return false;
        }

        public bool Got(string name, ref int v)
        {
            StringValues values;
            if (Request.Query.TryGetValue(name, out values))
            {
                string str = values[0];
                int num;
                if (int.TryParse(str, out num))
                {
                    v = num;
                    return true;
                }
            }
            return false;
        }

        public bool Got(string name, ref long v)
        {
            StringValues values;
            if (Request.Query.TryGetValue(name, out values))
            {
                string str = values[0];
                long num;
                if (long.TryParse(str, out num))
                {
                    v = num;
                    return true;
                }
            }
            return false;
        }

        public bool Got(string name, ref decimal v)
        {
            StringValues values;
            if (Request.Query.TryGetValue(name, out values))
            {
                string str = values[0];
                decimal num;
                if (decimal.TryParse(str, out num))
                {
                    v = num;
                    return true;
                }
            }
            return false;
        }

        public bool Got(string name, ref Number v)
        {
            throw new NotImplementedException();
        }

        public bool Got(string name, ref DateTime v)
        {
            throw new NotImplementedException();
        }

        public bool Got(string name, ref char[] v)
        {
            throw new NotImplementedException();
        }

        public bool Got(string name, ref string v)
        {
            StringValues values;
            if (Request.Query.TryGetValue(name, out values))
            {
                v = values[0];
                return true;
            }
            return false;
        }

        public bool Got(string name, ref byte[] v)
        {
            throw new NotImplementedException();
        }

        public bool Got(string name, ref ArraySegment<byte> v)
        {
            throw new NotImplementedException();
        }

        public bool Got<T>(string name, ref T v, ushort x = ushort.MaxValue) where T : IPersist, new()
        {
            StringValues values;
            if (Request.Query.TryGetValue(name, out values))
            {
                string str = values[0];
                JTextParse parse = new JTextParse(str);
                JObj jo = (JObj)parse.Parse();
                v = new T();
                v.Load(jo, x);
                return true;
            }
            return false;
        }

        public bool Got(string name, ref JObj v)
        {
            StringValues values;
            if (Request.Query.TryGetValue(name, out values))
            {
                string str = values[0];
                JTextParse parse = new JTextParse(str);
                v = (JObj)parse.Parse();
                return true;
            }
            return false;
        }

        public bool Got(string name, ref JArr v)
        {
            StringValues values;
            if (Request.Query.TryGetValue(name, out values))
            {
                string str = values[0];
                JTextParse parse = new JTextParse(str);
                v = (JArr)parse.Parse();
                return true;
            }
            return false;
        }

        public bool Got(string name, ref short[] v)
        {
            StringValues values;
            if (Request.Query.TryGetValue(name, out values))
            {
                int len = values.Count;
                v = new short[len];
                for (int i = 0; i < len; i++)
                {
                    string str = values[i];
                    short e = 0;
                    if (short.TryParse(str, out e))
                    {
                        v[i] = e;
                    }
                }
                return true;
            }
            return false;
        }

        public bool Got(string name, ref int[] v)
        {
            StringValues values;
            if (Request.Query.TryGetValue(name, out values))
            {
                int len = values.Count;
                v = new int[len];
                for (int i = 0; i < len; i++)
                {
                    string str = values[i];
                    int e = 0;
                    if (int.TryParse(str, out e))
                    {
                        v[i] = e;
                    }
                }
                return true;
            }
            return false;
        }

        public bool Got(string name, ref long[] v)
        {
            StringValues values;
            if (Request.Query.TryGetValue(name, out values))
            {
                int len = values.Count;
                v = new long[len];
                for (int i = 0; i < len; i++)
                {
                    string str = values[i];
                    long e = 0;
                    if (long.TryParse(str, out e))
                    {
                        v[i] = e;
                    }
                }
                return true;
            }
            return false;
        }

        public bool Got(string name, ref string[] v)
        {
            StringValues values;
            if (Request.Query.TryGetValue(name, out values))
            {
                int len = values.Count;
                v = new string[len];
                for (int i = 0; i < len; i++)
                {
                    v[i] = values[i];
                }
                return true;
            }
            return false;
        }

        public bool Got<T>(string name, ref T[] v, ushort x = ushort.MaxValue) where T : IPersist, new()
        {
            StringValues values;
            if (Request.Query.TryGetValue(name, out values))
            {
                string str = values[0];
                JTextParse parse = new JTextParse(str);
                JArr ja = (JArr)parse.Parse();
                int len = ja.Count;
                v = new T[len];
                for (int i = 0; i < len; i++)
                {
                    JObj jo = (JObj)ja[i];
                    T obj = new T();
                    obj.Load(jo, x);
                    v[i] = obj;
                }
                return true;
            }
            return false;
        }


        public void SetLocation(string v)
        {
            Response.Headers.Add("Location", new StringValues(v));
        }

        //
        // RESPONSE
        //

        public int StatusCode
        {
            get { return Response.StatusCode; }
            set { Response.StatusCode = value; }
        }

        public IContent Content { get; set; }

        internal bool? Pub { get; set; }

        internal int MaxAge { get; set; }

        public void Respond<T>(int status, T obj, ushort x = 0, bool? pub = false, int maxage = 0) where T : IPersist
        {
            Respond(status, jcont => jcont.PutObj(obj, x), pub, maxage);
        }

        public void Respond<T>(int status, T[] arr, ushort x = 0, bool? pub = false, int maxage = 0) where T : IPersist
        {
            Respond(status, jcont => jcont.PutArr(arr, x), pub, maxage);
        }

        public void Respond(int status, Action<JContent> a, bool? pub = false, int maxage = 0)
        {
            JContent jcont = new JContent(8 * 1024);
            a?.Invoke(jcont);

            Respond(status, jcont, pub, maxage);
        }

        public void Respond(int status, Action<HtmlContent> a, bool? pub = true, int maxage = 1000)
        {
            StatusCode = status;

            this.Pub = pub;
            this.MaxAge = maxage;

            HtmlContent html = new HtmlContent(16 * 1024);
            a?.Invoke(html);
            Content = html;
        }

        public void Respond(int status, IContent cont, bool? pub = null, int maxage = 0)
        {
            StatusCode = status;
            if (cont != null)
            {
                Content = cont;
                Pub = pub;
                MaxAge = maxage;
            }
        }

        internal Task WriteContentAsync()
        {
            if (Content != null)
            {
                HttpResponse rsp = Response;
                rsp.ContentLength = Content.Length;
                rsp.ContentType = Content.Type;

                // etag

                //
                return rsp.Body.WriteAsync(Content.Buffer, 0, Content.Length);
            }
            return Task.CompletedTask;
        }


        //
        // RPC
        //

        public void Get(string service, string part)
        {
            // token impersonate
            WebClient cli = Controller.Service.FindClient(service, part);
            if (cli != null)
            {

            }
        }

        public void Post(string service, string part, object content)
        {
            // token impersonate
            WebClient cli = Controller.Service.FindClient(service, part);
            if (cli != null)
            {

            }
        }



        public void Dispose()
        {
            // return request content buffer
            byte[] buf = bytesSeg.Array;
            if (buf != null)
            {
                BufferPool.Return(buf);
            }

            // return response content buffer
            if (Content is DynamicContent)
            {
                BufferPool.Return(Content.Buffer);
            }
        }

    }
}