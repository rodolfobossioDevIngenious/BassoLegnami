using System;
using System.Linq;
using System.Reflection;

namespace System.ComponentModel.DataAnnotations
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
	public class StringValueAttribute : Attribute
	{
		public string StringValue { get; set; }

		public StringValueAttribute(string value)
		{
			StringValue = value;
		}
	}

	public static class StaticEnumStringValueAttribute
	{
		public static string GetStringValue(this Enum value)
		{
			Type type = value.GetType();
			FieldInfo fieldInfo = type.GetField(value.ToString());
			StringValueAttribute[] attrs = fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
			return attrs.Length > 0 ? attrs[0].StringValue : null;
		}

		public static string GetEnumDisplayName(this Enum enumType)
		{
			if (enumType is null)
			{
				return null;
			}
			return enumType.GetType().GetMember(enumType.ToString())
						   .FirstOrDefault()?
						   .GetCustomAttribute<DisplayAttribute>()
						   .GetName();
		}
	}
}