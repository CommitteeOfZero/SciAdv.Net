using Caliburn.Micro;
using ProjectAmadeus.Models;
using SciAdvNet.SC3;
using SciAdvNet.SC3.Text;
using System.Collections.Generic;
using System.IO;
using System;

namespace ProjectAmadeus.ViewModels
{
    public sealed class TabViewModel : Screen, ITab
    {
        public TabViewModel()
        {
        }

        public String FilePath
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public SC3Module Module
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        protected override void OnDeactivate(bool close)
        {
            if (close)
            {
                Module.Dispose();
            }
        }
    }
}
