using System;

namespace SciAdvNet.CriwareVfs
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    internal sealed class CpkTableFieldAttribute : Attribute
    {
        public CpkTableFieldAttribute(string cpkTableFieldName)
        {
            CpkTableFieldName = cpkTableFieldName;
        }

        public string CpkTableFieldName { get; }
    }
}
