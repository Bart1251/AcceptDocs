using System.ComponentModel;
using System.Reflection;

namespace AcceptDocs.SharedKernel.Helpers
{
    public class EnumItem<T>
    {
        public T Value { get; set; }
        public string Name { get; set; }
    }

    public static class EnumHelper
    {
        public static List<EnumItem<T>> GetEnumList<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>()
                .Select(e => new EnumItem<T>() {
                    Value = e,
                    Name = GetDescription(e)
                }).ToList();
        }

        public static string GetDescription<T>(T value) where T : Enum
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = field?.GetCustomAttribute<DescriptionAttribute>();
            return attr?.Description ?? value.ToString();
        }
    }
}
