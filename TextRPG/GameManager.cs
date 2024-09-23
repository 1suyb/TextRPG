using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
	public enum GameState
	{
		Start = -1,
		Main = 0,
		Status = 1,
		Inventory = 2,
		Shop = 3,
	}
	internal class GameManager
	{
		private GameState _state;
		private Warrior _warrior;
		private Inventory _inventory;
		public GameManager()
		{
			_state = GameState.Start;
			_warrior = new Warrior();
			_inventory = new Inventory();
			_inventory.TestInit();
		}

		public void GameMain()
		{
			while (true)
			{
				switch (_state)
				{
					case GameState.Start:
						StartScene();
						break;
					case GameState.Main:
						MainScene();
						break;
					case GameState.Status:
						StatusScene();
						break;
					case GameState.Inventory:
						InventoryScene();	
						break;
					case GameState.Shop:
						ShopScene();
						break;

				}
			}
		}
		public int Input()
		{
			string? input = "";
			while (input == null || input == "")
			{
				input = Console.ReadLine();
			}
			return int.Parse(input);
		}
		
		public void StartScene()
		{
			Console.WriteLine("어서오세요 TextRPG입니다");
			Console.WriteLine("이름을 설정해주세요");
			_warrior.SetName(Console.ReadLine());
			_state = GameState.Main;
		}
		public void MainScene()
		{
			Console.WriteLine("1. 상태보기");
			Console.WriteLine("2. 인벤토리");
			Console.WriteLine("3. 상점");
			Console.WriteLine();
			Console.WriteLine("원하시는 행동을 입력해주세요");
			Console.Write(">>>");
			GetUserInputInMainScene();
		}
		public void GetUserInputInMainScene()
		{
			int userInput = 0;
			while (true)
			{
				userInput = Input();
				Console.WriteLine(userInput);
				Console.WriteLine((GameState)userInput);
				if (userInput <= 0 || userInput > 3)
				{
					Console.WriteLine(" 잘못된 입력입니다. ");
				}
				else
				{
					break;
				}
				
			}
			_state = (GameState)userInput;
		}
		public void StatusScene()
		{
			Console.WriteLine("Debug - Staus 진입");
			Console.WriteLine(_warrior.ShowDetailStatus());
			_state = GameState.Main;
		}
		public void InventoryScene()
		{
			Console.WriteLine("Debug - Inventory 진입");
			Console.WriteLine(_inventory.ShowItemList());
			_state = GameState.Main;
		}
		public void ShopScene()
		{
			Console.WriteLine("Debug - Shop 진입");
			_state = GameState.Main;
		}

	}
}
