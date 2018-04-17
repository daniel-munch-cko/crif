using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using FluentValidation;

namespace Crif.Api
{
    public static class ValidationRules
    {
        public const string DateTimeFormat = "yyyy-MM-dd";
        public static IRuleBuilderOptions<T, string> MustBeConvertibleToEnum<T>
                                    (this IRuleBuilder<T, string> ruleBuilder, Type typeEnum)
        {
            return ruleBuilder.Must(x => Enum.GetValues(typeEnum)
                             .CastToString(typeEnum)
                             .Contains(x, StringComparer.CurrentCultureIgnoreCase));
        }

        public static IRuleBuilderOptions<T, string> MustBeValidDate<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(x => x.HasCorrectDateTimeFormat());
        }

        public static IRuleBuilderOptions<T, string> MustBeValidEmailAddress<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(x => x.IsValidEmail());
        }

        public static IRuleBuilderOptions<T, string> MustBeValidCountryCode<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(x => CountryBook.Codes.Contains(x, StringComparer.InvariantCultureIgnoreCase));
        }

        private static IEnumerable<string> CastToString(this IEnumerable source, Type typeEnum)
        {
            foreach (var item in source)
            {
                yield return Convert.ChangeType(item, typeEnum).ToString();
            }
        }

        private static bool HasCorrectDateTimeFormat(this string dateTime)
        {
            return DateTime.TryParseExact(
                                            dateTime,
                                            DateTimeFormat,
                                            CultureInfo.InvariantCulture,
                                            DateTimeStyles.None,
                                            out var _);
        }

        // See https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
        private static bool IsValidEmail(this string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}