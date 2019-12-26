#if UNITY_STANDALONE || UNITY_EDITOR
#define PC
#endif
using UnityEngine;

public class SystemData : Utils.Singleton<SystemData>
{
#if PC
	string pathDefault = Application.persistentDataPath + "/save/system.bin";
#endif
	SystemDataChunk chunk;
	public SystemDataChunk Chunk
	{
		get
		{
			if (chunk == null)
				Load();
			return chunk;
		}
	}

#if PC || UNITY_SWITCH
	public void Save() => Save(pathDefault, Chunk);
	public void Save(string path, SystemDataChunk newChunk)
	{
		if (!path.Any() || newChunk == null)
		{
			Debug.LogError("Save失敗");
			return;
		}
#if PC
		StandaloneManager.Instance.FileSave(path, newChunk);
#elif UNITY_SWITCH
		SwitchManager.Instance.Save(path, newChunk);
#endif
		Debug.Log($"システムデータをセーブしました。\npath:{path}, \n{newChunk}");
	}

	public void Load() => Load(pathDefault);
	public void Load(string path)
	{
		chunk = LoadData();
		if (chunk == null)
		{
			chunk = MakeNewData();
			Save(path, chunk);
		}
		else
		{
			Debug.Log($"システムデータをロードしました。\npath:{path}, \n{chunk}");
		}
#if PC
		// 解像度指定
		// var r = Constant.Resolusions;
		// StandaloneManager.Instance.SetResolution(r[chunk.resolution].w, r[chunk.resolution].h, (WindowMode)chunk.windowMode);
		// StandaloneManager.Instance.UpdateQualitySettings(chunk);
#endif
	}
#endif

#if PC || UNITY_SWITCH
	public SystemDataChunk LoadData()
	{
#if PC
		return StandaloneManager.Instance.FileLoad<SystemDataChunk>(pathDefault);
#elif UNITY_SWITCH
		return SwitchManager.Instance.FileLoad(path);
#endif
	}
#endif

	public SystemDataChunk MakeNewData()
	{
		Debug.Log("新しいシステムデータを作成します。");
		var chunk = new SystemDataChunk();
		chunk.Init();
		return chunk;
	}
}