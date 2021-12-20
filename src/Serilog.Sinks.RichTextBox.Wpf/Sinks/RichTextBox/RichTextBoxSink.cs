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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Threading;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.RichTextBox.Abstraction;



namespace Serilog.Sinks.RichTextBox
{
    internal sealed class RichTextBoxSink : ILogEventSink, IDisposable
    {
        private readonly IRichTextBox _richTextBox;
        private readonly ITextFormatter _formatter;

        private readonly Task _consumerThread;
        private readonly ChannelReader<LogEvent> _messageReader;
        private readonly ChannelWriter<LogEvent> _messageWriter;

        public RichTextBoxSink(IRichTextBox richTextBox, ITextFormatter formatter)
        {
            _richTextBox = richTextBox ?? throw new ArgumentNullException(nameof(richTextBox));
            _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));

            var chan = Channel.CreateUnbounded<LogEvent>(new UnboundedChannelOptions() {
                AllowSynchronousContinuations = false,
                SingleReader = true,
                SingleWriter = false,
            });

            _messageReader = chan.Reader;
            _messageWriter = chan.Writer;

            _consumerThread = Task.Run(ProcessMessages);
        }

        private async Task ProcessMessages()
        {

            var moredata = true;

            while (moredata)
            {
                moredata = await _messageReader.WaitToReadAsync()
                    .ConfigureAwait(false)
                    ;

                if (moredata)
                {
                    await Task.Delay(100)
                        .ConfigureAwait(false)
                        ;

                    var xamlParagraphMessages = new List<string>();

                    while (_messageReader.TryRead(out var logEvent))
                    {
                        var sb = new StringBuilder();
                        sb.Append($"<Paragraph xmlns =\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xml:space=\"preserve\">");

                        StringWriter writer = new();
                        _formatter.Format(logEvent, writer);
                        sb.Append(writer.ToString());
                        sb.Append("</Paragraph>");

                        var xamlParagraphText = sb.ToString();
                        xamlParagraphMessages.Add(xamlParagraphText);
                    }

                    if(xamlParagraphMessages.Count > 1)
                    {

                    }

                    if (xamlParagraphMessages.Count > 0) {

                        await _richTextBox.WriteAsync(xamlParagraphMessages)
                            .ConfigureAwait(false)
                            ;

                    }

                }

            }

        }

        public void Emit(LogEvent logEvent)
        {
            _messageWriter.TryWrite(logEvent);
        }

        public void Dispose()
        {
            _messageWriter.TryComplete();
        }

    }
}
