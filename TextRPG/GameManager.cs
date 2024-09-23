using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
		private Player _player;
		private Shop _shop;
		public GameManager()
		{
			_state = GameState.Start;
			_player = new Player();
			_shop = new Shop();
			_shop.TestInit();
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
			int inputnum = 0;
			while (true)
			{
				input = Console.ReadLine();
				if(int.TryParse(input,out inputnum)) { return inputnum; }
				else { Console.WriteLine("잘못된 입력입니다."); }
			}
		}
		
		public void StartScene()
		{
			Console.WriteLine("어서오세요 TextRPG입니다");
			Console.WriteLine("이름을 설정해주세요");
			_player.SetName(Console.ReadLine());
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
		public bool IsVaildInput(int min, int max, int input)
		{
			if (min <= input || input < max) { return true; }
			else { return false; }
		}

		public void GetUserInputInMainScene()
		{
			int userInput = 0;
			while (true)
			{
				userInput = Input();
				if (IsVaildInput(1, 4, userInput)) { break; }
				else { Console.WriteLine("잘못된 입력입니다. 다시 입력하세요."); }
			}
			_state = (GameState)userInput;
		}
		public void StatusScene()
		{
			Console.WriteLine("Debug - Staus 진입");
			Console.WriteLine(_player.ShowInfo());
			_state = GameState.Main;
		}
		public void InventoryScene()
		{
			Console.WriteLine("Debug - Inventory 진입");
			Console.WriteLine(_player.ShowInventoryInfo());
			_state = GameState.Main;
		}
		public void ShopScene()
		{
			Console.WriteLine("Debug - Shop 진입");
			Console.WriteLine(_shop.ShowShopItemList());
			_state = GameState.Main;
		}

	}
}
