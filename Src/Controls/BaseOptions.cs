﻿// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;

namespace PPlus.Controls
{
    /// <inheritdoc cref="IPromptConfig"/>
    public abstract class BaseOptions : IPromptConfig
    {
        private readonly Dictionary<SymbolType, (string value, string unicode)> _optSymbols = new();

        private BaseOptions()
        {
            throw new PromptPlusException("BaseOptions CTOR Not Implemented");
        }

        internal BaseOptions(bool? showCursor = true)
        {
            Init();
            OptShowCursor = showCursor ?? true;
        }

        internal bool OptShowCursor { get; } = false;
        internal string OptPrompt { get; set; } = string.Empty;
        internal string OptDescription { get; set; } = string.Empty;
        internal bool OptShowTooltip { get; set; } = PromptPlus.Config.ShowTooltip;

        internal Dictionary<StageControl, Action<object, object?>> OptUserActions { get; private set; } = new();
        internal StyleSchema OptStyleSchema { get; private set; } = new();
        internal string OptToolTip { get; private set; } = string.Empty;
        internal bool OptHideAfterFinish { get; private set; } = PromptPlus.Config.HideAfterFinish;
        internal bool OptHideOnAbort { get; private set; } = PromptPlus.Config.HideOnAbort;
        internal bool OptEnabledAbortKey { get; private set; } = PromptPlus.Config.EnabledAbortKey;
        internal object? OptContext { get; private set; } = null;

        #region IPromptConfig

        /// <inheritdoc/>
        public IPromptConfig EnabledAbortKey(bool value)
        {
            OptEnabledAbortKey = value;
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig ShowTooltip(bool value)
        {
            OptShowTooltip = value;
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig HideAfterFinish(bool value)
        {
            OptHideAfterFinish = value;
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig HideOnAbort(bool value)
        {
            OptHideOnAbort = value;
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig SetContext(object value)
        {
            OptContext = value;
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig AddExtraAction(StageControl stage, Action<object, object?> useraction)
        {
            OptUserActions.Remove(stage);
            OptUserActions.Add(stage, useraction);
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig Description(StringStyle value)
        {
            OptDescription = value.Text;
            OptStyleSchema.ApplyStyle(StyleControls.Description, value.Style);
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig Prompt(StringStyle value)
        {
            OptPrompt = value.Text;
            OptStyleSchema.ApplyStyle(StyleControls.Prompt, value.Style);
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig Tooltips(StringStyle value)
        {   
            OptToolTip = value.Text;
            OptStyleSchema.ApplyStyle(StyleControls.Tooltips, value.Style);
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig Description(string value)
        {
            OptDescription = value;
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig Prompt(string value)
        {
            OptPrompt = value;
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig Tooltips(string value)
        {
            OptToolTip = value;
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig ApplyStyle(StyleControls styleControl, Style value)
        {
            OptStyleSchema.ApplyStyle(styleControl, value);
            return this;
        }

        /// <inheritdoc/>
        public IPromptConfig Symbols(SymbolType schema, string value, string? unicode = null)
        {
            _optSymbols[schema] = (value,unicode??value);
            return this;
        }

        internal string Symbol(SymbolType schema)
        {
            if (PromptPlus.Console.IsUnicodeSupported)
            {
                return _optSymbols[schema].unicode;
            }
            return _optSymbols[schema].value;
        }

        #endregion

        private void Init()
        {
            foreach (var item in PromptPlus.Config._globalSymbols.Keys)
            {
                _optSymbols.Add(item, PromptPlus.Config._globalSymbols[item]);
            }
        }
    }
}