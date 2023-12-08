﻿#region Copyright 2021-2023 C. Augusto Proiete & Contributors
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
        private readonly DispatcherPriority _dispatcherPriority;
        private readonly object _syncRoot;

        private readonly RenderAction _renderAction;
        private const int _defaultWriteBufferCapacity = 256;

        private const int _batchSize = 200;
        private const int _minimumDelayForIncompleteBatch = 25;
        private Channel<LogEvent> _messageQueue;

        public RichTextBoxSink(IRichTextBox richTextBox, ITextFormatter formatter, DispatcherPriority dispatcherPriority, object syncRoot)
        {
            _richTextBox = richTextBox ?? throw new ArgumentNullException(nameof(richTextBox));
            _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));

            if (!Enum.IsDefined(typeof(DispatcherPriority), dispatcherPriority))
            {
                throw new InvalidEnumArgumentException(nameof(dispatcherPriority), (int)dispatcherPriority,
                    typeof(DispatcherPriority));
            }

            _dispatcherPriority = dispatcherPriority;
            _syncRoot = syncRoot ?? throw new ArgumentNullException(nameof(syncRoot));

            _renderAction = Render;

            _messageQueue = Channel.CreateUnbounded<LogEvent>();

            Task.Run(ProcessMessages);
        }

        private async Task ProcessMessages()
        {
            int msgCounter = 0;
            const string initial = $"<Paragraph xmlns =\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xml:space=\"preserve\">";

            var sb = new StringBuilder(initial);

            async Task ReadChannelAsync()
            {
                var logEvent = await _messageQueue.Reader.ReadAsync();
                StringWriter writer = new();
                _formatter.Format(logEvent, writer);
                msgCounter++;

                sb.Append(writer.ToString());
            }

            Task restartTimer() => Task.Delay(_minimumDelayForIncompleteBatch);

            Task incompleteBatchTask = restartTimer();
            Task logEventTask = ReadChannelAsync();

            while (true)
            {
                var firstTask = await Task.WhenAny(incompleteBatchTask, logEventTask);

                if (firstTask == logEventTask && msgCounter < _batchSize)
                {
                    logEventTask = ReadChannelAsync();
                    continue;
                }
                else if (msgCounter == 0)
                {
                    //no messages, restart timer
                    incompleteBatchTask = restartTimer();
                    continue;
                }

                sb.Append("</Paragraph>");
                string xamlParagraphText = sb.ToString();
                await _richTextBox.BeginInvoke(_dispatcherPriority, _renderAction, xamlParagraphText);
                sb.Clear();
                sb.Append(initial);
                msgCounter = 0;
            }
        }

        public void Emit(LogEvent logEvent)
        {
            _messageQueue.Writer.TryWrite(logEvent);
        }

        private void Render(string xamlParagraphText)
        {
            var richTextBox = _richTextBox;

            lock (_syncRoot)
            {
                richTextBox.Write(xamlParagraphText);
            }
        }

        public void Dispose()
        {
        }

        internal delegate void RenderAction(string xamlParagraphText);
    }
}
