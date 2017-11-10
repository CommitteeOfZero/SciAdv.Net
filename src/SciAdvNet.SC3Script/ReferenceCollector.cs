using System.Collections.Immutable;

namespace SciAdvNet.SC3Script
{
    // Not implemented lol
    public sealed class ReferenceCollector : CodeWalker
    {
        private readonly SC3Script _script;

        public ReferenceCollector(SC3Script script)
        {
            _script = script;

            _stringReferences = ImmutableArray.CreateBuilder<StringReference>();
            _codeBlockReferences = ImmutableArray.CreateBuilder<CodeBlockReference>();
            _externalCodeBlockReferences = ImmutableArray.CreateBuilder<ExternalCodeBlockReference>();
            _dataBlockReferences = ImmutableArray.CreateBuilder<DataBlockReference>();
        }

        private readonly ImmutableArray<StringReference>.Builder _stringReferences;
        private readonly ImmutableArray<CodeBlockReference>.Builder _codeBlockReferences;
        private readonly ImmutableArray<ExternalCodeBlockReference>.Builder _externalCodeBlockReferences;
        private readonly ImmutableArray<DataBlockReference>.Builder _dataBlockReferences;

        public ImmutableArray<StringReference> StringReferences => _stringReferences.ToImmutable();
        public ImmutableArray<CodeBlockReference> CodeBlockReferences => _codeBlockReferences.ToImmutable();
        public ImmutableArray<ExternalCodeBlockReference> ExternalCodeBlockReferences => _externalCodeBlockReferences.ToImmutable();
        public ImmutableArray<DataBlockReference> DataBlockReferences => _dataBlockReferences.ToImmutable();


        public override void VisitDataBlockReferenceExpression(DataBlockReferenceExpression expression)
        {
            var blockRef = new DataBlockReference(_script, expression.BlockId);
            _dataBlockReferences.Add(blockRef);

            base.VisitDataBlockReferenceExpression(expression);
        }
    }
}
