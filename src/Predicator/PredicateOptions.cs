using System;
using System.Collections.Generic;
using System.Linq;

namespace Predicator
{
    public class PredicateOptions
    {
        public string StringValue1 { get; set; }
        public Guid? GuidValue1 => ToGuid(StringValue1);
        public T? EnumValue1<T>(T? fallback = null) where T : struct => ToEnum(StringValue1, fallback);
        public T ClassValue1<T>(Func<string, T> map) where T : class => ToClass(StringValue1, map);
        public string StringValue2 { get; set; }
        public Guid? GuidValue2 => ToGuid(StringValue2);
        public List<string> StringArrayValue1 { get; set; } = new List<string>();
        public List<Guid> GuidArrayValue1 => ToGuidList(StringArrayValue1);
        public List<T> EnumArrayValue1<T>() where T : struct => ToEnumList<T>(StringArrayValue1);
        public List<T> ClassArrayValue1<T>(Func<string, T> map) where T : class => ToClassList(StringArrayValue1, map);
        public List<string> StringArrayValue2 { get; set; } = new List<string>();
        public List<Guid> GuidArrayValue2 => ToGuidList(StringArrayValue2);
        public List<T> EnumArrayValue2<T>() where T : struct => ToEnumList<T>(StringArrayValue2);
        public List<T> ClassArrayValue2<T>(Func<string, T> map) where T : class => ToClassList(StringArrayValue2, map);
        public decimal NumberValue1 { get; set; }
        public decimal NumberValue2 { get; set; }
        public DateTime DateValue1 { get; set; }
        public DateTime DateValue2 { get; set; }

        T? ToEnum<T>(string value, T? fallback = null) where T : struct
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return fallback;
            }

            if (!Enum.TryParse<T>(value, out var enumValue))
            {
                return fallback;
            }

            return enumValue;
        }

        T ToClass<T>(string value, Func<string, T> map) where T : class
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return default;
            }

            return map(value);
        }
        Guid? ToGuid(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (!Guid.TryParse(value, out var guidValue))
            {
                return null;
            }

            return guidValue;
        }
        List<Guid> ToGuidList(IEnumerable<string> value)
        {
            var list = new List<Guid>();
            if (value == null || !value.Any())
            {
                return list;
            }

            foreach (var item in value)
            {
                var guid = ToGuid(item);
                if (guid.HasValue)
                {
                    list.Add(guid.Value);
                }
            }

            return list;
        }
        List<T> ToEnumList<T>(IEnumerable<string> value) where T : struct
        {
            var list = new List<T>();
            if (value == null || !value.Any())
            {
                return list;
            }

            foreach (var item in value)
            {
                var enumValue = ToEnum<T>(item);
                if (enumValue.HasValue)
                {
                    list.Add(enumValue.Value);
                }
            }

            return list;
        }

        List<T> ToClassList<T>(IEnumerable<string> value, Func<string, T> map) where T : class
        {
            var list = new List<T>();
            if (value == null || !value.Any())
            {
                return list;
            }

            foreach (var item in value)
            {
                var classValue = ToClass(item, map);
                if (classValue != null)
                {
                    list.Add(classValue);
                }
            }

            return list;
        }
    }
}
