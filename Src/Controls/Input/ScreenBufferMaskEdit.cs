﻿// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using System.Globalization;
using System;

namespace PPlus.Controls
{
    internal static class ScreenBufferMaskEdit
    {
        public static void WriteNegativeAnswer(this ScreenBuffer screenBuffer, MaskEditOptions options, string input)
        {
            screenBuffer.AddBuffer(input, options.NegativeStyle, true);
        }

        public static void WritePositiveAnswer(this ScreenBuffer screenBuffer, MaskEditOptions options, string input)
        {
            screenBuffer.AddBuffer(input, options.PositiveStyle, true);
        }

        public static void WriteLineTooltipsMaskEdit(this ScreenBuffer screenBuffer, MaskEditOptions options, bool isInAutoCompleteMode)
        {
            if (options.OptShowTooltip)
            {
                var tp = options.OptToolTip;
                var smk = false;
                if (string.IsNullOrEmpty(tp))
                {
                    tp = DefaultToolTipMaskedit(options, isInAutoCompleteMode);
                    smk = true;
                }
                if (!string.IsNullOrEmpty(tp))
                {
                    screenBuffer.NewLine();
                    screenBuffer.AddBuffer(tp, options.OptStyleSchema.Tooltips(),smk);
                }
            }
        }

        public static void WriteLineDescriptionMaskEdit(this ScreenBuffer screenBuffer, MaskEditOptions options, string input, string tooltips)
        {
            var result = options.OptDescription;
            if (options.ChangeDescription != null)
            {
                result = options.ChangeDescription.Invoke(input);
            }
            var extradesc = GetExtraDescriptionMaskEdit(options, input, tooltips);
            if (!string.IsNullOrEmpty(result))
            {
                screenBuffer.NewLine();
                screenBuffer.AddBuffer(result, options.OptStyleSchema.Description());
                if (!string.IsNullOrEmpty(extradesc))
                {
                    screenBuffer.AddBuffer($", ", options.OptStyleSchema.Description(), true, false);
                    screenBuffer.AddBuffer($"Tip: {extradesc}", options.TypeTipStyle, true, false);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(extradesc))
                {
                    screenBuffer.NewLine();
                    screenBuffer.AddBuffer($"Tip: {extradesc}", options.TypeTipStyle, true);
                }
            }
        }

        private static string GetExtraDescriptionMaskEdit(MaskEditOptions value, string input, string tooltips)
        {
            if (!value.DescriptionWithInputType)
            {
                return string.Empty;
            }
            var wd = string.Empty;
            if (value.ShowDayWeek != FormatWeek.None && (value.Type == ControlMaskedType.DateOnly || value.Type == ControlMaskedType.DateTime))
            {
                if (DateTime.TryParse(input, value.CurrentCulture, DateTimeStyles.None, out var dtaux))
                {
                    var fmt = "ddd";
                    if (value.ShowDayWeek == FormatWeek.Long)
                    {
                        fmt = "dddd";
                    }
                    wd = $"({dtaux.ToString(fmt, value.CurrentCulture)})";
                }
            }
            if (!string.IsNullOrEmpty(wd) || !string.IsNullOrEmpty(tooltips))
            {
                var aux = $"{tooltips ?? string.Empty} {wd}".Trim();
                return aux;
            }
            return string.Empty;
        }

        private static string DefaultToolTipMaskedit(MaskEditOptions baseOptions, bool isInAutoCompleteMode)
        {
            if (baseOptions.HistoryEnabled)
            {
                if (baseOptions.ShowingHistory)
                {
                    return string.Format("{0}, {1}, {2}\n{3}, {4}",
                    string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                    Messages.TooltipHistoryEsc,
                    Messages.SelectFisnishEnter,
                    Messages.TooltipPages,
                    Messages.TooltipHistoryClear);
                }
                else
                {
                    if (isInAutoCompleteMode)
                    {
                        return string.Format("{0}, {1}\n{2}, {3}",
                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                        Messages.TooltipSugestionEsc,
                        string.Format(Messages.TooltipHistoryToggle, baseOptions.HistoryMinimumPrefixLength),
                        Messages.TooltipSugestionToggle);
                    }
                    else
                    {
                        if (baseOptions.SuggestionHandler != null)
                        {
                            if (baseOptions.OptEnabledAbortKey)
                            {
                                if (baseOptions.Type == ControlMaskedType.DateOnly || baseOptions.Type == ControlMaskedType.DateTime)
                                {
                                    var aux = $"'{baseOptions.CurrentCulture.DateTimeFormat.DateSeparator}'";
                                    if (baseOptions.Type == ControlMaskedType.DateTime)
                                    {
                                        aux = aux + " or " + $"'{baseOptions.CurrentCulture.DateTimeFormat.TimeSeparator}'";
                                    }
                                    return string.Format("{0}, {1}, {2},\n{3}, {4}, {5}",
                                    string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                    string.Format(Messages.TooltipHistoryToggle, baseOptions.HistoryMinimumPrefixLength),
                                    Messages.TooltipSugestionToggle,
                                    Messages.MaskEditErase,
                                    string.Format(Messages.MaskEditJump, aux));
                                }
                                else if (baseOptions.Type == ControlMaskedType.Number || baseOptions.Type == ControlMaskedType.Currency)
                                {
                                    string aux;
                                    if (baseOptions.Type == ControlMaskedType.Number)
                                    {
                                        aux = $"'{baseOptions.CurrentCulture.NumberFormat.NumberDecimalSeparator}'";
                                    }
                                    else
                                    {
                                        aux = $"'{baseOptions.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}'";
                                    }
                                    return string.Format("{0}, {1}, {2},\n{3}, {4}, {5}",
                                    string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                    string.Format(Messages.TooltipHistoryToggle, baseOptions.HistoryMinimumPrefixLength),
                                    Messages.TooltipSugestionToggle,
                                    Messages.MaskEditErase,
                                    string.Format(Messages.MaskEditJump, aux));
                                }
                                else
                                {
                                    return string.Format("{0}, {1}\n{2}, {3}, {4}",
                                    string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                    string.Format(Messages.TooltipHistoryToggle, baseOptions.HistoryMinimumPrefixLength),
                                    Messages.TooltipSugestionToggle,
                                    Messages.MaskEditErase);
                                }
                            }
                            else
                            {
                                if (baseOptions.Type == ControlMaskedType.DateOnly || baseOptions.Type == ControlMaskedType.DateTime)
                                {
                                    var aux = $"'{baseOptions.CurrentCulture.DateTimeFormat.DateSeparator}'";
                                    if (baseOptions.Type == ControlMaskedType.DateTime)
                                    {
                                        aux = aux + " or " + $"'{baseOptions.CurrentCulture.DateTimeFormat.TimeSeparator}'";
                                    }
                                    return string.Format("{0}, {1}, {2},\n{3}, {4}",
                                    string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipHistoryToggle, baseOptions.HistoryMinimumPrefixLength),
                                    Messages.TooltipSugestionToggle,
                                    Messages.MaskEditErase,
                                    string.Format(Messages.MaskEditJump, aux));
                                }
                                else if (baseOptions.Type == ControlMaskedType.Number || baseOptions.Type == ControlMaskedType.Currency)
                                {
                                    string aux;
                                    if (baseOptions.Type == ControlMaskedType.Number)
                                    {
                                        aux = $"'{baseOptions.CurrentCulture.NumberFormat.NumberDecimalSeparator}'";
                                    }
                                    else
                                    {
                                        aux = $"'{baseOptions.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}'";
                                    }
                                    return string.Format("{0}, {1}, {2},\n{3}, {4}",
                                    string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipHistoryToggle, baseOptions.HistoryMinimumPrefixLength),
                                    Messages.TooltipSugestionToggle,
                                    Messages.MaskEditErase,
                                    string.Format(Messages.MaskEditJump, aux));
                                }
                                else
                                {
                                    return string.Format("{0}, {1},\n{2}, {3}",
                                    string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipHistoryToggle, baseOptions.HistoryMinimumPrefixLength),
                                    Messages.TooltipSugestionToggle,
                                    Messages.MaskEditErase);
                                }
                            }
                        }
                        else
                        {
                            if (baseOptions.OptEnabledAbortKey)
                            {
                                if (baseOptions.Type == ControlMaskedType.DateOnly || baseOptions.Type == ControlMaskedType.DateTime)
                                {
                                    var aux = $"'{baseOptions.CurrentCulture.DateTimeFormat.DateSeparator}'";
                                    if (baseOptions.Type == ControlMaskedType.DateTime)
                                    {
                                        aux = aux + " or " + $"'{baseOptions.CurrentCulture.DateTimeFormat.TimeSeparator}'";
                                    }
                                    return string.Format("{0}, {1},\n{2}, {3}, {4}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                        string.Format(Messages.TooltipHistoryToggle, baseOptions.HistoryMinimumPrefixLength),
                                        Messages.MaskEditErase,
                                        string.Format(Messages.MaskEditJump, aux));
                                }
                                else if (baseOptions.Type == ControlMaskedType.Number || baseOptions.Type == ControlMaskedType.Currency)
                                {
                                    string aux;
                                    if (baseOptions.Type == ControlMaskedType.Number)
                                    {
                                        aux = $"'{baseOptions.CurrentCulture.NumberFormat.NumberDecimalSeparator}'";
                                    }
                                    else
                                    {
                                        aux = $"'{baseOptions.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}'";
                                    }
                                    return string.Format("{0}, {1},\n{2}, {3}, {4}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                        string.Format(Messages.TooltipHistoryToggle, baseOptions.HistoryMinimumPrefixLength),
                                        Messages.MaskEditErase,
                                        string.Format(Messages.MaskEditJump, aux));
                                }
                                else
                                {
                                    return string.Format("{0}, {1},\n{2}, {3}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                        string.Format(Messages.TooltipHistoryToggle, baseOptions.HistoryMinimumPrefixLength),
                                        Messages.MaskEditErase);
                                }
                            }
                            else
                            {
                                if (baseOptions.Type == ControlMaskedType.DateOnly || baseOptions.Type == ControlMaskedType.DateTime)
                                {
                                    var aux = $"'{baseOptions.CurrentCulture.DateTimeFormat.DateSeparator}'";
                                    if (baseOptions.Type == ControlMaskedType.DateTime)
                                    {
                                        aux = aux + " or " + $"'{baseOptions.CurrentCulture.DateTimeFormat.TimeSeparator}'";
                                    }
                                    return string.Format("{0}, {1}\n{2}, {3}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        string.Format(Messages.TooltipHistoryToggle, baseOptions.HistoryMinimumPrefixLength),
                                        Messages.MaskEditErase,
                                        string.Format(Messages.MaskEditJump, aux));
                                }
                                else if (baseOptions.Type == ControlMaskedType.Number || baseOptions.Type == ControlMaskedType.Currency)
                                {
                                    string aux;
                                    if (baseOptions.Type == ControlMaskedType.Number)
                                    {
                                        aux = $"'{baseOptions.CurrentCulture.NumberFormat.NumberDecimalSeparator}'";
                                    }
                                    else
                                    {
                                        aux = $"'{baseOptions.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}'";
                                    }
                                    return string.Format("{0}, {1}\n{2}, {3}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        string.Format(Messages.TooltipHistoryToggle, baseOptions.HistoryMinimumPrefixLength),
                                        Messages.MaskEditErase,
                                        string.Format(Messages.MaskEditJump, aux));
                                }
                                else
                                {
                                    return string.Format("{0}, {1}, {2}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        string.Format(Messages.TooltipHistoryToggle, baseOptions.HistoryMinimumPrefixLength),
                                        Messages.MaskEditErase);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (isInAutoCompleteMode)
                {
                    return string.Format("{0}, {1}, {2}",
                    string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                    Messages.TooltipSugestionEsc,
                    Messages.TooltipSugestionToggle);
                }
                else
                {
                    if (baseOptions.SuggestionHandler != null)
                    {
                        if (baseOptions.OptEnabledAbortKey)
                        {
                            return string.Format("{0}, {1},\n{2}, {3}",
                            string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                            Messages.TooltipSugestionToggle,
                            string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                            Messages.MaskEditErase);
                        }
                        else
                        {
                            return string.Format("{0}, {1}, {2}",
                            string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                            Messages.TooltipSugestionToggle,
                            Messages.MaskEditErase);
                        }
                    }
                    else
                    {
                        if (baseOptions.OptEnabledAbortKey)
                        {
                            if (baseOptions.Type == ControlMaskedType.DateOnly || baseOptions.Type == ControlMaskedType.DateTime)
                            {
                                var aux = $"'{baseOptions.CurrentCulture.DateTimeFormat.DateSeparator}'";
                                if (baseOptions.Type == ControlMaskedType.DateTime)
                                {
                                    aux = aux + " or " + $"'{baseOptions.CurrentCulture.DateTimeFormat.TimeSeparator}'";
                                }
                                return string.Format("{0}, {1}\n{2}, {3}",
                                    string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                    Messages.MaskEditErase,
                                    string.Format(Messages.MaskEditJump, aux));
                            }
                            else if (baseOptions.Type == ControlMaskedType.Number || baseOptions.Type == ControlMaskedType.Currency)
                            {
                                string aux;
                                if (baseOptions.Type == ControlMaskedType.Number)
                                {
                                    aux = $"'{baseOptions.CurrentCulture.NumberFormat.NumberDecimalSeparator}'";
                                }
                                else
                                {
                                    aux = $"'{baseOptions.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}'";
                                }
                                return string.Format("{0}, {1}\n{2}, {3}",
                                    string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                    Messages.MaskEditErase,
                                    string.Format(Messages.MaskEditJump, aux));
                            }
                            else
                            {
                                return string.Format("{0}, {1}, {2}",
                                    string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                    Messages.MaskEditErase);
                            }
                        }
                        else
                        {
                            if (baseOptions.Type == ControlMaskedType.DateOnly || baseOptions.Type == ControlMaskedType.DateTime)
                            {
                                var aux = $"'{baseOptions.CurrentCulture.DateTimeFormat.DateSeparator}'";
                                if (baseOptions.Type == ControlMaskedType.DateTime)
                                {
                                    aux = aux + " or " + $"'{baseOptions.CurrentCulture.DateTimeFormat.TimeSeparator}'";
                                }
                                return string.Format("{0}, {1}, {2}",
                                    string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                    Messages.MaskEditErase,
                                    string.Format(Messages.MaskEditJump, aux));
                            }
                            else if (baseOptions.Type == ControlMaskedType.Number || baseOptions.Type == ControlMaskedType.Currency)
                            {
                                string aux;
                                if (baseOptions.Type == ControlMaskedType.Number)
                                {
                                    aux = $"'{baseOptions.CurrentCulture.NumberFormat.NumberDecimalSeparator}'";
                                }
                                else
                                {
                                    aux = $"'{baseOptions.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}'";
                                }
                                return string.Format("{0}, {1}, {2}",
                                    string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                    Messages.MaskEditErase,
                                    string.Format(Messages.MaskEditJump, aux));
                            }
                            else
                            {
                                return string.Format("{0}, {1}",
                                    string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                    Messages.MaskEditErase);
                            }
                        }
                    }
                }
            }
        }
    }
}
