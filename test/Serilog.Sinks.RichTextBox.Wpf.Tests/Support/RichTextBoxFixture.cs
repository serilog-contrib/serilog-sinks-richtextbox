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
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Serilog.Sinks.RichTextBox.Abstraction;

namespace Serilog.Sinks.RichTextBox.Wpf.Tests.Support
{
    internal class RichTextBoxFixture : IRichTextBox
    {
        private const string _xamlParagraphStart = "<Paragraph xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xml:space=\"preserve\">";
        private const string _xamlParagraphEnd = "</Paragraph>";

        private readonly StringBuilder _contentBuilder = new();

        public string Content => _contentBuilder.ToString();

        public Task WriteAsync(List<string> xamlParagraphTexts) {
            foreach (var xamlParagraphText in xamlParagraphTexts)
            {
                Write(xamlParagraphText);
            }

            return Task.CompletedTask;
        }

        public void Write(string xamlParagraphText)
        {
            if (xamlParagraphText is null)
            {
                throw new ArgumentNullException(nameof(xamlParagraphText));
            }

            if (!xamlParagraphText.StartsWith(_xamlParagraphStart, StringComparison.Ordinal))
            {
                throw new ArgumentException($"{nameof(xamlParagraphText)} must start with `{_xamlParagraphStart}`", nameof(xamlParagraphText));
            }

            if (!xamlParagraphText.EndsWith(_xamlParagraphEnd, StringComparison.Ordinal))
            {
                throw new ArgumentException($"{nameof(xamlParagraphText)} must end with `{_xamlParagraphEnd}`", nameof(xamlParagraphText));
            }

            var prefixLength = _xamlParagraphStart.Length;
            var suffixLength = _xamlParagraphEnd.Length;

            var inlines =
                xamlParagraphText.Substring(prefixLength, xamlParagraphText.Length - prefixLength - suffixLength);

            _contentBuilder.Append(inlines);
        }

    }
}
