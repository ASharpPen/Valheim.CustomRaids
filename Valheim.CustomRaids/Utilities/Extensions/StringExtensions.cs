﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Valheim.CustomRaids.Utilities.Extensions
{
    internal static class StringExtensions
    {
        private static char[] Comma = new[] { ',' };

        public static List<string> SplitByComma(this string value, bool toUpper = false)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new List<string>(0);
            }

            var split = value.Split(Comma, StringSplitOptions.RemoveEmptyEntries);

            if ((split?.Length ?? 0) == 0)
            {
                return new List<string>(0);
            }

            return split.Select(Clean).ToList();

            string Clean(string x)
            {
                var result = x.Trim();
                if (toUpper)
                {
                    return result.ToUpperInvariant();
                }
                return result;
            }
        }
    }
}
