﻿using System.IO;

namespace Greatbone.Core
{
    ///
    /// The context for a particular node in the web folder hierarchy.
    ///
    public class FolderContext
    {
        readonly string name;

        public FolderContext(string name)
        {
            this.name = name;
        }

        public string Name => name;

        public CheckAttribute[] Checks { get; internal set; }

        public UiAttribute Ui { get; internal set; }

        public bool IsVar { get; internal set; }

        public int Level { get; internal set; }

        public Folder Parent { get; internal set; }

        public virtual string Directory { get; internal set; }

        public Service Service { get; internal set; }

        public JObj Configuration { get; internal set; }

        public string GetFilePath(string file)
        {
            return Path.Combine(Directory, file);
        }

    }
}