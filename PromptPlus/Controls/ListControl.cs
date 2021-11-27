﻿// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;


using PPlus.Internal;

using PPlus.Objects;
using PPlus.Resources;

namespace PPlus.Controls
{
    internal class ListControl<T> : ControlBase<IEnumerable<T>>, IControlList<T>
    {
        private readonly ListOptions<T> _options;
        private readonly Type _targetType = typeof(T);
        private readonly Type _underlyingType = Nullable.GetUnderlyingType(typeof(T));
        private readonly InputBuffer _inputBuffer = new();
        private readonly List<T> _inputItems = new();
        private Paginator<T> _localpaginator;
        private string _inputDesc;
        private readonly string _startDesc;

        public ListControl(ListOptions<T> options) : base(options, true)
        {
            _options = options;
            _startDesc = _options.Description;
        }

        public override void InitControl()
        {
            Thread.CurrentThread.CurrentCulture = PromptPlus.DefaultCulture;
            Thread.CurrentThread.CurrentUICulture = PromptPlus.DefaultCulture;

            foreach (var item in _options.InitialItems)
            {
                var localitem = item;
                if (typeof(T).Equals(typeof(string)))
                {
                    localitem = TypeHelper<T>.ConvertTo(item.ToString().ToUpperInvariant());
                }
                if (TryValidate(localitem, _options.Validators))
                {
                    if (!_options.AllowDuplicate)
                    {
                        if (!_inputItems.Contains(localitem))
                        {
                            _inputItems.Add(localitem);
                        }
                    }
                    else
                    {
                        _inputItems.Add(localitem);
                    }
                }
            }

            ClearError();

            _localpaginator = new Paginator<T>(_inputItems, _options.PageSize, Optional<T>.s_empty, _options.TextSelector);
            _localpaginator.FirstItem();

            if (_options.InitialValue != null)
            {
                _inputBuffer.Load(_options.TextSelector(_options.InitialValue));
            }

            Thread.CurrentThread.CurrentCulture = AppcurrentCulture;
            Thread.CurrentThread.CurrentUICulture = AppcurrentUICulture;

        }

        public override bool? TryResult(bool summary, CancellationToken cancellationToken, out IEnumerable<T> result)
        {
            bool? isvalidhit = false;
            if (summary)
            {
                result = default;
                return false;
            }
            do
            {
                var keyInfo = WaitKeypress(cancellationToken);

                if (CheckDefaultKey(keyInfo))
                {
                    continue;
                }
                else if (IskeyPageNavagator(keyInfo, _localpaginator))
                {
                    continue;
                }

                else if (PromptPlus.RemoveAll.Equals(keyInfo))
                {
                    var aux = _inputItems.Where(x => _options.TextSelector(x).IndexOf(_inputBuffer.ToString(), StringComparison.OrdinalIgnoreCase) != -1).ToArray();
                    _inputItems.RemoveAll(x => aux.Contains(x));
                    _inputBuffer.Clear();
                    _localpaginator = new Paginator<T>(_inputItems, _options.PageSize, Optional<T>.s_empty, _options.TextSelector);
                    _localpaginator.FirstItem();
                    continue;
                }


                switch (keyInfo.Key)
                {
                    case ConsoleKey.Enter when keyInfo.Modifiers == ConsoleModifiers.Control:
                    {
                        result = _inputItems;
                        return true;
                    }
                    case ConsoleKey.Enter when keyInfo.Modifiers == 0:
                    {
                        var input = _inputBuffer.ToString();
                        try
                        {
                            result = _inputItems;

                            if (string.IsNullOrEmpty(input))
                            {
                                if (_inputItems.Count >= _options.Minimum)
                                {
                                    return true;
                                }
                                SetError(string.Format(Messages.ListMinSelection, _options.Minimum));
                                break;
                            }

                            if (_inputItems.Count >= _options.Maximum)
                            {
                                SetError(string.Format(Messages.ListMaxSelection, _options.Maximum));
                                break;
                            }

                            T inputValue;
                            inputValue = TypeHelper<T>.ConvertTo(input);
                            if (!TryValidate(inputValue, _options.Validators))
                            {
                                result = _inputItems;
                                break;
                            }
                            if (!_options.AllowDuplicate)
                            {
                                if (_inputItems.Contains(inputValue))
                                {
                                    SetError(Messages.ListItemAlreadyexists);
                                    break;
                                }
                            }
                            _inputBuffer.Clear();
                            _inputItems.Add(inputValue);
                            _localpaginator = new Paginator<T>(_inputItems, _options.PageSize, Optional<T>.s_empty, _options.TextSelector);
                            if (_options.InitialValue != null && _options.EverInitialValue)
                            {
                                _inputBuffer.Load(_options.TextSelector(_options.InitialValue));
                            }
                        }
                        catch (FormatException)
                        {
                            SetError(PromptPlus.LocalizateFormatException(typeof(T)));
                        }
                        catch (Exception ex)
                        {
                            SetError(ex);
                        }
                        break;
                    }
                    case ConsoleKey.LeftArrow when keyInfo.Modifiers == 0 && !_inputBuffer.IsStart:
                        _inputBuffer.Backward();
                        break;
                    case ConsoleKey.RightArrow when keyInfo.Modifiers == 0 && !_inputBuffer.IsEnd:
                        _inputBuffer.Forward();
                        break;
                    case ConsoleKey.Backspace when keyInfo.Modifiers == 0 && !_inputBuffer.IsStart:
                        _inputBuffer.Backspace();
                        break;
                    case ConsoleKey.Delete when keyInfo.Modifiers == 0 && !_inputBuffer.IsEnd:
                        _inputBuffer.Delete();
                        break;
                    case ConsoleKey.Delete when keyInfo.Modifiers == ConsoleModifiers.Control:
                    {
                        if (_localpaginator.TryGetSelectedItem(out var selected))
                        {
                            var inputValue = (T)Convert.ChangeType(selected, _underlyingType ?? _targetType);

                            if (_inputItems.Contains(inputValue))
                            {
                                _inputItems.Remove(inputValue);
                            }

                            _inputBuffer.Clear();
                            _localpaginator = new Paginator<T>(_inputItems, _options.PageSize, Optional<T>.s_empty, _options.TextSelector);
                            _localpaginator.FirstItem();
                        }
                        break;
                    }
                    default:
                    {
                        if (!cancellationToken.IsCancellationRequested)
                        {
                            if (!char.IsControl(keyInfo.KeyChar))
                            {
                                _inputBuffer.Insert(_options.UpperCase ? char.ToUpper(keyInfo.KeyChar) : keyInfo.KeyChar);
                            }
                            else
                            {
                                isvalidhit = null;
                            }
                        }
                        break;
                    }
                }

            } while (KeyAvailable && !cancellationToken.IsCancellationRequested);
            if (_inputDesc != _inputBuffer.ToString())
            {
                _inputDesc = _inputBuffer.ToString();
                if (string.IsNullOrEmpty(_inputDesc))
                {
                    _options.Description = _startDesc;
                }
                else
                {
                    if (_options.DescriptionSelector != null)
                    {
                        _options.Description = _options.DescriptionSelector.Invoke(_inputDesc);
                    }
                }
            }
            result = null;
            return isvalidhit;
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            screenBuffer.WritePrompt(_options.Message);

            screenBuffer.PushCursor(_inputBuffer);

            if (HasDescription)
            {
                if (!HideDescription)
                {
                    screenBuffer.WriteLineDescription(_options.Description);
                }
            }

            if (EnabledStandardTooltip)
            {
                screenBuffer.WriteLineStandardHotKeys(OverPipeLine, _options.EnabledAbortKey, _options.EnabledAbortAllPipes, !HasDescription);
                if (_options.EnabledPromptTooltip)
                {
                    screenBuffer.WriteLine();
                    if (_localpaginator.PageCount > 1)
                    {
                        screenBuffer.WriteHint(Messages.KeyNavPaging);
                    }
                    screenBuffer.WriteHint(Messages.ListKeyNavigation);
                }
            }
            var subset = _localpaginator.ToSubset();
            var index = 0;
            foreach (var item in subset)
            {
                var value = _options.TextSelector(item);
                if (_inputBuffer.Length == 0 || EqualityComparer<string>.Default.Equals(_inputBuffer.ToString(), value))
                {
                    if (_localpaginator.SelectedIndex == index)
                    {
                        screenBuffer.WriteLineMarkSelect(value);
                    }
                    else
                    {
                        screenBuffer.WriteLineNotMarkSelect(value);
                    }
                }
                else
                {
                    if (_localpaginator.SelectedIndex == index)
                    {
                        screenBuffer.WriteLineSelect(value);
                    }
                    else
                    {
                        screenBuffer.WriteLineNotSelect(value);
                    }
                }
                index++;
            }
            if (_localpaginator.PageCount > 1)
            {
                screenBuffer.WriteLinePagination(_localpaginator.PaginationMessage());
            }

            if (_options.ValidateOnDemand && _options.Validators.Count > 0 && _inputBuffer.Length > 0)
            {
                TryValidate(_inputBuffer.ToString(), _options.Validators);
            }
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, IEnumerable<T> result)
        {
            screenBuffer.WriteDone(_options.Message);
            FinishResult = string.Format(Messages.FinishResultList, result.Count());
            screenBuffer.WriteAnswer(FinishResult);
        }

        #region IControlList

        public IControlList<T> ValidateOnDemand()
        {
            _options.ValidateOnDemand = true;
            return this;
        }
        public IControlList<T> DescriptionSelector(Func<string, string> value)
        {
            _options.DescriptionSelector = value;
            return this;
        }


        public IControlList<T> Prompt(string value, string description = null)
        {
            _options.Message = value;
            if (description != null)
            {
                _options.Description = description;
            }
            return this;
        }

        public IControlList<T> PageSize(int value)
        {
            if (value < 0)
            {
                _options.PageSize = null;
            }
            else
            {
                _options.PageSize = value;
            }
            return this;
        }

        public IControlList<T> TextSelector(Func<T, string> value)
        {
            _options.TextSelector = value ?? (x => x.ToString());
            return this;
        }

        public IControlList<T> Range(int minvalue, int maxvalue)
        {
            if (minvalue < 0)
            {
                minvalue = 0;
            }
            if (maxvalue < 0)
            {
                maxvalue = minvalue;
            }
            if (minvalue > maxvalue)
            {
                throw new ArgumentException(string.Format(Exceptions.Ex_InvalidValue, $"{minvalue},{maxvalue}"));
            }
            _options.Minimum = minvalue;
            _options.Maximum = maxvalue;
            return this;
        }

        public IControlList<T> InitialValue(T value, bool ever = false)
        {
            if (value is not null)
            {
                _options.InitialValue = value;
                _options.EverInitialValue = ever;
            }
            return this;
        }

        public IControlList<T> AddItem(T value)
        {
            if (value is not null)
            {
                if (typeof(T).Equals(typeof(string)))
                {
                    if (!string.IsNullOrEmpty(value.ToString()))
                    {
                        _options.InitialItems.Add(value);
                    }
                }
                else
                {
                    _options.InitialItems.Add(value);
                }
            }
            return this;
        }

        public IControlList<T> AddItems(IEnumerable<T> value)
        {
            if (value is not null)
            {
                foreach (var item in value)
                {
                    if (item is not null)
                    {
                        if (typeof(T).Equals(typeof(string)))
                        {
                            if (!string.IsNullOrEmpty(item.ToString()))
                            {
                                _options.InitialItems.Add(item);
                            }
                        }
                        else
                        {
                            _options.InitialItems.Add(item);
                        }
                    }
                }
            }
            return this;
        }

        public IControlList<T> UpperCase(bool value)
        {
            _options.UpperCase = value;
            return this;
        }

        public IControlList<T> AllowDuplicate(bool value)
        {
            _options.AllowDuplicate = value;
            return this;
        }

        public IControlList<T> AddValidator(Func<object, ValidationResult> validator)
        {
            if (validator == null)
            {
                return this;
            }
            return AddValidators(new List<Func<object, ValidationResult>> { validator });
        }

        public IControlList<T> AddValidators(IEnumerable<Func<object, ValidationResult>> validators)
        {
            if (validators == null)
            {
                return this;
            }
            _options.Validators.Merge(validators);
            return this;
        }

        public IControlList<T> Config(Action<IPromptConfig> context)
        {
            context.Invoke(this);
            return this;
        }

        public IPromptConfig EnabledAbortKey(bool value)
        {
            _options.EnabledAbortKey = value;
            return this;
        }

        public IPromptConfig EnabledAbortAllPipes(bool value)
        {
            _options.EnabledAbortAllPipes = value;
            return this;
        }

        public IPromptConfig EnabledPromptTooltip(bool value)
        {
            _options.EnabledPromptTooltip = value;
            return this;
        }

        public IPromptConfig HideAfterFinish(bool value)
        {
            _options.HideAfterFinish = value;
            return this;
        }

        public ResultPromptPlus<IEnumerable<T>> Run(CancellationToken? value = null)
        {
            InitControl();
            try
            {
                return Start(value ?? CancellationToken.None);
            }
            finally
            {
                Dispose();
            }
        }

        public IPromptPipe PipeCondition(Func<ResultPipe[], object, bool> condition)
        {
            Condition = condition;
            return this;
        }

        public IFormPlusBase ToPipe(string id, string title, object state = null)
        {
            PipeId = id ?? Guid.NewGuid().ToString();
            PipeTitle = title ?? string.Empty;
            ContextState = state;
            return this;
        }
        #endregion
    }
}
