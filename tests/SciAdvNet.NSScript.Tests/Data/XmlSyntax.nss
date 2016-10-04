// 0. XmlElementStartTag (PRE)
// 1. Xml_TextStartTag
// 2. Xml_Text
// 3. Xml_Text
// 4. Xml_LineBreak
// 5. XmlElementStartTag (voice)
// 6. Xml_Text
// 7. XmlElementStartTag (FONT)
// 8. Xml_Text
// 9. XmlElementEndTag (FONT)
// 10. Xml_Text
// 11. Xml_LineBreak
// 12. XmlElementEndTag (PRE)

<PRE box00>
[text001]
This is a test.
It features comments
// This is a comment (duh)
A voice tag

<voice name="Takumi" class="Takumi" src="testpath">
A <FONT incolor="#FFFFFF" outcolor="BLACK">FONT</FONT> tag
And line breaks, which actually matter here.

</PRE>