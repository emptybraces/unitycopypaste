using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;
using System;
using NPOI.SS.UserModel;

namespace Editor
{
	public class CustomImporter : AssetPostprocessor
	{
		static readonly string pathDir = "Assets/scriptable";

		static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			var file_updated = false;
			foreach (string imported_asset_path in importedAssets)
			{
				// エクセルファイルの排他ファイルを無視
				if (imported_asset_path.Contains("~"))
					continue;
				// excel file
				if (Path.GetExtension(imported_asset_path).Contains(".xls"))
				{
					var filename = Path.GetFileNameWithoutExtension(imported_asset_path);
					Debug.Log("Excelファイルのインポート開始: " + imported_asset_path);
					// ファイル名と同じインポートクラスを読み込む
					try
					{
						var importer = Type.GetType("Editor.Importer" + filename);
						if (importer == null)
						{
							Debug.LogWarning("インポートクラスがありません: Import" + filename);
							continue;
						}
						var entity_type = Assembly.Load("Assembly-CSharp").GetType(filename);
						if (entity_type == null)
						{
							Debug.LogWarning("エンティティクラスがありません: " + filename);
							continue;
						}
						// フォルダがなければ作成
						if (!AssetDatabase.IsValidFolder(pathDir))
							AssetDatabase.CreateFolder(Path.GetDirectoryName(pathDir), Path.GetFileName(pathDir));
						// データを上書きする
						var entity_path = $"{pathDir}/{filename}.asset";
						var data = AssetDatabase.LoadAssetAtPath(entity_path, entity_type);
						// データ実体がないなら作成
						if (data == null && entity_type != null)
						{
							data = ScriptableObject.CreateInstance(entity_type);
							data.hideFlags = HideFlags.NotEditable;
							AssetDatabase.CreateAsset(data, entity_path);
						}
						Debug.Log("ファイル読み込み開始: " + entity_path);
						var result = (bool)importer.GetMethod("Import", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { imported_asset_path, data });
						if (!result)
						{
							Debug.LogError("ファイル読み込み失敗");
							continue;
						}
						EditorUtility.SetDirty(data);
						file_updated = true;
						Debug.Log("インポートが完了しました: " + entity_path);
					}
					catch (Exception e)
					{
						Debug.LogError("インポートエラー\n" + e);
						continue;
					}
				}
				if (file_updated)
				{
					AssetDatabase.SaveAssets();
					AssetDatabase.Refresh();
				}
			}
		}
	}

	public static class CellExtension
	{
		public static object GetValue(this ICell row, Type type)
		{
			switch (Type.GetTypeCode(type))
			{
				case TypeCode.String: return row.StringCellValueEx();
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.Int64:
				case TypeCode.Single: return row.NumericCellValueEx();
				case TypeCode.Boolean: return row.StringCellValueEx();
				default:
					Debug.LogError("定義されていないTypeです: " + type);
					return 0;
			}
		}
		public static string StringCellValueEx(this ICell cell)
		{
			if (cell == null || cell.StringCellValue == null)
				return "";
			return cell.StringCellValue;
		}
		public static int NumericCellValueEx(this ICell cell, int defaultValue = 0)
		{
			if (cell == null)
				return defaultValue;
			return (int)cell.NumericCellValue;
		}
		public static bool BooleanCellValueEx(this ICell cell, bool defaultValue = false)
		{
			if (cell == null)
				return defaultValue;
			return cell.BooleanCellValue;
		}
	}
}