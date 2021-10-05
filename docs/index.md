# Welcome to PromptPlus
<!--
[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
-->
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)

Interactive command-line based application framework for **C# with powerful controls**.

[**Snapshot of All Controls**](#snapshot)

![](./images/PipeLine.gif)

## Help
- [Install](#install)
- [Organization](#organization)
    - [Paging Behavior](#paging-behavior)
- [Global Settings](#global-settings)
    - [Culture](#culture)
    - [Colors](#colors)
    - [Symbols](#symbols)
    - [Hotkeys](#hotkeys)
- [Load and Save Settings](#load-and-save-settings)
- [**Api Controls**](#apis)

# Documentation

## Install
[**Top**](#help)

PromptPlus was developed in c# with the **netstandard2.1** target framework, with compatibility for:

- NET Framework 4.8
- .NET Core 3.1
- .NET Core 5.0
- .NET Core 6.X


```
Install-Package PromptPlus
```

```
dotnet add package PromptPlus
```

## Organization
[**Top**](#help)

All controls have the same lines organization:
- Message and data entry (ever)
- Filter (depends on the control)
- Tooltips (configurable)
- Collection subset items (depends on the control, page size and size of console/terminal)
- Page information (depends on size colletion, page size and size of console/terminal)
- Error message (depends on the control and validators)

tooltips can be global (hotkey always active - default F1) and control specific. All controls have the properties to show hide tooltips.

### Paging behavior
[**Top**](#help)

When a control has a collection it can be paged with a limit of items per page. When the item per page limit is not entered, the number of items per page is set to the maximum allowed by the console/terminal size. If the console/terminal is resized, an adjustment will be made to a number of items per page and a message will be issued on the console (only when it is a terminal)

## Global Settings
[**Top**](#help)

- EnabledBeep 
    - Enabled/disabled beep. Default = true
- EnabledStandardTooltip 
    - Show/Hide global tooltip. Default = true. When false then control´s tooltip not show.
- EnabledPromptTooltip
    - Enabled/disabled control´s tootip for default value. Default = true
- EnabledAbortKey
    - Enabled/disabled hotkey to abort prompt/control executation. Default = true
- EnabledAbortAllPipes 
    - Enabled/disabled hotkey to abort all pipes. Default = true
- PasswordChar 
    - Character default to password input. Default = '#'
- DefaultCulture 
    - Set default language/culture for all controls. Default = Current Culture application
 

```csharp
//sample
PromptPlus.EnabledAbortKey = false;
```

### Culture
[**Top**](#help)

PromptPlus applies the language/culture **only when running controls**. The language/culture of the application is **not affected**. If language/culture is not informed, the application's language/culture will be used with fallback to en-US.

All messages are affected when changed language/culture. PromptPlus has languages embeded:
- en-US (Default)
- pt-BR
 
```csharp
//sample
PromptPlus.DefaultCulture = new CultureInfo("en-US");
```

To use a non-embedded language/culture:

- Use the **PromptPlusResources.resx** file that already has the package in the resources folder
- Translate messages with same format to your language/culture
- Convert .resx files to binary .resources files ([**reference link here**](https://docs.microsoft.com/en-us/dotnet/core/extensions/work-with-resx-files-programmatically))
- Publish the compiled file (**PromptPlus.{Language}.resources**) in the same folder as the binaries.

### Colors
[**Top**](#help)

PromptPlus has a configurable color(16 color) schema.

- ForeColorSchema 
    - Screen foreground color. Default = Console.ForegroundColor
- BackColorSchema
    - Screen background color. Default = Console.BackgroundColor
- Pagination
    - Page info foreground color. Default = Console.DarkGray
- Hint
    - Hit/tooltip info foreground color. Default = Console.DarkGray
- Answer
    - Answer foreground color. Default = Console.Cyan
- Select
    - Seleted item foreground color. Default = Console.Green
- Disabled
    - Disabled item foreground color. Default = Console.DarkGray
- Filter
    - Filter info foreground color. Default = Console.Yellow
- Error
    - Error info foreground color. Default = Console.Red
- DoneSymbol
    - Done symbol foreground color. Default = Console.Cyan
- PromptSymbol
    - Prompt symbol foreground color. Default = Console.Green
- SliderBackcolor
    - Sliders / Progress bar background color. Default = Console.DarkGray
- SliderForecolor
    - Sliders / Progress bar foreground color. Default = Console.Cyan
 
 ```csharp
//sample
PromptPlus.ColorSchema.Answer = ConsoleColor.Cyan;
```

### Symbols
[**Top**](#help)

PromptPlus has a configurable symbos with Unicode support and Fallback.

- MaskEmpty
    - Symbol : ■
    - Fallback : double space
    - Apply to : Empty character for input masked
- File
    - Symbol : ■
    - Fallback : "- "
    - Apply to : Symbol for file in browser control
 - Folder
    - Symbol : ►
    - Fallback : "> "
    - Apply to : Symbol for folder in browser control
 - Prompt
    - Symbol : →
    - Fallback : "->"
    - Apply to : Symbol for prompt control
 - Done
    - Symbol : √
    - Fallback : "V "
    - Apply to : Symbol for prompt done
 - Error
    - Symbol : »
    - Fallback : "\>\>"
    - Apply to : Symbol for message error
 - Selector
    - Symbol : ›
    - Fallback : "> "
    - Apply to : Symbol for seletor item.
 - Selected
    - Symbol : ♦
    - Fallback : "* "
    - Apply to : Symbol for Selected item.
 - NotSelect
    - Symbol : ○
    - Fallback : double space
    - Apply to : Symbol for not Selected item.
 - TaskRun
    - Symbol : ♦
    - Fallback : "* "
    - Apply to : Symbol for backgound process running.
 - Skiped
    - Symbol : ×
    - Fallback : "x "
    - Apply to : Symbol for  backgound process canceled.

 ```csharp
//sample
PromptPlus.Symbols.Done = new Symbol("√", "V ");
```
**_Note: new Symbol() return : Symbol = single space and Fallback = double space._**

### Hotkeys
[**Top**](#help)

Hotkeys (global and control-specific) are configurable. Some hotkeys are internal and reserved.

**internal and reserved hotkeys:**

- Enter (all Modifiers)
- Delete (all Modifiers)
- LeftArrow (all Modifiers)
- RightArrow (all Modifiers)
- UpArrow (all Modifiers)
- DownArrow (all Modifiers)
- PageUp (all Modifiers)
- PageDown (all Modifiers)
- Backspace (all Modifiers)

**_Note: If you use one of these keys, an ArgumentException will be generated._**


**Hotkeys Configurables:**

- AbortAllPipesKeyPress
    - Key for abort all pipes. Default = Alt+X
- AbortKeyPress
    - Key for abort current control. Default = Escape
- TooltipKeyPress
    - Key for Show/Hide tooltips. Default = F1
- ResumePipesKeyPress
    - Key for Show/Hide summary pipes. Default = F2
- UnSelectFilter
    - Key for Show input filter. Default = Alt+F
- SwitchViewPassword
    - Key for Show/hide input password. Default = Alt+V
- SelectAll
    - Key for toggle all items to selected. Default = Alt+A
- InvertSelect
    - Key for invet selection to all items. Default = Alt+I
- RemoveAll
    - Key for remove all items. Default = Alt+R
 
 ```csharp
//sample
PromptPlus.AbortAllPipesKeyPress = new HotKey(key: "X", alt: true, ctrl: false, shift: false);
```

**_Note: the key parameter is case-insentive;_**

## Load and Save Settings
[**Top**](#help)

PromptPlus allows saving and loading a previous configuration of culture, behavior, hotkeys, colors and symbols.A file with the default configuration is available in the package in the Resources folder named **PromptPlus.config.json** . To load automatically the file must be placed in your project and published in the **same folder** as the binaries.

```csharp
//sample save
PromptPlus.SaveConfigToFile(folderfile: "YourFolder");
//sample load
PromptPlus.LoadConfigFromFile(folderfile: "YourFolder");
```

**_Note: if the folderfile parameter is omitted, it will be saved/loaded from the default application folder_**

## Apis
[**Top**](#help)

controls | Details
--- | ---
[Any key](anykey.md) |  Simple any key press
[Key Press](keypress.md) | Simple specific key
[Confirm](confirm.md) | Simple confirm with  with tool tips and language detection 
[Input](input.md) | Generic input with input validator with tooltips
[Password](password.md) | Input password with input validator and show/hide(optional) input value
[MaskEdit Generic](maskeditgeneric.md) | Input with masked input , tooltips and input validator
[MaskEdit Date](maskeditdate.md) | Date input with language parameter, tooltips and input validator
[MaskEdit Time](maskeditdate.md) | Time input with language parameter, tooltips and input validator
[MaskEdit Date and Time](maskeditdate.md) | Date and time input with language parameter, tooltips and input validator
[MaskEdit Number](maskeditnumber.md) | Numeric input with language parameter, tooltips and input validator
[MaskEdit Currency](maskeditnumber.md) | Currency input with language parameter, tooltips and input validator
[Select](select.md)| Generic select input IEnumerable/Enum with auto-paginator and tooltips and more
[MultiSelect](multiselect.md) | Generic multi select input IEnumerable/Enum with auto-paginator , tooltips and more
[List](list.md) | Create Generic IEnumerable with auto-paginator, tooptip , input validator, message error by type/format and more
[ListMasked](listmasked.md) | Create generic IEnumerable with masked input, auto-paginator, tooptip , input validator
[Browser](browser.md) | Browser files/folder with auto-paginator and tooltips
[Slider Number](slidernumber.md) | Numeric ranger with short/large step and tooltips
[Number Up/Down](numberupdown.md) | Numeric ranger with step and tooltips
[Slider Switche](sliderswitche.md) | Generic choice with customization and tooltips
[Progress Bar](progressbar.md) | Progress Bar with interation customization
[WaitProcess](waitprocess.md) | Wait process with animation
[PipeLine](pipeline.md) | Pipeline sequence to all prompts with condition by pipe and status summary

[**Top**](#help)

## Snapshot
[**Top**](#help)

### Input
[**Top**](#help) | [Input](input.md) | [Password](password.md)

![](./images/Input.gif)
![](./images/Password.gif)

### MaskEdit
[**Top**](#help) | [MaskEdit Generic](maskeditgeneric.md) | [MaskEdit Date/Time](maskeditdate.md) | [MaskEdit Number/Currency](maskeditnumber.md) 

![](./images/MaskEditGeneric.gif)
![](./images/MaskEditDateTime.gif)
![](./images/MaskEditNumber.gif)
![](./images/MaskEditCurrency.gif)

### KeyPress
[**Top**](#help) | [Any key](anykey.md) | [Key Press](keypress.md)

![](./images/Anykey.gif)
![](./images/KeyPress.gif)

### Selectors
[**Top**](#help) | [Select](select.md) | [MultiSelect](multiselect.md)

![](./images/Select.gif)
![](./images/MultSelect.gif)

### Confirm
[**Top**](#help) | [Confirm](confirm.md) | [Slider Switche](sliderswitche.md)[Slider Switche](sliderswitche.md)

![](./images/Confirm.gif)
![](./images/SliderSwitche.gif)

### WaitProcess
[**Top**](#help) | [WaitProcess](waitprocess.md) 

![](./images/WaitProcess.gif)

### ProgressBar
[**Top**](#help) | [Progress Bar](progressbar.md)

![](./images/ProgressBar.gif)

### Slider Number
[**Top**](#help) | [Slider Number](slidernumber.md) | [Number Up/Down](numberupdown.md)

![](./images/SliderNumber.gif)
![](./images/NumberUpDown.gif)

### List
[**Top**](#help) | [List](list.md) | [ListMasked](listmasked.md)

![](./images/List.gif)
![](./images/MaskedList.gif)

### Browser
[**Top**](#help) | [Browser](browser.md)

![](./images/Browser.gif)

### PipeLine
[**Top**](#help) | [PipeLine](pipeline.md)

![](./images/PipeLine.gif)
