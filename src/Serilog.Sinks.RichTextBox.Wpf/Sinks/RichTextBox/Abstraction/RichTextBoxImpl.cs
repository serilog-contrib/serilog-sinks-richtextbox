#region Copyright 2021 C. Augusto Proiete & Contributors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using System;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Threading;
using Serilog.Debugging;
using Serilog.Sinks.RichTextBox.Output;

namespace Serilog.Sinks.RichTextBox.Abstraction
{
    internal class RichTextBoxImpl : IRichTextBox
    {
        private readonly System.Windows.Controls.RichTextBox _richTextBox;
        private readonly IRichTextBoxOutputAppender _outputAppender;

        public RichTextBoxImpl(System.Windows.Controls.RichTextBox richTextBox, IRichTextBoxOutputAppender outputAppender)
        {
            _richTextBox = richTextBox ?? throw new ArgumentNullException(nameof(richTextBox));
            _outputAppender = outputAppender ?? throw new ArgumentNullException(nameof(outputAppender));
        }

        public void Write(string xamlParagraphText)
        {
            Paragraph parsedParagraph;

            try
            {
                parsedParagraph = (Paragraph) XamlReader.Parse(xamlParagraphText);
            }
            catch (XamlParseException ex)
            {
                SelfLog.WriteLine($"Error parsing `{xamlParagraphText}` to XAML: {ex.Message}");
                throw;
            }
            _outputAppender.Append(_richTextBox, parsedParagraph);

        }

        public bool CheckAccess()
        {
            return _richTextBox.CheckAccess();
        }

        public DispatcherOperation BeginInvoke(DispatcherPriority priority, Delegate method, object arg)
        {
            return _richTextBox.Dispatcher.BeginInvoke(priority, method, arg);
        }
    }
}
