using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace Serilog.Sinks.RichTextBox.Output
{
    public class ParagraphRichTextBoxOutputAppender : RichTextBoxOutputAppenderBase<ParagraphRichTextBoxOutputAppenderArgs>
    {

        public ParagraphRichTextBoxOutputAppender(ParagraphRichTextBoxOutputAppenderArgs args) : base(args)
        {

        }

        protected override void Append(System.Windows.Controls.RichTextBox richTextBox, FlowDocument document, List<Paragraph> paragraphs)
        {
            foreach (var paragraph in paragraphs)
            {

                if (paragraph.Inlines.LastInline is Run { } Run && (Run.Text == Environment.NewLine || Run.Text == "\n"))
                {
                    paragraph.Inlines.Remove(Run);
                }
            }

            
            if (Args.Prepend)
            {
                foreach (var paragraph in paragraphs)
                {
                    document.Blocks.InsertBefore(document.Blocks.FirstBlock, paragraph);
                }
            }
            else
            {
                document.Blocks.AddRange(paragraphs);
            }
    

            if (Args.MaxItems is { } Trim && Trim > 0)
            {
                while (document.Blocks.Count > Trim)
                {
                    if (Args.Prepend)
                    {
                        document.Blocks.Remove(document.Blocks.LastBlock);
                    }
                    else
                    {
                        document.Blocks.Remove(document.Blocks.FirstBlock);
                    }
                }
            }

            if (Args.ScrollOnChange)
            {
                if (Args.Prepend)
                {
                    richTextBox.ScrollToHome();
                }
                else
                {
                    richTextBox.ScrollToEnd();
                }
            }

        }

    }

}
