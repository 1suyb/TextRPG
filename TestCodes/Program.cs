using System.Text.Json;
using System.Text;
namespace TestCodes
{
	internal class Program
	{
		static void Main(string[] args)
		{
			string json = File.ReadAllText(@"C:\Users\122no\Documents\GitHub\TextRPG\TextRPG\bin\Debug\net6.0\saves\test.json", Encoding.UTF8); // JSON 파일 읽기

			// JSON을 Dictionary<string, object>로 역직렬화
			var dictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(json);


			// 결과 출력
			foreach (var kvp in dictionary)
			{
				Console.WriteLine($"{kvp.Key}: {kvp.Value}");
			}

			// Inventory 데이터 접근
			if (dictionary != null && dictionary.ContainsKey("Inventory"))
			{
				var inventoryElement = (JsonElement)dictionary["Inventory"];

				// Gold 추출
				if (inventoryElement.TryGetProperty("Gold", out JsonElement goldElement))
				{
					int gold = goldElement.GetInt32();
					Console.WriteLine($"Gold: {gold}");
				}

				// Items 배열 추출
				if (inventoryElement.TryGetProperty("Items", out JsonElement itemsElement))
				{
					foreach (var item in itemsElement.EnumerateArray())
					{
						string name = item.GetProperty("Name").GetString();
						string description = item.GetProperty("Description").GetString();
						int price = item.GetProperty("Price").GetInt32();

						Console.WriteLine($"Item: {name}, Description: {description}, Price: {price}");
					}
				}
			}
		}
	}
}
