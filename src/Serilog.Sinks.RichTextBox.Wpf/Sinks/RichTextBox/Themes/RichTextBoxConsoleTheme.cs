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
using System.Linq;
using System.Text;
using Serilog.Sinks.RichTextBox.Rendering;

namespace Serilog.Sinks.RichTextBox.Themes
{
    /// <summary>
    /// A RichTextBox theme using the styling facilities of the <see cref="System.Windows.Documents.Run"/> class.
    /// </summary>
    public class RichTextBoxConsoleTheme : RichTextBoxTheme
    {
        private readonly IReadOnlyDictionary<RichTextBoxThemeStyle, RichTextBoxConsoleThemeStyle> _styles;

        private const string _runStyleReset = "</Run>";

        /// <summary>
        /// Construct a theme given a set of styles.
        /// </summary>
        /// <param name="styles">Styles to apply within the theme.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="styles"/> is <code>null</code></exception>
        public RichTextBoxConsoleTheme(IReadOnlyDictionary<RichTextBoxThemeStyle, RichTextBoxConsoleThemeStyle> styles)
        {
            if (styles is null)
            {
                throw new ArgumentNullException(nameof(styles));
            }

            _styles = styles.ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        /// <summary>
        /// A theme in the style of the original <i>Serilog.Sinks.Literate</i>. This is the default when no theme is specified.
        /// </summary>
        public static RichTextBoxConsoleTheme Literate { get; } = RichTextBoxConsoleThemes.Literate;

        /// <summary>
        /// A theme using only gray, black and white.
        /// </summary>
        public static RichTextBoxConsoleTheme Grayscale { get; } = RichTextBoxConsoleThemes.Grayscale;

        /// <summary>
        /// A theme based on the original Serilog "colored console" sink.
        /// </summary>
        public static RichTextBoxConsoleTheme Colored { get; } = RichTextBoxConsoleThemes.Colored;

        /// <inheritdoc/>
        public override bool CanBuffer => true;

        /// <inheritdoc/>
        protected override int ResetCharCount { get; } = _runStyleReset.Length;

        /// <inheritdoc/>
        public override int Set(TextWriter output, RichTextBoxThemeStyle style)
        {
            if (_styles.TryGetValue(style, out var runStyle))
            {
                var buffer = new StringBuilder();
                buffer.Append("<Run");

                var _ = 0;

                var backgroundColor = runStyle.Background;
                if (!string.IsNullOrWhiteSpace(backgroundColor))
                {
                    buffer.AppendFormat(" Background=\"{0}\"", SpecialCharsEscaping.Apply(backgroundColor, ref _));
                }

                var foregroundColor = runStyle.Foreground;
                if (!string.IsNullOrWhiteSpace(foregroundColor))
                {
                    buffer.AppendFormat(" Foreground=\"{0}\"", SpecialCharsEscaping.Apply(foregroundColor, ref _));
                }

                var fontWeight = runStyle.FontWeight;
                if (!string.IsNullOrWhiteSpace(fontWeight))
                {
                    buffer.AppendFormat(" FontWeight=\"{0}\"", SpecialCharsEscaping.Apply(fontWeight, ref _));
                }

                buffer.Append('>');

                output.Write(buffer);
                return buffer.Length;
            }

            return 0;
        }

        /// <inheritdoc/>
        public override void Reset(TextWriter output)
        {
            output.Write(_runStyleReset);
        }
    }
}
