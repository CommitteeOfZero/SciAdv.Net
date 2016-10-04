namespace SciAdvNet.SC3
{
    public interface IVisitable
    {
        void Accept(CodeVisitor visitor);
    }
}
