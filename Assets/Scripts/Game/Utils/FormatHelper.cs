namespace Game.Utils
{
    public static class FormatHelper
    {
        public static string FormatValue(int value, bool isShort = false)
        {
            if (value < 1000)
            {
                return $"{value}";
            }

            if (!isShort && value < 100000)
            {
                return $"{value / 1000}.{value % 1000 / 100}K";
            }

            if (value < 1000000)
            {
                return $"{value / 1000}K";
            }

            if (!isShort && value < 100000000)
            {
                return $"{value / 1000000}.{value % 1000000 / 100000}M";
            }

            return $"{value / 1000000}M";
        }
    }
}