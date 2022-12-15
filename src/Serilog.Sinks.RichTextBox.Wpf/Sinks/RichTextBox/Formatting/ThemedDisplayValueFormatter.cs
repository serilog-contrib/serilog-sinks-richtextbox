﻿#region Copyright 2021-2022 C. Augusto Proiete & Contributors
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
using System.Globalization;
using System.IO;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.RichTextBox.Rendering;
using Serilog.Sinks.RichTextBox.Themes;

namespace Serilog.Sinks.RichTextBox.Formatting
{
    internal class ThemedDisplayValueFormatter : ThemedValueFormatter
    {
        private readonly IFormatProvider _formatProvider;

        public ThemedDisplayValueFormatter(RichTextBoxTheme theme, IFormatProvider formatProvider)
            : base(theme)
        {
            _formatProvider = formatProvider;
        }

        public override ThemedValueFormatter SwitchTheme(RichTextBoxTheme theme)
        {
            return new ThemedDisplayValueFormatter(theme, _formatProvider);
        }

        protected override int VisitScalarValue(ThemedValueFormatterState state, ScalarValue scalar)
        {
            if (scalar is null)
            {
                throw new ArgumentNullException(nameof(scalar));
            }

            return FormatLiteralValue(scalar, state.Output, state.Format);
        }

        protected override int VisitSequenceValue(ThemedValueFormatterState state, SequenceValue sequence)
        {
            if (sequence is null)
            {
                throw new ArgumentNullException(nameof(sequence));
            }

            var count = 0;

            using (ApplyStyle(state.Output, RichTextBoxThemeStyle.TertiaryText, ref count))
            {
                state.Output.Write('[');
            }

            var delim = string.Empty;

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = 0; index < sequence.Elements.Count; ++index)
            {
                if (delim.Length != 0)
                {
                    using (ApplyStyle(state.Output, RichTextBoxThemeStyle.TertiaryText, ref count))
                    {
                        state.Output.Write(delim);
                    }
                }

                delim = ", ";
                Visit(state, sequence.Elements[index]);
            }

            using (ApplyStyle(state.Output, RichTextBoxThemeStyle.TertiaryText, ref count))
            {
                state.Output.Write(']');
            }

            return count;
        }

        protected override int VisitStructureValue(ThemedValueFormatterState state, StructureValue structure)
        {
            var count = 0;

            if (structure.TypeTag != null)
            {
                using (ApplyStyle(state.Output, RichTextBoxThemeStyle.Name, ref count))
                {
                    state.Output.Write(structure.TypeTag);
                }

                state.Output.Write(' ');
            }

            using (ApplyStyle(state.Output, RichTextBoxThemeStyle.TertiaryText, ref count))
            {
                state.Output.Write('{');
            }

            var delim = string.Empty;

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = 0; index < structure.Properties.Count; ++index)
            {
                if (delim.Length != 0)
                {
                    using (ApplyStyle(state.Output, RichTextBoxThemeStyle.TertiaryText, ref count))
                    {
                        state.Output.Write(delim);
                    }
                }

                delim = ", ";

                var property = structure.Properties[index];

                using (ApplyStyle(state.Output, RichTextBoxThemeStyle.Name, ref count))
                {
                    var escapedPropertyName = SpecialCharsEscaping.Apply(property.Name, ref count);
                    state.Output.Write(escapedPropertyName);
                }

                using (ApplyStyle(state.Output, RichTextBoxThemeStyle.TertiaryText, ref count))
                {
                    state.Output.Write('=');
                }

                count += Visit(state.Nest(), property.Value);
            }

            using (ApplyStyle(state.Output, RichTextBoxThemeStyle.TertiaryText, ref count))
            {
                state.Output.Write('}');
            }

            return count;
        }

        protected override int VisitDictionaryValue(ThemedValueFormatterState state, DictionaryValue dictionary)
        {
            var count = 0;

            using (ApplyStyle(state.Output, RichTextBoxThemeStyle.TertiaryText, ref count))
            {
                state.Output.Write('{');
            }

            var delim = string.Empty;
            foreach (var element in dictionary.Elements)
            {
                if (delim.Length != 0)
                {
                    using (ApplyStyle(state.Output, RichTextBoxThemeStyle.TertiaryText, ref count))
                    {
                        state.Output.Write(delim);
                    }
                }

                delim = ", ";

                using (ApplyStyle(state.Output, RichTextBoxThemeStyle.TertiaryText, ref count))
                {
                    state.Output.Write('[');
                }

                using (ApplyStyle(state.Output, RichTextBoxThemeStyle.String, ref count))
                {
                    count += Visit(state.Nest(), element.Key);
                }

                using (ApplyStyle(state.Output, RichTextBoxThemeStyle.TertiaryText, ref count))
                {
                    state.Output.Write("]=");
                }

                count += Visit(state.Nest(), element.Value);
            }

            using (ApplyStyle(state.Output, RichTextBoxThemeStyle.TertiaryText, ref count))
            {
                state.Output.Write('}');
            }

            return count;
        }

        public int FormatLiteralValue(ScalarValue scalar, TextWriter output, string format)
        {
            var value = scalar.Value;
            var count = 0;

            if (value is null)
            {
                using (ApplyStyle(output, RichTextBoxThemeStyle.Null, ref count))
                {
                    output.Write("null");
                }

                return count;
            }

            if (value is string str)
            {
                using (ApplyStyle(output, RichTextBoxThemeStyle.String, ref count))
                {
                    var escapedStr = SpecialCharsEscaping.Apply(str, ref count);

                    if (format != "l")
                    {
                        JsonValueFormatter.WriteQuotedJsonString(escapedStr, output);
                    }
                    else
                    {
                        output.Write(escapedStr);
                    }
                }

                return count;
            }

            if (value is ValueType)
            {
                if (value is int || value is uint || value is long || value is ulong ||
                    value is decimal || value is byte || value is sbyte || value is short ||
                    value is ushort || value is float || value is double)
                {
                    using (ApplyStyle(output, RichTextBoxThemeStyle.Number, ref count))
                    {
                        scalar.Render(output, format, _formatProvider);
                    }

                    return count;
                }

                if (value is bool b)
                {
                    using (ApplyStyle(output, RichTextBoxThemeStyle.Boolean, ref count))
                    {
                        output.Write(b.ToString(CultureInfo.InvariantCulture));
                    }

                    return count;
                }

                if (value is char ch)
                {
                    using (ApplyStyle(output, RichTextBoxThemeStyle.Scalar, ref count))
                    {
                        output.Write('\'');
                        output.Write(SpecialCharsEscaping.Apply(ch.ToString(), ref count));
                        output.Write('\'');
                    }

                    return count;
                }
            }

            using (ApplyStyle(output, RichTextBoxThemeStyle.Scalar, ref count))
            {
                scalar.Render(output, format, _formatProvider);
            }

            return count;
        }
    }
}
