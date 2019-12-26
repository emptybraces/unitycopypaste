using System.IO;
using System.Linq;
using UnityEngine;
namespace Utils
{
	public static class CSVReader
	{
		public static string[][] Read(string path)
		{
			string csv = "";
			try
			{
#if UNITY_EDITOR || UNITY_STANDALONE
				using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				using (TextReader sr = new StreamReader(fs))
				{
					// csv = await sr.ReadToEndAsync();
					csv = sr.ReadToEnd();
				}
#endif
			}
			catch
			{
				Debug.LogError("CSVファイルが読み込めませんでした。path:" + path);
				return null;
			}
			var csv_lines = csv.Trim().Split(new[] { '\n' }, System.StringSplitOptions.None).ToArray();
			var result = new string[csv_lines.Length][];
			for (int i = 0; i < csv_lines.Length; ++i)
			{
				result[i] = csv_lines[i].Split(new[] { ',' }, System.StringSplitOptions.None);
			}
			return result;
		}
	}
}
