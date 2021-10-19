﻿// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading;

using PromptPlusControls.Internal;
using PromptPlusControls.ValueObjects;

namespace PromptPlusControls.Controls
{
    internal class ConfirmControl : ControlBase<bool>, IControlConfirm
    {
        private bool _initform;
        private readonly ConfirmOptions _options;
        private readonly InputBuffer _inputBuffer = new();

        public ConfirmControl(ConfirmOptions options) : base(options.HideAfterFinish, true, options.EnabledAbortKey, options.EnabledAbortAllPipes)
        {
            _options = options;
        }

        public override void InitControl()
        {
            _initform = true;
        }

        public override bool? TryResult(bool summary, CancellationToken cancellationToken, out bool result)
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

                switch (keyInfo.Key)
                {
                    case ConsoleKey.Enter when keyInfo.Modifiers == 0:
                    {
                        {
                            var input = _inputBuffer.ToString();

                            if (string.IsNullOrEmpty(input))
                            {
                                if (_options.DefaultValue != null)
                                {
                                    result = _options.DefaultValue.Value;

                                    return true;
                                }

                                SetError(Messages.Required);
                            }
                            else
                            {
                                var lowerInput = input.ToLower();

                                if (lowerInput == Messages.YesKey.ToString().ToLower()
                                    || lowerInput == Messages.LongYesKey.ToLower())
                                {
                                    result = true;
                                    return true;
                                }

                                if (lowerInput == Messages.NoKey.ToString().ToLower()
                                    || lowerInput == Messages.LongNoKey.ToLower())
                                {
                                    result = false;
                                    return true;
                                }

                                SetError(Messages.Invalid);
                            }

                            break;
                        }
                    }
                    case ConsoleKey.LeftArrow when keyInfo.Modifiers == 0 && !_inputBuffer.IsStart:
                        _inputBuffer.Backward();
                        break;
                    case ConsoleKey.RightArrow when keyInfo.Modifiers == 0 && _inputBuffer.IsEnd:
                        _inputBuffer.Forward();
                        break;
                    case ConsoleKey.Backspace when keyInfo.Modifiers == 0 && !_inputBuffer.IsStart:
                        _inputBuffer.Backspace();
                        break;
                    case ConsoleKey.Delete when keyInfo.Modifiers == 0 && !_inputBuffer.IsEnd:
                        _inputBuffer.Delete();
                        break;
                    default:
                    {
                        if (!cancellationToken.IsCancellationRequested)
                        {
                            if (!char.IsControl(keyInfo.KeyChar))
                            {
                                _inputBuffer.Insert(keyInfo.KeyChar);
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

            result = default;

            return isvalidhit;
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            screenBuffer.WritePrompt(_options.Message);
            if (_options.DefaultValue == null)
            {
                screenBuffer.Write($"({char.ToLower(Messages.YesKey)}/{ char.ToLower(Messages.NoKey)}) ");
            }
            else if (_options.DefaultValue.Value)
            {
                screenBuffer.Write($"({char.ToUpper(Messages.YesKey)}/{char.ToLower(Messages.NoKey)}) ");
            }
            else
            {
                screenBuffer.Write($"({char.ToLower(Messages.YesKey)}/{char.ToUpper(Messages.NoKey)}) ");
            }

            if (_options.DefaultValue.HasValue)
            {
                if (_initform)
                {
                    if (_options.DefaultValue.Value)
                    {
                        _inputBuffer.Insert(Messages.YesKey);
                    }
                    else
                    {
                        _inputBuffer.Insert(Messages.NoKey);
                    }
                }
            }

            screenBuffer.PushCursor(_inputBuffer);

            if (EnabledStandardTooltip)
            {
                screenBuffer.WriteLineStandardHotKeys(OverPipeLine, _options.EnabledAbortKey, _options.EnabledAbortAllPipes);
                if (_options.EnabledPromptTooltip)
                {
                    screenBuffer.WriteLineHint(Messages.EnterFininsh);
                }
            }
            _initform = false;
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, bool result)
        {
            screenBuffer.WriteDone(_options.Message);
            FinishResult = result ? Messages.YesKey.ToString() : Messages.NoKey.ToString();
            screenBuffer.WriteAnswer(FinishResult);
        }

        #region IControlConfirm

        public IControlConfirm Prompt(string value)
        {
            _options.Message = value;
            return this;
        }
        public IControlConfirm Default(bool value)
        {
            _options.DefaultValue = value;
            return this;
        }
        public IPromptControls<bool> EnabledAbortKey(bool value)
        {
            _options.EnabledAbortKey = value;
            return this;
        }

        public IPromptControls<bool> EnabledAbortAllPipes(bool value)
        {
            _options.EnabledAbortAllPipes = value;
            return this;
        }

        public IPromptControls<bool> EnabledPromptTooltip(bool value)
        {
            _options.EnabledPromptTooltip = value;
            return this;
        }

        public IPromptControls<bool> HideAfterFinish(bool value)
        {
            _options.HideAfterFinish = value;
            return this;
        }

        public ResultPromptPlus<bool> Run(CancellationToken? value = null)
        {
            InitControl();
            return Start(value ?? CancellationToken.None);
        }

        public IPromptPipe Condition(Func<ResultPipe[], object, bool> condition)
        {
            PipeCondition = condition;
            return this;
        }

        public IFormPlusBase AddPipe(string id, string title, object state = null)
        {
            PipeId = id ?? Guid.NewGuid().ToString();
            PipeTitle = title ?? string.Empty;
            ContextState = state;
            return this;
        }

        #endregion

    }
}
