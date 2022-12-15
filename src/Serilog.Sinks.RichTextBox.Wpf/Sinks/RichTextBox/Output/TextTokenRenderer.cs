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

using System.IO;
using Serilog.Events;
using Serilog.Sinks.RichTextBox.Rendering;
using Serilog.Sinks.RichTextBox.Themes;

namespace Serilog.Sinks.RichTextBox.Output
{
    internal class TextTokenRenderer : OutputTemplateTokenRenderer
    {
        private readonly RichTextBoxTheme _theme;
        private readonly string _text;

        public TextTokenRenderer(RichTextBoxTheme theme, string text)
        {
            _theme = theme;
            _text = text;
        }

        public override void Render(LogEvent logEvent, TextWriter output)
        {
            var _ = 0;
            var text = _text;

            using (_theme.Apply(output, RichTextBoxThemeStyle.TertiaryText, ref _))
            {
                text = SpecialCharsEscaping.Apply(text, ref _);
                output.Write(text);
            }
        }
    }
}
