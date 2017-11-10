using System.Collections.Immutable;

namespace SciAdvNet.SC3Script
{
    public sealed class DisassemblyResult : IVisitable
    {
        internal DisassemblyResult(CodeBlockDefinition codeBlock)
        {
            CodeBlock = codeBlock;
        }

        public CodeBlockDefinition CodeBlock { get; }
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
