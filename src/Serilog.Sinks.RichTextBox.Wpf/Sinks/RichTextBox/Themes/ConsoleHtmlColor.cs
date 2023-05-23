#region Copyright 2021-2023 C. Augusto Proiete & Contributors
//
// Licensed under the Apache License, Version 2.0 (the "License";
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

namespace Serilog.Sinks.RichTextBox.Themes
{
    internal static class ConsoleHtmlColor
    {
        private static readonly object _syncLock = new object();

        static ConsoleHtmlColor()
        {
            lock (_syncLock)
            {
                Black = "#000000";
                DarkBlue =  "#000080";
                DarkGreen = "#008000";
                DarkCyan = "#008080";
                DarkRed = "#800000";
                DarkMagenta  = "#800080";
                DarkYellow = "#808000";
                Gray = "#c0c0c0";
                DarkGray = "#808080";
                Blue = "#0000ff";
                Green = "#00ff00";
                Cyan = "#00ffff";
                Red = "#ff0000";
                Magenta = "#ff00ff";
                Yellow = "#ffff00";
                White = "#ffffff";
            }
        }

        public static string Black { get; }
        public static string DarkBlue { get; } 
        public static string DarkGreen { get; }
        public static string DarkCyan { get; }
        public static string DarkRed { get; }
        public static string DarkMagenta  { get; }
        public static string DarkYellow { get; }
        public static string Gray { get; }
        public static string DarkGray { get; }
        public static string Blue { get; }
        public static string Green { get; }
        public static string Cyan { get; }
        public static string Red { get; }
        public static string Magenta { get; }
        public static string Yellow { get; }
        public static string White { get; }
    }
}
