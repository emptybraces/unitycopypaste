using System.IO;
using UnityEngine;
using UnityEditor;

namespace Serializable
{
	[CustomPropertyDrawer(typeof(AssetRef))]
	public class AssetRefDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			using (new EditorGUI.PropertyScope(position, label, property))
			{
				var asset = property.FindPropertyRelative("assetEditor");
				var path = property.FindPropertyRelative("assetPath");
				using (var check = new EditorGUI.ChangeCheckScope())
				{
					var new_clip = EditorGUI.ObjectField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), label, asset.objectReferenceValue, typeof(Object), false);
					if (check.changed)
					{
						asset.objectReferenceValue = new_clip;
						if (new_clip != null)
							path.stringValue = AssetDatabase.GetAssetPath(new_clip);
						else
							path.stringValue = "";
					}
					if (IsMissing(property))
					{
						EditorGUI.HelpBox(new Rect(EditorGUIUtility.labelWidth, position.y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight), $"[{label.text}] refernce is missing. path:{path.stringValue}", MessageType.Error);
					}
				}
			}
		}
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var h = base.GetPropertyHeight(property, label);
			if (IsMissing(property))
				h += EditorGUIUtility.singleLineHeight;
			return h;
		}

		bool IsMissing(SerializedProperty property)
		{
			var path = property.FindPropertyRelative("assetPath");
			// return "" == AssetDatabase.AssetPathToGUID(path.stringValue);
			return null == AssetDatabase.LoadAssetAtPath<Object>(path.stringValue);
		}
	}
}