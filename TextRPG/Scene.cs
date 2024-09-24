using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TextRPG.InventoryScene;
using static TextRPG.ShopScene;

namespace TextRPG
{
	public abstract class Scene
	{
		protected GameManager _gameManager;
		protected int waitTime;
		public Scene(GameManager gamemanager)
		{
			_gameManager = gamemanager;
			waitTime = 2000;
		}

		public abstract void ShowScene();
		public abstract void PlayScene();

		public void PromptMessage()
		{
			Console.WriteLine("원하시는 행동을 입력해주세요");
			Console.Write(">>>");
		}
		public void IntroMessage(string message)
		{
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine($"[{message}]");
			Console.ForegroundColor = ConsoleColor.White;
		}
		public void WrongInputMessage()
		{
			Console.WriteLine("잘못된 입력입니다. 다시 입력하세요.");
			Console.Write(">>>  ");
		}

	}
	public class MainScene : Scene
	{
		public MainScene(GameManager gamemanager) : base(gamemanager)
		{
		}

		public override void PlayScene()
		{
			ShowScene();
			int userInput = 0;
			while (true)
			{
				userInput = GameManager.Input();
				if (Utils.IsVaildInput(1, 6, userInput)) { break; }
				else { WrongInputMessage(); }
			}
			_gameManager.State = (GameState)userInput;
		}

		public override void ShowScene()
		{
			IntroMessage("마을");
			Console.WriteLine("어디로 이동하시겠습니까?");
			Console.WriteLine();
			Console.WriteLine("1. 상태보기");
			Console.WriteLine("2. 인벤토리");
			Console.WriteLine("3. 상점");
			Console.WriteLine("5. 휴식하기");
			Console.WriteLine();
			PromptMessage();

		}
	}
	public class StatusScene : Scene
	{
		private Player _player;
		public StatusScene(GameManager gamemanager, Player player) : base(gamemanager)
		{
			_player = player;
		}

		public override void PlayScene()
		{
			ShowScene();
			int userInput = 0;
			while (true)
			{
				userInput = GameManager.Input();
				if (Utils.IsVaildInput(1, 6, userInput)) { break; }
				else { WrongInputMessage(); }
			}
			_gameManager.State = (GameState)userInput;
		}

		public override void ShowScene()
		{
			IntroMessage("상태보기");
			Console.WriteLine("캐릭터의 정보가 표시됩니다.");
			Console.WriteLine();
			Console.WriteLine(_player.ShowInfo());
			Console.WriteLine();
			Console.WriteLine("0. 나가기");
			PromptMessage();
		}
	}
	public class InventoryScene : Scene
	{
		public enum InventoryState
		{
			ItemList,
			ItemEuip,
		}
		private InventoryState _state;
		private Player _player;
		public InventoryScene(GameManager gamemanager, Player player) : base(gamemanager)
		{
			_player = player;
			_state = InventoryState.ItemList;
		}

		public override void PlayScene()
		{
			int userInput = 0;
			switch (_state)
			{
				case InventoryState.ItemList:
					ShowScene();
					while (true)
					{
						userInput = GameManager.Input();
						if (Utils.IsVaildInput(0, 2, userInput)) { break; }
						else { WrongInputMessage(); }
					}
					if (userInput == 0)
					{
						_gameManager.State = GameState.Main;
						return;
					}
					else { _state = (InventoryState)userInput; }
					break;
				case InventoryState.ItemEuip:
					ShowItemList();
					while (true)
					{
						userInput = GameManager.Input();
						if (Utils.IsVaildInput(0, _player.GetInventorySize(), userInput)) { break; }
						else { WrongInputMessage(); }
					}
					if (userInput == 0)
					{
						_gameManager.State = GameState.Main;
						_state = InventoryState.ItemList;
						return;
					}
					else
					{
						_player.Equip(userInput);
						_state = InventoryState.ItemList;
					}
					break;
			}
		}

		public override void ShowScene()
		{
			IntroMessage("인벤토리");
			Console.WriteLine("보유중인 아이템을 관리합니다");
			Console.WriteLine();
			Console.WriteLine(_player.ShowInventoryInfo());
			Console.WriteLine();
			Console.WriteLine("1. 착용관리");
			Console.WriteLine("0. 나가기");
			Console.WriteLine();
			PromptMessage();
		}
		public void ShowItemList()
		{
			IntroMessage("인벤토리");
			Console.WriteLine("착용할 아이템의 번호를 입력하세요.");
			Console.WriteLine();
			Console.WriteLine(_player.ShowItemList());
			Console.WriteLine();
			Console.WriteLine("0. 나가기");
			Console.WriteLine();
			PromptMessage();

		}
	}
	public class ShopScene : Scene
	{
		public enum ShopSceneState
		{
			ItemList,
			ItemPurchase,
			ItemSell,
		}
		private ShopSceneState _state;
		private Shop _shop;
		private Player _player;
		public ShopScene(GameManager gamemanager, Shop shop, Player player) : base(gamemanager)
		{
			_shop = shop;
			_player = player;
		}

		public override void PlayScene()
		{
			int userInput = 0;
			switch (_state)
			{
				case ShopSceneState.ItemList:
					ShowScene();
					while (true)
					{
						userInput = GameManager.Input();
						if (Utils.IsVaildInput(0, 3, userInput)) { break; }
						else { WrongInputMessage(); }
					}
					if (userInput == 0)
					{
						_gameManager.State = GameState.Main;
						return;
					}
					else { _state = (ShopSceneState)userInput; }
					break;
				case ShopSceneState.ItemPurchase:
					ShowPurchaseList();
					while (true)
					{
						userInput = GameManager.Input();
						if (Utils.IsVaildInput(0, _shop.GetShopItemSize(), userInput)) { break; }
						else { WrongInputMessage(); }
					}
					if (userInput == 0)
					{
						_gameManager.State = GameState.Main;
						_state = ShopSceneState.ItemList;
						return;
					}
					else{
						Item item = _shop.Items[userInput - 1];
						int beforeGold = _player.Inventory.Gold;
						int state = _player.PurchaseItem(item);
						int afterGold = _player.Inventory.Gold;
						if (state == 0) { ShowSuccessPurchase(item.Name, beforeGold, afterGold); }
						else { ShowFailPurchase(item.Name, state); }
						Thread.Sleep(waitTime);
						_state = ShopSceneState.ItemList;
					}
					break;
				case ShopSceneState.ItemSell:
					ShowSellList();
					while (true)
					{
						userInput = GameManager.Input();
						if (Utils.IsVaildInput(0, _player.GetInventorySize(), userInput)) { break; }
						else { WrongInputMessage(); }
					}
					if (userInput == 0)
					{
						_gameManager.State = GameState.Main;
						_state = ShopSceneState.ItemList;
						return;
					}
					else{
						Item item = _player.Inventory.Items[userInput - 1];
						int beforeGold = _player.Inventory.Gold;
						_player.SellItem(userInput);
						int afterGold = _player.Inventory.Gold;
						ShowSuccessSell(item.Name,beforeGold, afterGold);
						Thread.Sleep(waitTime);
						_state = ShopSceneState.ItemList;
					}
					break;
			}
		}

		public override void ShowScene()
		{
			IntroMessage("상점");
			Console.WriteLine("아이템을 구매하거나 판매할 수 있는 상점입니다.");
			Console.WriteLine();
			Console.WriteLine(_shop.ShowShopItemList());
			Console.WriteLine();
			Console.WriteLine("1. 아이템 구매");
			Console.WriteLine("2. 아이템 판매");
			Console.WriteLine("0. 나가기");
			Console.WriteLine();
			PromptMessage();
		}
		public void ShowPurchaseList()
		{
			IntroMessage("상점");
			Console.WriteLine("구매할 아이템을 선택하세요.");
			Console.WriteLine();
			Console.WriteLine(_shop.ShowShopItemList(_player.Inventory));
			Console.WriteLine();
			Console.WriteLine("0. 나가기");
			Console.WriteLine();
			PromptMessage();
		}
		public void ShowSuccessPurchase(string name, int beforeGold, int afterGold)
		{
			IntroMessage("상점");
			Console.WriteLine("구매에 성공했습니다.");
			Console.WriteLine();
			Console.WriteLine($"{name}을/를 구매했습니다.");
			Console.WriteLine($"구매후 남은 골드 : {beforeGold} -> {afterGold}.");
			Console.WriteLine();
		}
		public void ShowFailPurchase(string name, int error)
		{
			IntroMessage("상점");
			Console.WriteLine("구매에 실패했습니다.");
			Console.WriteLine();
			Console.WriteLine($"{name}을/를 구매할 수 없습니다.");
			if(error == 1) { Console.WriteLine($"골드가 부족합니다."); }
			if(error == 2) { Console.WriteLine($"이미 보유한 아이템입니다."); }
			Console.WriteLine();
		}
		public void ShowSellList()
		{
			IntroMessage("상점");
			Console.WriteLine("판매할 아이템을 선택하세요.");
			Console.WriteLine();
			Console.WriteLine(_player.ShowItemList());
			Console.WriteLine();
			Console.WriteLine("0. 나가기");
			Console.WriteLine();
			PromptMessage();
		}
		public void ShowSuccessSell(string name, int beforeGold,int afterGold)
		{
			IntroMessage("상점");
			Console.WriteLine("판매에 성공했습니다.");
			Console.WriteLine();
			Console.WriteLine($"{name}을/를 판매했습니다.");
			Console.WriteLine($"판매후 골드 : {beforeGold} -> {afterGold}.");
			Console.WriteLine();
		}
	}
	public class RestScene : Scene
	{
		private Player _player;
		public RestScene(GameManager gamemanager,Player player) : base(gamemanager)
		{
			_gameManager = gamemanager;
			_player = player;
		}

		public override void PlayScene()
		{
			int userInput = 0;
			ShowScene();
			while (true)
			{
				userInput = GameManager.Input();
				if (Utils.IsVaildInput(0, 2, userInput)) { break; }
				else { WrongInputMessage(); }
			}
			if (userInput == 0)
			{
				_gameManager.State = GameState.Main;
				return;
			}
			else 
			{
				int beforeHP = _player.Character.HP;
				_player.Character.RecoveryHP(100);
				int afterHP = _player.Character.HP;
				ShowEndScene(beforeHP, afterHP);
				Thread.Sleep(waitTime);
				_gameManager.State = GameState.Main;
			}

		}

		public override void ShowScene()
		{
			IntroMessage("휴식하기");
			Console.WriteLine($"500G를 사용하여  HP를 회복합니다. (보유골드 : {_player.Inventory.Gold})");
			Console.WriteLine();
			Console.WriteLine("1. 휴식하기");
			Console.WriteLine("0. 나가기");
			Console.WriteLine();
			PromptMessage();
		}
		public void ShowEndScene(int beforeHp, int afterHP)
		{
			IntroMessage("휴식하기");
			Console.WriteLine($"휴식을 완료했습니다.");
			Console.WriteLine();
			Console.WriteLine($"HP를 회복하였습니다 {beforeHp} -> {afterHP}");
			Console.WriteLine("1초후 메인화면으로 돌아갑니다.");
			Console.WriteLine();
		}
	}
}
