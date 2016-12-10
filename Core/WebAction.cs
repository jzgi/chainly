﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace Greatbone.Core
{
    ///
    /// The descriptor for an action method.
    ///
    public class WebAction : IRollable
    {
        readonly WebDirectory directory;

        readonly string name;

        readonly Action<WebActionContext> doer;

        readonly CheckAttribute[] checks;

        // if requires auth through header or cookie
        readonly bool header, cookie;

        readonly UiAttribute ui;

        internal WebAction(WebDirectory dir, MethodInfo mi)
        {
            directory = dir;
            name = mi.Name;
            doer = (Action<WebActionContext>)mi.CreateDelegate(typeof(Action<WebActionContext>), dir);

            // prepare checks
            List<CheckAttribute> lst = null;
            foreach (var chk in mi.GetCustomAttributes<CheckAttribute>())
            {
                if (lst == null)
                {
                    lst = new List<CheckAttribute>(8);
                }
                lst.Add(chk);

                if (chk.IsCookie) header = true;
                else cookie = true;
            }
            checks = lst?.ToArray();

            ui = mi.GetCustomAttribute<UiAttribute>();
        }

        public WebDirectory Directory => directory;

        public string Name => name;

        public bool IsGet => ui?.IsGet ?? false;

        public string Icon => ui?.Icon;

        public int Dialog => ui?.Dialog ?? 3;

        // for generating unique digest nonce
        const string PrivateKey = "3e43a7180";

        internal bool Check(WebActionContext ac)
        {
            // access check 
            if (checks != null)
            {
                if (header && ac.Principal == null)
                {
                    ac.StatusCode = 401; // unauthorized
                    ac.SetHeader("WWW-Authenticate", "Bearer");
                    return false;
                }
                else if (cookie && ac.Principal == null)
                {
                    string loc = directory.Service.SignOn + "?orig=" + ac.Uri;
                    ac.SetHeader("Location", loc);
                    ac.StatusCode = 303; // see other - redirect to signon url
                    return false;
                }

                for (int i = 0; i < checks.Length; i++)
                {
                    if (!checks[i].Test(ac))
                    {
                        ac.StatusCode = 403; // forbidden
                        return false;
                    }
                }
            }
            return true;
        }

        internal void Do(WebActionContext ac)
        {
            doer(ac);
        }

        public override string ToString()
        {
            return name;
        }
    }
}