using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Media;
using Serilog;
using Serilog.Debugging;

namespace WinFormsHostNet50Sample
{
    public partial class MainForm : Form
    {
        private static readonly object _syncRoot = new object();
        private readonly System.Windows.Controls.RichTextBox _wpfRichTextBox;

        public MainForm()
        {
            InitializeComponent();

            var richTextBoxHost = new ElementHost
            {
                Dock = DockStyle.Fill,
            };

            _richTextBoxPanel.Controls.Add(richTextBoxHost);

            var wpfRichTextBox = new System.Windows.Controls.RichTextBox
            {
                Background = Brushes.Black,
                Foreground = Brushes.LightGray,
                FontFamily = new FontFamily("Cascadia Mono, Consolas, Courier New, monospace"),
                FontSize = 14,
                IsReadOnly = true,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                Margin = new Thickness(0),
            };

            richTextBoxHost.Child = wpfRichTextBox;
            _wpfRichTextBox = wpfRichTextBox;

            _clearToolStripButton.Click += Clear_OnClick;
            _logVerboseToolStripButton.Click += LogVerbose_OnClick;
            _logDebugToolStripButton.Click += LogDebug_OnClick;
            _logInformationToolStripButton.Click += LogInformation_OnClick;
            _logWarningToolStripButton.Click += LogWarning_OnClick;
            _logErrorToolStripButton.Click += LogError_OnClick;
            _logFatalToolStripButton.Click += LogFatal_OnClick;
            _logParallelForToolStripButton.Click += LogParallelFor_OnClick;
            _logTaskRunToolStripButton.Click += LogTaskRun_OnClick;

            SelfLog.Enable(message => Trace.WriteLine($"INTERNAL ERROR: {message}"));

            const string outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.RichTextBox(wpfRichTextBox, outputTemplate: outputTemplate, syncRoot: _syncRoot)
                .Enrich.WithThreadId()
                .CreateLogger();
        }

        private void MainForm_Load(object sender, EventArgs e)
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

        private void Clear_OnClick(object sender, EventArgs e)
        {
            lock (_syncRoot)
            {
                _wpfRichTextBox.Document.Blocks.Clear();
            }
        }

        private void LogVerbose_OnClick(object sender, EventArgs e)
        {
            Log.Verbose("Hello! Now => {Now}", DateTime.Now);
        }

        private void LogDebug_OnClick(object sender, EventArgs e)
        {
            Log.Debug("Hello! Now => {Now}", DateTime.Now);
        }

        private void LogInformation_OnClick(object sender, EventArgs e)
        {
            Log.Information("Hello! Now => {Now}", DateTime.Now);
        }

        private void LogWarning_OnClick(object sender, EventArgs e)
        {
            Log.Warning("Hello! Now => {Now}", DateTime.Now);
        }

        private void LogError_OnClick(object sender, EventArgs e)
        {
            Log.Error("Hello! Now => {Now}", DateTime.Now);
        }

        private void LogFatal_OnClick(object sender, EventArgs e)
        {
            Log.Fatal("Hello! Now => {Now}", DateTime.Now);
        }

        private void LogParallelFor_OnClick(object sender, EventArgs e)
        {
            Parallel.For(1, 101, stepNumber =>
            {
                var stepName = $"Step {stepNumber.ToString("000", CultureInfo.InvariantCulture)}";

                Log.Verbose("Hello from Parallel.For({StepName}) Verbose", stepName);
                Log.Debug("Hello from Parallel.For({StepName}) Debug", stepName);
                Log.Information("Hello from Parallel.For({StepName}) Information", stepName);
                Log.Warning("Hello from Parallel.For({StepName}) Warning", stepName);
                Log.Error("Hello from Parallel.For({StepName}) Error", stepName);
                Log.Fatal("Hello from Parallel.For({StepName}) Fatal", stepName);
            });
        }

        private async void LogTaskRun_OnClick(object sender, EventArgs e)
        {
            var tasks = new System.Collections.Generic.List<Task>();

            for (var i = 1; i <= 100; i++)
            {
                var stepNumber = i;
                var task = Task.Run(() =>
                {
                    var stepName = $"Step {stepNumber.ToString("000", CultureInfo.InvariantCulture)}";

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
