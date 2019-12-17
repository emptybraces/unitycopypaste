using System.Collections.Generic;
using UnityEngine;

public class EntityItemData : ScriptableObject
{
	public EntityItem[] dataList;
}

[System.Serializable]
public class EntityItem
{
	public enum Type { LocalizeId, Category, Atk, Def, Price }
	public string lid;
	public string category;
	public float atk;
	public float def;
	public int price;

	public override string ToString()
	{
		return this.ToStringFields();
	}

	public override bool Equals(object obj)
	{
		var data = obj as EntityItem;
		return data != null && data.lid == lid;
	}

	public override int GetHashCode()
	{
		int hashCode = 1826942062;
		hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(lid);
		return hashCode;
	}
}
