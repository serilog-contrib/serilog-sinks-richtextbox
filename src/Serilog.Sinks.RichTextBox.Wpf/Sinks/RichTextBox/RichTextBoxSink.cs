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
        private Thread _consumerThread;
        private ConcurrentQueue<LogEvent> _messageQueue;

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

            _messageQueue = new ConcurrentQueue<LogEvent>();

            _consumerThread = new Thread(new ThreadStart(ProcessMessages)) { IsBackground = true };
            _consumerThread.Start();
        }

        private enum States
        {
            Init,
            Dequeue,
            Log,
        }

        private void ProcessMessages()
        {
            StringBuilder sb = new();
            Stopwatch sw = Stopwatch.StartNew();
            States state = States.Init;
            int msgCounter = 0;

            while (true)
            {
                switch (state)
                {
                    //prepare the string builder and data
                    case States.Init:
                        sb.Clear();
                        sb.Append($"<Paragraph xmlns =\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xml:space=\"preserve\">");
                        msgCounter = 0;
                        state = States.Dequeue;
                        break;

                    case States.Dequeue:
                        if (sw.Elapsed.TotalMilliseconds >= 25 || msgCounter >= _batchSize)
                        {
                            if (msgCounter == 0)
                            {
                                //no messages, retick
                                sw.Restart();
                            }
                            else
                            {
                                //valid log condition
                                state = States.Log;
                                break;
                            }
                        }

                        if (_messageQueue.TryDequeue(out LogEvent logEvent) == false)
                        {
                            Thread.Sleep(1);
                            continue;
                        }

                        StringWriter writer = new();
                        _formatter.Format(logEvent, writer);

                        //got a message from the queue, retick
                        sw.Restart();

                        msgCounter++;
                        sb.Append(writer.ToString());
                        break;

                    case States.Log:
                        sb.Append("</Paragraph>");
                        string xamlParagraphText = sb.ToString();
                        _richTextBox.BeginInvoke(_dispatcherPriority, _renderAction, xamlParagraphText);
                        state = States.Init;
                        break;
                }
            }
        }

        public void Emit(LogEvent logEvent)
        {            
            _messageQueue.Enqueue(logEvent);
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
