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
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Serilog;
using Serilog.Debugging;

namespace WpfNet50Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private static readonly object _syncRoot = new object();

        public MainWindow()
        {
            InitializeComponent();

            SelfLog.Enable(message => Trace.WriteLine($"INTERNAL ERROR: {message}"));

            const string outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.RichTextBox(_richTextBox, outputTemplate: outputTemplate, syncRoot: _syncRoot)
                .Enrich.WithThreadId()
                .CreateLogger();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Log.Debug("Getting started");

            Log.Information("Hello {Name} from thread {ThreadId}", Environment.UserName,
                Thread.CurrentThread.ManagedThreadId);

            Log.Warning("No coins remain at position {@Position}", new { Lat = 25, Long = 134 });

            try
            {
                Fail();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Oops... Something went wrong");
            }
        }

        private static void Fail()
        {
            throw new DivideByZeroException();
        }

        private void Clear_OnClick(object sender, RoutedEventArgs e)
        {
            lock (_syncRoot)
            {
                _richTextBox.Document.Blocks.Clear();
            }
        }

        private void LogVerbose_OnClick(object sender, RoutedEventArgs e)
        {
            Log.Verbose("Hello! Now => {Now}", DateTime.Now);
        }

        private void LogDebug_OnClick(object sender, RoutedEventArgs e)
        {
            Log.Debug("Hello! Now => {Now}", DateTime.Now);
        }

        private void LogInformation_OnClick(object sender, RoutedEventArgs e)
        {
            Log.Information("Hello! Now => {Now}", DateTime.Now);
        }

        private void LogWarning_OnClick(object sender, RoutedEventArgs e)
        {
            Log.Warning("Hello! Now => {Now}", DateTime.Now);
        }

        private void LogError_OnClick(object sender, RoutedEventArgs e)
        {
            Log.Error("Hello! Now => {Now}", DateTime.Now);
        }

        private void LogFatal_OnClick(object sender, RoutedEventArgs e)
        {
            Log.Fatal("Hello! Now => {Now}", DateTime.Now);
        }

        private void LogParallelFor_OnClick(object sender, RoutedEventArgs e)
        {
            Parallel.For(1, 101, stepNumber =>
            {
                var stepName = FormattableString.Invariant($"Step {stepNumber:000}");

                Log.Verbose("Hello from Parallel.For({StepName}) Verbose", stepName);
                Log.Debug("Hello from Parallel.For({StepName}) Debug", stepName);
                Log.Information("Hello from Parallel.For({StepName}) Information", stepName);
                Log.Warning("Hello from Parallel.For({StepName}) Warning", stepName);
                Log.Error("Hello from Parallel.For({StepName}) Error", stepName);
                Log.Fatal("Hello from Parallel.For({StepName}) Fatal", stepName);
            });
        }

        private async void LogTaskRun_OnClick(object sender, RoutedEventArgs e)
        {
            var tasks = new System.Collections.Generic.List<Task>();

            for (var i = 1; i <= 100; i++)
            {
                var stepNumber = i;
                var task = Task.Run(() =>
                {
                    var stepName = FormattableString.Invariant($"Step {stepNumber:000}");

                    Log.Verbose("Hello from Task.Run({StepName}) Verbose", stepName);
                    Log.Debug("Hello from Task.Run({StepName}) Debug", stepName);
                    Log.Information("Hello from Task.Run({StepName}) Information", stepName);
                    Log.Warning("Hello from Task.Run({StepName}) Warning", stepName);
                    Log.Error("Hello from Task.Run({StepName}) Error", stepName);
                    Log.Fatal("Hello from Task.Run({StepName}) Fatal", stepName);
                });
            
                tasks.Add(task);
            }
            
            await Task.WhenAll(tasks);
        }
    }
}
