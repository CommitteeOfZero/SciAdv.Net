using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciAdvNet.Engine
{
    public abstract class Asset
    {
        protected Asset(string fileName)
        {
            FileName = fileName;
        }

        public string FileName { get; }
    }
}
