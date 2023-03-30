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
using Serilog.Parsing;
using Serilog.Sinks.RichTextBox.Formatting;
using Serilog.Sinks.RichTextBox.Themes;

namespace Serilog.Sinks.RichTextBox.Rendering
{
    internal class ThemedMessageTemplateRenderer
    {
        private readonly RichTextBoxTheme _theme;
        private readonly ThemedValueFormatter _valueFormatter;
        private readonly bool _isLiteral;
        private static readonly RichTextBoxTheme _noTheme = new EmptyRichTextBoxTheme();
        private readonly ThemedValueFormatter _unthemedValueFormatter;

        public ThemedMessageTemplateRenderer(RichTextBoxTheme theme, ThemedValueFormatter valueFormatter, bool isLiteral)
        {
            _theme = theme ?? throw new ArgumentNullException(nameof(theme));
            _valueFormatter = valueFormatter;
            _isLiteral = isLiteral;
            _unthemedValueFormatter = valueFormatter.SwitchTheme(_noTheme);
        }

        public int Render(MessageTemplate template, IReadOnlyDictionary<string, LogEventPropertyValue> properties, TextWriter output)
        {
            var count = 0;

            foreach (var token in template.Tokens)
            {
                if (token is TextToken tt)
                {
                    count += RenderTextToken(tt, output);
                }
                else
                {
                    var pt = (PropertyToken)token;
                    count += RenderPropertyToken(pt, properties, output);
                }
            }

            return count;
        }

        private int RenderTextToken(TextToken tt, TextWriter output)
        {
            var count = 0;
            using (_theme.Apply(output, RichTextBoxThemeStyle.Text, ref count))
            {
                var text = SpecialCharsEscaping.Apply(tt.Text, ref count);
                output.Write(text);
            }

            return count;
        }

        private int RenderPropertyToken(PropertyToken pt, IReadOnlyDictionary<string, LogEventPropertyValue> properties, TextWriter output)
        {
            if (!properties.TryGetValue(pt.PropertyName, out var propertyValue))
            {
                var count = 0;
                using (_theme.Apply(output, RichTextBoxThemeStyle.Invalid, ref count))
                {
                    output.Write(SpecialCharsEscaping.Apply(pt.ToString(), ref count));
                }

                return count;
            }

            if (!pt.Alignment.HasValue)
            {
                return RenderValue(_theme, _valueFormatter, propertyValue, output, pt.Format);
            }

            var valueOutput = new StringWriter();

            if (!_theme.CanBuffer)
            {
                return RenderAlignedPropertyTokenUnbuffered(pt, output, propertyValue);
            }

            var invisibleCount = RenderValue(_theme, _valueFormatter, propertyValue, valueOutput, pt.Format);

            var value = valueOutput.ToString();

            if (value.Length - invisibleCount >= pt.Alignment.Value.Width)
            {
                output.Write(value);
            }
            else
            {
                Padding.Apply(output, value, pt.Alignment.Value.Widen(invisibleCount));
            }

            return invisibleCount;
        }

        private int RenderAlignedPropertyTokenUnbuffered(PropertyToken pt, TextWriter output, LogEventPropertyValue propertyValue)
        {
            var valueOutput = new StringWriter();
            RenderValue(_noTheme, _unthemedValueFormatter, propertyValue, valueOutput, pt.Format);

            var valueLength = valueOutput.ToString().Length;

            // ReSharper disable once PossibleInvalidOperationException
            if (valueLength >= pt.Alignment.Value.Width)
            {
                return RenderValue(_theme, _valueFormatter, propertyValue, output, pt.Format);
            }

            if (pt.Alignment.Value.Direction == AlignmentDirection.Left)
            {
                var invisible = RenderValue(_theme, _valueFormatter, propertyValue, output, pt.Format);
                Padding.Apply(output, string.Empty, pt.Alignment.Value.Widen(-valueLength));

                return invisible;
            }

            Padding.Apply(output, string.Empty, pt.Alignment.Value.Widen(-valueLength));

            return RenderValue(_theme, _valueFormatter, propertyValue, output, pt.Format);
        }

        private int RenderValue(RichTextBoxTheme theme, ThemedValueFormatter valueFormatter, LogEventPropertyValue propertyValue, TextWriter output, string format)
        {
            if (_isLiteral && propertyValue is ScalarValue {Value: string} sv)
            {
                var count = 0;
                using (theme.Apply(output, RichTextBoxThemeStyle.String, ref count))
                {
                    var text = SpecialCharsEscaping.Apply(sv.Value.ToString(), ref count);
                    output.Write(text);
                }

                return count;
            }

            return valueFormatter.Format(propertyValue, output, format, _isLiteral);
        }
    }
}
