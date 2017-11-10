using SciAdvNet.SC3Script.Text;
using System;
using System.Collections.Immutable;

namespace SciAdvNet.SC3Script
{
    public abstract class MetadataEntity : IEquatable<MetadataEntity>
    {
        private readonly Lazy<ImmutableArray<byte>> _data;

        protected MetadataEntity(SC3Script script, int id, int offset, int length)
        {
            Script = script;
            Id = id;
            Offset = offset;
            Length = length;

            _data = new Lazy<ImmutableArray<byte>>(ReadData);
        }

        public SC3Script Script { get; }
        public int Id { get; }
        public int Offset { get; }
        public int EndOffset => Offset + Length;
        public int Length { get; }

        public ImmutableArray<byte> RawData => _data.Value;

        public bool Equals(MetadataEntity other) => Id == other.Id;
        public override bool Equals(object obj) => obj is MetadataEntity && Equals((MetadataEntity)obj);
        public override int GetHashCode() => Id.GetHashCode();

        public static bool operator ==(MetadataEntity left, MetadataEntity right) => left.Equals(right);
        public static bool operator !=(MetadataEntity left, MetadataEntity right) => !left.Equals(right);


        private ImmutableArray<byte> ReadData()
        {
            Script.Stream.Position = Offset;
            return Script.Reader.ReadBytes(Length).ToImmutableArray();
        }
    }

    public class BlockDefinition : MetadataEntity
    {
        internal BlockDefinition(SC3Script script, int id, int offset, int length)
            : base(script, id, offset, length)
        {
        }

        public CodeBlockDefinition AsCode() => new CodeBlockDefinition(this);
        public DataBlockDefinition AsData(ArrayDataType elementDataType) => new DataBlockDefinition(this, elementDataType);

        public override string ToString() => $"Label #{Id} (Offset = {Offset}, Length = {Length})";
    }

    public sealed class CodeBlockDefinition : MetadataEntity
    {
        internal CodeBlockDefinition(BlockDefinition blockDef)
            : base(blockDef.Script, blockDef.Id, blockDef.Offset, blockDef.Length)
        {
        }
    }

    public sealed class DataBlockDefinition : MetadataEntity
    {
        internal DataBlockDefinition(BlockDefinition blockDef, ArrayDataType dataType)
            : base(blockDef.Script, blockDef.Id, blockDef.Offset, blockDef.Length)
        {
            DataType = dataType;
        }

        public ArrayDataType DataType { get; }
    }

    public sealed class StringHandle : MetadataEntity
    {
        internal StringHandle(SC3Script script, int id, int offset, int length)
            : base(script, id, offset, length)
        {
        }

        public SC3String Resolve() => Script.GetString(Id);
    }


    public enum ArrayDataType
    {
        Int16,
        Int32,
        StringRef
    }
}
