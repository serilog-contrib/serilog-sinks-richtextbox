using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Serilog.Sinks.RichTextBox.Output {

    public record InlinesRichTextBoxOutputAppenderArgs : RichTextBoxOutputAppenderArgs {
        public bool ScrollOnChange { get; init; }
    }

}
