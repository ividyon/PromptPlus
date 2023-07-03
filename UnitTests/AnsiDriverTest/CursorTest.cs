﻿using PPlus.Tests.Util;

namespace PPlus.Tests.AnsiDriverTest
{
    
    public class CursorTest : BaseTest
    {
        [Fact]
        public void Should_Cursor_Default()
        {
            Assert.True(PromptPlus.Console.CursorVisible);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_Change_Cursor(bool expected)
        {
            PromptPlus.Console.CursorVisible = expected;
            // Then
            Assert.Equal(expected, PromptPlus.Console.CursorVisible);
        }


        [Theory]
        [InlineData(CursorDirection.Up,2,8)]
        [InlineData(CursorDirection.Down,2,12)]
        [InlineData(CursorDirection.Up, 100, 0)]
        [InlineData(CursorDirection.Down, 100, 110)]
        public void Should_move_Cursor_Updown(CursorDirection direction, int step, int expected)
        {
            PromptPlus.Console.SetCursorPosition(10, 10);
            PromptPlus.Console.MoveCursor(direction, step);
            // Then
            Assert.Equal(expected, PromptPlus.Console.CursorTop);
        }

        [Theory]
        [InlineData(CursorDirection.Left, 2, 8)]
        [InlineData(CursorDirection.Right, 2, 12)]
        [InlineData(CursorDirection.Left, 200, 0)]
        [InlineData(CursorDirection.Right, 200, 132)]
        public void Should_move_Cursor_LeftRight(CursorDirection direction, int step, int expected)
        {
            PromptPlus.Console.SetCursorPosition(10, 10);
            PromptPlus.Console.MoveCursor(direction, step);
            // Then
            Assert.Equal(expected, PromptPlus.Console.CursorLeft);
        }

    }
}
