namespace SciAdvNet.SC3
{
    public abstract class Reference
    {
        internal Reference(SC3Module module)
        {
            Module = module;
        }

        public SC3Module Module { get; }
    }

    public abstract class LabelReference : Reference
    {
        internal LabelReference(SC3Module module)
            : base(module)
        {
        }
    }

    public sealed class CodeBlockReference : LabelReference
    {
        internal CodeBlockReference(SC3Module module, int blockId)
            : base(module)
        {
            BlockId = blockId;
        }

        public int BlockId { get; }

        public CodeBlockDefinition Resove() => Module.Blocks[BlockId].AsCode();
    }

    public sealed class ExternalCodeBlockReference : LabelReference
    {
        internal ExternalCodeBlockReference(SC3Module module, int blockId)
            : base(module)
        {
            BlockId = blockId;
        }

        public int BlockId { get; }

        public CodeBlockDefinition Resove(SC3Module externalModule) => externalModule.Blocks[BlockId].AsCode();
    }

    public class DataBlockReference : LabelReference
    {
        internal DataBlockReference(SC3Module module, Expression blockId)
            : base(module)
        {
        }
    }

    public sealed class StringReference : Reference
    {
        internal StringReference(SC3Module module, int stringId)
            : base(module)
        {
            StringId = stringId;
        }

        public int StringId { get; }
        //public StringHandle Resolve() => Module.StringMap[StringId];

    }
}
