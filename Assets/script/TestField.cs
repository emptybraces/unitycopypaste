using UnityEngine;

public class TestField : MonoBehaviour
{
	[SerializeField] Serializable.AssetRef assetRef1;
	[SerializeField] Serializable.AssetRef assetRef2;
	[SerializeField] Serializable.AssetRef assetRef3;
	[SerializeField] Serializable.TogglyBool togglyBool;
	[SerializeField] Serializable.TogglyFloat togglyFloat;
	[SerializeField] Serializable.TogglyVector3 togglyVector3;

	void Start()
	{
		var audio_clip = GetComponent<AudioSource>();
		audio_clip.clip = (AudioClip)assetRef1.Asset;
		audio_clip.Play();
	}
}