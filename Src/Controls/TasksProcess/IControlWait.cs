﻿// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Threading;

namespace PPlus.Controls
{
    ///<inheritdoc cref="IPromptControls{T}"/>
    /// <summary>
    /// Represents the interface with all Methods of the WaitTimer/WaitProcess control
    /// </summary>
    /// <typeparam name="T">typeof return</typeparam>
    public interface IControlWait<T> : IPromptControls<ResultWaitProcess<T>>
    {
        /// <summary>
        /// Execute a action foreach item of colletion passed as a parameter
        /// </summary>
        /// <param name="values">Colletion for interaction</param>
        /// <param name="action">Action to execute</param>
        /// <returns><see cref="IControlWait{T}"/></returns>
        IControlWait<T> Interaction<T1>(IEnumerable<T1> values, Action<IControlWait<T>,T1> action);

        /// <summary>
        /// Maximum number of concurrent tasks enable. Default vaue is number of processors.
        /// </summary>
        /// <param name="value">Number of concurrent tasks</param>
        /// <returns><see cref="IControlWait{T}"/></returns>
        IControlWait<T> MaxDegreeProcess(int value);

        /// <summary>
        /// Set Contex value for all tasks
        /// </summary>
        /// <param name="value">Context value</param>
        /// <returns><see cref="IControlWait{T}"/></returns>
        IControlWait<T> Context(T value);

        /// <summary>
        /// Overwrite Task Title . Default task title comes from the embedded resource.
        /// </summary>
        /// <param name="value">TaskTitle Task</param>
        /// <returns><see cref="IControlWait{T}"/></returns>
        IControlWait<T> TaskTitle(string value);

        /// <summary>
        /// Define if show Elapsed Time for each task.
        /// </summary>
        /// <returns><see cref="IControlWait{T}"/></returns>
        IControlWait<T> ShowElapsedTime();

        /// <summary>
        /// Custom config the control.
        /// </summary>
        /// <param name="context">Action to apply changes. <see cref="IPromptConfig"/></param>
        /// <returns><see cref="IControlWait{T}"/></returns>
        IControlWait<T> Config(Action<IPromptConfig> context);


        /// <summary>
        /// Finish answer to show when Wait process is completed.
        /// </summary>
        /// <param name="text">Text Finish answer</param>
        /// <returns><see cref="IControlWait{T}"/></returns>
        IControlWait<T> Finish(string text);

        /// <summary>
        /// Overwrite <see cref="SpinnersType"/>. Default value is SpinnersType.Ascii
        /// <br>When use custom spinner, if has unicode values console does not support it, the rendering may not be as expected</br>
        /// </summary>
        /// <param name="spinnersType">The <see cref="SpinnersType"/></param>
        /// <param name="SpinnerStyle">Style of spinner. <see cref="Style"/></param>
        /// <param name="speedAnimation">Number of mileseconds foreach interation of spinner. Valid only to SpinnersType.custom, otherwise will be ignored</param>
        /// <param name="customspinner">IEnumerable values for custom spinner. Valid only to SpinnersType.custom, otherwise will be ignored</param>
        /// <returns><see cref="IControlWait{T}"/></returns>
        IControlWait<T> Spinner(SpinnersType spinnersType, Style? SpinnerStyle = null, int? speedAnimation = null, IEnumerable<string>? customspinner = null);

        /// <summary>
        /// Add list of tasks to execute.
        /// </summary>
        /// <param name="stepMode">Sequential or parallel execution</param>
        /// <param name="process">list of tasks</param>
        /// <returns><see cref="IControlWait{T}"/></returns>
        IControlWait<T> AddStep(StepMode stepMode, params Action<EventWaitProcess<T>, CancellationToken>[] process);

        /// <summary>
        /// Add list of tasks to execute with title and description
        /// </summary>
        /// <param name="stepMode">Sequential or parallel execution</param>
        /// <param name="id">Id of tasks</param>
        /// <param name="label">Label of tasks</param>
        /// <param name="process">list of tasks</param>
        /// <returns><see cref="IControlWait{T}"/></returns>
        IControlWait<T> AddStep(StepMode stepMode, string? id, string? label, params Action<EventWaitProcess<T>,CancellationToken>[] process);
    }
}
