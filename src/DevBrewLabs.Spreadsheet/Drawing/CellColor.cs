using System;
using System.Collections.Generic;

namespace DevBrewLabs.Spreadsheet.Drawing
{
    public struct CellColor : IEquatable<CellColor>
    {
        private static Dictionary<CellKnownColor, CellColor> _colorsCache;

        static CellColor()
        {
            _colorsCache = new Dictionary<CellKnownColor, CellColor>();
        }

        public byte A { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public CellColor(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public static CellColor FromArgb(byte a, byte r, byte g, byte b)
        {
            return new CellColor(a, r, g, b);
        }

        public bool Equals(CellColor color2)
        {
            return this.A == color2.A && this.R == color2.R && this.G == color2.G && this.B == color2.B;
        }

        public static bool operator ==(CellColor color1, CellColor color2)
        {
            return color1.Equals(color2);
        }

        public static bool operator !=(CellColor color1, CellColor color2)
        {
            return !color1.Equals(color2);
        }

        #region Colors
        public static CellColor Transparent
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Transparent, out color))
                {
                    color = FromArgb(0, 255, 255, 255);
                    _colorsCache.Add(CellKnownColor.Transparent, color);
                }

                return color;
            }
        }

        public static CellColor AliceBlue
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.AliceBlue, out color))
                {
                    color = FromArgb(255, 240, 248, 255);
                    _colorsCache.Add(CellKnownColor.AliceBlue, color);
                }

                return color;
            }
        }

        public static CellColor AntiqueWhite
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.AntiqueWhite, out color))
                {
                    color = FromArgb(255, 250, 235, 215);
                    _colorsCache.Add(CellKnownColor.AntiqueWhite, color);
                }

                return color;
            }
        }

        public static CellColor Aqua
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Aqua, out color))
                {
                    color = FromArgb(255, 0, 255, 255);
                    _colorsCache.Add(CellKnownColor.Aqua, color);
                }

                return color;
            }
        }

        public static CellColor Aquamarine
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Aquamarine, out color))
                {
                    color = FromArgb(255, 127, 255, 212);
                    _colorsCache.Add(CellKnownColor.Aquamarine, color);
                }

                return color;
            }
        }

        public static CellColor Azure
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Azure, out color))
                {
                    color = FromArgb(255, 240, 255, 255);
                    _colorsCache.Add(CellKnownColor.Azure, color);
                }

                return color;
            }
        }

        public static CellColor Beige
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Beige, out color))
                {
                    color = FromArgb(255, 245, 245, 220);
                    _colorsCache.Add(CellKnownColor.Beige, color);
                }

                return color;
            }
        }

        public static CellColor Bisque
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Bisque, out color))
                {
                    color = FromArgb(255, 255, 228, 196);
                    _colorsCache.Add(CellKnownColor.Bisque, color);
                }

                return color;
            }
        }

        public static CellColor Black
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Black, out color))
                {
                    color = FromArgb(255, 0, 0, 0);
                    _colorsCache.Add(CellKnownColor.Black, color);
                }

                return color;
            }
        }

        public static CellColor BlanchedAlmond
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.BlanchedAlmond, out color))
                {
                    color = FromArgb(255, 255, 235, 205);
                    _colorsCache.Add(CellKnownColor.BlanchedAlmond, color);
                }

                return color;
            }
        }

        public static CellColor Blue
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Blue, out color))
                {
                    color = FromArgb(255, 0, 0, 255);
                    _colorsCache.Add(CellKnownColor.Blue, color);
                }

                return color;
            }
        }

        public static CellColor BlueViolet
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.BlueViolet, out color))
                {
                    color = FromArgb(255, 138, 43, 226);
                    _colorsCache.Add(CellKnownColor.BlueViolet, color);
                }

                return color;
            }
        }

        public static CellColor Brown
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Brown, out color))
                {
                    color = FromArgb(255, 165, 42, 42);
                    _colorsCache.Add(CellKnownColor.Brown, color);
                }

                return color;
            }
        }

        public static CellColor BurlyWood
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.BurlyWood, out color))
                {
                    color = FromArgb(255, 222, 184, 135);
                    _colorsCache.Add(CellKnownColor.BurlyWood, color);
                }

                return color;
            }
        }

        public static CellColor CadetBlue
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.CadetBlue, out color))
                {
                    color = FromArgb(255, 95, 158, 160);
                    _colorsCache.Add(CellKnownColor.CadetBlue, color);
                }

                return color;
            }
        }

        public static CellColor Chartreuse
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Chartreuse, out color))
                {
                    color = FromArgb(255, 127, 255, 0);
                    _colorsCache.Add(CellKnownColor.Chartreuse, color);
                }

                return color;
            }
        }

        public static CellColor Chocolate
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Chocolate, out color))
                {
                    color = FromArgb(255, 210, 105, 30);
                    _colorsCache.Add(CellKnownColor.Chocolate, color);
                }

                return color;
            }
        }

        public static CellColor Coral
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Coral, out color))
                {
                    color = FromArgb(255, 255, 127, 80);
                    _colorsCache.Add(CellKnownColor.Coral, color);
                }

                return color;
            }
        }

        public static CellColor CornflowerBlue
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.CornflowerBlue, out color))
                {
                    color = FromArgb(255, 100, 149, 237);
                    _colorsCache.Add(CellKnownColor.CornflowerBlue, color);
                }

                return color;
            }
        }

        public static CellColor Cornsilk
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Cornsilk, out color))
                {
                    color = FromArgb(255, 255, 248, 220);
                    _colorsCache.Add(CellKnownColor.Cornsilk, color);
                }

                return color;
            }
        }

        public static CellColor Crimson
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Crimson, out color))
                {
                    color = FromArgb(255, 220, 20, 60);
                    _colorsCache.Add(CellKnownColor.Crimson, color);
                }

                return color;
            }
        }

        public static CellColor Cyan
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Cyan, out color))
                {
                    color = FromArgb(255, 0, 255, 255);
                    _colorsCache.Add(CellKnownColor.Cyan, color);
                }

                return color;
            }
        }

        public static CellColor DarkBlue
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.DarkBlue, out color))
                {
                    color = FromArgb(255, 0, 0, 139);
                    _colorsCache.Add(CellKnownColor.DarkBlue, color);
                }

                return color;
            }
        }

        public static CellColor DarkCyan
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.DarkCyan, out color))
                {
                    color = FromArgb(255, 0, 139, 139);
                    _colorsCache.Add(CellKnownColor.DarkCyan, color);
                }

                return color;
            }
        }

        public static CellColor DarkGoldenrod
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.DarkGoldenrod, out color))
                {
                    color = FromArgb(255, 184, 134, 11);
                    _colorsCache.Add(CellKnownColor.DarkGoldenrod, color);
                }

                return color;
            }
        }

        public static CellColor DarkGray
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.DarkGray, out color))
                {
                    color = FromArgb(255, 169, 169, 169);
                    _colorsCache.Add(CellKnownColor.DarkGray, color);
                }

                return color;
            }
        }

        public static CellColor DarkGreen
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.DarkGreen, out color))
                {
                    color = FromArgb(255, 0, 100, 0);
                    _colorsCache.Add(CellKnownColor.DarkGreen, color);
                }

                return color;
            }
        }

        public static CellColor DarkKhaki
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.DarkKhaki, out color))
                {
                    color = FromArgb(255, 189, 183, 107);
                    _colorsCache.Add(CellKnownColor.DarkKhaki, color);
                }

                return color;
            }
        }

        public static CellColor DarkMagenta
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.DarkMagenta, out color))
                {
                    color = FromArgb(255, 139, 0, 139);
                    _colorsCache.Add(CellKnownColor.DarkMagenta, color);
                }

                return color;
            }
        }

        public static CellColor DarkOliveGreen
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.DarkOliveGreen, out color))
                {
                    color = FromArgb(255, 85, 107, 47);
                    _colorsCache.Add(CellKnownColor.DarkOliveGreen, color);
                }

                return color;
            }
        }

        public static CellColor DarkOrange
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.DarkOrange, out color))
                {
                    color = FromArgb(255, 255, 140, 0);
                    _colorsCache.Add(CellKnownColor.DarkOrange, color);
                }

                return color;
            }
        }

        public static CellColor DarkOrchid
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.DarkOrchid, out color))
                {
                    color = FromArgb(255, 153, 50, 204);
                    _colorsCache.Add(CellKnownColor.DarkOrchid, color);
                }

                return color;
            }
        }

        public static CellColor DarkRed
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.DarkRed, out color))
                {
                    color = FromArgb(255, 139, 0, 0);
                    _colorsCache.Add(CellKnownColor.DarkRed, color);
                }

                return color;
            }
        }

        public static CellColor DarkSalmon
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.DarkSalmon, out color))
                {
                    color = FromArgb(255, 233, 150, 122);
                    _colorsCache.Add(CellKnownColor.DarkSalmon, color);
                }

                return color;
            }
        }

        public static CellColor DarkSeaGreen
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.DarkSeaGreen, out color))
                {
                    color = FromArgb(255, 143, 188, 139);
                    _colorsCache.Add(CellKnownColor.DarkSeaGreen, color);
                }

                return color;
            }
        }

        public static CellColor DarkSlateBlue
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.DarkSlateBlue, out color))
                {
                    color = FromArgb(255, 72, 61, 139);
                    _colorsCache.Add(CellKnownColor.DarkSlateBlue, color);
                }

                return color;
            }
        }

        public static CellColor DarkSlateGray
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.DarkSlateGray, out color))
                {
                    color = FromArgb(255, 47, 79, 79);
                    _colorsCache.Add(CellKnownColor.DarkSlateGray, color);
                }

                return color;
            }
        }

        public static CellColor DarkTurquoise
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.DarkTurquoise, out color))
                {
                    color = FromArgb(255, 0, 206, 209);
                    _colorsCache.Add(CellKnownColor.DarkTurquoise, color);
                }

                return color;
            }
        }

        public static CellColor DarkViolet
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.DarkViolet, out color))
                {
                    color = FromArgb(255, 148, 0, 211);
                    _colorsCache.Add(CellKnownColor.DarkViolet, color);
                }

                return color;
            }
        }

        public static CellColor DeepPink
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.DeepPink, out color))
                {
                    color = FromArgb(255, 255, 20, 147);
                    _colorsCache.Add(CellKnownColor.DeepPink, color);
                }

                return color;
            }
        }

        public static CellColor DeepSkyBlue
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.DeepSkyBlue, out color))
                {
                    color = FromArgb(255, 0, 191, 255);
                    _colorsCache.Add(CellKnownColor.DeepSkyBlue, color);
                }

                return color;
            }
        }

        public static CellColor DimGray
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.DimGray, out color))
                {
                    color = FromArgb(255, 105, 105, 105);
                    _colorsCache.Add(CellKnownColor.DimGray, color);
                }

                return color;
            }
        }

        public static CellColor DodgerBlue
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.DodgerBlue, out color))
                {
                    color = FromArgb(255, 30, 144, 255);
                    _colorsCache.Add(CellKnownColor.DodgerBlue, color);
                }

                return color;
            }
        }

        public static CellColor Firebrick
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Firebrick, out color))
                {
                    color = FromArgb(255, 178, 34, 34);
                    _colorsCache.Add(CellKnownColor.Firebrick, color);
                }

                return color;
            }
        }

        public static CellColor FloralWhite
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.FloralWhite, out color))
                {
                    color = FromArgb(255, 255, 250, 240);
                    _colorsCache.Add(CellKnownColor.FloralWhite, color);
                }

                return color;
            }
        }

        public static CellColor ForestGreen
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.ForestGreen, out color))
                {
                    color = FromArgb(255, 34, 139, 34);
                    _colorsCache.Add(CellKnownColor.ForestGreen, color);
                }

                return color;
            }
        }

        public static CellColor Fuchsia
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Fuchsia, out color))
                {
                    color = FromArgb(255, 255, 0, 255);
                    _colorsCache.Add(CellKnownColor.Fuchsia, color);
                }

                return color;
            }
        }

        public static CellColor Gainsboro
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Gainsboro, out color))
                {
                    color = FromArgb(255, 220, 220, 220);
                    _colorsCache.Add(CellKnownColor.Gainsboro, color);
                }

                return color;
            }
        }

        public static CellColor GhostWhite
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.GhostWhite, out color))
                {
                    color = FromArgb(255, 248, 248, 255);
                    _colorsCache.Add(CellKnownColor.GhostWhite, color);
                }

                return color;
            }
        }

        public static CellColor Gold
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Gold, out color))
                {
                    color = FromArgb(255, 255, 215, 0);
                    _colorsCache.Add(CellKnownColor.Gold, color);
                }

                return color;
            }
        }

        public static CellColor Goldenrod
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Goldenrod, out color))
                {
                    color = FromArgb(255, 218, 165, 32);
                    _colorsCache.Add(CellKnownColor.Goldenrod, color);
                }

                return color;
            }
        }

        public static CellColor Gray
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Gray, out color))
                {
                    color = FromArgb(255, 128, 128, 128);
                    _colorsCache.Add(CellKnownColor.Gray, color);
                }

                return color;
            }
        }

        public static CellColor Green
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Green, out color))
                {
                    color = FromArgb(255, 0, 128, 0);
                    _colorsCache.Add(CellKnownColor.Green, color);
                }

                return color;
            }
        }

        public static CellColor GreenYellow
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.GreenYellow, out color))
                {
                    color = FromArgb(255, 173, 255, 47);
                    _colorsCache.Add(CellKnownColor.GreenYellow, color);
                }

                return color;
            }
        }

        public static CellColor Honeydew
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Honeydew, out color))
                {
                    color = FromArgb(255, 240, 255, 240);
                    _colorsCache.Add(CellKnownColor.Honeydew, color);
                }

                return color;
            }
        }

        public static CellColor HotPink
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.HotPink, out color))
                {
                    color = FromArgb(255, 255, 105, 180);
                    _colorsCache.Add(CellKnownColor.HotPink, color);
                }

                return color;
            }
        }

        public static CellColor IndianRed
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.IndianRed, out color))
                {
                    color = FromArgb(255, 205, 92, 92);
                    _colorsCache.Add(CellKnownColor.IndianRed, color);
                }

                return color;
            }
        }

        public static CellColor Indigo
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Indigo, out color))
                {
                    color = FromArgb(255, 75, 0, 130);
                    _colorsCache.Add(CellKnownColor.Indigo, color);
                }

                return color;
            }
        }

        public static CellColor Ivory
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Ivory, out color))
                {
                    color = FromArgb(255, 255, 255, 240);
                    _colorsCache.Add(CellKnownColor.Ivory, color);
                }

                return color;
            }
        }

        public static CellColor Khaki
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Khaki, out color))
                {
                    color = FromArgb(255, 240, 230, 140);
                    _colorsCache.Add(CellKnownColor.Khaki, color);
                }

                return color;
            }
        }

        public static CellColor Lavender
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Lavender, out color))
                {
                    color = FromArgb(255, 230, 230, 250);
                    _colorsCache.Add(CellKnownColor.Lavender, color);
                }

                return color;
            }
        }

        public static CellColor LavenderBlush
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.LavenderBlush, out color))
                {
                    color = FromArgb(255, 255, 240, 245);
                    _colorsCache.Add(CellKnownColor.LavenderBlush, color);
                }

                return color;
            }
        }

        public static CellColor LawnGreen
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.LawnGreen, out color))
                {
                    color = FromArgb(255, 124, 252, 0);
                    _colorsCache.Add(CellKnownColor.LawnGreen, color);
                }

                return color;
            }
        }

        public static CellColor LemonChiffon
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.LemonChiffon, out color))
                {
                    color = FromArgb(255, 255, 250, 205);
                    _colorsCache.Add(CellKnownColor.LemonChiffon, color);
                }

                return color;
            }
        }

        public static CellColor LightBlue
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.LightBlue, out color))
                {
                    color = FromArgb(255, 173, 216, 230);
                    _colorsCache.Add(CellKnownColor.LightBlue, color);
                }

                return color;
            }
        }

        public static CellColor LightCoral
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.LightCoral, out color))
                {
                    color = FromArgb(255, 240, 128, 128);
                    _colorsCache.Add(CellKnownColor.LightCoral, color);
                }

                return color;
            }
        }

        public static CellColor LightCyan
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.LightCyan, out color))
                {
                    color = FromArgb(255, 224, 255, 255);
                    _colorsCache.Add(CellKnownColor.LightCyan, color);
                }

                return color;
            }
        }

        public static CellColor LightGoldenrodYellow
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.LightGoldenrodYellow, out color))
                {
                    color = FromArgb(255, 250, 250, 210);
                    _colorsCache.Add(CellKnownColor.LightGoldenrodYellow, color);
                }

                return color;
            }
        }

        public static CellColor LightGreen
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.LightGreen, out color))
                {
                    color = FromArgb(255, 144, 238, 144);
                    _colorsCache.Add(CellKnownColor.LightGreen, color);
                }

                return color;
            }
        }

        public static CellColor LightGray
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.LightGray, out color))
                {
                    color = FromArgb(255, 211, 211, 211);
                    _colorsCache.Add(CellKnownColor.LightGray, color);
                }

                return color;
            }
        }

        public static CellColor LightPink
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.LightPink, out color))
                {
                    color = FromArgb(255, 255, 182, 193);
                    _colorsCache.Add(CellKnownColor.LightPink, color);
                }

                return color;
            }
        }

        public static CellColor LightSalmon
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.LightSalmon, out color))
                {
                    color = FromArgb(255, 255, 160, 122);
                    _colorsCache.Add(CellKnownColor.LightSalmon, color);
                }

                return color;
            }
        }

        public static CellColor LightSeaGreen
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.LightSeaGreen, out color))
                {
                    color = FromArgb(255, 32, 178, 170);
                    _colorsCache.Add(CellKnownColor.LightSeaGreen, color);
                }

                return color;
            }
        }

        public static CellColor LightSkyBlue
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.LightSkyBlue, out color))
                {
                    color = FromArgb(255, 135, 206, 250);
                    _colorsCache.Add(CellKnownColor.LightSkyBlue, color);
                }

                return color;
            }
        }

        public static CellColor LightSlateGray
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.LightSlateGray, out color))
                {
                    color = FromArgb(255, 119, 136, 153);
                    _colorsCache.Add(CellKnownColor.LightSlateGray, color);
                }

                return color;
            }
        }

        public static CellColor LightSteelBlue
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.LightSteelBlue, out color))
                {
                    color = FromArgb(255, 176, 196, 222);
                    _colorsCache.Add(CellKnownColor.LightSteelBlue, color);
                }

                return color;
            }
        }

        public static CellColor LightYellow
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.LightYellow, out color))
                {
                    color = FromArgb(255, 255, 255, 224);
                    _colorsCache.Add(CellKnownColor.LightYellow, color);
                }

                return color;
            }
        }

        public static CellColor Lime
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Lime, out color))
                {
                    color = FromArgb(255, 0, 255, 0);
                    _colorsCache.Add(CellKnownColor.Lime, color);
                }

                return color;
            }
        }

        public static CellColor LimeGreen
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.LimeGreen, out color))
                {
                    color = FromArgb(255, 50, 205, 50);
                    _colorsCache.Add(CellKnownColor.LimeGreen, color);
                }

                return color;
            }
        }

        public static CellColor Linen
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Linen, out color))
                {
                    color = FromArgb(255, 250, 240, 230);
                    _colorsCache.Add(CellKnownColor.Linen, color);
                }

                return color;
            }
        }

        public static CellColor Magenta
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Magenta, out color))
                {
                    color = FromArgb(255, 255, 0, 255);
                    _colorsCache.Add(CellKnownColor.Magenta, color);
                }

                return color;
            }
        }

        public static CellColor Maroon
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Maroon, out color))
                {
                    color = FromArgb(255, 128, 0, 0);
                    _colorsCache.Add(CellKnownColor.Maroon, color);
                }

                return color;
            }
        }

        public static CellColor MediumAquamarine
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.MediumAquamarine, out color))
                {
                    color = FromArgb(255, 102, 205, 170);
                    _colorsCache.Add(CellKnownColor.MediumAquamarine, color);
                }

                return color;
            }
        }

        public static CellColor MediumBlue
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.MediumBlue, out color))
                {
                    color = FromArgb(255, 0, 0, 205);
                    _colorsCache.Add(CellKnownColor.MediumBlue, color);
                }

                return color;
            }
        }

        public static CellColor MediumOrchid
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.MediumOrchid, out color))
                {
                    color = FromArgb(255, 186, 85, 211);
                    _colorsCache.Add(CellKnownColor.MediumOrchid, color);
                }

                return color;
            }
        }

        public static CellColor MediumPurple
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.MediumPurple, out color))
                {
                    color = FromArgb(255, 147, 112, 219);
                    _colorsCache.Add(CellKnownColor.MediumPurple, color);
                }

                return color;
            }
        }

        public static CellColor MediumSeaGreen
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.MediumSeaGreen, out color))
                {
                    color = FromArgb(255, 60, 179, 113);
                    _colorsCache.Add(CellKnownColor.MediumSeaGreen, color);
                }

                return color;
            }
        }

        public static CellColor MediumSlateBlue
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.MediumSlateBlue, out color))
                {
                    color = FromArgb(255, 123, 104, 238);
                    _colorsCache.Add(CellKnownColor.MediumSlateBlue, color);
                }

                return color;
            }
        }

        public static CellColor MediumSpringGreen
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.MediumSpringGreen, out color))
                {
                    color = FromArgb(255, 0, 250, 154);
                    _colorsCache.Add(CellKnownColor.MediumSpringGreen, color);
                }

                return color;
            }
        }

        public static CellColor MediumTurquoise
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.MediumTurquoise, out color))
                {
                    color = FromArgb(255, 72, 209, 204);
                    _colorsCache.Add(CellKnownColor.MediumTurquoise, color);
                }

                return color;
            }
        }

        public static CellColor MediumVioletRed
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.MediumVioletRed, out color))
                {
                    color = FromArgb(255, 199, 21, 133);
                    _colorsCache.Add(CellKnownColor.MediumVioletRed, color);
                }

                return color;
            }
        }

        public static CellColor MidnightBlue
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.MidnightBlue, out color))
                {
                    color = FromArgb(255, 25, 25, 112);
                    _colorsCache.Add(CellKnownColor.MidnightBlue, color);
                }

                return color;
            }
        }

        public static CellColor MintCream
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.MintCream, out color))
                {
                    color = FromArgb(255, 245, 255, 250);
                    _colorsCache.Add(CellKnownColor.MintCream, color);
                }

                return color;
            }
        }

        public static CellColor MistyRose
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.MistyRose, out color))
                {
                    color = FromArgb(255, 255, 228, 225);
                    _colorsCache.Add(CellKnownColor.MistyRose, color);
                }

                return color;
            }
        }

        public static CellColor Moccasin
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Moccasin, out color))
                {
                    color = FromArgb(255, 255, 228, 181);
                    _colorsCache.Add(CellKnownColor.Moccasin, color);
                }

                return color;
            }
        }

        public static CellColor NavajoWhite
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.NavajoWhite, out color))
                {
                    color = FromArgb(255, 255, 222, 173);
                    _colorsCache.Add(CellKnownColor.NavajoWhite, color);
                }

                return color;
            }
        }

        public static CellColor Navy
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Navy, out color))
                {
                    color = FromArgb(255, 0, 0, 128);
                    _colorsCache.Add(CellKnownColor.Navy, color);
                }

                return color;
            }
        }

        public static CellColor OldLace
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.OldLace, out color))
                {
                    color = FromArgb(255, 253, 245, 230);
                    _colorsCache.Add(CellKnownColor.OldLace, color);
                }

                return color;
            }
        }

        public static CellColor Olive
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Olive, out color))
                {
                    color = FromArgb(255, 128, 128, 0);
                    _colorsCache.Add(CellKnownColor.Olive, color);
                }

                return color;
            }
        }

        public static CellColor OliveDrab
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.OliveDrab, out color))
                {
                    color = FromArgb(255, 107, 142, 35);
                    _colorsCache.Add(CellKnownColor.OliveDrab, color);
                }

                return color;
            }
        }

        public static CellColor Orange
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Orange, out color))
                {
                    color = FromArgb(255, 255, 165, 0);
                    _colorsCache.Add(CellKnownColor.Orange, color);
                }

                return color;
            }
        }

        public static CellColor OrangeRed
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.OrangeRed, out color))
                {
                    color = FromArgb(255, 255, 69, 0);
                    _colorsCache.Add(CellKnownColor.OrangeRed, color);
                }

                return color;
            }
        }

        public static CellColor Orchid
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Orchid, out color))
                {
                    color = FromArgb(255, 218, 112, 214);
                    _colorsCache.Add(CellKnownColor.Orchid, color);
                }

                return color;
            }
        }

        public static CellColor PaleGoldenrod
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.PaleGoldenrod, out color))
                {
                    color = FromArgb(255, 238, 232, 170);
                    _colorsCache.Add(CellKnownColor.PaleGoldenrod, color);
                }

                return color;
            }
        }

        public static CellColor PaleGreen
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.PaleGreen, out color))
                {
                    color = FromArgb(255, 152, 251, 152);
                    _colorsCache.Add(CellKnownColor.PaleGreen, color);
                }

                return color;
            }
        }

        public static CellColor PaleTurquoise
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.PaleTurquoise, out color))
                {
                    color = FromArgb(255, 175, 238, 238);
                    _colorsCache.Add(CellKnownColor.PaleTurquoise, color);
                }

                return color;
            }
        }

        public static CellColor PaleVioletRed
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.PaleVioletRed, out color))
                {
                    color = FromArgb(255, 219, 112, 147);
                    _colorsCache.Add(CellKnownColor.PaleVioletRed, color);
                }

                return color;
            }
        }

        public static CellColor PapayaWhip
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.PapayaWhip, out color))
                {
                    color = FromArgb(255, 255, 239, 213);
                    _colorsCache.Add(CellKnownColor.PapayaWhip, color);
                }

                return color;
            }
        }

        public static CellColor PeachPuff
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.PeachPuff, out color))
                {
                    color = FromArgb(255, 255, 218, 185);
                    _colorsCache.Add(CellKnownColor.PeachPuff, color);
                }

                return color;
            }
        }

        public static CellColor Peru
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Peru, out color))
                {
                    color = FromArgb(255, 205, 133, 63);
                    _colorsCache.Add(CellKnownColor.Peru, color);
                }

                return color;
            }
        }

        public static CellColor Pink
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Pink, out color))
                {
                    color = FromArgb(255, 255, 192, 203);
                    _colorsCache.Add(CellKnownColor.Pink, color);
                }

                return color;
            }
        }

        public static CellColor Plum
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Plum, out color))
                {
                    color = FromArgb(255, 221, 160, 221);
                    _colorsCache.Add(CellKnownColor.Plum, color);
                }

                return color;
            }
        }

        public static CellColor PowderBlue
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.PowderBlue, out color))
                {
                    color = FromArgb(255, 176, 224, 230);
                    _colorsCache.Add(CellKnownColor.PowderBlue, color);
                }

                return color;
            }
        }

        public static CellColor Purple
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Purple, out color))
                {
                    color = FromArgb(255, 128, 0, 128);
                    _colorsCache.Add(CellKnownColor.Purple, color);
                }

                return color;
            }
        }

        public static CellColor Red
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Red, out color))
                {
                    color = FromArgb(255, 255, 0, 0);
                    _colorsCache.Add(CellKnownColor.Red, color);
                }

                return color;
            }
        }

        public static CellColor RosyBrown
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.RosyBrown, out color))
                {
                    color = FromArgb(255, 188, 143, 143);
                    _colorsCache.Add(CellKnownColor.RosyBrown, color);
                }

                return color;
            }
        }

        public static CellColor RoyalBlue
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.RoyalBlue, out color))
                {
                    color = FromArgb(255, 65, 105, 225);
                    _colorsCache.Add(CellKnownColor.RoyalBlue, color);
                }

                return color;
            }
        }

        public static CellColor SaddleBrown
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.SaddleBrown, out color))
                {
                    color = FromArgb(255, 139, 69, 19);
                    _colorsCache.Add(CellKnownColor.SaddleBrown, color);
                }

                return color;
            }
        }

        public static CellColor Salmon
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Salmon, out color))
                {
                    color = FromArgb(255, 250, 128, 114);
                    _colorsCache.Add(CellKnownColor.Salmon, color);
                }

                return color;
            }
        }

        public static CellColor SandyBrown
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.SandyBrown, out color))
                {
                    color = FromArgb(255, 244, 164, 96);
                    _colorsCache.Add(CellKnownColor.SandyBrown, color);
                }

                return color;
            }
        }

        public static CellColor SeaGreen
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.SeaGreen, out color))
                {
                    color = FromArgb(255, 46, 139, 87);
                    _colorsCache.Add(CellKnownColor.SeaGreen, color);
                }

                return color;
            }
        }

        public static CellColor SeaShell
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.SeaShell, out color))
                {
                    color = FromArgb(255, 255, 245, 238);
                    _colorsCache.Add(CellKnownColor.SeaShell, color);
                }

                return color;
            }
        }

        public static CellColor Sienna
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Sienna, out color))
                {
                    color = FromArgb(255, 160, 82, 45);
                    _colorsCache.Add(CellKnownColor.Sienna, color);
                }

                return color;
            }
        }

        public static CellColor Silver
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Silver, out color))
                {
                    color = FromArgb(255, 192, 192, 192);
                    _colorsCache.Add(CellKnownColor.Silver, color);
                }

                return color;
            }
        }

        public static CellColor SkyBlue
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.SkyBlue, out color))
                {
                    color = FromArgb(255, 135, 206, 235);
                    _colorsCache.Add(CellKnownColor.SkyBlue, color);
                }

                return color;
            }
        }

        public static CellColor SlateBlue
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.SlateBlue, out color))
                {
                    color = FromArgb(255, 106, 90, 205);
                    _colorsCache.Add(CellKnownColor.SlateBlue, color);
                }

                return color;
            }
        }

        public static CellColor SlateGray
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.SlateGray, out color))
                {
                    color = FromArgb(255, 112, 128, 144);
                    _colorsCache.Add(CellKnownColor.SlateGray, color);
                }

                return color;
            }
        }

        public static CellColor Snow
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Snow, out color))
                {
                    color = FromArgb(255, 255, 250, 250);
                    _colorsCache.Add(CellKnownColor.Snow, color);
                }

                return color;
            }
        }

        public static CellColor SpringGreen
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.SpringGreen, out color))
                {
                    color = FromArgb(255, 0, 255, 127);
                    _colorsCache.Add(CellKnownColor.SpringGreen, color);
                }

                return color;
            }
        }

        public static CellColor SteelBlue
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.SteelBlue, out color))
                {
                    color = FromArgb(255, 70, 130, 180);
                    _colorsCache.Add(CellKnownColor.SteelBlue, color);
                }

                return color;
            }
        }

        public static CellColor Tan
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Tan, out color))
                {
                    color = FromArgb(255, 210, 180, 140);
                    _colorsCache.Add(CellKnownColor.Tan, color);
                }

                return color;
            }
        }

        public static CellColor Teal
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Teal, out color))
                {
                    color = FromArgb(255, 0, 128, 128);
                    _colorsCache.Add(CellKnownColor.Teal, color);
                }

                return color;
            }
        }

        public static CellColor Thistle
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Thistle, out color))
                {
                    color = FromArgb(255, 216, 191, 216);
                    _colorsCache.Add(CellKnownColor.Thistle, color);
                }

                return color;
            }
        }

        public static CellColor Tomato
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Tomato, out color))
                {
                    color = FromArgb(255, 255, 99, 71);
                    _colorsCache.Add(CellKnownColor.Tomato, color);
                }

                return color;
            }
        }

        public static CellColor Turquoise
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Turquoise, out color))
                {
                    color = FromArgb(255, 64, 224, 208);
                    _colorsCache.Add(CellKnownColor.Turquoise, color);
                }

                return color;
            }
        }

        public static CellColor Violet
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Violet, out color))
                {
                    color = FromArgb(255, 238, 130, 238);
                    _colorsCache.Add(CellKnownColor.Violet, color);
                }

                return color;
            }
        }

        public static CellColor Wheat
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Wheat, out color))
                {
                    color = FromArgb(255, 245, 222, 179);
                    _colorsCache.Add(CellKnownColor.Wheat, color);
                }

                return color;
            }
        }

        public static CellColor White
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.White, out color))
                {
                    color = FromArgb(255, 255, 255, 255);
                    _colorsCache.Add(CellKnownColor.White, color);
                }

                return color;
            }
        }

        public static CellColor WhiteSmoke
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.WhiteSmoke, out color))
                {
                    color = FromArgb(255, 245, 245, 245);
                    _colorsCache.Add(CellKnownColor.WhiteSmoke, color);
                }

                return color;
            }
        }

        public static CellColor Yellow
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.Yellow, out color))
                {
                    color = FromArgb(255, 255, 255, 0);
                    _colorsCache.Add(CellKnownColor.Yellow, color);
                }

                return color;
            }
        }

        public static CellColor YellowGreen
        {
            get
            {
                CellColor color;
                if (!_colorsCache.TryGetValue(CellKnownColor.YellowGreen, out color))
                {
                    color = FromArgb(255, 154, 205, 50);
                    _colorsCache.Add(CellKnownColor.YellowGreen, color);
                }

                return color;
            }
        }
        #endregion
    }
}
