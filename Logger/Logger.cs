using Android.Content;
using Android.OS;
using Mettarin.Android.Exceptions;
using Mettarin.Android.Views.Dialogs;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Log = Android.Util.Log;
using LogPriority = Android.Util.LogPriority;

namespace Mettarin.Android
{
    public interface ILogger
    {
        public string CurrentLogPath { get; }

        public string Path { get; }

        public void LogInfo(object info);

        public void LogWarn(object warn);

        public void LogError(Exception ex, bool silent = false);

        public Task LogErrorAsync(Exception ex, bool silent = false);
    }

    public class Logger : ILogger
    {
        private static readonly object _lock = new object();

        private readonly Context _context;

        public string Path { get; }

        public string CurrentLogPath
        {
            get
            {
                string logFileName = $"{DateTime.Now:yyyy-MM-dd}.txt";
                return $"{Path}/{logFileName}";
            }
        }

        public Logger(Context context)
        {
            _context = context;
            Path = $"{_context.ApplicationInfo.DataDir}/logs";
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
        }

        private void WriteLine(object message, LogPriority priority)
        {
            var trace = new StackTrace().GetFrame(2).GetMethod();
            var callerMethod = trace.DeclaringType.FullName;
            Log.WriteLine(priority, callerMethod, message.ToString());
            string logHeader = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss,fff} " +
                $"{priority.ToString().ToUpper()}  {callerMethod}";
            string logMessage = message.ToString();

            lock (_lock)
            {
                using StreamWriter logWritter = new StreamWriter(CurrentLogPath, true, Encoding.UTF8);
                logWritter.WriteLine(logHeader);
                logWritter.WriteLine($" {logMessage}");
                logWritter.WriteLine();
            }
        }

        public void LogInfo(object info)
        {
            WriteLine(info, LogPriority.Info);
        }

        public void LogWarn(object warn)
        {
            WriteLine(warn, LogPriority.Warn);
        }

        private SimpleMessageDialog LogErrorInternal(Exception ex, bool silent = false)
        {
            if (ex is LocalizedException localizedException)
            {
                WriteLine(localizedException.InnerException, LogPriority.Error);
            }

            else
            {
                WriteLine(ex, LogPriority.Error);
            }

            if (silent)
            {
                return null;
            }

            Vibrator vibrator = (Vibrator)_context.GetSystemService(Context.VibratorService);
            vibrator.Vibrate(VibrationEffect.CreateOneShot(100, VibrationEffect.DefaultAmplitude));

            var simpleMessageDialog = new SimpleMessageDialog(_context)
            {
                ButtonTextId = Resource.String.mettarin_ok,
                TitleTextId = Resource.String.mettarin_error
            };

            if (ex is LocalizedException localized)
            {
                string localizedString = _context.Resources.GetString(localized.ResourceId);

                if (localized.Args != null && localized.Args.Length > 0)
                {
                    localizedString = string.Format(localizedString, localized.Args);
                }

                simpleMessageDialog.Message = localizedString;
            }

            else
            {
                simpleMessageDialog.MessageTextId = Resource.String.mettarin_error_text;
            }

            return simpleMessageDialog;
        }

        public void LogError(Exception ex, bool silent = false)
        {
            LogErrorInternal(ex, silent)?.Show();
        }

        public async Task LogErrorAsync(Exception ex, bool silent = false)
        {
            await LogErrorInternal(ex, silent)?.ShowAndWaitForResult();
        }
    }
}
