using System;

namespace Framework.Extensions
{
    public static class ActionExtensions
    {
        public static void SafeInvoke(this Action action)
        {
            if (action != null)
            {
                action();
            }
        }

        public static void SafeInvoke<T>(this Action<T> action, T args)
        {
            if (action != null)
            {
                action(args);
            }
        }
    }
}