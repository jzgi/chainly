﻿namespace Greatbone.Core
{
    ///
    /// DB configuration embedded in WebConfig.
    ///
    public class DbConfig : IData
    {
        public string host;

        public int port;

        public string username;

        public string password;

        // whether to create message tables
        public bool queue;

        public void Load(ISource s, byte bits = 0)
        {
            s.Get(nameof(host), ref host);
            s.Get(nameof(port), ref port);
            s.Get(nameof(username), ref username);
            s.Get(nameof(password), ref password);
            s.Get(nameof(queue), ref queue);
        }

        public void Dump<R>(ISink<R> s, byte bits = 0) where R : ISink<R>
        {
            s.Put(nameof(host), host);
            s.Put(nameof(port), port);
            s.Put(nameof(username), username);
            s.Put(nameof(password), password);
            s.Put(nameof(queue), queue);
        }
    }
}