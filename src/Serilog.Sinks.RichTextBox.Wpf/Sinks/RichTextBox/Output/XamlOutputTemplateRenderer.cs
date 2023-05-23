#region Copyright 2021-2023 C. Augusto Proiete & Contributors
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
using System.IO;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using Serilog.Parsing;
using Serilog.Sinks.RichTextBox.Themes;

namespace Serilog.Sinks.RichTextBox.Output
{
    internal class XamlOutputTemplateRenderer : ITextFormatter
    {
        private readonly OutputTemplateTokenRenderer[] _renderers;

        public XamlOutputTemplateRenderer(RichTextBoxTheme theme, string outputTemplate, IFormatProvider formatProvider)
        {
            if (outputTemplate is null)
            {
                throw new ArgumentNullException(nameof(outputTemplate));
            }

            var template = new MessageTemplateParser().Parse(outputTemplate);

            var renderers = new List<OutputTemplateTokenRenderer>();
            foreach (var token in template.Tokens)
            {
                if (token is TextToken tt)
                {
                    renderers.Add(new TextTokenRenderer(theme, tt.Text));
                    continue;
                }

                var pt = (PropertyToken) token;

                switch (pt.PropertyName)
                {
                    case OutputProperties.LevelPropertyName:
                        renderers.Add(new LevelTokenRenderer(theme, pt));
                        break;
                    case OutputProperties.NewLinePropertyName:
                        renderers.Add(new NewLineTokenRenderer(pt.Alignment));
                        break;
                    case OutputProperties.ExceptionPropertyName:
                        renderers.Add(new ExceptionTokenRenderer(theme));
                        break;
                    case OutputProperties.MessagePropertyName:
                        renderers.Add(new MessageTemplateOutputTokenRenderer(theme, pt, formatProvider));
                        break;
                    case OutputProperties.TimestampPropertyName:
                        renderers.Add(new TimestampTokenRenderer(theme, pt, formatProvider));
                        break;
                    case "Properties":
                        renderers.Add(new PropertiesTokenRenderer(theme, pt, template, formatProvider));
                        break;
                    default:
                        renderers.Add(new EventPropertyTokenRenderer(theme, pt, formatProvider));
                        break;
                }
            }

            _renderers = renderers.ToArray();
        }

        public void Format(LogEvent logEvent, TextWriter output)
        {
            if (logEvent is null)
            {
                throw new ArgumentNullException(nameof(logEvent));
            }

            if (output is null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            foreach (var renderer in _renderers)
            {
                renderer.Render(logEvent, output);
            }
        }
    }
}
