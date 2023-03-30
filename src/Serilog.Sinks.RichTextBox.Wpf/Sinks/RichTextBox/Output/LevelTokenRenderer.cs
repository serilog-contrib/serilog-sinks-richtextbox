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

using System.Collections.Generic;
using System.IO;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.RichTextBox.Rendering;
using Serilog.Sinks.RichTextBox.Themes;

namespace Serilog.Sinks.RichTextBox.Output
{
    internal class LevelTokenRenderer : OutputTemplateTokenRenderer
    {
        private readonly RichTextBoxTheme _theme;
        private readonly PropertyToken _levelToken;

        private static readonly Dictionary<LogEventLevel, RichTextBoxThemeStyle> Levels = new Dictionary<LogEventLevel, RichTextBoxThemeStyle>
        {
            { LogEventLevel.Verbose, RichTextBoxThemeStyle.LevelVerbose },
            { LogEventLevel.Debug, RichTextBoxThemeStyle.LevelDebug },
            { LogEventLevel.Information, RichTextBoxThemeStyle.LevelInformation },
            { LogEventLevel.Warning, RichTextBoxThemeStyle.LevelWarning },
            { LogEventLevel.Error, RichTextBoxThemeStyle.LevelError },
            { LogEventLevel.Fatal, RichTextBoxThemeStyle.LevelFatal },
        };

        // ReSharper disable once UnusedMember.Global
        protected LevelTokenRenderer()
        {
        }

        public LevelTokenRenderer(RichTextBoxTheme theme, PropertyToken levelToken)
        {
            _theme = theme;
            _levelToken = levelToken;
        }

        public override void Render(LogEvent logEvent, TextWriter output)
        {
            var moniker = LevelOutputFormat.GetLevelMoniker(logEvent.Level, _levelToken.Format);
            if (!Levels.TryGetValue(logEvent.Level, out var levelStyle))
            {
                levelStyle = RichTextBoxThemeStyle.Invalid;
            }

            var _ = 0;
            using (_theme.Apply(output, levelStyle, ref _))
            {
                Padding.Apply(output, moniker, _levelToken.Alignment);
            }
        }
    }
}
