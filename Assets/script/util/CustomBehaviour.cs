using System;
using UnityEngine;
namespace Utils
{
	public class CustomBehaviour : MonoBehaviour
	{
	}
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = false)]
	public class ExposeInspectorAttribute : PropertyAttribute
	{
		public string Name;
		public ExposeInspectorAttribute() { }
		public ExposeInspectorAttribute(string s) { Name = s; }
	}
}
