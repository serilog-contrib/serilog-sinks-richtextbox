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
    internal class EmptyRichTextBoxTheme : RichTextBoxTheme
    {
        public override bool CanBuffer => true;

        protected override int ResetCharCount { get; } = 0;

        public override int Set(TextWriter output, RichTextBoxThemeStyle style) => 0;

        public override void Reset(TextWriter output)
        {
        }
    }
}
