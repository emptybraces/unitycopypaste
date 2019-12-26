using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Collections;

namespace Utils
{
	[CanEditMultipleObjects, CustomEditor(typeof(CustomBehaviour), true)]
	public class CustomBehaviourDrawer : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			if (PrefabUtility.IsPartOfAnyPrefab(target) && PrefabUtility.GetPrefabInstanceStatus(target) == PrefabInstanceStatus.NotAPrefab)
				return;
			GUILayout.Label("CustomBehaviour Control", EditorStyles.boldLabel);
			var type = target.GetType();
			foreach (var member in type.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)) {
				// make sure it is decorated by our custom attribute
				var attributes = member.GetCustomAttributes(typeof(ExposeInspectorAttribute), true);
				if (0 < attributes.Length) {
					switch (member.MemberType) {
						case MemberTypes.Field:
							GUI.enabled = false;
							ShowField((FieldInfo)member, target);
							break;
						case MemberTypes.Property:
							GUI.enabled = Application.isPlaying && ((PropertyInfo)member).CanWrite;
							ShowProperty((PropertyInfo)member, target);
							break;
						case MemberTypes.Method:
							GUI.enabled = true;
							ShowMethod((MethodInfo)member, (ExposeInspectorAttribute)attributes[0], target);
							break;
						default:
							throw new NotImplementedException();
					}
				}
			}
		}

		void ShowField(FieldInfo info, System.Object obj)
		{
			// if (Application.isPlaying)
				ShowPrimitiveField(info.Name, info.FieldType, info.GetValue(obj));
			// else
			// 	EditorGUILayout.LabelField(info.Name);
		}

		void ShowProperty(PropertyInfo info, System.Object obj)
		{
			if (Application.isPlaying) {
				if (info.CanWrite) {
					bool b;
					var r = ShowPrimitiveField(info.Name, obj, info.PropertyType, info.GetValue(obj, null), out b);
					if (b) {
						info.SetValue(obj, r, null);
					}
				}
				else {
					ShowPrimitiveField(info.Name, info.PropertyType, info.GetValue(obj, null));
				}
			}
			else
				EditorGUILayout.LabelField(info.Name);
		}

		void ShowMethod(MethodInfo info, ExposeInspectorAttribute attr, System.Object obj)
		{
			var type = target.GetType();
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			var button_name = attr.Name;
			if (GUILayout.Button("Run: " + (String.IsNullOrEmpty(button_name) ? info.Name : button_name), GUILayout.Width(EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth))) {
				// If the user clicks the button, invoke the method immediately.
				// There are many ways to do this but I chose to use Invoke which only works in Play Mode.
				foreach (var i in targets) {
					//((MonoBehaviour)i.GetComponent(type)).Invoke(method.Name, 0f);
					info.Invoke(((MonoBehaviour)i).GetComponent(type), null);
				}
			}
			EditorGUILayout.EndHorizontal();
		}

		void ShowPrimitiveField(string showName, Type type, System.Object value)
		{
			if (type == typeof(int) || type.IsEnum)
				EditorGUILayout.IntField(showName, (int)value);
			else if (type == typeof(float))
				EditorGUILayout.FloatField(showName, (float)value);
			else if (type == typeof(bool))
				EditorGUILayout.Toggle(showName, (bool)value);
			else if (type == typeof(string))
				EditorGUILayout.TextField(showName, (string)value);
			else if (type == typeof(Vector2))
				EditorGUILayout.Vector2Field(showName, (Vector2)value);
			else if (type == typeof(Vector3))
				EditorGUILayout.Vector3Field(showName, (Vector3)value);
			else if (type == typeof(GameObject))
				EditorGUILayout.ObjectField(showName, (GameObject)value, typeof(GameObject), false);
			else if (type.IsSubclassOf(typeof(MonoBehaviour)))
				EditorGUILayout.ObjectField(showName, (MonoBehaviour)value, typeof(MonoBehaviour), false);
			else if (type.IsSubclassOf(typeof(Component)))
				EditorGUILayout.ObjectField(showName, (Component)value, typeof(Component), false);
			else if (type.IsArray) {
				++EditorGUI.indentLevel;
				EditorGUILayout.Foldout(true, showName);
				var array = value as Array;
				if (array != null) {
					for (int i = 0, l = array.Length; i < l; ++i) {
						var label = String.Format("[{0}]", i);
						ShowPrimitiveField(label, type.GetElementType(), array.GetValue(i));
					}
				}
				--EditorGUI.indentLevel;
			}
			else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)) {
				++EditorGUI.indentLevel;
				EditorGUILayout.Foldout(true, showName);
				var list = value as IList;
				if (list != null) {
					for (int i = 0, l = list.Count; i < l; ++i) {
						var label = String.Format("[{0}]", i);
						ShowPrimitiveField(label, type.GetGenericArguments()[0], list[i]);
					}
				}
				--EditorGUI.indentLevel;
			}
		}

		System.Object ShowPrimitiveField(string showName, System.Object obj, Type type, System.Object value, out bool updated)
		{
			if (type == typeof(int)) {
				var r = EditorGUILayout.IntField(showName, (int)value);
				if (updated = r != (int)value)
					return r;
				return value;
			}
			else if (type == typeof(float)) {
				var r = EditorGUILayout.FloatField(showName, (float)value);
				if (updated = r != (float)value)
					return r;
				return value;
			}
			else if (type == typeof(bool)) {
				var r = EditorGUILayout.Toggle(showName, (bool)value);
				if (updated = r != (bool)value)
					return r;
				return value;
			}
			else if (type == typeof(string)) {
				var r = EditorGUILayout.TextField(showName, (string)value);
				if (updated = r != (string)value)
					return r;
				return value;
			}
			else if (type == typeof(Vector2)) {
				var r = EditorGUILayout.Vector2Field(showName, (Vector2)value);
				if (updated = r != (Vector2)value)
					return r;
				return value;
			}
			else if (type == typeof(Vector3)) {
				var r = EditorGUILayout.Vector3Field(showName, (Vector3)value);
				if (updated = r != (Vector3)value)
					return r;
				return value;
			}
			else if (type == typeof(GameObject)) {
				var r = EditorGUILayout.ObjectField(showName, (GameObject)value, typeof(GameObject), false);
				if (updated = r != (GameObject)value)
					return r;
				return value;
			}
			else if (type.IsSubclassOf(typeof(MonoBehaviour))) {
				var r = EditorGUILayout.ObjectField(showName, (MonoBehaviour)value, typeof(MonoBehaviour), false);
				if (updated = r != (MonoBehaviour)value)
					return r;
				return value;
			}
			else if (type.IsSubclassOf(typeof(Component))) {
				var r = EditorGUILayout.ObjectField(showName, (Component)value, typeof(Component), false);
				if (updated = r != (Component)value)
					return r;
				return value;
			}
			else if (type.IsArray) {
				++EditorGUI.indentLevel;
				var expands = EditorGUILayout.Foldout(true, showName);
				if (expands) {
					var array = value as Array;
					if (array != null) {
						for (int i = 0, l = array.Length; i < l; ++i) {
							var label = String.Format("[{0}]", i);
							if (Application.isPlaying) {
								bool b;
								var r = ShowPrimitiveField(label, value, type.GetElementType(), array.GetValue(i), out b);
								if (b) {
									array.SetValue(r, i);
								}
							}
							else
								EditorGUILayout.LabelField(label);
						}
					}
				}
				--EditorGUI.indentLevel;
				updated = false;
				return value;
			}
			updated = false;
			return null;
		}
	}
}