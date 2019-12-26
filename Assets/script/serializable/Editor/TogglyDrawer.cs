using UnityEngine;
using UnityEditor;
namespace Serializable
{
	[CustomPropertyDrawer(typeof(DrawableToggly), true)]
	public class TogglyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			using (new EditorGUI.PropertyScope(position, label, property))
			{
				// Draw label
				var rect_label = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
				EditorGUI.PrefixLabel(rect_label, GUIUtility.GetControlID(FocusType.Passive), label);
				var rect_enabled = new Rect(rect_label.xMax, position.y, 20f, position.height);
				var prop_enabled = property.FindPropertyRelative("enabled");
				EditorGUI.PropertyField(rect_enabled, prop_enabled, GUIContent.none);
				if (prop_enabled.boolValue)
				{
					EditorGUIUtility.labelWidth = GUI.skin.label.CalcSize(new GUIContent("value")).x;
					var rect_value = new Rect(rect_enabled.xMax, position.y, position.width - rect_label.width - rect_enabled.width, position.height);
					EditorGUI.PropertyField(rect_value, property.FindPropertyRelative("value"));
				}
			}
		}
	}
}
