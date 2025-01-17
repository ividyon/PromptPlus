﻿// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using System;
using System.Globalization;

namespace PPlus.Controls
{
    internal static class ScreenBufferMaskEditList
    {
        public static void WriteLineDescriptionMaskEditList(this ScreenBuffer screenBuffer, MaskEditListOptions options, string input, string tooltips)
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
                    screenBuffer.AddBuffer(", ", options.OptStyleSchema.Description(), true, false);
                    screenBuffer.AddBuffer($"Tip: {extradesc}", options.TypeTipStyle, true,false);
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

        public static void WriteLineTooltipsMaskEditList(this ScreenBuffer screenBuffer, MaskEditListOptions options, bool isInAutoCompleteMode, bool isEditMode, bool hasselected)
        {
            if (options.OptShowTooltip)
            {
                var tp = options.OptToolTip;
                var smk = false;
                if (string.IsNullOrEmpty(tp))
                {
                    tp = DefaultToolTipMaskEditList(options, isInAutoCompleteMode, isEditMode, hasselected);
                    smk = true;
                }
                if (!string.IsNullOrEmpty(tp))
                {
                    screenBuffer.NewLine();
                    screenBuffer.AddBuffer(tp, options.OptStyleSchema.Tooltips(), smk);
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

        private static string DefaultToolTipMaskEditList(MaskEditListOptions baseOptions, bool isInAutoCompleteMode, bool isEditMode, bool hasselected)
        {
            if (isInAutoCompleteMode)
            {
                return string.Format("{0}, {1}\n{2}, {3}",
                string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                Messages.TooltipSugestionEsc,
                Messages.TooltipSugestionToggle,
                Messages.TooltipEnterPressList);
            }
            else
            {
                if (baseOptions.SuggestionHandler != null)
                {
                    if (isEditMode)
                    {
                        if (baseOptions.Type == ControlMaskedType.DateOnly || baseOptions.Type == ControlMaskedType.DateTime)
                        {
                            var aux = $"'{baseOptions.CurrentCulture.DateTimeFormat.DateSeparator}'";
                            if (baseOptions.Type == ControlMaskedType.DateTime)
                            {
                                aux = aux + " or " + $"'{baseOptions.CurrentCulture.DateTimeFormat.TimeSeparator}'";
                            }
                            return string.Format("{0}, {1}, {2}, {3}\n{4}, {5}",
                                string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                Messages.TooltipAbortEdit,
                                Messages.TooltipSugestionToggle,
                                string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                Messages.TooltipEnterPressList,
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
                            return string.Format("{0}, {1}, {2}, {3}\n{4}, {5}",
                                string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                Messages.TooltipAbortEdit,
                                Messages.TooltipSugestionToggle,
                                string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                Messages.TooltipEnterPressList,
                                string.Format(Messages.MaskEditJump, aux));
                        }
                        else
                        {
                            return string.Format("{0}, {1}, {2}\n{3}, {4}",
                                string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                Messages.TooltipAbortEdit,
                                Messages.TooltipSugestionToggle,
                                string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                Messages.TooltipEnterPressList);
                        }
                    }
                    else
                    {
                        if (baseOptions.OptEnabledAbortKey)
                        {
                            if (hasselected)
                            {
                                if (baseOptions.Type == ControlMaskedType.DateOnly || baseOptions.Type == ControlMaskedType.DateTime)
                                {
                                    var aux = $"'{baseOptions.CurrentCulture.DateTimeFormat.DateSeparator}'";
                                    if (baseOptions.Type == ControlMaskedType.DateTime)
                                    {
                                        aux = aux + " or " + $"'{baseOptions.CurrentCulture.DateTimeFormat.TimeSeparator}'";
                                    }
                                    return string.Format("{0}, {1}, {2}, {3}, {4}\n{5}, {6}, {7}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                        Messages.TooltipSugestionToggle,
                                        string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                        string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                        Messages.TooltipPages,
                                        string.Format(Messages.MaskEditJump, aux),
                                        Messages.TooltipEnterPressList);
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
                                    return string.Format("{0}, {1}, {2}, {3}, {4}\n{5}, {6}, {7}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                        Messages.TooltipSugestionToggle,
                                        string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                        string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                        Messages.TooltipPages,
                                        string.Format(Messages.MaskEditJump, aux),
                                        Messages.TooltipEnterPressList);
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
                                        return string.Format("{0}, {1}, {2}, {3}, {4}\n{5}, {6}, {7}",
                                            string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                            string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                            Messages.TooltipSugestionToggle,
                                            string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                            string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                            Messages.TooltipPages,
                                            string.Format(Messages.MaskEditJump, aux),
                                            Messages.TooltipEnterPressList);
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
                                        return string.Format("{0}, {1}, {2}, {3}, {4}\n{5}, {6}, {7}",
                                            string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                            string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                            Messages.TooltipSugestionToggle,
                                            string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                            string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                            Messages.TooltipPages,
                                            string.Format(Messages.MaskEditJump, aux),
                                            Messages.TooltipEnterPressList);
                                    }
                                    else
                                    {
                                        return string.Format("{0}, {1}, {2}, {3}, {4}\n{5}, {6}",
                                            string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                            string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                            Messages.TooltipSugestionToggle,
                                            string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                            string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                            Messages.TooltipPages,
                                            Messages.TooltipEnterPressList);
                                    }
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
                                    return string.Format("{0}, {1}, {2},\n{3}, {4}, {5}",
                                         string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                         string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                         Messages.TooltipSugestionToggle,
                                         Messages.TooltipPages,
                                         string.Format(Messages.MaskEditJump, aux),
                                         Messages.TooltipEnterPressList);
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
                                         Messages.TooltipSugestionToggle,
                                         Messages.TooltipPages,
                                         string.Format(Messages.MaskEditJump, aux),
                                         Messages.TooltipEnterPressList);
                                }
                                else
                                {
                                    return string.Format("{0}, {1}, {2},\n{3}, {4}",
                                         string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                         string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                         Messages.TooltipSugestionToggle,
                                         Messages.TooltipPages,
                                         Messages.TooltipEnterPressList);
                                }
                            }
                        }
                        else
                        {
                            if (hasselected)
                            {
                                if (baseOptions.Type == ControlMaskedType.DateOnly || baseOptions.Type == ControlMaskedType.DateTime)
                                {
                                    var aux = $"'{baseOptions.CurrentCulture.DateTimeFormat.DateSeparator}'";
                                    if (baseOptions.Type == ControlMaskedType.DateTime)
                                    {
                                        aux = aux + " or " + $"'{baseOptions.CurrentCulture.DateTimeFormat.TimeSeparator}'";
                                    }
                                    return string.Format("{0}, {1}, {2}, {3}\n{4}, {5}, {6}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        Messages.TooltipSugestionToggle,
                                        string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                        string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                        Messages.TooltipPages,
                                        string.Format(Messages.MaskEditJump, aux),
                                        Messages.TooltipEnterPressList);
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
                                    return string.Format("{0}, {1}, {2}, {3}\n{4}, {5}, {6}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        Messages.TooltipSugestionToggle,
                                        string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                        string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                        Messages.TooltipPages,
                                        string.Format(Messages.MaskEditJump, aux),
                                        Messages.TooltipEnterPressList);
                                }
                                else
                                {
                                    return string.Format("{0}, {1}, {2}, {3}\n{4}, {5}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        Messages.TooltipSugestionToggle,
                                        string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                        string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                        Messages.TooltipPages,
                                        Messages.TooltipEnterPressList);
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
                                    return string.Format("{0}, {1}\n{2}, {3}, {4}",
                                         string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                         Messages.TooltipSugestionToggle,
                                         Messages.TooltipPages,
                                         string.Format(Messages.MaskEditJump, aux),
                                         Messages.TooltipEnterPressList);
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
                                    return string.Format("{0}, {1}\n{2}, {3}, {4}",
                                         string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                         Messages.TooltipSugestionToggle,
                                         Messages.TooltipPages,
                                         string.Format(Messages.MaskEditJump, aux),
                                         Messages.TooltipEnterPressList);
                                }
                                else
                                {
                                    return string.Format("{0}, {1}\n{2}, {3}",
                                         string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                         Messages.TooltipSugestionToggle,
                                         Messages.TooltipPages,
                                         Messages.TooltipEnterPressList);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (isEditMode)
                    {
                        if (baseOptions.Type == ControlMaskedType.DateOnly || baseOptions.Type == ControlMaskedType.DateTime)
                        {
                            var aux = $"'{baseOptions.CurrentCulture.DateTimeFormat.DateSeparator}'";
                            if (baseOptions.Type == ControlMaskedType.DateTime)
                            {
                                aux = aux + " or " + $"'{baseOptions.CurrentCulture.DateTimeFormat.TimeSeparator}'";
                            }
                            return string.Format("{0}, {1}, {2}\n{3}, {4}, {5}",
                                string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                Messages.TooltipAbortEdit,
                                string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                Messages.TooltipPages,
                                string.Format(Messages.MaskEditJump, aux),
                                Messages.TooltipEnterPressList);
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
                            return string.Format("{0}, {1}, {2}\n{3}, {4}, {5}",
                                string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                Messages.TooltipAbortEdit,
                                string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                Messages.TooltipPages,
                                string.Format(Messages.MaskEditJump, aux),
                                Messages.TooltipEnterPressList);
                        }
                        else
                        {
                            return string.Format("{0}, {1}, {2}\n{3}, {4}",
                                string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                Messages.TooltipAbortEdit,
                                string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                Messages.TooltipPages,
                                Messages.TooltipEnterPressList);
                        }
                    }
                    else
                    {
                        if (baseOptions.OptEnabledAbortKey)
                        {
                            if (hasselected)
                            {
                                if (baseOptions.Type == ControlMaskedType.DateOnly || baseOptions.Type == ControlMaskedType.DateTime)
                                {
                                    var aux = $"'{baseOptions.CurrentCulture.DateTimeFormat.DateSeparator}'";
                                    if (baseOptions.Type == ControlMaskedType.DateTime)
                                    {
                                        aux = aux + " or " + $"'{baseOptions.CurrentCulture.DateTimeFormat.TimeSeparator}'";
                                    }
                                    return string.Format("{0}, {1} {2}, {3}\n{4}, {5}, {6}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                        string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                        string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                        Messages.TooltipPages,
                                        string.Format(Messages.MaskEditJump, aux),
                                        Messages.TooltipEnterPressList);
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
                                    return string.Format("{0}, {1} {2}, {3}\n{4}, {5}, {6}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                        string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                        string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                        Messages.TooltipPages,
                                        string.Format(Messages.MaskEditJump, aux),
                                        Messages.TooltipEnterPressList);
                                }
                                else
                                {
                                    return string.Format("{0}, {1} {2}, {3}\n{4}, {5}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                        string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                        string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                        Messages.TooltipPages,
                                        Messages.TooltipEnterPressList);
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
                                    return string.Format("{0}, {1}\n{2}, {3}, {4}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                        Messages.TooltipPages,
                                        string.Format(Messages.MaskEditJump, aux),
                                        Messages.TooltipEnterPressList);
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
                                    return string.Format("{0}, {1}\n{2}, {3}, {4}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                        Messages.TooltipPages,
                                        string.Format(Messages.MaskEditJump, aux),
                                        Messages.TooltipEnterPressList);
                                }
                                else
                                {
                                    return string.Format("{0}, {1}\n{2}, {3}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                        Messages.TooltipPages,
                                        Messages.TooltipEnterPressList);
                                }
                            }
                        }
                        else
                        {
                            if (hasselected)
                            {
                                if (baseOptions.Type == ControlMaskedType.DateOnly || baseOptions.Type == ControlMaskedType.DateTime)
                                {
                                    var aux = $"'{baseOptions.CurrentCulture.DateTimeFormat.DateSeparator}'";
                                    if (baseOptions.Type == ControlMaskedType.DateTime)
                                    {
                                        aux = aux + " or " + $"'{baseOptions.CurrentCulture.DateTimeFormat.TimeSeparator}'";
                                    }
                                    return string.Format("{0}, {1}, {2}\n{3}, {4}, {5}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                        string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                        Messages.TooltipPages,
                                        string.Format(Messages.MaskEditJump, aux),
                                        Messages.TooltipEnterPressList);
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
                                    return string.Format("{0}, {1}, {2}\n{3}, {4}, {5}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                        string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                        Messages.TooltipPages,
                                        string.Format(Messages.MaskEditJump, aux),
                                        Messages.TooltipEnterPressList);
                                }
                                else
                                {
                                    return string.Format("{0}, {1}, {2}\n{3}, {4}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                        string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                        Messages.TooltipPages,
                                        Messages.TooltipEnterPressList);
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
                                        Messages.TooltipPages,
                                        string.Format(Messages.MaskEditJump, aux),
                                        Messages.TooltipEnterPressList);
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
                                        Messages.TooltipPages,
                                        string.Format(Messages.MaskEditJump, aux),
                                        Messages.TooltipEnterPressList);
                                }
                                else
                                {
                                    return string.Format("{0}\n{1}, {2}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        Messages.TooltipPages,
                                        Messages.TooltipEnterPressList);
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}
