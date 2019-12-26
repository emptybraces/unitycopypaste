using UnityEngine;
using UnityEditor;
namespace Utils
{
	[CustomPropertyDrawer(typeof(DrawableToggly), true)]
	public class TogglyDrawer : PropertyDrawer
	{
		// Draw the property inside the given rect
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			label = EditorGUI.BeginProperty(position, label, property);
			// Draw label
			var labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
			EditorGUI.PrefixLabel(labelRect, GUIUtility.GetControlID(FocusType.Passive), label);
			var enabledRect = new Rect(labelRect.xMax, position.y, 20f, position.height);
			var enabled_property = property.FindPropertyRelative("enabled");
			EditorGUI.PropertyField(enabledRect, enabled_property, GUIContent.none);
			if (enabled_property.boolValue) {
				EditorGUIUtility.labelWidth = GUI.skin.label.CalcSize(new GUIContent("value")).x;
				var valueRect = new Rect(enabledRect.xMax, position.y, position.width - labelRect.width - 20f, position.height);
				EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("value"));
			}
			EditorGUI.EndProperty();
		}
	}
}
