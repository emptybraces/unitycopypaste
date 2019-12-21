using UnityEngine;

public class Test : MonoBehaviour
{
	[SerializeField] AssetRef assetRef1;
	[SerializeField] AssetRef assetRef2;
	[SerializeField] AssetRef assetRef3;

	void Start()
	{
		var audio_clip = GetComponent<AudioSource>();
		audio_clip.clip = (AudioClip)assetRef1.Asset;
		audio_clip.Play();
	}
}