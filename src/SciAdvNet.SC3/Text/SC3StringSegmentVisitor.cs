using System;

namespace SciAdvNet.SC3.Text
{
    public abstract class SC3StringSegmentVisitor
    {
        public void Visit(SC3StringSegment segment) => segment.Accept(this);
        public virtual void VisitTextSegment(TextSegment text) { }

        public virtual void VisitLineBreakCommand(LineBreakCommand lineBreakCommand) { }
        public virtual void VisitCharacterNameStartCommand(CharacterNameStartCommand characterNameStartCommand) { }
        public virtual void VisitDialogueLineStartCommand(DialogueLineStartCommand dialogueLineStartCommand) { }
        public virtual void VisitSetColorCommand(SetColorCommand setColorCommand) { }
        public virtual void VisitPresentCommand(PresentCommand presentCommand) { }
        public virtual void VisitRubyBaseStartCommand(RubyBaseStartCommand rubyBaseStartCommand) { }
        public virtual void VisitRubyTextStartCommand(RubyTextStartCommand rubyTextStartCommand) { }
        public virtual void VisitRubyTextEndCommand(RubyTextEndCommand rubyTextEndCommand) { }
        public virtual void VisitSetFontSizeCommand(SetFontSizeCommand setFontSizeCommand) { }
        public virtual void VisitPrintInParallelCommand(PrintInParallelCommand printInParallelCommand) { }
        public virtual void VisitCenterTextCommand(CenterTextCommand centerTextCommand) { }
        public virtual void VisitSetMarginCommand(SetMarginCommand setMarginCommand) { }
        public virtual void VisitGetHardcodedValueCommand(GetHardcodedValueCommand getHardcodedValueCommand) { }
        public virtual void VisitEvaluateExpressionCommand(EvaluateExpressionCommand evaluateExpressionCommand) { }
        public virtual void VisitAutoForwardCommand(AutoForwardCommand autoForwardCommand) { }

        public virtual void VisitUnknownCommand(UnknownCommand unknownCommand) { }
    }
}
