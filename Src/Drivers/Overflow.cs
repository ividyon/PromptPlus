﻿// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// This code was based on work from https://github.com/spectreconsole/spectre.console
// ***************************************************************************************

namespace PPlus
{
    /// <summary>
    /// Represents text overflow.
    /// </summary>
    public enum Overflow
    {
        /// <summary>
        /// Put any excess characters on the next line.
        /// </summary>
        None = 0,

        /// <summary>
        /// Truncates the text at the end of the line.
        /// </summary>
        Crop = 1,

        /// <summary>
        /// Truncates the text at the end of the line and also inserts an ellipsis character.
        /// </summary>
        Ellipsis = 2,
    }
}
