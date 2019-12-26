using UnityEngine;
using System;
namespace Utils
{
	[System.Diagnostics.Conditional("UNITY_EDITOR")]
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public class NameAttribute : PropertyAttribute
	{
		public string Name { get; set; }
		public string[] Names { get; set; }
		public NameAttribute(string s) { Name = s; }
		public NameAttribute(params string[] s) { Names = s; }
	}
	[System.Diagnostics.Conditional("UNITY_EDITOR")]
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public class RequireFieldAttribute : PropertyAttribute
	{
		public string[] requirePropertyNames = new string[] { };
		public Type[] requireHasTypes = new Type[] { };
		public RequireFieldAttribute(params string[] a) { requirePropertyNames = a; }
		public RequireFieldAttribute(params Type[] a) { requireHasTypes = a; }
	}

	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public class EnumNameAttribute : Attribute
	{
		public string name { get; set; }
		public static string GetName(Enum value)
		{
			Type type = value.GetType();
			string name = Enum.GetName(type, value);
			EnumNameAttribute[] attrs =
			(EnumNameAttribute[])type.GetField(name)
			.GetCustomAttributes(typeof(EnumNameAttribute), false);
			if (attrs == null)
				return "";
			else
				return attrs[0].name;
		}
	}

	[AttributeUsage(AttributeTargets.Field,	AllowMultiple = true, Inherited = false)]
	public class SortIndex : Attribute
	{
		public int index { get; set; }
		public static int GetIndex(Enum value)
		{
			Type type = value.GetType();
			string name = Enum.GetName(type, value);
			SortIndex[] attrs =
			(SortIndex[])type.GetField(name)
			.GetCustomAttributes(typeof(SortIndex), false);
			return attrs[0].index;
		}
	}

	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public class EnumMaskAttribute : PropertyAttribute { }

	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public class PopupAttribute : PropertyAttribute
	{
		public string[] displays;
		public PopupAttribute(params string[] s) { displays = s; }
	}

	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public class FileNameGrabAttribute : PropertyAttribute
	{
		public bool ext;
		public Type type;
		public FileNameGrabAttribute(Type type, bool ext) { this.type = type; this.ext = ext; }
	}
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public class InitialAssignAttribute : PropertyAttribute
	{
		public string path;
		public InitialAssignAttribute(string path)
		{
			this.path = path;
		}
	}

	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
	public class HelpBoxAttribute : PropertyAttribute
	{
		public enum Type { None, Info, Warning, Error }
		public string msg;
		public Type type;
		public HelpBoxAttribute(string message, Type type = Type.None, int order = 0)
		{
			msg = message;
			this.type = type;
			this.order = order;
		}
	}
	public class MinMaxRangeAttribute : PropertyAttribute
	{
		public float minLimit, maxLimit;
		public bool IsFloat = true;
		public bool showSlider;
		public MinMaxRangeAttribute(float minLimit, float maxLimit, bool showSlider = true)
		{
			this.minLimit = minLimit;
			this.maxLimit = maxLimit;
			this.showSlider = showSlider;
		}
	}
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class PreviewableAttribute : PropertyAttribute
	{
		public int width = 100;
		public int height = 100;
	}
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class ReadOnlyAttribute : PropertyAttribute { }

	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public class SearchableAttribute : PropertyAttribute
	{
		public Type type = typeof(UnityEngine.Object);
		public bool allowSceneObjects;
		public string prefix;
		public string filter;
		public string replaceFrom;
		public string replaceTo;
		public bool onlyFilename;
		public bool extension;
	}
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public class UniqueIdentifierAttribute : PropertyAttribute { }

}
