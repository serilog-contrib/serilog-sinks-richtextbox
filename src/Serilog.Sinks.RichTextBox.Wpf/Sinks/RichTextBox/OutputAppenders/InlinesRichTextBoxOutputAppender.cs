using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

namespace Serilog.Sinks.RichTextBox.Output
{
    public class InlinesRichTextBoxOutputAppender : RichTextBoxOutputAppenderBase<InlinesRichTextBoxOutputAppenderArgs>
    {
        public InlinesRichTextBoxOutputAppender(InlinesRichTextBoxOutputAppenderArgs args) : base(args)
        {

        }

        protected override void Append(System.Windows.Controls.RichTextBox richTextBox, FlowDocument document, List<Paragraph> paragraphs)
        {
            var inlines = (
                from x in paragraphs
                from y in x.Inlines
                select y
                ).ToList();

            if (document.Blocks.LastBlock is Paragraph { } target)
            {
                target.Inlines.AddRange(inlines);
            }
            else
            {
                var paragraph = new Paragraph();
                paragraph.Inlines.AddRange(inlines);

                document.Blocks.Add(paragraph);
            }

            if (Args.ScrollOnChange)
            {
                richTextBox.ScrollToEnd();
            }

        }
    }

}
