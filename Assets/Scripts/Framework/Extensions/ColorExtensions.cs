using System;
using UnityEngine;

namespace Framework.Extensions
{
    public static class ColorExtensions
    {
        public static Color32 FromHex(this Color32 instance, uint hex)
        {
            instance.r = (byte) (hex >> 24);
            instance.g = (byte) (hex >> 16);
            instance.b = (byte) (hex >> 8);
            instance.a = (byte) (hex & 0x000000FF);

            return instance;
        }

        public static Color ToColor(this uint rgba)
        {
            var value = rgba & 0xffffffff;

            var r = (byte) ((value >> 24) & 0xff);
            var g = (byte) ((value >> 16) & 0xff);
            var b = (byte) ((value >> 8) & 0xff);
            var a = (byte) ((value >> 0) & 0xff);

            return new Color32(r, g, b, a);
        }

        public static Color ToColor(this int rgb)
        {
            var rgba = ((rgb & 0xffffffff) << 8) | 0xff;
            return Convert.ToUInt32(rgba).ToColor();
        }

        public static uint ToRGBA(this Color32 color)
        {
            return (uint) (color.r << 24 | color.g << 16 | color.b << 8 | color.a << 0) & 0xffffffff;
        }

        public static uint ToRGBA(this Color color)
        {
            return ((Color32) color).ToRGBA();
        }

        public static Color WithAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }
    }
}