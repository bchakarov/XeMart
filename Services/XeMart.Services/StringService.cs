namespace XeMart.Services
{
    using System;

    public class StringService : IStringService
    {
        public string TruncateAtWord(string input, int length)
        {
            if (input == null || input.Length < length)
            {
                return input;
            }

            int nextSpaceIndex = input.LastIndexOf(" ", length, StringComparison.Ordinal);
            return string.Format("{0}…", input.Substring(0, (nextSpaceIndex > 0) ? nextSpaceIndex : length).Trim());
        }
    }
}
