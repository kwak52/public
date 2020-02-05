using System;
using log4net.Core;
using System.Drawing;
using Dsu.Common.Utilities.ExtensionMethods;
using System.Diagnostics;
using log4net;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace PLCConvertor
{
    public partial class FormPLCConverter
    {
        public static ILog Logger { get; set; }


        public async void DoAppend(LoggingEvent logEntry)
        {
            await this.DoAsync(() =>
            {
                try
                {
                    var msg = logEntry.MessageObject.ToString();
                    var level = logEntry.Level.Name;
                    var cr = GetLogLevelColor(level).Name;
                    var now = logEntry.TimeStamp.ToString("HH:mm:ss.fff");
                    Trace.WriteLine(msg);
                    /*
                     * multi-line message 처리
                     */
                    var lines = msg.SplitByLines().ToArray();
                    if (lines.Length > 0)
                    {
                        var msgLine = lines[0].Replace("{", "{{").Replace("}", "}}");
                        var fmtMsg = string.Format($"<color={cr}>{now} [{level}]: {msgLine}</color>");
                        ucPanelLog1.Items.Add(fmtMsg);

                        for (int i = 1; i < lines.Length; i++)
                        {
                            fmtMsg = $"<color={cr}>    {lines[i]}</color>";
                            ucPanelLog1.Items.Add(fmtMsg);
                        }

                        ucPanelLog1.SelectedIndex = ucPanelLog1.Items.Count - 1;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"Failed to append log: {ex}");
                }
            });

            Color GetLogLevelColor(string levelName)
            {
                switch (levelName)
                {
                    case "DEBUG": return Color.Orange;
                    case "INFO": return Color.Navy;
                    case "ERROR": return Color.Red;
                    case "WARN": return Color.Brown;
                    default: return Color.Black;
                }
            }

        }
    }
}