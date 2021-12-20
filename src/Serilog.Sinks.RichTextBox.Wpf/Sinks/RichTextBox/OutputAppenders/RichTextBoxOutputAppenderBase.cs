using System.Collections.Generic;
using System.Windows.Documents;

namespace Serilog.Sinks.RichTextBox.Output
{
    public abstract class RichTextBoxOutputAppenderBase : IRichTextBoxOutputAppender
    {
        public void Append(System.Windows.Controls.RichTextBox richTextBox, List<Paragraph> paragraphs)
        {
            var flowDocument = richTextBox.Document ??= new FlowDocument();
            Append(richTextBox, flowDocument, paragraphs);
        }

        protected abstract void Append(System.Windows.Controls.RichTextBox richTextBox, FlowDocument document, List<Paragraph> paragraphs);

    }

}
