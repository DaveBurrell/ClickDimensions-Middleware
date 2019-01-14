using System;
using System.Diagnostics;
namespace FormCapture
{
    public static class TraceProcessor
    {
        #region Variables

        static TraceSwitch _TraceSwitch = new TraceSwitch("TraceLevelSwitch", "Application Switch");

        #endregion Variables

        #region Methods

        public static void Error(string Message, bool AppendLineBreak = false)
        {
            try
            {
                Trace.WriteLineIf(TraceProcessor._TraceSwitch.TraceError, Message);
                if (AppendLineBreak) Trace.WriteLineIf(TraceProcessor._TraceSwitch.TraceError, "");
            }
            catch
            {
                //Non fatal, ignore.
            }
        }

        public static void Warning(string Message, bool AppendLineBreak = false)
        {
            try
            {
                Trace.WriteLineIf(TraceProcessor._TraceSwitch.TraceWarning, Message);
                if (AppendLineBreak) Trace.WriteLineIf(TraceProcessor._TraceSwitch.TraceWarning, "");
            }
            catch
            {
                //Non fatal, ignore.
            }
        }

        public static void Information(string Message, bool AppendLineBreak = false)
        {
            try
            {
                Trace.WriteLineIf(TraceProcessor._TraceSwitch.TraceInfo, Message);
                if (AppendLineBreak) Trace.WriteLineIf(TraceProcessor._TraceSwitch.TraceInfo, "");
            }
            catch
            {
                //Non fatal, ignore.
            }
        }

        public static void Verbose(string Message, bool AppendLineBreak = false)
        {
            try
            {
                Trace.WriteLineIf(TraceProcessor._TraceSwitch.TraceVerbose, Message);
                if (AppendLineBreak) Trace.WriteLineIf(TraceProcessor._TraceSwitch.TraceVerbose, "");
            }
            catch
            {
                //Non fatal, ignore.
            }
        }

        #endregion Methods
    }
}