using System.Linq;
using System.Windows.Documents;

namespace Serilog.Sinks.RichTextBox.Output {
    public class InlinesRichTextBoxOutputAppender : RichTextBoxOutputAppenderBase<InlinesRichTextBoxOutputAppenderArgs> {
        public InlinesRichTextBoxOutputAppender(InlinesRichTextBoxOutputAppenderArgs args) : base(args) {
            
        }

        protected override void Append(System.Windows.Controls.RichTextBox richTextBox, FlowDocument document, Paragraph paragraph) {
            if(document.Blocks.LastBlock is Paragraph { } target) {
                var inlines = paragraph.Inlines.ToList();
                target.Inlines.AddRange(inlines);
            } else {
                document.Blocks.Add(paragraph);
            }

            if (Args.ScrollOnChange) {
                richTextBox.ScrollToEnd();
            }
            
        }
    }

}
