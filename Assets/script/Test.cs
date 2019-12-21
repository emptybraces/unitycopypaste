using UnityEngine;

public class Test : MonoBehaviour
{
	[SerializeField] Serializable.AssetRef assetRef1;
	[SerializeField] Serializable.AssetRef assetRef2;
	[SerializeField] Serializable.AssetRef assetRef3;

	void Start()
	{
		var audio_clip = GetComponent<AudioSource>();
		audio_clip.clip = (AudioClip)assetRef1.Asset;
		audio_clip.Play();
	}
}