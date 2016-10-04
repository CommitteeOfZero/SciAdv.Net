using Xunit;

namespace SciAdvNet.NSScript.Tests
{
    public class DialogueParsingTests
    {
        [Fact]
        public void TestEmptyDialogueBlock()
        {
            string text = "<PRE box>[text000]</PRE>";
            var block = NSScript.ParseDialogueBlock(text);
            Assert.Equal("box", block.BoxName);
            Assert.Equal(SyntaxNodeKind.DialogueBlock, block.Kind);
            Assert.Equal(0, block.Parts.Length);

            string toStringResult = Helpers.RemoveNewLineCharacters(block.ToString());
            Assert.Equal(text, toStringResult);
        }

        [Fact]
        public void TestDialogueLine_OneSegment()
        {
            string text = "<PRE box>[text000]This is a test.</PRE>";
            var block = NSScript.ParseDialogueBlock(text);
            Assert.Equal("text000", block.Identifier);
            Assert.Equal("box", block.BoxName);
            Assert.Equal(SyntaxNodeKind.DialogueBlock, block.Kind);
            Assert.Equal(1, block.Parts.Length);

            var line = block.Parts[0] as DialogueLine;
            Assert.Equal(SyntaxNodeKind.DialogueLine, line.Kind);
            Assert.Equal(1, line.Segments.Length);

            var segment = line.Segments[0];
            Assert.Equal(SyntaxNodeKind.TextSegment, segment.Kind);
            Assert.Equal("This is a test.", segment.Text);
        }
    }
}
