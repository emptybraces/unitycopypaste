using UnityEngine;

public class TestCSVRead : MonoBehaviour
{
	void Start()
	{
		var csv = Utils.CSVReader.Read(Application.dataPath + "/data/EntityItemData.csv");
		foreach (var i in csv) {
			Debug.Log(i.ToStringEnumerable());
		}
		Debug.Log(csv[1][3]);
		Debug.Log(csv[3][3]);
	}
}