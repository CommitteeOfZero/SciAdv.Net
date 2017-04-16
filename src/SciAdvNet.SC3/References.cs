namespace SciAdvNet.SC3
{
    public abstract class Reference
    {
        internal Reference(SC3Script script)
        {
            Script = script;
        }

        public SC3Script Script { get; }
    }

    public abstract class LabelReference : Reference
    {
        internal LabelReference(SC3Script script)
            : base(script)
        {
        }
    }

    public sealed class CodeBlockReference : LabelReference
    {
        internal CodeBlockReference(SC3Script script, int blockId)
            : base(script)
        {
            BlockId = blockId;
        }

        public int BlockId { get; }

        public CodeBlockDefinition Resove() => Script.Blocks[BlockId].AsCode();
    }

    public sealed class ExternalCodeBlockReference : LabelReference
    {
        internal ExternalCodeBlockReference(SC3Script script, int blockId)
            : base(script)
        {
            BlockId = blockId;
        }

        public int BlockId { get; }

        public CodeBlockDefinition Resove(SC3Script script) => script.Blocks[BlockId].AsCode();
    }

    public class DataBlockReference : LabelReference
    {
        internal DataBlockReference(SC3Script script, Expression blockId)
            : base(script)
        {
        }
    }

    public sealed class StringReference : Reference
    {
        internal StringReference(SC3Script script, int stringId)
            : base(script)
        {
            StringId = stringId;
        }

        public int StringId { get; }
        //public StringHandle Resolve() => Module.StringMap[StringId];

    }
}
