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

using System.IO;

namespace Serilog.Sinks.RichTextBox.Themes
{
    /// <summary>
    /// The base class for styled RichTextBox output.
    /// </summary>
    public abstract class RichTextBoxTheme
    {
        /// <summary>
        /// No styling applied.
        /// </summary>
        public static RichTextBoxTheme None { get; } = new EmptyRichTextBoxTheme();

        /// <summary>
        /// True if styling applied by the theme is written into the output, and can thus be
        /// buffered and measured.
        /// </summary>
        public abstract bool CanBuffer { get; }

        /// <summary>
        /// Begin a span of text in the specified <paramref name="style"/>.
        /// </summary>
        /// <param name="output">Output destination.</param>
        /// <param name="style">Style to apply.</param>
        /// <returns> The number of characters written to <paramref name="output"/>. </returns>
        public abstract int Set(TextWriter output, RichTextBoxThemeStyle style);

        /// <summary>
        /// Reset the output to un-styled colors.
        /// </summary>
        /// <param name="output">Output destination.</param>
        public abstract void Reset(TextWriter output);

        /// <summary>
        /// The number of characters written by the <see cref="Reset(TextWriter)"/> method.
        /// </summary>
        protected abstract int ResetCharCount { get; }

        internal StyleReset Apply(TextWriter output, RichTextBoxThemeStyle style, ref int invisibleCharacterCount)
        {
            invisibleCharacterCount += Set(output, style);
            invisibleCharacterCount += ResetCharCount;

            return new StyleReset(this, output);
        }
    }
}
