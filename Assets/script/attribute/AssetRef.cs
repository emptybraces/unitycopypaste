using UnityEngine;
[System.Serializable]
public class AssetRef
{
	[SerializeField] string assetPath;
#if UNITY_EDITOR
	public Object assetEditor;
#endif
	Object asset;
	public Object Asset => asset ?? (asset = Load<Object>());
	public string AssetPath => assetPath;
	public bool IsNull => "" == assetPath;
	public T Load<T>() where T : Object
	{
#if UNITY_EDITOR
		return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
#else

#endif
	}
}