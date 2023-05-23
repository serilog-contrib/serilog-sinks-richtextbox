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
using System.IO;

namespace Serilog.Sinks.RichTextBox.Themes
{
    internal readonly struct StyleReset : IDisposable
    {
        private readonly RichTextBoxTheme _theme;
        private readonly TextWriter _output;

        public StyleReset(RichTextBoxTheme theme, TextWriter output)
        {
            _theme = theme;
            _output = output;
        }

        public void Dispose()
        {
            _theme.Reset(_output);
        }
    }
}
