using UnityEngine;
using UnityEditor;
using UnityEditor.Sprites;
using System.IO;
using System.Linq;
using System;
using System.Reflection;

namespace Utils
{
	[CustomPropertyDrawer(typeof(RequireFieldAttribute))]
	public class RequireFieldAttributeDrawer : PropertyDrawer<RequireFieldAttribute>
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			GUI.enabled = CheckRequire(property);
			using (new EditorGUI.PropertyScope(position, label, property))
			{
				EditorGUI.PropertyField(position, property, label, true);
			}
		}
		bool CheckRequire(SerializedProperty property)
		{
			foreach (var i in Attribute.requirePropertyNames)
			{
				var prop = property.serializedObject.FindProperty(i);
				switch (prop.propertyType)
				{
					case SerializedPropertyType.Integer when prop.intValue == 0:
					case SerializedPropertyType.Float when Mathf.Approximately(0f, prop.floatValue):
					case SerializedPropertyType.String when System.String.IsNullOrEmpty(prop.stringValue):
					case SerializedPropertyType.Boolean when !prop.boolValue: 
					case SerializedPropertyType.ObjectReference when prop.objectReferenceValue == null:
						return false;
				}
			}
			if (gameObject != null)
			{
				if (Attribute.requireHasTypes.Any(e => gameObject.GetComponent(e) == null))
					return false;
			}
			return true;
		}
	}

	[CustomPropertyDrawer(typeof(NameAttribute))]
	public class NameAttributeDrawer : PropertyDrawer<NameAttribute>
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var name = Attribute.Name;
			if (Attribute.Names != null && label.text.Contains("Element"))
			{
				var num = int.Parse(System.Text.RegularExpressions.Regex.Replace(label.text, @"[^0-9]", ""));
				if (num < Attribute.Names.Length)
					name = Attribute.Names[num];
				else
					name = label.text;
			}
			using (new EditorGUI.PropertyScope(position, label, property))
			{
				EditorGUI.PropertyField(position, property, new GUIContent(name), true);
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(property, label, 0 < Attribute.Names.Length);
		}
	}
	[CustomPropertyDrawer(typeof(EnumMaskAttribute))]
	public class EnumMaskAttributeDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			label = EditorGUI.BeginProperty(position, label, property);
			property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
			EditorGUI.EndProperty();
		}
	}

	[CustomPropertyDrawer(typeof(PopupAttribute))]
	public class PopupAttributeDrawer : PropertyDrawer<PopupAttribute>
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var idx = Mathf.Max(0, ArrayUtility.IndexOf(Attribute.displays, property.stringValue));
			property.stringValue = Attribute.displays[EditorGUI.Popup(position, label.text, idx, Attribute.displays)];
		}
	}
	[CustomPropertyDrawer(typeof(InitialAssignAttribute))]
	public class InitialAssignAttributeDrawer : PropertyDrawer<InitialAssignAttribute>
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			base.OnGUI(position, property, label);
			label = EditorGUI.BeginProperty(position, label, property);
			if (property.objectReferenceValue == null)
			{
				property.objectReferenceValue = AssetDatabase.LoadAssetAtPath(Attribute.path, GetPropertyType(property));
				Debug.LogFormat("Auto Assign {0}{1}. path: {2}", property.objectReferenceValue, GetPropertyType(property), Attribute.path);
			}
			// property.objectReferenceValue = EditorGUI.ObjectField(position, label, property.objectReferenceValue, GetPropertyType(property), false);
			EditorGUI.PropertyField(position, property, label);
		}
	}
	[CustomPropertyDrawer(typeof(HelpBoxAttribute))]
	public sealed class HelpBoxDrawer : DecoratorDrawer
	{
		private HelpBoxAttribute HelpBoxAttribute { get { return attribute as HelpBoxAttribute; } }
		public override void OnGUI(Rect position)
		{
			var helpBoxPosition = EditorGUI.IndentedRect(position);
			helpBoxPosition.height = GetHelpBoxHeight();

			EditorGUI.HelpBox(helpBoxPosition, HelpBoxAttribute.msg, GetMessageType(HelpBoxAttribute.type));
		}

		public override float GetHeight()
		{
			return GetHelpBoxHeight();
		}

		public MessageType GetMessageType(HelpBoxAttribute.Type type)
		{
			switch (type)
			{
				case HelpBoxAttribute.Type.Error: return MessageType.Error;
				case HelpBoxAttribute.Type.Info: return MessageType.Info;
				case HelpBoxAttribute.Type.None: return MessageType.None;
				case HelpBoxAttribute.Type.Warning: return MessageType.Warning;
			}
			return 0;
		}

		public float GetHelpBoxHeight()
		{
			var style = new GUIStyle("HelpBox");
			var content = new GUIContent(HelpBoxAttribute.msg);
			return Mathf.Max(style.CalcHeight(content, Screen.width - (HelpBoxAttribute.type != HelpBoxAttribute.Type.None ? 53 : 21)), 40);
		}
	}
	[CanEditMultipleObjects, CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
	public class MinMaxRangeDrawer : Utils.PropertyDrawer<MinMaxRangeAttribute>
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (Attribute.showSlider)
				return base.GetPropertyHeight(property, label) * 2f;
			else
				return base.GetPropertyHeight(property, label);
		}
		// Draw the property inside the given rect
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			base.OnGUI(position, property, label);
			// Now draw the property as a Slider or an IntSlider based on whether it’s a float or integer.
			if (property.type != "Vector2")
			{
				EditorGUI.LabelField(position, "Use Vector2 type or MinMaxRange type");
				return;
			}
			var height = Attribute.showSlider ? position.height * 0.5f : position.height;
			var min_value = property.FindPropertyRelative("x");
			var max_value = property.FindPropertyRelative("y");
			var new_min = min_value.floatValue;
			var new_max = max_value.floatValue;
			var labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, height);
			var fromValueRect = new Rect(labelRect.xMax, position.y, (position.width - labelRect.width) * 0.5f, height);
			var toValueRect = new Rect(fromValueRect.xMax, position.y, (position.width - labelRect.width) * 0.5f, height);
			// field label
			EditorGUI.PrefixLabel(labelRect, GUIUtility.GetControlID(FocusType.Passive), label);
			// 最初のコンテンツだけインデントの影響を受けさせてあとはゼロ
			using (new ZeroIndent())
			{
				// min value
				EditorGUI.showMixedValue = min_value.hasMultipleDifferentValues;
				EditorGUI.BeginChangeCheck();
				EditorGUIUtility.labelWidth = CalcLabelSize("From:");
				var change_min = Mathf.Clamp(EditorGUI.FloatField(fromValueRect, new GUIContent("From:"), new_min), Attribute.minLimit, new_max);
				var change_min_value = EditorGUI.EndChangeCheck();
				EditorGUI.showMixedValue = false;
				// max value
				EditorGUI.showMixedValue = max_value.hasMultipleDifferentValues;
				EditorGUI.BeginChangeCheck();
				EditorGUIUtility.labelWidth = CalcLabelSize("To:");
				float change_max = Mathf.Clamp(EditorGUI.FloatField(toValueRect, new GUIContent("To:"), new_max), new_min, Attribute.maxLimit);
				var change_max_value = EditorGUI.EndChangeCheck();
				EditorGUI.showMixedValue = false;
				// slider
				if (Attribute.showSlider)
				{
					var slider_rect = new Rect(fromValueRect.xMin, position.y + height, position.width - labelRect.width, height);
					EditorGUI.MinMaxSlider(
						slider_rect,
						ref change_min,
						ref change_max,
						Attribute.minLimit,
						Attribute.maxLimit);
					change_min_value = change_min_value || !Mathf.Approximately(new_min, change_min);
					change_max_value = change_max_value || !Mathf.Approximately(new_max, change_max);
				}
				// update field
				if (change_min_value || change_max_value)
				{
					if (transform != null)
						Undo.RecordObject(transform, "MinMaxRange: " + transform.name);
					if (change_min_value)
					{
						min_value.floatValue = change_min;
					}
					if (change_max_value)
					{
						max_value.floatValue = change_max;
					}
				}
			}
		}
	}
	[CustomPropertyDrawer(typeof(PreviewableAttribute))]
	public class PreviewableAttributeDrawer : Utils.PropertyDrawer<PreviewableAttribute>
	{
		public override void OnGUI(Rect position,
								   SerializedProperty property,
								   GUIContent label)
		{
			EditorGUI.PropertyField(position, property, label, true);
			if (property.objectReferenceValue != null)
			{
				var rect = new Rect(position.width - Attribute.width, position.y, Attribute.width, Attribute.height);
				using (new ZeroIndent())
				{
					if (property.objectReferenceValue as Sprite)
						EditorGUI.DrawPreviewTexture(rect, SpriteUtility.GetSpriteTexture((Sprite)property.objectReferenceValue, false));
					else if (property.objectReferenceValue as Texture)
						EditorGUI.DrawPreviewTexture(rect, (Texture)property.objectReferenceValue);
				}
			}
		}
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (property.objectReferenceValue != null)
				return base.GetPropertyHeight(property, label) + Attribute.height;
			else
				return base.GetPropertyHeight(property, label);
		}
	}
	[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
	public class ReadOnlyAttributeDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position,
								   SerializedProperty property,
								   GUIContent label)
		{
			GUI.enabled = false;
			EditorGUI.PropertyField(position, property, label, true);
			GUI.enabled = true;
		}
	}

	[CustomPropertyDrawer(typeof(FileNameGrabAttribute), true)]
	public class FileNameGrabAttributeDrawer : PropertyDrawer<FileNameGrabAttribute>
	{
		// Draw the property inside the given rect
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.type != "string")
			{
				EditorGUI.LabelField(position, label, new GUIContent("only supported type string"));
				return;
			}
			base.OnGUI(position, property, label);
			label = EditorGUI.BeginProperty(position, label, property);
			var xmax = position.xMax;
			position.xMax -= 50;
			EditorGUI.PropertyField(position, property);
			position.xMin = position.xMax + 1;
			position.xMax = xmax;
			var r = EditorGUI.ObjectField(position, null, Attribute.type, false);
			if (r != null)
			{
				var path = AssetDatabase.GetAssetPath(r);
				property.stringValue = !Attribute.ext ? Path.GetFileNameWithoutExtension(path) : Path.GetFileName(path);
			}
			EditorGUI.EndProperty();
		}
	}

	[CustomPropertyDrawer(typeof(SearchableAttribute))]
	public class SearchableAttributeDrawer : PropertyDrawer<SearchableAttribute>
	{
		int controlId = -1;
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.propertyType != SerializedPropertyType.String && property.propertyType != SerializedPropertyType.ObjectReference)
			{
				EditorGUI.LabelField(position, label, new GUIContent("only support string or object ref"));
				return;
			}
			base.OnGUI(position, property, label);
			label = EditorGUI.BeginProperty(position, label, property);
			// 入力フィールド域サイズ
			position.width = EditorGUIUtility.labelWidth + (fullWidth - EditorGUIUtility.labelWidth) * 0.75f;
			EditorGUI.PropertyField(position, property, label);
			// 選択サイズ
			position.xMin = position.xMax + 5;
			position.width = fullWidth - position.xMin;
			if (GUI.Button(position, "Picker"))
			{
				var method = typeof(EditorGUIUtility).GetMethod("ShowObjectPicker");
				var constructed = method.MakeGenericMethod(Attribute.type);
				controlId = GUIUtility.GetControlID(label, FocusType.Passive, position);
				if (controlId == -1)
				{
					controlId = (int)UnityEngine.Random.Range(int.MinValue, int.MaxValue);
				}
				// Debug.Log(controlId);
				constructed.Invoke(null, new object[] { null, Attribute.allowSceneObjects, Attribute.filter, controlId });
			}
			if (Event.current.commandName == "ObjectSelectorUpdated" && EditorGUIUtility.GetObjectPickerControlID() == controlId)
			{
				var selected = EditorGUIUtility.GetObjectPickerObject();
				// currentPickerWindow = -1;
				if (selected != null)
				{
					// string case
					if (property.propertyType == SerializedPropertyType.String)
					{
						string path = "";
						if (!Attribute.allowSceneObjects)
						{
							path = AssetDatabase.GetAssetOrScenePath((UnityEngine.Object)selected);
							// var ext = Path.GetExtension(path);
							// only filename
							if (Attribute.onlyFilename)
								path = Path.GetFileName(path);
							// extension
							if (!Attribute.extension)
								path = path.Split('.')[0];
						}
						else
						{
							path = selected.name;
						}
						// replace
						if (Attribute.replaceFrom != null)
						{
							path = path.Replace(Attribute.replaceFrom, Attribute.replaceTo ?? "");
						}
						property.stringValue = path;
					}
					// ref case
					else if (property.propertyType == SerializedPropertyType.ObjectReference)
					{
						property.objectReferenceValue = selected;
					}
				}
			}
			EditorGUI.EndProperty();
		}
	}

	[CustomPropertyDrawer(typeof(UniqueIdentifierAttribute))]
	public class UniqueIdentifierDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// base.OnGUI(position, prop, label);
			// Generate a unique ID, defaults to an empty string if nothing has been serialized yet
			if (property.stringValue == "")
			{
				var guid = System.Guid.NewGuid();
				property.stringValue = guid.ToString();
			}
			EditorGUI.PropertyField(position, property, label);
			// Place a label so it can't be edited by accident
			// Rect textFieldPosition = position;
			// textFieldPosition.height = 16;
			// DrawLabelField (textFieldPosition, prop, label);
		}
		// void DrawLabelField (Rect position, SerializedProperty prop, GUIContent label) {
		//     EditorGUI.LabelField(position, label, new GUIContent (prop.stringValue));
		// } 
	}
	public class PropertyDrawer<T> : UnityEditor.PropertyDrawer where T : PropertyAttribute
	{
		protected T Attribute {
			get { return (T)attribute; }
		}
		protected GameObject gameObject;
		protected Transform transform;
		protected float fullWidth;
		const float INDENT_SIZE = 14f;
		Rect position_;
		protected float IndentSize {
			get {
				return EditorGUI.indentLevel * INDENT_SIZE;
			}
		}
		protected float Width {
			get {
				return position_.width - IndentSize;
			}
		}
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			position_ = position;
			fullWidth = position.width;
			var mono = property.serializedObject.targetObject as MonoBehaviour;
			if (mono != null) {
				gameObject = mono.gameObject;
				transform = gameObject.transform;
			}
		}
		Type type;
		public Type GetPropertyType(SerializedProperty property)
		{
			if (type == null) {
				//gets parent type info
				string[] slices = property.propertyPath.Split('.');
				type = property.serializedObject.targetObject.GetType();
				for (int i = 0; i < slices.Length; i++) {
					if (slices[i] == "Array") {
						i++; //skips "data[x]"
						type = type.GetElementType(); //gets info on array elements
					}
					//gets info on field and its type
					else
						type = type.GetField(slices[i], BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance).FieldType;
				}
			}
			return type;
		}
		public float CalcLabelSize(string label)
		{
			return GUI.skin.label.CalcSize(new GUIContent(label)).x + IndentSize;
		}
	}
	public class ZeroIndent : IDisposable
	{
		private readonly int originalIndent;
		public ZeroIndent()
		{
			originalIndent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
		}

		public void Dispose()
		{
			EditorGUI.indentLevel = originalIndent;
		}
	}
}
