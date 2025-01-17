﻿// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// This code was based on work from https://github.com/spectreconsole/spectre.console
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

namespace PPlus.Drivers.Colors
{
    internal static class ColorPalette
    {
        public static IReadOnlyList<Color> Legacy { get; }
        public static IReadOnlyList<Color> Standard { get; }
        public static IReadOnlyList<Color> EightBit { get; }

        static ColorPalette()
        {
            Legacy = GenerateLegacyPalette();
            Standard = GenerateStandardPalette(Legacy);
            EightBit = GenerateEightBitPalette(Standard);
        }

        internal static Color ExactOrClosest(ColorSystem system, Color color)
        {
            var exact = Exact(system, color);
            return exact ?? Closest(system, color);
        }

        private static Color? Exact(ColorSystem system, Color color)
        {
            if (system == ColorSystem.TrueColor)
            {
                return color;
            }

            var palette = system switch
            {
                ColorSystem.Legacy => Legacy,
                ColorSystem.Standard => Standard,
                ColorSystem.EightBit => EightBit,
                _ => throw new PromptPlusException("Not Supported ColorSystem"),
            };

            return palette
                .Where(c => c.Equals(color))
                .Cast<Color?>()
                .FirstOrDefault();
        }

        private static Color Closest(ColorSystem system, Color color)
        {
            if (system == ColorSystem.TrueColor)
            {
                return color;
            }

            var palette = system switch
            {
                ColorSystem.Legacy => Legacy,
                ColorSystem.Standard => Standard,
                ColorSystem.EightBit => EightBit,
                _ => throw new PromptPlusException($"Not Supported ColorSystem"),
            };

            // https://stackoverflow.com/a/9085524
            static double Distance(Color first, Color second)
            {
                var rmean = ((float)first.R + second.R) / 2;
                var r = first.R - second.R;
                var g = first.G - second.G;
                var b = first.B - second.B;
                return Math.Sqrt(
                    ((int)((512 + rmean) * r * r) >> 8)
                    + 4 * g * g
                    + ((int)((767 - rmean) * b * b) >> 8));
            }

            return Enumerable.Range(0, int.MaxValue)
                .Zip(palette, (id, other) => (Distance: Distance(other, color), Id: id, Color: other))
                .OrderBy(x => x.Distance)
                .FirstOrDefault().Color;
        }


        private static List<Color> GenerateLegacyPalette()
        {
            return new List<Color>
            {
                Color.Black,
                Color.Maroon,
                Color.Green,
                Color.Olive,
                Color.Navy,
                Color.Purple,
                Color.Teal,
                Color.Silver,
            };
        }

        private static List<Color> GenerateStandardPalette(IReadOnlyList<Color> legacy)
        {
            return new List<Color>(legacy)
            {
                Color.Grey,
                Color.Red,
                Color.Lime,
                Color.Yellow,
                Color.Blue,
                Color.Fuchsia,
                Color.Aqua,
                Color.White,
            };
        }

        private static List<Color> GenerateEightBitPalette(IReadOnlyList<Color> standard)
        {
            return new List<Color>(standard)
            {
                Color.Grey0,
                Color.NavyBlue,
                Color.DarkBlue,
                Color.Blue3,
                Color.Blue3_1,
                Color.Blue1,
                Color.DarkGreen,
                Color.DeepSkyBlue4,
                Color.DeepSkyBlue4_1,
                Color.DeepSkyBlue4_2,
                Color.DodgerBlue3,
                Color.DodgerBlue2,
                Color.Green4,
                Color.SpringGreen4,
                Color.Turquoise4,
                Color.DeepSkyBlue3,
                Color.DeepSkyBlue3_1,
                Color.DodgerBlue1,
                Color.Green3,
                Color.SpringGreen3,
                Color.DarkCyan,
                Color.LightSeaGreen,
                Color.DeepSkyBlue2,
                Color.DeepSkyBlue1,
                Color.Green3_1,
                Color.SpringGreen3_1,
                Color.SpringGreen2,
                Color.Cyan3,
                Color.DarkTurquoise,
                Color.Turquoise2,
                Color.Green1,
                Color.SpringGreen2_1,
                Color.SpringGreen1,
                Color.MediumSpringGreen,
                Color.Cyan2,
                Color.Cyan1,
                Color.DarkRed,
                Color.DeepPink4,
                Color.Purple4,
                Color.Purple4_1,
                Color.Purple3,
                Color.BlueViolet,
                Color.Orange4,
                Color.Grey37,
                Color.MediumPurple4,
                Color.SlateBlue3,
                Color.SlateBlue3_1,
                Color.RoyalBlue1,
                Color.Chartreuse4,
                Color.DarkSeaGreen4,
                Color.PaleTurquoise4,
                Color.SteelBlue,
                Color.SteelBlue3,
                Color.CornflowerBlue,
                Color.Chartreuse3,
                Color.DarkSeaGreen4_1,
                Color.CadetBlue,
                Color.CadetBlue_1,
                Color.SkyBlue3,
                Color.SteelBlue1,
                Color.Chartreuse3_1,
                Color.PaleGreen3,
                Color.SeaGreen3,
                Color.Aquamarine3,
                Color.MediumTurquoise,
                Color.SteelBlue1_1,
                Color.Chartreuse2,
                Color.SeaGreen2,
                Color.SeaGreen1,
                Color.SeaGreen1_1,
                Color.Aquamarine1,
                Color.DarkSlateGray2,
                Color.DarkRed_1,
                Color.DeepPink4_1,
                Color.DarkMagenta,
                Color.DarkMagenta_1,
                Color.DarkViolet,
                Color.Purple_1,
                Color.Orange4_1,
                Color.LightPink4,
                Color.Plum4,
                Color.MediumPurple3,
                Color.MediumPurple3_1,
                Color.SlateBlue1,
                Color.Yellow4,
                Color.Wheat4,
                Color.Grey53,
                Color.LightSlateGrey,
                Color.MediumPurple,
                Color.LightSlateBlue,
                Color.Yellow4_1,
                Color.DarkOliveGreen3,
                Color.DarkSeaGreen,
                Color.LightSkyBlue3,
                Color.LightSkyBlue3_1,
                Color.SkyBlue2,
                Color.Chartreuse2_1,
                Color.DarkOliveGreen3_1,
                Color.PaleGreen3_1,
                Color.DarkSeaGreen3,
                Color.DarkSlateGray3,
                Color.SkyBlue1,
                Color.Chartreuse1,
                Color.LightGreen,
                Color.LightGreen_1,
                Color.PaleGreen1,
                Color.Aquamarine1_1,
                Color.DarkSlateGray1,
                Color.Red3,
                Color.DeepPink4_2,
                Color.MediumVioletRed,
                Color.Magenta3,
                Color.DarkViolet_1,
                Color.Purple_2,
                Color.DarkOrange3,
                Color.IndianRed,
                Color.HotPink3,
                Color.MediumOrchid3,
                Color.MediumOrchid,
                Color.MediumPurple2,
                Color.DarkGoldenrod,
                Color.LightSalmon3,
                Color.RosyBrown,
                Color.Grey63,
                Color.MediumPurple2_1,
                Color.MediumPurple1,
                Color.Gold3,
                Color.DarkKhaki,
                Color.NavajoWhite3,
                Color.Grey69,
                Color.LightSteelBlue3,
                Color.LightSteelBlue,
                Color.Yellow3,
                Color.DarkOliveGreen3_2,
                Color.DarkSeaGreen3_1,
                Color.DarkSeaGreen2,
                Color.LightCyan3,
                Color.LightSkyBlue1,
                Color.GreenYellow,
                Color.DarkOliveGreen2,
                Color.PaleGreen1_1,
                Color.DarkSeaGreen2_1,
                Color.DarkSeaGreen1,
                Color.PaleTurquoise1,
                Color.Red3_1,
                Color.DeepPink3,
                Color.DeepPink3_1,
                Color.Magenta3_1,
                Color.Magenta3_2,
                Color.Magenta2,
                Color.DarkOrange3_1,
                Color.IndianRed_1,
                Color.HotPink3_1,
                Color.HotPink2,
                Color.Orchid,
                Color.MediumOrchid1,
                Color.Orange3,
                Color.LightSalmon3_1,
                Color.LightPink3,
                Color.Pink3,
                Color.Plum3,
                Color.Violet,
                Color.Gold3_1,
                Color.LightGoldenrod3,
                Color.Tan,
                Color.MistyRose3,
                Color.Thistle3,
                Color.Plum2,
                Color.Yellow3_1,
                Color.Khaki3,
                Color.LightGoldenrod2,
                Color.LightYellow3,
                Color.Grey84,
                Color.LightSteelBlue1,
                Color.Yellow2,
                Color.DarkOliveGreen1,
                Color.DarkOliveGreen1_1,
                Color.DarkSeaGreen1_1,
                Color.Honeydew2,
                Color.LightCyan1,
                Color.Red1,
                Color.DeepPink2,
                Color.DeepPink1,
                Color.DeepPink1_1,
                Color.Magenta2_1,
                Color.Magenta1,
                Color.OrangeRed1,
                Color.IndianRed1,
                Color.IndianRed1_1,
                Color.HotPink,
                Color.HotPink_1,
                Color.MediumOrchid1_1,
                Color.DarkOrange,
                Color.Salmon1,
                Color.LightCoral,
                Color.PaleVioletRed1,
                Color.Orchid2,
                Color.Orchid1,
                Color.Orange1,
                Color.SandyBrown,
                Color.LightSalmon1,
                Color.LightPink1,
                Color.Pink1,
                Color.Plum1,
                Color.Gold1,
                Color.LightGoldenrod2_1,
                Color.LightGoldenrod2_2,
                Color.NavajoWhite1,
                Color.MistyRose1,
                Color.Thistle1,
                Color.Yellow1,
                Color.LightGoldenrod1,
                Color.Khaki1,
                Color.Wheat1,
                Color.Cornsilk1,
                Color.Grey100,
                Color.Grey3,
                Color.Grey7,
                Color.Grey11,
                Color.Grey15,
                Color.Grey19,
                Color.Grey23,
                Color.Grey27,
                Color.Grey30,
                Color.Grey35,
                Color.Grey39,
                Color.Grey42,
                Color.Grey46,
                Color.Grey50,
                Color.Grey54,
                Color.Grey58,
                Color.Grey62,
                Color.Grey66,
                Color.Grey70,
                Color.Grey74,
                Color.Grey78,
                Color.Grey82,
                Color.Grey85,
                Color.Grey89,
                Color.Grey93,
            };
        }
    }
}
