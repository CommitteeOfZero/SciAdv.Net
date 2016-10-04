namespace SciAdvNet.SC3.Text
{
    internal static class SC3StringMarker
    {
        public const byte LineBreak = 0x00;
        public const byte CharacterName = 0x01;
        public const byte DialogueLine = 0x02;
        public const byte Present = 0x03;
        public const byte SetColor = 0x04;
        public const byte Present_ResetAlignment = 0x08;
        public const byte RubyBase = 0x09;
        public const byte RubyTextStart = 0x0A;
        public const byte RubyTextEnd = 0x0B;
        public const byte SetFontSize = 0x0C;
        public const byte SetAlignment_Center = 0x0F;
        public const byte SetTopMargin = 0x11;
        public const byte SetLeftMargin = 0x12;
        public const byte EvaluateExpression = 0x15;

        public const byte StringTerminator = 0xFF;
    }
}
