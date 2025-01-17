# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:HotKey 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# HotKey

Namespace: PPlus.Controls

Represents the HotKey to control

```csharp
public struct HotKey
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [HotKey](./pplus.controls.hotkey.md)<br>
Implements [IEquatable&lt;ConsoleKeyInfo&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1)

## Properties

### <a id="properties-calendarswitchnotes"/>**CalendarSwitchNotes**

Get HotKey default for Calendar SwitchNotes 'F2'

```csharp
public static HotKey CalendarSwitchNotes { get; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### <a id="properties-chartbarswitchlayout"/>**ChartBarSwitchLayout**

Get HotKey default for ChartBar Switch Layout 'F2'

```csharp
public static HotKey ChartBarSwitchLayout { get; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### <a id="properties-chartbarswitchlegend"/>**ChartBarSwitchLegend**

Get HotKey default for ChartBar Switch Legend 'F3'

```csharp
public static HotKey ChartBarSwitchLegend { get; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### <a id="properties-chartbarswitchorder"/>**ChartBarSwitchOrder**

Get HotKey default for ChartBar Switch Order 'F4'

```csharp
public static HotKey ChartBarSwitchOrder { get; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### <a id="properties-edititemdefault"/>**EditItemDefault**

Get HotKey default for Edit Item 'F2'

```csharp
public static HotKey EditItemDefault { get; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### <a id="properties-invertselecteddefault"/>**InvertSelectedDefault**

Get HotKey default for Invert Selected 'F3'

```csharp
public static HotKey InvertSelectedDefault { get; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### <a id="properties-keyinfo"/>**KeyInfo**

Get  to HotKey

```csharp
public ConsoleKeyInfo KeyInfo { get; }
```

#### Property Value

ConsoleKeyInfo<br>

### <a id="properties-passwordviewdefault"/>**PasswordViewDefault**

Get HotKey default for PasswordView 'F2'

```csharp
public static HotKey PasswordViewDefault { get; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### <a id="properties-removeitemdefault"/>**RemoveItemDefault**

Get HotKey default for Remove item 'F3'

```csharp
public static HotKey RemoveItemDefault { get; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### <a id="properties-selectalldefault"/>**SelectAllDefault**

Get HotKey default for Select All 'F2'

```csharp
public static HotKey SelectAllDefault { get; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### <a id="properties-toggleexpandallnodedefault"/>**ToggleExpandAllNodeDefault**

Get HotKey default for expand node 'F4'

```csharp
public static HotKey ToggleExpandAllNodeDefault { get; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### <a id="properties-toggleexpandnodedefault"/>**ToggleExpandNodeDefault**

Get HotKey default for expand node 'F3'

```csharp
public static HotKey ToggleExpandNodeDefault { get; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### <a id="properties-tooltipdefault"/>**TooltipDefault**

Get HotKey default for Tooltip 'F1'

```csharp
public static HotKey TooltipDefault { get; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### <a id="properties-tooltipfullpathdefault"/>**TooltipFullPathDefault**

Get HotKey default for Tooltip FullPath 'F2'

```csharp
public static HotKey TooltipFullPathDefault { get; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

## Constructors

### <a id="constructors-.ctor"/>**HotKey()**

Create a HotKey

```csharp
HotKey()
```

**Remarks:**

Do not use this constructor!

### <a id="constructors-.ctor"/>**HotKey(UserHotKey, Boolean, Boolean, Boolean)**

Create a HotKey

```csharp
HotKey(UserHotKey key, bool shift, bool alt, bool ctrl)
```

#### Parameters

`key` [UserHotKey](./pplus.controls.userhotkey.md)<br>
[UserHotKey](./pplus.controls.userhotkey.md) to create

`shift` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
With Shift key

`alt` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
With Alt key

`ctrl` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
With Ctrl key

## Methods

### <a id="methods-equals"/>**Equals(ConsoleKeyInfo)**

Checks Hotkey instances are equal the .

```csharp
bool Equals(ConsoleKeyInfo other)
```

#### Parameters

`other` ConsoleKeyInfo<br>
The ConsoleKeyInfo to compare.

#### Returns

`true` if the Hotkey are equal, otherwise `false`.

### <a id="methods-tostring"/>**ToString()**

```csharp
string ToString()
```

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)


- - -
[**Back to List Api**](./apis.md)
