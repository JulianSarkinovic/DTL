namespace DikkeTennisLijst.Core.Shared.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Throws an ArgumentException if the string is null or empty.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="parameterName"></param>
        public static void ThrowIfEmptyOrNull(this string str, string parameterName)
        {
            str = (str ?? string.Empty).Trim();
            if (str.Length == 0) throw new ArgumentException($"String parameter {parameterName} is empty.");
        }

        /// <summary>
        /// Consider if the content is empty if the string is null, empty, or white space.
        /// </summary>
        public static bool IsEmptyContent(this string str) => string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);

        /// <summary>
        /// Consider if the string is equal, ignoring any cases in the string.
        /// </summary>
        public static bool CompareStringIgnoreCase(this string str, string text) => str.Equals(text, StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        /// Consider if the provided string is contained, ignoring any cases in the strings.
        /// </summary>
        public static bool ContainsStringIgnoreCase(this string str, string text) => str.Contains(text, StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        /// Consider string to be uppercase if it has no lowercase letters.
        /// </summary>
        public static bool IsUpper(this string value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (char.IsLower(value[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Consider string to be lowercase if it has no uppercase letters.
        /// </summary>
        public static bool IsLower(this string value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (char.IsUpper(value[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}