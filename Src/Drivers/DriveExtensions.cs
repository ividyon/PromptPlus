﻿// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls;
using PPlus.Drivers;
using PPlus.Drivers.Ansi;
using PPlus.Drivers.Colors;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace PPlus
{
    /// <summary>
    /// Contains controls, methods, properties and extensions for <see cref="PromptPlus"/>.
    /// </summary>
    public static partial class PromptPlus
    {
        private static bool RunningConsoleMemory = false;
        private static IConsoleControl _consoledrive;
        private static Config _configcontrols;
        private static StyleSchema _styleschema;
        private static readonly object lockrecord = new();
        private const ConsoleColor DefaultForegroundColor = ConsoleColor.Gray;
        private const ConsoleColor DefaultBackgroundColor = ConsoleColor.Black;

        static PromptPlus()
        {
            Reset();
        }

        /// <summary>
        /// Reset all config and properties to default values
        /// </summary>
        /// <param name="culture">Default culture for console and threads</param>
        public static void Reset()
        {
            var (SupportsAnsi, _) = AnsiDetector.Detect();
            var termdetect = TerminalDetector.Detect();
            var colordetect = ColorSystemDetector.Detect(SupportsAnsi);
            var unicodesupported = false;
            if (IsRunningInUnitTest)
            {
                RunningConsoleMemory = true;
                var drvprofile = new ProfileDriveMemory(DefaultForegroundColor, DefaultBackgroundColor, true, true, true, ColorSystem.TrueColor, Overflow.None, 0, 0);
                _consoledrive = new ConsoleDriveMemory(drvprofile);
            }
            else
            {
                if (System.Console.OutputEncoding.CodePage == 850)
                {
                    System.Console.OutputEncoding = Encoding.GetEncoding(65001);
                }
                if (System.Console.OutputEncoding.CodePage == 65001 || System.Console.OutputEncoding.CodePage == 1200)
                {
                    unicodesupported = true;
                }
                else if (System.Console.OutputEncoding.Equals(Encoding.Unicode))
                {
                    unicodesupported = true;
                }
                else if (System.Console.OutputEncoding.Equals(Encoding.BigEndianUnicode))
                {
                    unicodesupported = true;
                }
                else if (System.Console.OutputEncoding.Equals(Encoding.UTF32))
                {
                    unicodesupported = true;
                }
                var drvprofile = new ProfileDriveConsole(DefaultForegroundColor, DefaultBackgroundColor, termdetect, unicodesupported, SupportsAnsi, colordetect, Overflow.None, 0, 0);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    _consoledrive = new ConsoleDriveWindows(drvprofile);
                }
                else
                {
                    _consoledrive = new ConsoleDriveLinux(drvprofile);
                }
            }
            _consoledrive.CursorVisible = true;
            _configcontrols = new();
            _styleschema = new();
        }

        /// <summary>
        /// InputBuffer to test console
        /// </summary>
        internal static void InputBuffer(string value)
        {
            if (_consoledrive.Provider != "Memory")
            {
                return;
            }
            ((ConsoleDriveMemory)_consoledrive).InputBuffer(value);
        }

        /// <summary>
        /// InputBuffer to test console
        /// </summary>
        internal static void InputBuffer(ConsoleKeyInfo value)
        {
            if (_consoledrive.Provider != "Memory")
            {
                return;
            }
            ((ConsoleDriveMemory)_consoledrive).InputBuffer(value);
        }

        /// <summary>
        /// Gets the current Console drive.
        /// </summary>
        public static IConsoleBase Console => _consoledrive;


        /// <summary>
        /// <para>Overwrite current console with new console profile.</para>
        /// <br>After overwrite the new console the screeen is clear</br>
        /// <br>and all Style-Schema are updated with backgoundcolor console</br>
        /// </summary>
        /// <param name="config">Action with <seealso cref="ProfileSetup"/> to configuration</param>
        public static void Setup(Action<ProfileSetup> config)
        {
            Reset();
            Profile(config);
        }


        /// <summary>
        /// <para>Overwrite current console with new console profile.</para>
        /// <br>After overwrite the new console the screeen is clear</br>
        /// <br>and all Style-Schema are updated with backgoundcolor console</br>
        /// </summary>
        /// <param name="config">Action with <seealso cref="ProfileSetup"/> to configuration</param>
        internal static void Setup(this IConsoleBase _, Action<ProfileSetup> config)
        {
            Reset();
            Profile(config);
        }

        /// <summary>
        /// Get global properties for all controls.
        /// </summary>
        public static Config Config => _configcontrols;

        /// <summary>
        /// Get global Style-Schema for all controls.
        /// </summary>
        public static StyleSchema StyleSchema => _styleschema;

        /// <summary>
        /// Write a text to output console.
        /// </summary>
        /// <param name="value">Exception to write</param>
        /// <param name="style">Style of text</param>
        /// <param name="clearrestofline">Clear rest of line after write</param>
        /// <returns>Number of lines write on console</returns>
        public static int Write(Exception value, Style? style = null, bool clearrestofline = false)
        {
            return Console.Write(value.ToString(), style, clearrestofline);
        }

        /// <summary>
        /// Write a text to output console.
        /// </summary>
        /// <param name="value">text to write</param>
        /// <param name="style">Style of text</param>
        /// <param name="clearrestofline">Clear rest of line after write</param>
        /// <returns>Number of lines write on console</returns>
        public static int Write(string value, Style? style = null, bool clearrestofline = false)
        { 
            return Console.Write(value,style,clearrestofline);
        }

        /// <summary>
        /// Write a text to output console with line terminator.
        /// </summary>
        /// <param name="value">Exception to write</param>
        /// <param name="style">Style of text</param>
        /// <param name="clearrestofline">Clear rest of line after write</param>
        /// <returns>Number of lines write on console</returns>
        public static int WriteLine(Exception value, Style? style = null, bool clearrestofline = true)
        {
            return Console.WriteLine(value.ToString(), style, clearrestofline);
        }

        /// <summary>
        /// Write a text to output console with line terminator.
        /// </summary>
        /// <param name="value">text to write</param>
        /// <param name="style">Style of text</param>
        /// <param name="clearrestofline">Clear rest of line after write</param>
        /// <returns>Number of lines write on console</returns>
        public static int WriteLine(string? value = null, Style? style = null, bool clearrestofline = true)
        { 
            return Console.WriteLine(value,style,clearrestofline);
        }

        /// <summary>
        /// Get provider mode.
        /// </summary>
        public static string Provider => Console.Provider;

        /// <summary>
        /// Get Terminal mode.
        /// </summary>
        public static bool IsTerminal => Console.IsTerminal;


        /// <summary>
        /// Get Unicode Supported.
        /// </summary>
        public static bool IsUnicodeSupported => Console.IsUnicodeSupported;

        /// <summary>
        /// Get SupportsAnsi mode.
        /// </summary>
        public static bool SupportsAnsi => Console.SupportsAnsi;

        /// <summary>
        /// Get Color capacity.<see cref="ColorSystem"/>
        /// </summary>
        public static ColorSystem ColorDepth => Console.ColorDepth;

        /// <summary>
        /// Get default <see cref="Style"/> console.
        /// </summary>
        public static Style DefaultStyle => Console.DefaultStyle;

        /// <summary>
        /// Get screen margin left
        /// </summary>
        public static byte PadLeft => Console.PadLeft;

        /// <summary>
        /// Get screen margin right
        /// </summary>
        public static byte PadRight => Console.PadRight;

        /// <summary>
        /// Gets the width of the buffer area.
        /// </summary>
        public static int BufferWidth => Console.BufferWidth;

        /// <summary>
        /// Gets the height of the buffer area.
        /// </summary>
        public static int BufferHeight => Console.BufferHeight;



        /// <summary>
        /// Get/Set Foreground console with color.
        /// </summary>
        public static ConsoleColor ForegroundColor 
        {
            get { return Console.ForegroundColor; }
            set { Console.ForegroundColor = value; } 
        }

        /// <summary>
        /// Get/set BackgroundColor console with color.
        /// </summary>
        public static ConsoleColor BackgroundColor 
        {
            get { return Console.BackgroundColor; }
            set { Console.BackgroundColor = value; }
        }

        /// <summary>
        /// Get write Overflow Strategy.
        /// </summary>
        public static Overflow OverflowStrategy => Console.OverflowStrategy;

        /// <summary>
        /// Reset colors to default values.
        /// </summary>
        public static void ResetColor()
        {
            Console.ResetColor();
        }

        /// <summary>
        /// Moves the cursor relative to the current position.
        /// </summary>
        /// <param name="direction">The direction to move the cursor.</param>
        /// <param name="steps">The number of steps to move the cursor.</param>
        public static void MoveCursor(CursorDirection direction, int steps)
        {
            Console.MoveCursor(direction, steps);
        }

        /// <summary>
        /// Sets the position of the cursor.
        /// </summary>
        /// <param name="left">The column position of the cursor. Columns are numbered from left to right starting at 0.</param>
        /// <param name="top">The row position of the cursor. Rows are numbered from top to bottom starting at 0.</param>
        public static void SetCursorPosition(int left, int top)
        {
            Console.SetCursorPosition(left, top);
        }

        /// <summary>
        ///  Obtains the next character or function key pressed by the user.
        /// </summary>
        /// <param name="intercept">Determines whether to display the pressed key in the console window. true to not display the pressed key; otherwise, false.</param>
        /// <returns>
        ///     <br>An oject that describes the System.ConsoleKey constant and Unicode character,</br>
        ///     <br>if any, that correspond to the pressed console key. The System.ConsoleKeyInfo</br>
        ///     <br>t also describes, in a bitwise combination of System.ConsoleModifiers values,</br>
        ///     <br>er one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously</br>
        ///     <br>with the console key.</br>
        /// </returns>
        public static ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            return Console.ReadKey(intercept);
        }

        /// <summary>
        ///  Obtains the next character or function key pressed by the user.
        /// </summary>
        /// <param name="intercept">Determines whether to display the pressed key in the console window. true to not display the pressed key; otherwise, false.</param>
        /// <returns>
        ///     <br>An oject that describes the System.ConsoleKey constant and Unicode character,</br>
        ///     <br>if any, that correspond to the pressed console key. The System.ConsoleKeyInfo</br>
        ///     <br>t also describes, in a bitwise combination of System.ConsoleModifiers values,</br>
        ///     <br>er one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously</br>
        ///     <br>with the console key.</br>
        /// </returns>
        public static string? ReadLine()
        {
            return Console.ReadLine();
        }

        /// <summary>
        /// Wait Keypress from standard input stream
        /// </summary>
        /// <param name="intercept">Determines whether to display the pressed key in the console window. true to not display the pressed key; otherwise, false.</param>
        /// <param name="cancellationToken"> The token to monitor for cancellation requests.</param> 
        /// <returns>
        ///     <br>An oject that describes the System.ConsoleKey constant and Unicode character,</br>
        ///     <br>if any, that correspond to the pressed console key. The System.ConsoleKeyInfo</br>
        ///     <br>t also describes, in a bitwise combination of System.ConsoleModifiers values,</br>
        ///     <br>er one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously</br>
        ///     <br>with the console key.</br> 
        ///</returns>
        public static ConsoleKeyInfo? WaitKeypress(bool intercept, CancellationToken? cancellationToken)
        { 
            return Console.WaitKeypress(intercept, cancellationToken);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the cursor is visible.
        /// </summary>
        public static bool CursorVisible 
        {
            get { return Console.CursorVisible; }
            set { Console.CursorVisible = value; } 
        }

        /// <summary>
        /// Gets the column position of the cursor within the buffer area.
        /// </summary>
        public static int CursorLeft => Console.CursorLeft;

        /// <summary>
        /// Gets the row position of the cursor within the buffer area.
        /// </summary>
        public static int CursorTop => Console.CursorTop;

        /// <summary>
        /// Gets a value indicating whether a key press is available in the input stream.
        /// </summary>
        public static bool KeyAvailable => Console.KeyAvailable;

        /// <summary>
        ///  Gets a value that indicates whether input has been redirected from the standard input stream.
        /// </summary>
        public static bool IsInputRedirected => Console.IsInputRedirected;


        /// <summary>
        /// Get/set an encoding for standard input stream.
        /// </summary>
        public static Encoding InputEncoding => Console.InputEncoding;

        /// <summary>
        /// Get standard input stream.
        /// </summary>
        public static TextReader In => Console.In;

        /// <summary>
        /// set standard input stream.
        /// </summary>
        /// <param name="value">A stream that is the new standard input.</param>
        public static void SetIn(TextReader value)
        {
            Console.SetIn(value);
        }

        /// <summary>
        /// Get output CodePage.
        /// </summary>
        public static int CodePage => Console.CodePage;


        /// <summary>
        ///  Gets a value that indicates whether output has been redirected from the standard output stream.
        /// </summary>     
        public static bool IsOutputRedirected => Console.IsOutputRedirected;

        /// <summary>
        ///  Gets a value that indicates whether error has been redirected from the standard error stream.
        /// </summary>
        public static bool IsErrorRedirected => Console.IsErrorRedirected;

        /// <summary>
        /// Get/set an encoding for standard output stream.
        /// </summary>
        public static Encoding OutputEncoding 
        {
            get { return Console.OutputEncoding; }
            set { Console.OutputEncoding = value; } 
        }

        /// <summary>
        /// Get standard output stream.
        /// </summary>
        public static TextWriter Out => Console.Out;

        /// <summary>
        /// Get standard error stream.
        /// </summary>
        public static TextWriter Error => Console.Error;


        /// <summary>
        /// set standard output stream.
        /// </summary>
        /// <param name="value">A stream that is the new standard output.</param>
        public static void SetOut(TextWriter value)
        { 
            Console.SetOut(value); 
        }


        /// <summary>
        /// set standard error stream.
        /// </summary>
        /// <param name="value">A stream that is the new standard error.</param>
        public static void SetError(TextWriter value)
        { 
            Console.SetError(value);
        }


        /// <summary>
        /// <para>Clears the console buffer and corresponding console window of display information.</para>
        /// <br>Move cursor fom top console.</br>
        /// </summary>
        public static void Clear()
        { 
            Console.Clear();
        }

        /// <summary>
        /// Plays the sound of a beep through the console speaker.
        /// </summary>
        public static void Beep()
        { 
            Console.Beep();
        }

        /// <summary>
        ///  Clear line
        /// </summary>
        /// <param name="row">The row to clear</param>
        /// <param name="style">The style color to clear.</param>
        public static void ClearLine(int? row = null, Style? style = null) 
        { 
            ClearLine(Console,row, style);  
        }

        /// <summary>
        ///  Clear line
        /// </summary>
        /// <param name="row">The row to clear</param>
        /// <param name="style">The style color to clear.</param>
        public static void ClearLine(this IConsoleBase consoleBase, int? row = null, Style? style = null)
        {
            style ??= consoleBase.DefaultStyle;
            row ??= consoleBase.CursorTop;
            consoleBase.SetCursorPosition(0, row.Value);
            if (consoleBase.SupportsAnsi)
            {
                consoleBase.Write("", style.Value, true);
            }
            else
            {
                var aux = new string(' ', consoleBase.BufferWidth);
                consoleBase.Write(aux, style.Value.Overflow(Overflow.Crop),true);
                consoleBase.SetCursorPosition(0, row.Value);
            }
        }

        /// <summary>
        ///  Clear rest of current line 
        /// </summary>
        /// <param name="style">The style color to clear.</param>
        public static void ClearRestOfLine(Style? style = null)
        {
            ClearRestOfLine(Console, style);
        }

        /// <summary>
        ///  Clear rest of current line 
        /// </summary>
        /// <param name="style">The style color to clear.</param>
        public static void ClearRestOfLine(this IConsoleBase consoleBase, Style? style = null)
        {
            style ??= consoleBase.DefaultStyle;
            if (consoleBase.SupportsAnsi)
            {
                consoleBase.Write("", style.Value, true);
            }
            else
            {
                var row = consoleBase.CursorTop;
                var col = consoleBase.CursorLeft;
                var aux = new string(' ', consoleBase.BufferWidth - consoleBase.CursorLeft);
                consoleBase.Write(aux, style.Value.Overflow(Overflow.Crop), true);
                consoleBase.SetCursorPosition(col, row);
            }
        }

        private static void Profile(Action<ProfileSetup> config)
        {

            var param = new ProfileSetup
            {
                Culture = CultureInfo.CurrentCulture,
                ColorDepth = RunningConsoleMemory ? ColorSystem.TrueColor : _consoledrive.ColorDepth,
                IsTerminal = RunningConsoleMemory || _consoledrive.IsTerminal,
                IsUnicodeSupported = RunningConsoleMemory || _consoledrive.IsUnicodeSupported,
                SupportsAnsi = RunningConsoleMemory || _consoledrive.SupportsAnsi,
                OverflowStrategy = Overflow.None,
                PadLeft = 0,
                PadRight = 0,
                ForegroundColor = DefaultForegroundColor,
                BackgroundColor = DefaultBackgroundColor
            };
            config.Invoke(param);
            _configcontrols.DefaultCulture = param.Culture;

            if (RunningConsoleMemory)
            {
                var drvprofile = new ProfileDriveMemory(param.ForegroundColor, param.BackgroundColor, param.IsTerminal, param.IsUnicodeSupported, param.SupportsAnsi, param.ColorDepth, param.OverflowStrategy, param.PadLeft, param.PadRight);
                _consoledrive = new ConsoleDriveMemory(drvprofile);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var drvprofile = new ProfileDriveConsole(param.ForegroundColor, param.BackgroundColor, param.IsTerminal, param.IsUnicodeSupported, param.SupportsAnsi, param.ColorDepth, param.OverflowStrategy, param.PadLeft, param.PadRight);
                _consoledrive = new ConsoleDriveWindows(drvprofile);
            }
            else
            {
                var drvprofile = new ProfileDriveConsole(param.ForegroundColor, param.BackgroundColor, param.IsTerminal, param.IsUnicodeSupported, param.SupportsAnsi, param.ColorDepth, param.OverflowStrategy, param.PadLeft, param.PadRight);
                _consoledrive = new ConsoleDriveLinux(drvprofile);
            }
            _consoledrive.CursorVisible = true;
            _styleschema.UpdateBackgoundColor(param.BackgroundColor);
            _consoledrive.Clear();
        }

        private static bool IsRunningInUnitTest
        {
            get
            {
                if ((Environment.GetEnvironmentVariable("PromptPlusOverUnitTest") ?? string.Empty) == "true")
                {
                    return true;
                }
                return false;
            }
        }

        internal static string RecordOutput(Action action)
        {
            if (_consoledrive.Provider != "Memory")
            {
                return string.Empty;
            }
            lock (lockrecord)
            {
                _consoledrive.EnabledRecord = true;
                action.Invoke();
                _consoledrive.EnabledRecord = false;
                var aux = _consoledrive.RecordConsole();
                return aux;
            }
        }

        internal static string CaptureRecord(bool clearrecord)
        {
            if (_consoledrive.Provider != "Memory")
            {
                return string.Empty;
            }
            return _consoledrive.CaptureRecord(clearrecord);
        }
    }
}
