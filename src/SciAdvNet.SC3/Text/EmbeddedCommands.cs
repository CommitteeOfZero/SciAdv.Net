namespace SciAdvNet.SC3.Text
{
    internal static class EmbeddedCommands
    {
        public const byte LineBreak = 0x00;
        public const byte CharacterNameStart = 0x01;
        public const byte DialogueLineStart = 0x02;
        public const byte Present = 0x03;
        public const byte SetColor = 0x04;
        public const byte Present_ResetAlignment = 0x08;
        public const byte RubyBaseStart = 0x09;
        public const byte RubyTextStart = 0x0A;
        public const byte RubyTextEnd = 0x0B;
        public const byte SetFontSize = 0x0C;
        public const byte PrintInParallel = 0x0E;
        public const byte CenterText = 0x0F;
        public const byte SetTopMargin = 0x11;
        public const byte SetLeftMargin = 0x12;
        public const byte GetHardcodedValue = 0x13;
        public const byte EvaluateExpression = 0x15;
        public const byte Present_0x18 = 0x18;
        public const byte AutoForward = 0x19;

        public const byte StringTerminator = 0xFF;
    }
}
