﻿// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using System;

namespace PPlus.Controls
{
    internal static class ScreenBufferSelect
    {
        public static void WriteFilterSelect<T>(this ScreenBuffer screenBuffer, SelectOptions<T> options, string input, EmacsBuffer filter)
        {
            if (options.FilterType == FilterMode.StartsWith)
            {
                if (input.StartsWith(filter.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    screenBuffer.WriteAnswer(options, input.Substring(0, filter.Length));
                    screenBuffer.SaveCursor();
                    screenBuffer.WriteSugestion(options, input.Substring(filter.Length));
                }
                else
                {
                    screenBuffer.WriteEmptyFilter(options, filter.ToBackward());
                    screenBuffer.SaveCursor();
                    screenBuffer.WriteEmptyFilter(options, filter.ToForward());
                }
            }
            else
            {
                var parts = input.ToUpperInvariant().Split(filter.ToString().ToUpperInvariant());
                if (parts.Length == 1 && string.IsNullOrEmpty(parts[0]))
                {
                    screenBuffer.WriteEmptyFilter(options, filter.ToString());
                    screenBuffer.SaveCursor();
                    return;
                }
                var first = true;
                var pos = 0;
                foreach (var itempart in parts)
                {
                    pos++;
                    screenBuffer.WriteSugestion(options, itempart);
                    if (pos < parts.Length)
                    {
                        screenBuffer.WriteAnswer(options, filter.ToString());
                    }
                    if (first)
                    {
                        first = false;
                        screenBuffer.SaveCursor();
                    }
                }
            }
        }

        public static void WriteLineNotSelectorDisabled(this ScreenBuffer screenBuffer, BaseOptions options, string message)
        {
            screenBuffer.NewLine();
            screenBuffer.AddBuffer(' ', Style.Default, true);
            screenBuffer.AddBuffer($" {message}", options.OptStyleSchema.Disabled(),false, false);
        }

        public static void WriteLineSelector(this ScreenBuffer screenBuffer, BaseOptions options, string message)
        {
            screenBuffer.NewLine();
            screenBuffer.AddBuffer($"{options.Symbol(SymbolType.Selector)} {message}", options.OptStyleSchema.Selected(), false);
        }

        public static void WriteLineNotSelector(this ScreenBuffer screenBuffer, BaseOptions options, string message)
        {
            screenBuffer.NewLine();
            screenBuffer.AddBuffer(' ', Style.Default, true);
            screenBuffer.AddBuffer($" {message}", options.OptStyleSchema.UnSelected(), false,false);
        }


        public static void WriteEmptyFilter(this ScreenBuffer screenBuffer, BaseOptions options, string filter)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                screenBuffer.AddBuffer(filter, options.OptStyleSchema.Error(), true);
            }
        }

        public static void WriteLineDescriptionSelect<T>(this ScreenBuffer screenBuffer, SelectOptions<T> options, ItemSelect<T> input)
        {
            var result = options.OptDescription;
            if (options.DescriptionSelector != null)
            {
                result = options.DescriptionSelector.Invoke(input.Value);
            }
            if (!string.IsNullOrEmpty(result))
            {
                screenBuffer.NewLine();
                screenBuffer.AddBuffer(result, options.OptStyleSchema.Description());
            }
        }

        public static void WriteLineTooltipsSelect<T>(this ScreenBuffer screenBuffer, SelectOptions<T> options)
        {
            if (options.OptShowTooltip)
            {
                var tp = options.OptToolTip;
                var swm = false;
                if (string.IsNullOrEmpty(tp))
                {
                    tp = DefaultToolTipSelect(options);
                    swm = true;
                }
                if (!string.IsNullOrEmpty(tp))
                {
                    screenBuffer.NewLine();
                    screenBuffer.AddBuffer(tp, options.OptStyleSchema.Tooltips(), swm);
                }
            }
        }

        private static string DefaultToolTipSelect<T>(SelectOptions<T> options)
        {
            if (options.OptEnabledAbortKey)
            {
                return string.Format("{0}, {1}, {2}\n{3}, {4}",
                    string.Format(Messages.TooltipToggle, options.Config.TooltipKeyPress),
                    string.Format(Messages.TooltipCancelEsc, options.Config.AbortKeyPress),
                    Messages.SelectFisnishEnter,
                    Messages.TooltipPages,
                    Messages.TooltipSelectFilter);
            }
            else
            {
                return string.Format("{0}, {1}\n{2}, {3}",
                    string.Format(Messages.TooltipToggle, options.Config.TooltipKeyPress),
                    Messages.SelectFisnishEnter,
                    Messages.TooltipPages,
                    Messages.TooltipSelectFilter);
            }

        }
    }
}
