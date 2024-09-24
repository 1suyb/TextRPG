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
			if (min <= input && input <= max) { return true; }
			else { return false; }
		}
		public static void Save<T>(T data, string savePath = @"test.json")
		{
			savePath = Path.Combine(path, savePath);
			if (!File.Exists(path)) {
				Directory.CreateDirectory(path);
			}

			string jsonString = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(savePath, jsonString,Encoding.UTF8);
		}
		public static Player Load(string loadPath = @"test.json")
		{
			Player player = new Player();
			string json = File.ReadAllText(loadPath);
			player = JsonSerializer.Deserialize<Player>(json);
			return player;
		}

	}
}
