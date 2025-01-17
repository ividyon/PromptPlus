# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:StateProcess 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# StateProcess

Namespace: PPlus.Controls

Represents The Process state

```csharp
public struct StateProcess
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [StateProcess](./pplus.controls.stateprocess.md)

## Properties

### <a id="properties-description"/>**Description**

Description of Task

```csharp
public string Description { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-elapsedtime"/>**ElapsedTime**

Elapsed Time of Task

```csharp
public TimeSpan ElapsedTime { get; }
```

#### Property Value

[TimeSpan](https://docs.microsoft.com/en-us/dotnet/api/system.timespan)<br>

### <a id="properties-exceptionprocess"/>**ExceptionProcess**

Exception of Task

```csharp
public Exception ExceptionProcess { get; }
```

#### Property Value

[Exception](https://docs.microsoft.com/en-us/dotnet/api/system.exception)<br>

### <a id="properties-id"/>**Id**

TaskTitle of Task

```csharp
public string Id { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-status"/>**Status**

Status of Task [TaskStatus](https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.taskstatus)

```csharp
public TaskStatus Status { get; }
```

#### Property Value

[TaskStatus](https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.taskstatus)<br>

### <a id="properties-stepmode"/>**StepMode**

Step Mode of Task [StateProcess.StepMode](./pplus.controls.stateprocess.md#stepmode)

```csharp
public StepMode StepMode { get; }
```

#### Property Value

[StepMode](./pplus.controls.stepmode.md)<br>

## Constructors

### <a id="constructors-.ctor"/>**StateProcess()**

Create a StateProcess

```csharp
StateProcess()
```

**Remarks:**

Do not use this constructor!

### <a id="constructors-.ctor"/>**StateProcess(String, String, TaskStatus, Exception, TimeSpan, StepMode)**

Create The Process state.Purpose only for unit testing

```csharp
StateProcess(string id, string description, TaskStatus status, Exception exceptionProcess, TimeSpan elapsedtime, StepMode stepMode)
```

#### Parameters

`id` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`status` [TaskStatus](https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.taskstatus)<br>

`exceptionProcess` [Exception](https://docs.microsoft.com/en-us/dotnet/api/system.exception)<br>

`elapsedtime` [TimeSpan](https://docs.microsoft.com/en-us/dotnet/api/system.timespan)<br>

`stepMode` [StepMode](./pplus.controls.stepmode.md)<br>


- - -
[**Back to List Api**](./apis.md)
