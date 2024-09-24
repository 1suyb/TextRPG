using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;


namespace TextRPG
{
	public static class Utils
	{
		public static readonly string path = @"saves";
		public static bool IsVaildInput(int min, int max, int input)
		{
			if (min <= input || input < max) { return true; }
			else { return false; }
		}
		public static void Save<T>(T data, string savePath = @"test.json")
		{
			savePath = Path.Combine(path, savePath);
			if (!File.Exists(path)) {
				Directory.CreateDirectory(path);
			}
			var options = new JsonSerializerOptions
			{
				Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // 한글을 제대로 인코딩
				WriteIndented = true // 읽기 쉽게 출력
			};
			string jsonString = JsonSerializer.Serialize(data, options);
			File.WriteAllText(savePath, jsonString,Encoding.UTF8);
		}
		//public static Player Load(string loadPath = @"test.json")
		//{
		//	Player player = new Player();
		//	string json = File.ReadAllText(loadPath);
		//	Dictionary<string, object>? dict = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
		//	if(dict==null || !dict.ContainsKey("Character") || !dict.ContainsKey("Inventory"))
		//	{
		//		return player;
		//	}
			
		//	var characterElement = (JsonElement)dict["Character"];

		//	if(characterElement.TryGetProperty("Class", out JsonElement classElement))
		//	{
		//		string jobClass = classElement.ToString();
		//	}
		//	if (characterElement.TryGetProperty("Level", out JsonElement levelEelemnt))
		//	{
		//		int level = levelEelemnt.GetInt32();
		//	}
		//	if (characterElement.TryGetProperty("Name", out JsonElement classElement))
		//	{
		//		string jobClass = classElement.ToString();
		//	}
		//	if (characterElement.TryGetProperty("MaxHp", out JsonElement classElement))
		//	{
		//		string jobClass = classElement.ToString();
		//	}
		//	if (characterElement.TryGetProperty("HP", out JsonElement classElement))
		//	{
		//		string jobClass = classElement.ToString();
		//	}
		//	if (characterElement.TryGetProperty("DefaultAttack", out JsonElement classElement))
		//	{
		//		string jobClass = classElement.ToString();
		//	}
		//	if (characterElement.TryGetProperty("Attack", out JsonElement classElement))
		//	{
		//		string jobClass = classElement.ToString();
		//	}
		//	if (characterElement.TryGetProperty("Attack", out JsonElement classElement))
		//	{
		//		string jobClass = classElement.ToString();
		//	}

		//	return player;
		//}

	}
}
