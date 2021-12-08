namespace Serilog.Sinks.RichTextBox.Output
{
    public interface IRichTextBoxOutputAppender
    {
        void Append(System.Windows.Controls.RichTextBox RichTextBox, System.Windows.Documents.Paragraph Paragraph);
    }

}
