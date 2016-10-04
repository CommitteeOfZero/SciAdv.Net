using System.Collections.Immutable;

namespace SciAdvNet.SC3
{
    public class CodeBlock : IVisitable
    {
        internal CodeBlock(CodeBlockDefinition blockDefinition)
        {
            Definition = blockDefinition;
        }

        public CodeBlockDefinition Definition { get; }
        public ImmutableArray<Instruction> Instructions { get; internal set; }
        public ImmutableArray<byte> UnrecognizedBytes { get; internal set; }

        public ImmutableArray<StringReference> StringReferences { get; internal set; }
        public ImmutableArray<CodeBlockReference> CodeBlockReferences { get; internal set; }
        public ImmutableArray<ExternalCodeBlockReference> ExternalCodeBlockReferences { get; internal set; }
        public ImmutableArray<DataBlockReference> DataBlockReferences { get; internal set; }

        public void Accept(CodeVisitor visitor)
        {
            visitor.VisitCodeBlock(this);
        }
    }
}
