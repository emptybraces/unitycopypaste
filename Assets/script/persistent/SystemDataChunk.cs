using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;

[System.Serializable, DataContract]
public class SystemDataChunk
{
	[DataMember] public int version;
	[DataMember] public string language;
	[DataMember] public int windowMode;
	[DataMember] public int resolution;
	[DataMember] public int saveCount;
	[DataMember] public int loadCount;
	[DataMember] public Dictionary<string, SystemDataChunkChild> dic1 = new Dictionary<string, SystemDataChunkChild>();
	public SystemDataChunk Clone()
	{
		var clone = (SystemDataChunk)MemberwiseClone();
		return clone;
	}
	public void Init()
	{
		version = 1;
		for (int i = 0; i < 10; ++i)
		{
			dic1.Add(i.ToString(), new SystemDataChunkChild());
		}
	}

	public override string ToString()
	{
		return this.ToStringFields();
	}
}

[System.Serializable]
public class SystemDataChunkChild
{
	public string s;
	public List<int> list1;
	public SystemDataChunkChild()
	{
		s = Guid.NewGuid().ToString();
		list1 = Extensions.EnumerableRepeatEx(3, () => UnityEngine.Random.Range(0, 999)).ToList();
	}
	public override string ToString()
	{
		return this.ToStringFields();
	}
}