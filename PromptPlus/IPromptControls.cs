﻿// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using PromptPlusControls.FIGlet;
using PromptPlusControls.ValueObjects;

namespace PromptPlusControls
{
    public interface IPromptControls<T>
    {
        IPromptControls<T> EnabledAbortKey(bool value);
        IPromptControls<T> EnabledAbortAllPipes(bool value);
        IPromptControls<T> EnabledPromptTooltip(bool value);
        IPromptControls<T> HideAfterFinish(bool value);
        ResultPromptPlus<T> Run(CancellationToken? value = null);
    }

    public interface IFIGlet
    {
        IFIGlet LoadFont(string value);
        IFIGlet LoadFont(Stream value);
        IFIGlet FIGletWidth(CharacterWidth value);
        void Run(ConsoleColor? color = null);
    }

    public interface IFormPlusBase : IDisposable
    {
        string PipeId { get; }

        string PipeTitle { get; }

        object ContextState { get; }

        Func<ResultPipe[], object, bool> Condition { get; }
    }

    public interface IPromptPipe
    {
        IPromptPipe PipeCondition(Func<ResultPipe[], object, bool> condition);
        IFormPlusBase ToPipe(string id, string title, object state = null);
    }

    public interface IControlPipeLine
    {
        IControlPipeLine AddPipe(IFormPlusBase value);
        IControlPipeLine AddPipes(IEnumerable<IFormPlusBase> value);
        ResultPromptPlus<IEnumerable<ResultPipe>> Run(CancellationToken? value = null);
    }

    public interface IControlKeyPress : IPromptControls<bool>, IPromptPipe
    {
        IControlKeyPress Prompt(string value);
    }

    public interface IControlMaskEdit : IPromptControls<ResultMasked>, IPromptPipe
    {
        IControlMaskEdit Prompt(string value);
        IControlMaskEdit ShowInputType(bool value);
        IControlMaskEdit AddValidator(Func<object, ValidationResult> value);
        IControlMaskEdit AddValidators(IEnumerable<Func<object, ValidationResult>> value);
        IControlMaskEdit Mask(string value);
        IControlMaskEdit Default(object value);
        IControlMaskEdit UpperCase(bool value);
        IControlMaskEdit Culture(CultureInfo cultureinfo);
        IControlMaskEdit FillZeros(bool value);
        IControlMaskEdit FormatYear(FormatYear value);
        IControlMaskEdit FormatTime(FormatTime value);
        IControlMaskEdit AmmoutPositions(int intvalue, int decimalvalue);
        IControlMaskEdit AcceptSignal(bool value);
        IControlMaskEdit ShowDayWeek(FormatWeek value);
        IControlMaskEdit ValidateOnDemand();
    }

    public interface IControlInput : IPromptControls<string>, IPromptPipe
    {
        IControlInput Prompt(string value);
        IControlInput Default(string value);
        IControlInput IsPassword(bool swithVisible);
        IControlInput AddValidators(Func<object, ValidationResult> validator);
        IControlInput AddValidators(IEnumerable<Func<object, ValidationResult>> validators);
        IControlInput ValidateOnDemand();
    }

    public interface IControlSliderNumber : IPromptControls<double>, IPromptPipe
    {
        IControlSliderNumber Prompt(string value);
        IControlSliderNumber Default(double value);
        IControlSliderNumber Range(double minvalue, double maxvalue);
        IControlSliderNumber Step(double value);
        IControlSliderNumber LargeStep(double value);
        IControlSliderNumber FracionalDig(int value);
    }

    public interface IControlSliderSwitch : IPromptControls<bool>, IPromptPipe
    {
        IControlSliderSwitch Prompt(string value);
        IControlSliderSwitch Default(bool value);
        IControlSliderSwitch OffValue(string value);
        IControlSliderSwitch OnValue(string value);
    }

    public interface IControlProgressbar : IPromptControls<ProgressBarInfo>, IPromptPipe
    {
        IControlProgressbar Prompt(string value);
        IControlProgressbar UpdateHandler(Func<ProgressBarInfo, CancellationToken, Task<ProgressBarInfo>> value);
        IControlProgressbar Width(int value);
        IControlProgressbar StartInterationId(object value);
    }

    public interface IControlWaitProcess : IPromptControls<IEnumerable<ResultProcess>>, IPromptPipe
    {
        IControlWaitProcess Prompt(string value);
        IControlWaitProcess AddProcess(SingleProcess process);
        IControlWaitProcess SpeedAnimation(int value);
    }

    public interface IControlConfirm : IPromptControls<bool>, IPromptPipe
    {
        IControlConfirm Prompt(string value);
        IControlConfirm Default(bool value);
    }

    public interface IControlAutoComplete : IPromptControls<string>, IPromptPipe
    {
        IControlAutoComplete Prompt(string value);
        IControlAutoComplete PageSize(int value);
        IControlAutoComplete AddValidator(Func<object, ValidationResult> validator);
        IControlAutoComplete AddValidators(IEnumerable<Func<object, ValidationResult>> validators);
        IControlAutoComplete ValidateOnDemand();
        IControlAutoComplete AcceptWithoutMatch();
        IControlAutoComplete SpeedAnimation(int value);
        IControlAutoComplete MinimumPrefixLength(int value);
        IControlAutoComplete CompletionInterval(int value);
        IControlAutoComplete CompletionMaxCount(int value);
        IControlAutoComplete CompletionAsyncService(Func<string, int, CancellationToken, Task<string[]>> value);
    }


    public interface IControlSelect<T> : IPromptControls<T>, IPromptPipe
    {
        IControlSelect<T> Prompt(string value);
        IControlSelect<T> Default(T value);
        IControlSelect<T> PageSize(int value);
        IControlSelect<T> TextSelector(Func<T, string> value);
        IControlSelect<T> AddItem(T value);
        IControlSelect<T> AddItems(IEnumerable<T> value);
        IControlSelect<T> HideItem(T value);
        IControlSelect<T> HideItems(IEnumerable<T> value);
        IControlSelect<T> DisableItem(T value);
        IControlSelect<T> DisableItems(IEnumerable<T> value);
        IControlSelect<T> AutoSelectIfOne();
    }

    public interface IControlMultiSelect<T> : IPromptControls<IEnumerable<T>>, IPromptPipe
    {
        IControlMultiSelect<T> Prompt(string value);
        IControlMultiSelect<T> AddDefault(T value);
        IControlMultiSelect<T> AddDefaults(IEnumerable<T> value);
        IControlMultiSelect<T> PageSize(int value);
        IControlMultiSelect<T> TextSelector(Func<T, string> value);
        IControlMultiSelect<T> AddItem(T value);
        IControlMultiSelect<T> AddItems(IEnumerable<T> value);
        IControlMultiSelect<T> AddGroup(IEnumerable<T> value, string group);
        IControlMultiSelect<T> HideItem(T value);
        IControlMultiSelect<T> HideItems(IEnumerable<T> value);
        IControlMultiSelect<T> DisableItem(T value);
        IControlMultiSelect<T> DisableItems(IEnumerable<T> value);
        IControlMultiSelect<T> Range(int minvalue, int maxvalue);
    }

    public interface IControlList<T> : IPromptControls<IEnumerable<T>>, IPromptPipe
    {
        IControlList<T> Prompt(string value);
        IControlList<T> PageSize(int value);
        IControlList<T> TextSelector(Func<T, string> value);
        IControlList<T> Range(int minvalue, int maxvalue);
        IControlList<T> UpperCase(bool value);
        IControlList<T> AllowDuplicate(bool value);
        IControlList<T> AddValidator(Func<object, ValidationResult> validator);
        IControlList<T> AddValidators(IEnumerable<Func<object, ValidationResult>> validators);
        IControlList<T> ValidateOnDemand();

    }

    public interface IControlListMasked : IPromptControls<IEnumerable<ResultMasked>>, IPromptPipe
    {
        IControlListMasked Prompt(string value);
        IControlListMasked PageSize(int value);
        IControlListMasked Range(int minvalue, int maxvalue);
        IControlListMasked ShowInputType(bool value);
        IControlListMasked AddValidator(Func<object, ValidationResult> validator);
        IControlListMasked AddValidators(IEnumerable<Func<object, ValidationResult>> validators);
        IControlListMasked MaskType(MaskedType value, string mask = null);
        IControlListMasked UpperCase(bool value);
        IControlListMasked Culture(CultureInfo cultureinfo);
        IControlListMasked FillZeros(bool value);
        IControlListMasked FormatYear(FormatYear value);
        IControlListMasked FormatTime(FormatTime value);
        IControlListMasked AmmoutPositions(int intvalue, int decimalvalue);
        IControlListMasked AcceptSignal(bool signal);
        IControlListMasked ValidateOnDemand();

    }

    public interface IControlBrowser : IPromptControls<ResultBrowser>, IPromptPipe
    {
        IControlBrowser Prompt(string value);
        IControlBrowser Filter(BrowserFilter value);
        IControlBrowser Default(string value);
        IControlBrowser PrefixExtension(string value);
        IControlBrowser AllowNotSelected(bool value);
        IControlBrowser Root(string value);
        IControlBrowser SearchPattern(string value);
        IControlBrowser PageSize(int value);
        IControlBrowser SupressHidden(bool value);
        IControlBrowser PromptCurrentPath(bool value);
        IControlBrowser promptSearchPattern(bool value);

    }
}