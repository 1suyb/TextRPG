using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
		Rest = 5,
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
					case GameState.Rest:
						RestScene();
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
			Console.WriteLine();
			Console.WriteLine("1. 상태보기");
			Console.WriteLine("2. 인벤토리");
			Console.WriteLine("3. 상점");
			Console.WriteLine("5. 휴식하기");
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
				if (IsVaildInput(1, 6, userInput)) { break; }
				else { Console.WriteLine("잘못된 입력입니다. 다시 입력하세요."); }
			}
			_state = (GameState)userInput;
		}
		public void StatusScene()
		{
			Console.WriteLine();
			Console.WriteLine("Debug - Staus 진입");
			Console.WriteLine(_player.ShowInfo());
			_state = GameState.Main;
		}
		public void InventoryScene()
		{
			Console.WriteLine();
			Console.WriteLine("Debug - Inventory 진입");
			Console.WriteLine(_player.ShowInventoryInfo());
			Console.WriteLine("1. 착용 관리");
			Console.WriteLine("0. 나가기");
			Console.WriteLine();
			int input = 0;
			while (true) 
			{
				input = Input();
				if(IsVaildInput(0,2, input)) { break; }
				else { Console.WriteLine("잘못된 입력입니다."); }
			}
			if(input == 1)
			{
				Console.WriteLine(_player.ShowItemList());
				Console.WriteLine("0. 나가기");
				while (true)
				{
					input = Input();
					if (IsVaildInput(0, _player.GetInventorySize()-1, input)) { break; }
					else { Console.WriteLine("잘못된 입력입니다."); }
				}
				if(input != 0)
				{
					_player.Equip(input);
				}
			}
			_state = GameState.Main;
		}
		public void ShopScene()
		{
			Console.WriteLine();
			Console.WriteLine("Debug - Shop 진입");
			Console.WriteLine(_shop.ShowShopItemList());
			Console.WriteLine("1. 아이템 구매");
			Console.WriteLine("2. 아이템 판매");
			Console.WriteLine("0. 나가기");
			int input = 0;
			while (true)
			{
				input = Input();
				if (IsVaildInput(0, 2, input)) { break; }
				else { Console.WriteLine("잘못된 입력입니다."); }
			}
			if (input == 1)
			{
				Console.WriteLine(_shop.ShowShopItemList(_player.Inventory));
				Console.WriteLine("0. 나가기");
				while (true)
				{
					input = Input();
					if (IsVaildInput(0, _shop.GetShopItemSize() - 1, input)) { break; }
					else { Console.WriteLine("잘못된 입력입니다."); }
				}
				if (input != 0)
				{
					_player.PurchaseItem(_shop.Items[input-1]);
				}
			}
			if(input == 2)
			{
				Console.WriteLine(_player.ShowItemList());
				while (true)
				{
					input = Input();
					if (IsVaildInput(0, _player.GetInventorySize() - 1, input)) { break; }
					else { Console.WriteLine("잘못된 입력입니다."); }
				}
				if (input != 0) 
				{
					_player.SellItem(input);
				}
			}
			_state = GameState.Main;
		}
		public void RestScene()
		{
			Console.WriteLine("");
			Console.WriteLine($"500G로 HP를 회복 할 수 있습니다. (보유골드 : {_player.Inventory.Gold})");
			Console.WriteLine("1. 휴식하기");
			Console.WriteLine("0. 나가기");
			int input = 0;
			while (true)
			{
				input = Input();
				if (IsVaildInput(0, 2, input)) { break; }
				else { Console.WriteLine("잘못된 입력입니다."); }
			}
			if (input == 1)
			{
				if (_player.Inventory.Gold < 500)
				{
					Console.WriteLine("골드가 부족합니다.");
					return;
				}
                else
                {
					int beforeHP = _player.Character.HP;
					_player.Character.RecoveryHP(100);
					int afterHP = _player.Character.HP;
					Console.WriteLine($"HP를 회복하였습니다. {beforeHP} -> {afterHP}");
                }
			}
			_state = GameState.Main;
		}
	}
}
