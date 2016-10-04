using System.Collections.Generic;
using System.Collections.Immutable;

namespace SciAdvNet.SC3
{
    public sealed class ReferenceCollector : CodeWalker
    {
        private readonly SC3Module _module;

        public ReferenceCollector(SC3Module module)
        {
            _module = module;
        }

        private readonly List<StringReference> _strings = new List<StringReference>();
        private readonly List<CodeBlockReference> _codeBlocks = new List<CodeBlockReference>();
        private readonly List<ExternalCodeBlockReference> _externalCodeBlocks = new List<ExternalCodeBlockReference>();
        private readonly List<DataBlockReference> _dataBlocks = new List<DataBlockReference>();

        public ImmutableArray<StringReference> Strings => _strings.ToImmutableArray();
        public ImmutableArray<CodeBlockReference> CodeBlocks => _codeBlocks.ToImmutableArray();
        public ImmutableArray<ExternalCodeBlockReference> ExternalCodeBlocks => _externalCodeBlocks.ToImmutableArray();
        public ImmutableArray<DataBlockReference> DataBlocks => _dataBlocks.ToImmutableArray();


        public override void VisitDataBlockReferenceExpression(DataBlockReferenceExpression expression)
        {
            var blockRef = new DataBlockReference(_module, expression.BlockId);
            _dataBlocks.Add(blockRef);

            base.VisitDataBlockReferenceExpression(expression);
        }
    }
}
