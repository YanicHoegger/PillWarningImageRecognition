using System;

namespace Utilities
{
    public static class ArgumentExtensions
    {
        public static void CheckNotNull(this object argument, string argumentName)
        {
            if (argument is null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        public static void CheckNotNullOrEmpty(this string argument, string argumentName)
        {
            if (string.IsNullOrEmpty(argument))
            {
                throw new ArgumentException("Can not be null or empty", argumentName);
            }
        }
    }
}
