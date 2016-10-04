namespace SciAdvNet.SC3.Text
{
    public abstract class SC3StringSegmentVisitor
    {
        public void Visit(SC3StringSegment segment) => segment.Accept(this);
        public virtual void VisitTextSegment(TextSegment text) { }
        public virtual void VisitMarker(Marker marker) { }
        public virtual void VisitSetColorCommand(SetColorCommand setColorCommand) { }
        public virtual void VisitSetFontSizeCommand(SetFontSizeCommand setFontSizeCommand) { }
        public virtual void VisitSetMarginCommand(SetMarginCommand setMarginCommand) { }
        public virtual void VisitEvaluateExpressionCommand(EvaluateExpressionCommand evaluateExpressionCommand) { }
        public virtual void VisitPresentCommand(PresentCommand presentCommand) { }
        public virtual void VisitCenterTextCommand(CenterTextCommand centerTextCommand) { }
    }
}
