namespace Serilog.Sinks.RichTextBox.Output
{

    public record InlinesRichTextBoxOutputAppenderArgs : RichTextBoxOutputAppenderArgs
    {
        public bool ScrollOnChange { get; init; }
    }

}
