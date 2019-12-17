using UnityEngine;
using System.IO;
using System.Collections.Generic;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Reflection;

namespace Editor
{
	public static class ImporterEntityItemData
	{
		const int kStartRow = 2;
		const int kStartColumn = 0;
		public static bool Import(string filepath, EntityItemData entityItemData)
		{
			// using (var fs = File.Open(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			using (var fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				// ブック読み込み
				IWorkbook book;
				if (Path.GetExtension(filepath) == ".xls")
					book = new HSSFWorkbook(fs);
				else if (Path.GetExtension(filepath) == ".xlsx")
					book = new XSSFWorkbook(fs);
				else
				{
					Debug.LogError("拡張子がエクセルファイルではありません: " + filepath);
					return false;
				}
				// シート読み込み
				var items = new List<EntityItem>();
				var entity_type = typeof(EntityItem);
				var entity_fields = entity_type.GetFields(BindingFlags.Instance | BindingFlags.Public);
				for (int sheet_idx = 0, total_sheets = book.NumberOfSheets; sheet_idx < total_sheets; ++sheet_idx)
				{
					// シート取得
					ISheet sheet = book.GetSheetAt(sheet_idx);
					// シート名の先頭に#がついていたら、無視する
					if (sheet.SheetName.StartsWith("#"))
						continue;
					// 行データ読み込み
					for (int i = kStartRow, l = sheet.LastRowNum; i <= l; ++i)
					{
						Debug.Log($"\t行{i}の読み込み");
						var row = sheet.GetRow(i);
						var clm = kStartColumn;
						var item = new EntityItem();
						// skip
						if (row.GetCell(clm++).StringCellValueEx().Any())
							continue;
						foreach (var field in entity_fields) {
							field.SetValue(item, row.GetCell(clm++).GetValue(field.FieldType));
						}
						items.Add(item);
						Debug.Log($"\t結果:{item}");
					}
				}
				entityItemData.dataList = items.ToArray();
			}
			return true;
		}
	}
}