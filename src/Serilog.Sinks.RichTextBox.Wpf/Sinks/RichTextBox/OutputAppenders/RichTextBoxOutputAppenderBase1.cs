namespace Serilog.Sinks.RichTextBox.Output
{
    public abstract class RichTextBoxOutputAppenderBase<TArgs> : RichTextBoxOutputAppenderBase
        where TArgs : RichTextBoxOutputAppenderArgs
    {

        protected TArgs Args { get; }

        public RichTextBoxOutputAppenderBase(TArgs args)
        {
            this.Args = args;
        }

    }

}
