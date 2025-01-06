using System;
using System.Collections.Generic;

namespace AlphaX.Sheets.Drawing
{
    public struct Color : IEquatable<Color>
    {
        private static Dictionary<KnownColor, Color> _colorsCache;

        static Color()
        {
            _colorsCache = new Dictionary<KnownColor, Color>();
        }

        public byte A { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public Color(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public static Color FromArgb(byte a, byte r, byte g, byte b)
        {
            return new Color(a, r, g, b);
        }

        public bool Equals(Color color2)
        {
            return this.A == color2.A && this.R == color2.R && this.G == color2.G && this.B == color2.B;
        }

        public static bool operator ==(Color color1, Color color2)
        {
            return color1.Equals(color2);
        }

        public static bool operator !=(Color color1, Color color2)
        {
            return !color1.Equals(color2);
        }

        #region Colors
        public static Color Transparent
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Transparent, out color))
                {
                    color = FromArgb(0, 255, 255, 255);
                    _colorsCache.Add(KnownColor.Transparent, color);
                }

                return color;
            }
        }

        public static Color AliceBlue
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.AliceBlue, out color))
                {
                    color = FromArgb(255, 240, 248, 255);
                    _colorsCache.Add(KnownColor.AliceBlue, color);
                }

                return color;
            }
        }

        public static Color AntiqueWhite
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.AntiqueWhite, out color))
                {
                    color = FromArgb(255, 250, 235, 215);
                    _colorsCache.Add(KnownColor.AntiqueWhite, color);
                }

                return color;
            }
        }

        public static Color Aqua
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Aqua, out color))
                {
                    color = FromArgb(255, 0, 255, 255);
                    _colorsCache.Add(KnownColor.Aqua, color);
                }

                return color;
            }
        }

        public static Color Aquamarine
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Aquamarine, out color))
                {
                    color = FromArgb(255, 127, 255, 212);
                    _colorsCache.Add(KnownColor.Aquamarine, color);
                }

                return color;
            }
        }

        public static Color Azure
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Azure, out color))
                {
                    color = FromArgb(255, 240, 255, 255);
                    _colorsCache.Add(KnownColor.Azure, color);
                }

                return color;
            }
        }

        public static Color Beige
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Beige, out color))
                {
                    color = FromArgb(255, 245, 245, 220);
                    _colorsCache.Add(KnownColor.Beige, color);
                }

                return color;
            }
        }

        public static Color Bisque
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Bisque, out color))
                {
                    color = FromArgb(255, 255, 228, 196);
                    _colorsCache.Add(KnownColor.Bisque, color);
                }

                return color;
            }
        }

        public static Color Black
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Black, out color))
                {
                    color = FromArgb(255, 0, 0, 0);
                    _colorsCache.Add(KnownColor.Black, color);
                }

                return color;
            }
        }

        public static Color BlanchedAlmond
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.BlanchedAlmond, out color))
                {
                    color = FromArgb(255, 255, 235, 205);
                    _colorsCache.Add(KnownColor.BlanchedAlmond, color);
                }

                return color;
            }
        }

        public static Color Blue
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Blue, out color))
                {
                    color = FromArgb(255, 0, 0, 255);
                    _colorsCache.Add(KnownColor.Blue, color);
                }

                return color;
            }
        }

        public static Color BlueViolet
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.BlueViolet, out color))
                {
                    color = FromArgb(255, 138, 43, 226);
                    _colorsCache.Add(KnownColor.BlueViolet, color);
                }

                return color;
            }
        }

        public static Color Brown
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Brown, out color))
                {
                    color = FromArgb(255, 165, 42, 42);
                    _colorsCache.Add(KnownColor.Brown, color);
                }

                return color;
            }
        }

        public static Color BurlyWood
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.BurlyWood, out color))
                {
                    color = FromArgb(255, 222, 184, 135);
                    _colorsCache.Add(KnownColor.BurlyWood, color);
                }

                return color;
            }
        }

        public static Color CadetBlue
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.CadetBlue, out color))
                {
                    color = FromArgb(255, 95, 158, 160);
                    _colorsCache.Add(KnownColor.CadetBlue, color);
                }

                return color;
            }
        }

        public static Color Chartreuse
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Chartreuse, out color))
                {
                    color = FromArgb(255, 127, 255, 0);
                    _colorsCache.Add(KnownColor.Chartreuse, color);
                }

                return color;
            }
        }

        public static Color Chocolate
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Chocolate, out color))
                {
                    color = FromArgb(255, 210, 105, 30);
                    _colorsCache.Add(KnownColor.Chocolate, color);
                }

                return color;
            }
        }

        public static Color Coral
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Coral, out color))
                {
                    color = FromArgb(255, 255, 127, 80);
                    _colorsCache.Add(KnownColor.Coral, color);
                }

                return color;
            }
        }

        public static Color CornflowerBlue
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.CornflowerBlue, out color))
                {
                    color = FromArgb(255, 100, 149, 237);
                    _colorsCache.Add(KnownColor.CornflowerBlue, color);
                }

                return color;
            }
        }

        public static Color Cornsilk
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Cornsilk, out color))
                {
                    color = FromArgb(255, 255, 248, 220);
                    _colorsCache.Add(KnownColor.Cornsilk, color);
                }

                return color;
            }
        }

        public static Color Crimson
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Crimson, out color))
                {
                    color = FromArgb(255, 220, 20, 60);
                    _colorsCache.Add(KnownColor.Crimson, color);
                }

                return color;
            }
        }

        public static Color Cyan
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Cyan, out color))
                {
                    color = FromArgb(255, 0, 255, 255);
                    _colorsCache.Add(KnownColor.Cyan, color);
                }

                return color;
            }
        }

        public static Color DarkBlue
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.DarkBlue, out color))
                {
                    color = FromArgb(255, 0, 0, 139);
                    _colorsCache.Add(KnownColor.DarkBlue, color);
                }

                return color;
            }
        }

        public static Color DarkCyan
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.DarkCyan, out color))
                {
                    color = FromArgb(255, 0, 139, 139);
                    _colorsCache.Add(KnownColor.DarkCyan, color);
                }

                return color;
            }
        }

        public static Color DarkGoldenrod
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.DarkGoldenrod, out color))
                {
                    color = FromArgb(255, 184, 134, 11);
                    _colorsCache.Add(KnownColor.DarkGoldenrod, color);
                }

                return color;
            }
        }

        public static Color DarkGray
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.DarkGray, out color))
                {
                    color = FromArgb(255, 169, 169, 169);
                    _colorsCache.Add(KnownColor.DarkGray, color);
                }

                return color;
            }
        }

        public static Color DarkGreen
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.DarkGreen, out color))
                {
                    color = FromArgb(255, 0, 100, 0);
                    _colorsCache.Add(KnownColor.DarkGreen, color);
                }

                return color;
            }
        }

        public static Color DarkKhaki
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.DarkKhaki, out color))
                {
                    color = FromArgb(255, 189, 183, 107);
                    _colorsCache.Add(KnownColor.DarkKhaki, color);
                }

                return color;
            }
        }

        public static Color DarkMagenta
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.DarkMagenta, out color))
                {
                    color = FromArgb(255, 139, 0, 139);
                    _colorsCache.Add(KnownColor.DarkMagenta, color);
                }

                return color;
            }
        }

        public static Color DarkOliveGreen
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.DarkOliveGreen, out color))
                {
                    color = FromArgb(255, 85, 107, 47);
                    _colorsCache.Add(KnownColor.DarkOliveGreen, color);
                }

                return color;
            }
        }

        public static Color DarkOrange
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.DarkOrange, out color))
                {
                    color = FromArgb(255, 255, 140, 0);
                    _colorsCache.Add(KnownColor.DarkOrange, color);
                }

                return color;
            }
        }

        public static Color DarkOrchid
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.DarkOrchid, out color))
                {
                    color = FromArgb(255, 153, 50, 204);
                    _colorsCache.Add(KnownColor.DarkOrchid, color);
                }

                return color;
            }
        }

        public static Color DarkRed
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.DarkRed, out color))
                {
                    color = FromArgb(255, 139, 0, 0);
                    _colorsCache.Add(KnownColor.DarkRed, color);
                }

                return color;
            }
        }

        public static Color DarkSalmon
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.DarkSalmon, out color))
                {
                    color = FromArgb(255, 233, 150, 122);
                    _colorsCache.Add(KnownColor.DarkSalmon, color);
                }

                return color;
            }
        }

        public static Color DarkSeaGreen
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.DarkSeaGreen, out color))
                {
                    color = FromArgb(255, 143, 188, 139);
                    _colorsCache.Add(KnownColor.DarkSeaGreen, color);
                }

                return color;
            }
        }

        public static Color DarkSlateBlue
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.DarkSlateBlue, out color))
                {
                    color = FromArgb(255, 72, 61, 139);
                    _colorsCache.Add(KnownColor.DarkSlateBlue, color);
                }

                return color;
            }
        }

        public static Color DarkSlateGray
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.DarkSlateGray, out color))
                {
                    color = FromArgb(255, 47, 79, 79);
                    _colorsCache.Add(KnownColor.DarkSlateGray, color);
                }

                return color;
            }
        }

        public static Color DarkTurquoise
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.DarkTurquoise, out color))
                {
                    color = FromArgb(255, 0, 206, 209);
                    _colorsCache.Add(KnownColor.DarkTurquoise, color);
                }

                return color;
            }
        }

        public static Color DarkViolet
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.DarkViolet, out color))
                {
                    color = FromArgb(255, 148, 0, 211);
                    _colorsCache.Add(KnownColor.DarkViolet, color);
                }

                return color;
            }
        }

        public static Color DeepPink
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.DeepPink, out color))
                {
                    color = FromArgb(255, 255, 20, 147);
                    _colorsCache.Add(KnownColor.DeepPink, color);
                }

                return color;
            }
        }

        public static Color DeepSkyBlue
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.DeepSkyBlue, out color))
                {
                    color = FromArgb(255, 0, 191, 255);
                    _colorsCache.Add(KnownColor.DeepSkyBlue, color);
                }

                return color;
            }
        }

        public static Color DimGray
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.DimGray, out color))
                {
                    color = FromArgb(255, 105, 105, 105);
                    _colorsCache.Add(KnownColor.DimGray, color);
                }

                return color;
            }
        }

        public static Color DodgerBlue
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.DodgerBlue, out color))
                {
                    color = FromArgb(255, 30, 144, 255);
                    _colorsCache.Add(KnownColor.DodgerBlue, color);
                }

                return color;
            }
        }

        public static Color Firebrick
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Firebrick, out color))
                {
                    color = FromArgb(255, 178, 34, 34);
                    _colorsCache.Add(KnownColor.Firebrick, color);
                }

                return color;
            }
        }

        public static Color FloralWhite
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.FloralWhite, out color))
                {
                    color = FromArgb(255, 255, 250, 240);
                    _colorsCache.Add(KnownColor.FloralWhite, color);
                }

                return color;
            }
        }

        public static Color ForestGreen
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.ForestGreen, out color))
                {
                    color = FromArgb(255, 34, 139, 34);
                    _colorsCache.Add(KnownColor.ForestGreen, color);
                }

                return color;
            }
        }

        public static Color Fuchsia
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Fuchsia, out color))
                {
                    color = FromArgb(255, 255, 0, 255);
                    _colorsCache.Add(KnownColor.Fuchsia, color);
                }

                return color;
            }
        }

        public static Color Gainsboro
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Gainsboro, out color))
                {
                    color = FromArgb(255, 220, 220, 220);
                    _colorsCache.Add(KnownColor.Gainsboro, color);
                }

                return color;
            }
        }

        public static Color GhostWhite
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.GhostWhite, out color))
                {
                    color = FromArgb(255, 248, 248, 255);
                    _colorsCache.Add(KnownColor.GhostWhite, color);
                }

                return color;
            }
        }

        public static Color Gold
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Gold, out color))
                {
                    color = FromArgb(255, 255, 215, 0);
                    _colorsCache.Add(KnownColor.Gold, color);
                }

                return color;
            }
        }

        public static Color Goldenrod
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Goldenrod, out color))
                {
                    color = FromArgb(255, 218, 165, 32);
                    _colorsCache.Add(KnownColor.Goldenrod, color);
                }

                return color;
            }
        }

        public static Color Gray
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Gray, out color))
                {
                    color = FromArgb(255, 128, 128, 128);
                    _colorsCache.Add(KnownColor.Gray, color);
                }

                return color;
            }
        }

        public static Color Green
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Green, out color))
                {
                    color = FromArgb(255, 0, 128, 0);
                    _colorsCache.Add(KnownColor.Green, color);
                }

                return color;
            }
        }

        public static Color GreenYellow
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.GreenYellow, out color))
                {
                    color = FromArgb(255, 173, 255, 47);
                    _colorsCache.Add(KnownColor.GreenYellow, color);
                }

                return color;
            }
        }

        public static Color Honeydew
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Honeydew, out color))
                {
                    color = FromArgb(255, 240, 255, 240);
                    _colorsCache.Add(KnownColor.Honeydew, color);
                }

                return color;
            }
        }

        public static Color HotPink
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.HotPink, out color))
                {
                    color = FromArgb(255, 255, 105, 180);
                    _colorsCache.Add(KnownColor.HotPink, color);
                }

                return color;
            }
        }

        public static Color IndianRed
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.IndianRed, out color))
                {
                    color = FromArgb(255, 205, 92, 92);
                    _colorsCache.Add(KnownColor.IndianRed, color);
                }

                return color;
            }
        }

        public static Color Indigo
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Indigo, out color))
                {
                    color = FromArgb(255, 75, 0, 130);
                    _colorsCache.Add(KnownColor.Indigo, color);
                }

                return color;
            }
        }

        public static Color Ivory
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Ivory, out color))
                {
                    color = FromArgb(255, 255, 255, 240);
                    _colorsCache.Add(KnownColor.Ivory, color);
                }

                return color;
            }
        }

        public static Color Khaki
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Khaki, out color))
                {
                    color = FromArgb(255, 240, 230, 140);
                    _colorsCache.Add(KnownColor.Khaki, color);
                }

                return color;
            }
        }

        public static Color Lavender
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Lavender, out color))
                {
                    color = FromArgb(255, 230, 230, 250);
                    _colorsCache.Add(KnownColor.Lavender, color);
                }

                return color;
            }
        }

        public static Color LavenderBlush
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.LavenderBlush, out color))
                {
                    color = FromArgb(255, 255, 240, 245);
                    _colorsCache.Add(KnownColor.LavenderBlush, color);
                }

                return color;
            }
        }

        public static Color LawnGreen
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.LawnGreen, out color))
                {
                    color = FromArgb(255, 124, 252, 0);
                    _colorsCache.Add(KnownColor.LawnGreen, color);
                }

                return color;
            }
        }

        public static Color LemonChiffon
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.LemonChiffon, out color))
                {
                    color = FromArgb(255, 255, 250, 205);
                    _colorsCache.Add(KnownColor.LemonChiffon, color);
                }

                return color;
            }
        }

        public static Color LightBlue
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.LightBlue, out color))
                {
                    color = FromArgb(255, 173, 216, 230);
                    _colorsCache.Add(KnownColor.LightBlue, color);
                }

                return color;
            }
        }

        public static Color LightCoral
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.LightCoral, out color))
                {
                    color = FromArgb(255, 240, 128, 128);
                    _colorsCache.Add(KnownColor.LightCoral, color);
                }

                return color;
            }
        }

        public static Color LightCyan
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.LightCyan, out color))
                {
                    color = FromArgb(255, 224, 255, 255);
                    _colorsCache.Add(KnownColor.LightCyan, color);
                }

                return color;
            }
        }

        public static Color LightGoldenrodYellow
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.LightGoldenrodYellow, out color))
                {
                    color = FromArgb(255, 250, 250, 210);
                    _colorsCache.Add(KnownColor.LightGoldenrodYellow, color);
                }

                return color;
            }
        }

        public static Color LightGreen
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.LightGreen, out color))
                {
                    color = FromArgb(255, 144, 238, 144);
                    _colorsCache.Add(KnownColor.LightGreen, color);
                }

                return color;
            }
        }

        public static Color LightGray
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.LightGray, out color))
                {
                    color = FromArgb(255, 211, 211, 211);
                    _colorsCache.Add(KnownColor.LightGray, color);
                }

                return color;
            }
        }

        public static Color LightPink
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.LightPink, out color))
                {
                    color = FromArgb(255, 255, 182, 193);
                    _colorsCache.Add(KnownColor.LightPink, color);
                }

                return color;
            }
        }

        public static Color LightSalmon
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.LightSalmon, out color))
                {
                    color = FromArgb(255, 255, 160, 122);
                    _colorsCache.Add(KnownColor.LightSalmon, color);
                }

                return color;
            }
        }

        public static Color LightSeaGreen
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.LightSeaGreen, out color))
                {
                    color = FromArgb(255, 32, 178, 170);
                    _colorsCache.Add(KnownColor.LightSeaGreen, color);
                }

                return color;
            }
        }

        public static Color LightSkyBlue
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.LightSkyBlue, out color))
                {
                    color = FromArgb(255, 135, 206, 250);
                    _colorsCache.Add(KnownColor.LightSkyBlue, color);
                }

                return color;
            }
        }

        public static Color LightSlateGray
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.LightSlateGray, out color))
                {
                    color = FromArgb(255, 119, 136, 153);
                    _colorsCache.Add(KnownColor.LightSlateGray, color);
                }

                return color;
            }
        }

        public static Color LightSteelBlue
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.LightSteelBlue, out color))
                {
                    color = FromArgb(255, 176, 196, 222);
                    _colorsCache.Add(KnownColor.LightSteelBlue, color);
                }

                return color;
            }
        }

        public static Color LightYellow
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.LightYellow, out color))
                {
                    color = FromArgb(255, 255, 255, 224);
                    _colorsCache.Add(KnownColor.LightYellow, color);
                }

                return color;
            }
        }

        public static Color Lime
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Lime, out color))
                {
                    color = FromArgb(255, 0, 255, 0);
                    _colorsCache.Add(KnownColor.Lime, color);
                }

                return color;
            }
        }

        public static Color LimeGreen
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.LimeGreen, out color))
                {
                    color = FromArgb(255, 50, 205, 50);
                    _colorsCache.Add(KnownColor.LimeGreen, color);
                }

                return color;
            }
        }

        public static Color Linen
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Linen, out color))
                {
                    color = FromArgb(255, 250, 240, 230);
                    _colorsCache.Add(KnownColor.Linen, color);
                }

                return color;
            }
        }

        public static Color Magenta
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Magenta, out color))
                {
                    color = FromArgb(255, 255, 0, 255);
                    _colorsCache.Add(KnownColor.Magenta, color);
                }

                return color;
            }
        }

        public static Color Maroon
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Maroon, out color))
                {
                    color = FromArgb(255, 128, 0, 0);
                    _colorsCache.Add(KnownColor.Maroon, color);
                }

                return color;
            }
        }

        public static Color MediumAquamarine
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.MediumAquamarine, out color))
                {
                    color = FromArgb(255, 102, 205, 170);
                    _colorsCache.Add(KnownColor.MediumAquamarine, color);
                }

                return color;
            }
        }

        public static Color MediumBlue
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.MediumBlue, out color))
                {
                    color = FromArgb(255, 0, 0, 205);
                    _colorsCache.Add(KnownColor.MediumBlue, color);
                }

                return color;
            }
        }

        public static Color MediumOrchid
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.MediumOrchid, out color))
                {
                    color = FromArgb(255, 186, 85, 211);
                    _colorsCache.Add(KnownColor.MediumOrchid, color);
                }

                return color;
            }
        }

        public static Color MediumPurple
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.MediumPurple, out color))
                {
                    color = FromArgb(255, 147, 112, 219);
                    _colorsCache.Add(KnownColor.MediumPurple, color);
                }

                return color;
            }
        }

        public static Color MediumSeaGreen
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.MediumSeaGreen, out color))
                {
                    color = FromArgb(255, 60, 179, 113);
                    _colorsCache.Add(KnownColor.MediumSeaGreen, color);
                }

                return color;
            }
        }

        public static Color MediumSlateBlue
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.MediumSlateBlue, out color))
                {
                    color = FromArgb(255, 123, 104, 238);
                    _colorsCache.Add(KnownColor.MediumSlateBlue, color);
                }

                return color;
            }
        }

        public static Color MediumSpringGreen
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.MediumSpringGreen, out color))
                {
                    color = FromArgb(255, 0, 250, 154);
                    _colorsCache.Add(KnownColor.MediumSpringGreen, color);
                }

                return color;
            }
        }

        public static Color MediumTurquoise
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.MediumTurquoise, out color))
                {
                    color = FromArgb(255, 72, 209, 204);
                    _colorsCache.Add(KnownColor.MediumTurquoise, color);
                }

                return color;
            }
        }

        public static Color MediumVioletRed
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.MediumVioletRed, out color))
                {
                    color = FromArgb(255, 199, 21, 133);
                    _colorsCache.Add(KnownColor.MediumVioletRed, color);
                }

                return color;
            }
        }

        public static Color MidnightBlue
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.MidnightBlue, out color))
                {
                    color = FromArgb(255, 25, 25, 112);
                    _colorsCache.Add(KnownColor.MidnightBlue, color);
                }

                return color;
            }
        }

        public static Color MintCream
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.MintCream, out color))
                {
                    color = FromArgb(255, 245, 255, 250);
                    _colorsCache.Add(KnownColor.MintCream, color);
                }

                return color;
            }
        }

        public static Color MistyRose
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.MistyRose, out color))
                {
                    color = FromArgb(255, 255, 228, 225);
                    _colorsCache.Add(KnownColor.MistyRose, color);
                }

                return color;
            }
        }

        public static Color Moccasin
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Moccasin, out color))
                {
                    color = FromArgb(255, 255, 228, 181);
                    _colorsCache.Add(KnownColor.Moccasin, color);
                }

                return color;
            }
        }

        public static Color NavajoWhite
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.NavajoWhite, out color))
                {
                    color = FromArgb(255, 255, 222, 173);
                    _colorsCache.Add(KnownColor.NavajoWhite, color);
                }

                return color;
            }
        }

        public static Color Navy
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Navy, out color))
                {
                    color = FromArgb(255, 0, 0, 128);
                    _colorsCache.Add(KnownColor.Navy, color);
                }

                return color;
            }
        }

        public static Color OldLace
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.OldLace, out color))
                {
                    color = FromArgb(255, 253, 245, 230);
                    _colorsCache.Add(KnownColor.OldLace, color);
                }

                return color;
            }
        }

        public static Color Olive
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Olive, out color))
                {
                    color = FromArgb(255, 128, 128, 0);
                    _colorsCache.Add(KnownColor.Olive, color);
                }

                return color;
            }
        }

        public static Color OliveDrab
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.OliveDrab, out color))
                {
                    color = FromArgb(255, 107, 142, 35);
                    _colorsCache.Add(KnownColor.OliveDrab, color);
                }

                return color;
            }
        }

        public static Color Orange
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Orange, out color))
                {
                    color = FromArgb(255, 255, 165, 0);
                    _colorsCache.Add(KnownColor.Orange, color);
                }

                return color;
            }
        }

        public static Color OrangeRed
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.OrangeRed, out color))
                {
                    color = FromArgb(255, 255, 69, 0);
                    _colorsCache.Add(KnownColor.OrangeRed, color);
                }

                return color;
            }
        }

        public static Color Orchid
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Orchid, out color))
                {
                    color = FromArgb(255, 218, 112, 214);
                    _colorsCache.Add(KnownColor.Orchid, color);
                }

                return color;
            }
        }

        public static Color PaleGoldenrod
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.PaleGoldenrod, out color))
                {
                    color = FromArgb(255, 238, 232, 170);
                    _colorsCache.Add(KnownColor.PaleGoldenrod, color);
                }

                return color;
            }
        }

        public static Color PaleGreen
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.PaleGreen, out color))
                {
                    color = FromArgb(255, 152, 251, 152);
                    _colorsCache.Add(KnownColor.PaleGreen, color);
                }

                return color;
            }
        }

        public static Color PaleTurquoise
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.PaleTurquoise, out color))
                {
                    color = FromArgb(255, 175, 238, 238);
                    _colorsCache.Add(KnownColor.PaleTurquoise, color);
                }

                return color;
            }
        }

        public static Color PaleVioletRed
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.PaleVioletRed, out color))
                {
                    color = FromArgb(255, 219, 112, 147);
                    _colorsCache.Add(KnownColor.PaleVioletRed, color);
                }

                return color;
            }
        }

        public static Color PapayaWhip
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.PapayaWhip, out color))
                {
                    color = FromArgb(255, 255, 239, 213);
                    _colorsCache.Add(KnownColor.PapayaWhip, color);
                }

                return color;
            }
        }

        public static Color PeachPuff
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.PeachPuff, out color))
                {
                    color = FromArgb(255, 255, 218, 185);
                    _colorsCache.Add(KnownColor.PeachPuff, color);
                }

                return color;
            }
        }

        public static Color Peru
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Peru, out color))
                {
                    color = FromArgb(255, 205, 133, 63);
                    _colorsCache.Add(KnownColor.Peru, color);
                }

                return color;
            }
        }

        public static Color Pink
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Pink, out color))
                {
                    color = FromArgb(255, 255, 192, 203);
                    _colorsCache.Add(KnownColor.Pink, color);
                }

                return color;
            }
        }

        public static Color Plum
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Plum, out color))
                {
                    color = FromArgb(255, 221, 160, 221);
                    _colorsCache.Add(KnownColor.Plum, color);
                }

                return color;
            }
        }

        public static Color PowderBlue
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.PowderBlue, out color))
                {
                    color = FromArgb(255, 176, 224, 230);
                    _colorsCache.Add(KnownColor.PowderBlue, color);
                }

                return color;
            }
        }

        public static Color Purple
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Purple, out color))
                {
                    color = FromArgb(255, 128, 0, 128);
                    _colorsCache.Add(KnownColor.Purple, color);
                }

                return color;
            }
        }

        public static Color Red
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Red, out color))
                {
                    color = FromArgb(255, 255, 0, 0);
                    _colorsCache.Add(KnownColor.Red, color);
                }

                return color;
            }
        }

        public static Color RosyBrown
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.RosyBrown, out color))
                {
                    color = FromArgb(255, 188, 143, 143);
                    _colorsCache.Add(KnownColor.RosyBrown, color);
                }

                return color;
            }
        }

        public static Color RoyalBlue
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.RoyalBlue, out color))
                {
                    color = FromArgb(255, 65, 105, 225);
                    _colorsCache.Add(KnownColor.RoyalBlue, color);
                }

                return color;
            }
        }

        public static Color SaddleBrown
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.SaddleBrown, out color))
                {
                    color = FromArgb(255, 139, 69, 19);
                    _colorsCache.Add(KnownColor.SaddleBrown, color);
                }

                return color;
            }
        }

        public static Color Salmon
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Salmon, out color))
                {
                    color = FromArgb(255, 250, 128, 114);
                    _colorsCache.Add(KnownColor.Salmon, color);
                }

                return color;
            }
        }

        public static Color SandyBrown
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.SandyBrown, out color))
                {
                    color = FromArgb(255, 244, 164, 96);
                    _colorsCache.Add(KnownColor.SandyBrown, color);
                }

                return color;
            }
        }

        public static Color SeaGreen
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.SeaGreen, out color))
                {
                    color = FromArgb(255, 46, 139, 87);
                    _colorsCache.Add(KnownColor.SeaGreen, color);
                }

                return color;
            }
        }

        public static Color SeaShell
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.SeaShell, out color))
                {
                    color = FromArgb(255, 255, 245, 238);
                    _colorsCache.Add(KnownColor.SeaShell, color);
                }

                return color;
            }
        }

        public static Color Sienna
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Sienna, out color))
                {
                    color = FromArgb(255, 160, 82, 45);
                    _colorsCache.Add(KnownColor.Sienna, color);
                }

                return color;
            }
        }

        public static Color Silver
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Silver, out color))
                {
                    color = FromArgb(255, 192, 192, 192);
                    _colorsCache.Add(KnownColor.Silver, color);
                }

                return color;
            }
        }

        public static Color SkyBlue
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.SkyBlue, out color))
                {
                    color = FromArgb(255, 135, 206, 235);
                    _colorsCache.Add(KnownColor.SkyBlue, color);
                }

                return color;
            }
        }

        public static Color SlateBlue
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.SlateBlue, out color))
                {
                    color = FromArgb(255, 106, 90, 205);
                    _colorsCache.Add(KnownColor.SlateBlue, color);
                }

                return color;
            }
        }

        public static Color SlateGray
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.SlateGray, out color))
                {
                    color = FromArgb(255, 112, 128, 144);
                    _colorsCache.Add(KnownColor.SlateGray, color);
                }

                return color;
            }
        }

        public static Color Snow
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Snow, out color))
                {
                    color = FromArgb(255, 255, 250, 250);
                    _colorsCache.Add(KnownColor.Snow, color);
                }

                return color;
            }
        }

        public static Color SpringGreen
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.SpringGreen, out color))
                {
                    color = FromArgb(255, 0, 255, 127);
                    _colorsCache.Add(KnownColor.SpringGreen, color);
                }

                return color;
            }
        }

        public static Color SteelBlue
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.SteelBlue, out color))
                {
                    color = FromArgb(255, 70, 130, 180);
                    _colorsCache.Add(KnownColor.SteelBlue, color);
                }

                return color;
            }
        }

        public static Color Tan
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Tan, out color))
                {
                    color = FromArgb(255, 210, 180, 140);
                    _colorsCache.Add(KnownColor.Tan, color);
                }

                return color;
            }
        }

        public static Color Teal
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Teal, out color))
                {
                    color = FromArgb(255, 0, 128, 128);
                    _colorsCache.Add(KnownColor.Teal, color);
                }

                return color;
            }
        }

        public static Color Thistle
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Thistle, out color))
                {
                    color = FromArgb(255, 216, 191, 216);
                    _colorsCache.Add(KnownColor.Thistle, color);
                }

                return color;
            }
        }

        public static Color Tomato
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Tomato, out color))
                {
                    color = FromArgb(255, 255, 99, 71);
                    _colorsCache.Add(KnownColor.Tomato, color);
                }

                return color;
            }
        }

        public static Color Turquoise
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Turquoise, out color))
                {
                    color = FromArgb(255, 64, 224, 208);
                    _colorsCache.Add(KnownColor.Turquoise, color);
                }

                return color;
            }
        }

        public static Color Violet
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Violet, out color))
                {
                    color = FromArgb(255, 238, 130, 238);
                    _colorsCache.Add(KnownColor.Violet, color);
                }

                return color;
            }
        }

        public static Color Wheat
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Wheat, out color))
                {
                    color = FromArgb(255, 245, 222, 179);
                    _colorsCache.Add(KnownColor.Wheat, color);
                }

                return color;
            }
        }

        public static Color White
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.White, out color))
                {
                    color = FromArgb(255, 255, 255, 255);
                    _colorsCache.Add(KnownColor.White, color);
                }

                return color;
            }
        }

        public static Color WhiteSmoke
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.WhiteSmoke, out color))
                {
                    color = FromArgb(255, 245, 245, 245);
                    _colorsCache.Add(KnownColor.WhiteSmoke, color);
                }

                return color;
            }
        }

        public static Color Yellow
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.Yellow, out color))
                {
                    color = FromArgb(255, 255, 255, 0);
                    _colorsCache.Add(KnownColor.Yellow, color);
                }

                return color;
            }
        }

        public static Color YellowGreen
        {
            get
            {
                Color color;
                if (!_colorsCache.TryGetValue(KnownColor.YellowGreen, out color))
                {
                    color = FromArgb(255, 154, 205, 50);
                    _colorsCache.Add(KnownColor.YellowGreen, color);
                }

                return color;
            }
        }
        #endregion
    }
}
