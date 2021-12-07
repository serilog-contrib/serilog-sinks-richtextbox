namespace Serilog.Sinks.RichTextBox.Output {
    public record ParagraphRichTextBoxOutputAppenderArgs : RichTextBoxOutputAppenderArgs {
        public bool ScrollOnChange { get; init; }
        public bool Prepend { get; init; }
        public long? MaxItems { get; init; }
    }

}
