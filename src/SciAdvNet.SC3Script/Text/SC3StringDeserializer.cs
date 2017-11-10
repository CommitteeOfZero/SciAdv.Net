namespace SciAdvNet.SC3Script.Text
{
    /// <summary>
    /// Represents a deserializer that constructs an instance of <see cref="SC3String"/> from its serialized text form.
    /// </summary>
    public abstract class SC3StringDeserializer
    {
        public abstract SC3String Deserialize(string text);
    }
}
