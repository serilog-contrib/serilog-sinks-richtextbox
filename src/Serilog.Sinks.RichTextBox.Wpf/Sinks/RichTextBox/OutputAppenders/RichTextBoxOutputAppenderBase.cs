using System.Windows.Documents;

namespace Serilog.Sinks.RichTextBox.Output {
    public abstract class RichTextBoxOutputAppenderBase : IRichTextBoxOutputAppender {
        public void Append(System.Windows.Controls.RichTextBox richTextBox, Paragraph paragraph) {
            var flowDocument = richTextBox.Document ??= new FlowDocument();
            Append(richTextBox, flowDocument, paragraph);
        }

        protected abstract void Append(System.Windows.Controls.RichTextBox richTextBox, FlowDocument document, Paragraph paragraph);

    }

}
