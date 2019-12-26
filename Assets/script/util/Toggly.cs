using UnityEngine;
namespace Utils
{
	[System.Serializable]
	public abstract class DrawableToggly { }
	[System.Serializable]
	public class TogglyBool : DrawableToggly
	{
		public bool enabled;
		public bool value;
		public TogglyBool(bool value)
		{
			enabled = true;
			this.value = value;
		}
	}
	[System.Serializable]
	public class TogglyFloat : DrawableToggly
	{
		public bool enabled;
		public float value;
		public TogglyFloat(float value)
		{
			enabled = true;
			this.value = value;
		}
	}
	[System.Serializable]
	public class TogglyInt : DrawableToggly
	{
		public TogglyInt(int value)
		{
			enabled = true;
			this.value = value;
		}
		public bool enabled;
		public int value;
	}
	[System.Serializable]
	public class TogglyString : DrawableToggly
	{
		public TogglyString(string value)
		{
			enabled = true;
			this.value = value;
		}
		public bool enabled;
		public string value;
	}
	[System.Serializable]
	public class TogglyColor : DrawableToggly
	{
		public TogglyColor(Color value)
		{
			enabled = true;
			this.value = value;
		}
		public bool enabled;
		public Color value;
	}
	[System.Serializable]
	public class TogglyVector3 : DrawableToggly
	{
		public TogglyVector3(Vector3 value)
		{
			enabled = true;
			this.value = value;
		}
		public bool enabled;
		public Vector3 value;
	}
}
