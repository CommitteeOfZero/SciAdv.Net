namespace SciAdvNet.SC3Script
{
    public interface IVisitable
    {
        void Accept(CodeVisitor visitor);
    }
}
