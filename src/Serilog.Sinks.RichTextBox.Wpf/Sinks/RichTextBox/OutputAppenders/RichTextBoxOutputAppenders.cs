namespace Serilog.Sinks.RichTextBox.Output {
    public static class RichTextBoxOutputAppenders {
        public static IRichTextBoxOutputAppender Legacy { get; }
        public static IRichTextBoxOutputAppender Default { get; }

        static RichTextBoxOutputAppenders() {
            var LegacyArgs = new InlinesRichTextBoxOutputAppenderArgs() {
                ScrollOnChange = false,
            };

            Legacy = new InlinesRichTextBoxOutputAppender(LegacyArgs) {

            };

            var DefaultArgs = new ParagraphRichTextBoxOutputAppenderArgs() {
                ScrollOnChange = true,
                MaxItems = 1000,
            };

            Default = new ParagraphRichTextBoxOutputAppender(DefaultArgs) {

            };

        }

    }

}
