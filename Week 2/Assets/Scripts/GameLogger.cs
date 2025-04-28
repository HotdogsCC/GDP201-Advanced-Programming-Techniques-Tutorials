using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;

public class GameLogger : MonoBehaviour
{
    // enum defines the logging levels
    public enum LogLevel
    {
        Verbose = 0,
        Info = 1,
        Warning = 2,
        Error = 3
    }


    // Current log level threshold
    public static LogLevel LogThreshold { get; set; } = LogLevel.Info;

    // init for the log file path
    private static string _logFilePath;
    private static string LogFilePath
    {
        get
        {
            //checks if a file path already exists 
            if (string.IsNullOrEmpty(_logFilePath))
            {
                //Application.persistentdatapath is a safe place for the data to live
                //usually in appdata for windows
                _logFilePath = Path.Combine(Application.persistentDataPath, "game_logs.txt");
            }
            return _logFilePath;
        }
    }

    // general log method
    public static void Log(string message, LogLevel level = LogLevel.Info, string tag = "Null", System.Diagnostics.StackFrame stackFrame = null)
    {
        //if the warning is smaller than something we care about, dont do anything
        if (level < LogThreshold)
        {
            return;
        }

        //otherwise log the message

        // gets the line number and file the log was called from
        if(stackFrame == null)
        {
            stackFrame = new StackTrace(1, true).GetFrame(0);
        }
        string fileName = stackFrame.GetFileName();
        int lineNumber = stackFrame.GetFileLineNumber();
        string formattedMessage = $"{DateTime.Now} [{level}] [Tag:{tag}]: {message} (at {fileName}:{lineNumber})";

        //if this is running in the editor, send the log as a unity debug message

#if UNITY_EDITOR
        UnityEngine.Debug.Log(formattedMessage);
#else
        File.AppendAllText(LogFilePath, formattedMessage + Environment.NewLine);
#endif
    }

    // specific methods for log levels
    public static void LogVerbose(string message, string tag = "Null")
    {
        var stackFrame = new StackTrace(1, true).GetFrame(0);
        Log(message, LogLevel.Verbose, tag, stackFrame);
    }

    public static void LogInfo(string message, string tag = "Null")
    {
        var stackFrame = new StackTrace(1, true).GetFrame(0);
        Log(message, LogLevel.Info, tag, stackFrame);
    }

    public static void LogWarning(string message, string tag = "Null")
    {
        var stackFrame = new StackTrace(1, true).GetFrame(0);
        Log(message, LogLevel.Warning, tag, stackFrame);
    }

    public void LogError(string message, string tag = "Null")
    {
        var stackFrame = new StackTrace(1, true).GetFrame(0);
        Log(message, LogLevel.Error, tag, stackFrame);
    }

}
