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
using System.Globalization;
using System.IO;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.RichTextBox.Rendering;
using Serilog.Sinks.RichTextBox.Themes;

namespace Serilog.Sinks.RichTextBox.Formatting
{
    internal class ThemedJsonValueFormatter : ThemedValueFormatter
    {
        private readonly ThemedDisplayValueFormatter _displayFormatter;
        private readonly IFormatProvider _formatProvider;

        public ThemedJsonValueFormatter(RichTextBoxTheme theme, IFormatProvider formatProvider)
            : base(theme)
        {
            _displayFormatter = new ThemedDisplayValueFormatter(theme, formatProvider);
            _formatProvider = formatProvider;
        }

        public override ThemedValueFormatter SwitchTheme(RichTextBoxTheme theme)
        {
            return new ThemedJsonValueFormatter(theme, _formatProvider);
        }

        protected override int VisitScalarValue(ThemedValueFormatterState state, ScalarValue scalar)
        {
            if (scalar is null)
            {
                throw new ArgumentNullException(nameof(scalar));
            }

            // At the top level, for scalar values, use "display" rendering.
            if (state.IsTopLevel)
            {
                return _displayFormatter.FormatLiteralValue(scalar, state.Output, state.Format);
            }

            return FormatLiteralValue(scalar, state.Output);
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
                Visit(state.Nest(), sequence.Elements[index]);
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
                    JsonValueFormatter.WriteQuotedJsonString(escapedPropertyName, state.Output);
                }

                using (ApplyStyle(state.Output, RichTextBoxThemeStyle.TertiaryText, ref count))
                {
                    state.Output.Write(": ");
                }

                count += Visit(state.Nest(), property.Value);
            }

            if (structure.TypeTag != null)
            {
                using (ApplyStyle(state.Output, RichTextBoxThemeStyle.TertiaryText, ref count))
                {
                    state.Output.Write(delim);
                }

                using (ApplyStyle(state.Output, RichTextBoxThemeStyle.Name, ref count))
                {
                    JsonValueFormatter.WriteQuotedJsonString("$type", state.Output);
                }

                using (ApplyStyle(state.Output, RichTextBoxThemeStyle.TertiaryText, ref count))
                {
                    state.Output.Write(": ");
                }

                using (ApplyStyle(state.Output, RichTextBoxThemeStyle.String, ref count))
                {
                    var typeTag = SpecialCharsEscaping.Apply(structure.TypeTag, ref count);
                    JsonValueFormatter.WriteQuotedJsonString(typeTag, state.Output);
                }
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

                var style = element.Key.Value is null
                    ? RichTextBoxThemeStyle.Null
                    : element.Key.Value is string
                        ? RichTextBoxThemeStyle.String
                        : RichTextBoxThemeStyle.Scalar;

                using (ApplyStyle(state.Output, style, ref count))
                {
                    var escapedKey = SpecialCharsEscaping.Apply((element.Key.Value ?? "null").ToString(), ref count);
                    JsonValueFormatter.WriteQuotedJsonString(escapedKey, state.Output);
                }

                using (ApplyStyle(state.Output, RichTextBoxThemeStyle.TertiaryText, ref count))
                {
                    state.Output.Write(": ");
                }

                count += Visit(state.Nest(), element.Value);
            }

            using (ApplyStyle(state.Output, RichTextBoxThemeStyle.TertiaryText, ref count))
            {
                state.Output.Write('}');
            }

            return count;
        }

        private int FormatLiteralValue(ScalarValue scalar, TextWriter output)
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
                    var escapedValue = SpecialCharsEscaping.Apply(str, ref count);
                    JsonValueFormatter.WriteQuotedJsonString(escapedValue, output);
                }

                return count;
            }

            if (value is ValueType)
            {
                if (value is int || value is uint || value is long || value is ulong || value is decimal ||
                    value is byte || value is sbyte || value is short || value is ushort)
                {
                    using (ApplyStyle(output, RichTextBoxThemeStyle.Number, ref count))
                    {
                        output.Write(((IFormattable) value).ToString(null, CultureInfo.InvariantCulture));
                    }

                    return count;
                }

                if (value is double d)
                {
                    using (ApplyStyle(output, RichTextBoxThemeStyle.Number, ref count))
                    {
                        if (double.IsNaN(d) || double.IsInfinity(d))
                        {
                            JsonValueFormatter.WriteQuotedJsonString(d.ToString(CultureInfo.InvariantCulture), output);
                        }
                        else
                        {
                            output.Write(d.ToString("R", CultureInfo.InvariantCulture));
                        }
                    }

                    return count;
                }

                if (value is float f)
                {
                    using (ApplyStyle(output, RichTextBoxThemeStyle.Number, ref count))
                    {
                        if (double.IsNaN(f) || double.IsInfinity(f))
                        {
                            JsonValueFormatter.WriteQuotedJsonString(f.ToString(CultureInfo.InvariantCulture), output);
                        }
                        else
                        {
                            output.Write(f.ToString("R", CultureInfo.InvariantCulture));
                        }
                    }

                    return count;
                }

                if (value is bool b)
                {
                    using (ApplyStyle(output, RichTextBoxThemeStyle.Boolean, ref count))
                    {
                        output.Write(b ? "true" : "false");
                    }

                    return count;
                }

                if (value is char ch)
                {
                    using (ApplyStyle(output, RichTextBoxThemeStyle.Scalar, ref count))
                    {
                        var charString = SpecialCharsEscaping.Apply(ch.ToString(), ref count);
                        JsonValueFormatter.WriteQuotedJsonString(charString, output);
                    }

                    return count;
                }

                if (value is DateTime || value is DateTimeOffset)
                {
                    using (ApplyStyle(output, RichTextBoxThemeStyle.Scalar, ref count))
                    {
                        output.Write('\"');
                        output.Write(((IFormattable) value).ToString("O", CultureInfo.InvariantCulture));
                        output.Write('\"');
                    }

                    return count;
                }
            }

            using (ApplyStyle(output, RichTextBoxThemeStyle.Scalar, ref count))
            {
                var escapedValue = SpecialCharsEscaping.Apply(value.ToString(), ref count);
                JsonValueFormatter.WriteQuotedJsonString(escapedValue, output);
            }

            return count;
        }
    }
}
