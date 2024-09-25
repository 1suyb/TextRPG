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
			waitTime = 1500;
		}

		public abstract void DisplayScene();
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
			DisplayScene();
			int userInput = 0;
			while (true)
			{
				userInput = GameManager.Input();
				if (Utils.IsVaildInput(1, 6, userInput)) { break; }
				else { WrongInputMessage(); }
			}
			_gameManager.State = (GameState)userInput;
		}

		public override void DisplayScene()
		{
			IntroMessage("마을");
			Console.WriteLine("어디로 이동하시겠습니까?");
			Console.WriteLine();
			Console.WriteLine("1. 상태보기");
			Console.WriteLine("2. 인벤토리");
			Console.WriteLine("3. 상점");
			Console.WriteLine("4. 던전");
			Console.WriteLine("5. 휴식하기");
			Console.WriteLine("6. 저장하기");
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
			DisplayScene();
			int userInput = 0;
			while (true)
			{
				userInput = GameManager.Input();
				if (Utils.IsVaildInput(0, 0, userInput)) { break; }
				else { WrongInputMessage(); }
			}
			_gameManager.State = (GameState)userInput;
		}

		public override void DisplayScene()
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
					DisplayScene();
					while (true)
					{
						userInput = GameManager.Input();
						if (Utils.IsVaildInput(0, 1, userInput)) { break; }
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
					DisplayItemList();
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

		public override void DisplayScene()
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
		public void DisplayItemList()
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
					DisplayScene();
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
					else { _state = (ShopSceneState)userInput; }
					break;
				case ShopSceneState.ItemPurchase:
					DisplayPurchaseList();
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
						Equipment item = _shop.Items[userInput - 1];
						int beforeGold = _player.Inventory.Gold;
						int state = _player.PurchaseItem(item);
						int afterGold = _player.Inventory.Gold;
						if (state == 0) { DisplaySuccessPurchase(item.Name, beforeGold, afterGold); }
						else { DisplayFailPurchase(item.Name, state); }
						Thread.Sleep(waitTime);
						_state = ShopSceneState.ItemList;
					}
					break;
				case ShopSceneState.ItemSell:
					DisplaySellList();
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
						DisplaySuccessSell(item.Name,beforeGold, afterGold);
						Thread.Sleep(waitTime);
						_state = ShopSceneState.ItemList;
					}
					break;
			}
		}

		public override void DisplayScene()
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
		public void DisplayPurchaseList()
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
		public void DisplaySuccessPurchase(string name, int beforeGold, int afterGold)
		{
			IntroMessage("상점");
			Console.WriteLine("구매에 성공했습니다.");
			Console.WriteLine();
			Console.WriteLine($"{name}을/를 구매했습니다.");
			Console.WriteLine($"구매후 남은 골드 : {beforeGold} -> {afterGold}.");
			Console.WriteLine();
		}
		public void DisplayFailPurchase(string name, int error)
		{
			IntroMessage("상점");
			Console.WriteLine("구매에 실패했습니다.");
			Console.WriteLine();
			Console.WriteLine($"{name}을/를 구매할 수 없습니다.");
			if(error == 1) { Console.WriteLine($"골드가 부족합니다."); }
			if(error == 2) { Console.WriteLine($"이미 보유한 아이템입니다."); }
			Console.WriteLine();
		}
		public void DisplaySellList()
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
		public void DisplaySuccessSell(string name, int beforeGold,int afterGold)
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
			DisplayScene();
			while (true)
			{
				userInput = GameManager.Input();
				if (Utils.IsVaildInput(0, 1, userInput)) { break; }
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
				DisplayEndScene(beforeHP, afterHP);
				Thread.Sleep(waitTime);
				_gameManager.State = GameState.Main;
			}

		}

		public override void DisplayScene()
		{
			IntroMessage("휴식하기");
			Console.WriteLine($"500G를 사용하여  HP를 회복합니다. (보유골드 : {_player.Inventory.Gold})");
			Console.WriteLine();
			Console.WriteLine("1. 휴식하기");
			Console.WriteLine("0. 나가기");
			Console.WriteLine();
			PromptMessage();
		}
		public void DisplayEndScene(int beforeHp, int afterHP)
		{
			IntroMessage("휴식하기");
			Console.WriteLine($"휴식을 완료했습니다.");
			Console.WriteLine();
			Console.WriteLine($"HP를 회복하였습니다 {beforeHp} -> {afterHP}");
			Console.WriteLine("1초후 메인화면으로 돌아갑니다.");
			Console.WriteLine();
		}
	}
	public class DungeonScene : Scene
	{
		private Player _player;
		private DungeonManager _dungeonManager;
		public DungeonScene(GameManager gamemanager,Player player) : base(gamemanager)
		{
			_player = player;
			_dungeonManager = new DungeonManager();	
		}

		public override void PlayScene()
		{
			_gameManager.State = GameState.Main;
			int userInput = 0;
			DisplayScene();
			while (true)
			{
				userInput = GameManager.Input();
				if (Utils.IsVaildInput(0, _dungeonManager.GetDungeonsCount(), userInput)) { break; }
				else { WrongInputMessage(); }
			}
			if (userInput == 0)
				return;
			else
			{
				int reward = 0,spentHP = 0;
				int index = userInput - 1;
				if (_dungeonManager.TryDeongeon(index, _player.Character, out reward, out spentHP))
				{
					_player.Inventory.AddGold(reward);
					_player.Character.TakeDamage(spentHP);
					DisplayClearDungeon(reward,_player.Inventory.Gold, spentHP,_player.Character.HP);
				}
				else
				{
					_player.Character.TakeDamage(spentHP);
					DisplayFailDungeon(spentHP, _player.Character.HP);
				}
				Thread.Sleep(waitTime);
			}
		}

		public override void DisplayScene()
		{
			IntroMessage("던전");
			Console.WriteLine($"던전에 들어가시겠습니까?");
			Console.WriteLine();
			Console.WriteLine(_dungeonManager.ShowDungeons());
			Console.WriteLine("0. 나가기");
			Console.WriteLine();
			PromptMessage();
		}
		public void DisplayClearDungeon(int reward, int afterGold, int HP, int aftereHp)
		{
			IntroMessage("던전");
			Console.WriteLine($"던전 클리어!");
			Console.WriteLine();
			Console.WriteLine($"획득한 보상");
			Console.WriteLine($"Gold : {reward} ({afterGold-reward} -> {afterGold})");
			Console.WriteLine($"경미한 부상!");
			Console.WriteLine($"HP : {aftereHp+ HP} -> {aftereHp}");
			Console.WriteLine();

		}
		public void DisplayFailDungeon(int HP, int aftereHp)
		{
			IntroMessage("던전");
			Console.WriteLine($"던전 실패!");
			Console.WriteLine();
			Console.WriteLine($"큰 부상!");
			Console.WriteLine($"HP : {aftereHp + HP} -> {aftereHp}");
			Console.WriteLine();
		}
	}
	public class SaveScene : Scene
	{
		private Player _player;
		public SaveScene(GameManager gamemanager, Player player) : base(gamemanager)
		{
			_player = player;
		}

		public override void DisplayScene()
		{
			IntroMessage("저장하기");
			Console.WriteLine($"현재 데이터를 저장합니다.");
			Console.WriteLine();
			Console.WriteLine("1. 저장하기");
			Console.WriteLine("0. 나가기");
			Console.WriteLine();
			PromptMessage();
		}
		public void DisplaySaveName()
		{
			IntroMessage("저장하기");
			Console.WriteLine($"현재 데이터를 저장합니다.");
			Console.WriteLine();
			Console.WriteLine($"저장 파일 이름을 설정해주세요.");
			Console.WriteLine();
			PromptMessage();
		}
		public void DisplayEndScene(string path)
		{
			IntroMessage("저장하기");
			Console.WriteLine($"현재 데이터가 저장되었습니다.");
			Console.WriteLine();
			Console.WriteLine($"{Directory.GetCurrentDirectory() +@"\"+ path}에 데이터가 저장되었습니다.");
			Console.WriteLine();
			PromptMessage();
		}


		public override void PlayScene()
		{
			_gameManager.State = GameState.Main;
			int userInput = 0;
			DisplayScene();
			while (true)
			{
				userInput = GameManager.Input();
				if (Utils.IsVaildInput(0, 1, userInput)) { break; }
				else { WrongInputMessage(); }
			}
			if (userInput == 0)
				return;
			else
			{
				DisplaySaveName();
				string saveFileName = Console.ReadLine();
				Utils.Save<Player>(_player,saveFileName);
				DisplayEndScene(Utils.path);
				Thread.Sleep(waitTime);
			}
		}
		
	}

	public class StartScene : Scene
	{
		public enum StartSceneState
		{
			Start,
			CreateCharacter,
			LoadData,
			LoadDataFail,
		}
		private Player _player;
		private StartSceneState _state;
		public StartScene(GameManager gamemanager) : base(gamemanager)
		{
		}

		public override void DisplayScene()
		{
			IntroMessage("TextRPG");
			Console.WriteLine("어서오세요! TextRPG 입니다.");
			Console.WriteLine();
			Console.WriteLine("1. 새로하기") ;
			Console.WriteLine("2. 이어하기");
			Console.WriteLine();
			PromptMessage();

		}
		public void DisplayCreateNewCharacter()
		{
			IntroMessage("TextRPG");
			Console.WriteLine("어서오세요! TextRPG 세계에!");
			Console.WriteLine();
			Console.WriteLine("케릭터 이름을 입력해주세요.");
			Console.WriteLine();
			PromptMessage();
		}
		public void DisplayLoadCharacter(string[] savedatas)
		{
			IntroMessage("TextRPG");
			Console.WriteLine("다시만나서 반갑습니다!");
			Console.WriteLine();
			Console.WriteLine("로드할 세이브 데이터를 선택해주세요!");
			for(int i = 0; i < savedatas.Length; i++)
			{
				Console.WriteLine($" - {i + 1} {savedatas[i]}");
			}
			Console.WriteLine();
			Console.WriteLine("0. 새로하기");
			Console.WriteLine();
			PromptMessage();
		}
		public void DisplayLoadFail()
		{
			IntroMessage("TextRPG");
			Console.WriteLine("어서오세요! TextRPG 세계에!");
			Console.WriteLine();
			Console.WriteLine("저장된 데이터가 없는것 같아요...");
			Console.WriteLine("1. 새로하기");
			Console.WriteLine();
			PromptMessage();
		}
		public override void PlayScene()
		{
			int userInput = 0;
			switch (_state)
			{
				case StartSceneState.Start:
					DisplayScene();
					while (true)
					{
						userInput = GameManager.Input();
						if (Utils.IsVaildInput(1, 2, userInput)) { break; }
						else { WrongInputMessage(); }
					}
					_state = (StartSceneState)userInput;
					break;
				case StartSceneState.CreateCharacter:
					DisplayCreateNewCharacter();
					string input = Console.ReadLine();
					_player = new Player();
					_player.SetName(input);
					_gameManager.SetPlayer( _player );
					_gameManager.State = GameState.Main;
					Thread.Sleep(waitTime);
					break;
				case StartSceneState.LoadData:
					string[]? files = null;
					if (Directory.Exists(Utils.path))
					{
						files = Directory.GetFiles(Utils.path);
					}
					if (files.Length>0)
					{
						DisplayLoadCharacter(files);
						while (true)
						{
							userInput = GameManager.Input();
							if (Utils.IsVaildInput(0, files.Length, userInput)) { break; }
							else { WrongInputMessage(); }
						}
						if (userInput == 0) { _state =  StartSceneState.CreateCharacter; }
						else {
							_player = Utils.Load(files[userInput - 1]);
							Console.WriteLine(_player.ShowInfo());
							_gameManager.SetPlayer(_player);
							Thread.Sleep(waitTime);
							_gameManager.State = GameState.Main;
						}
					}
					else
						_state = StartSceneState.LoadDataFail;
					break;
				case StartSceneState.LoadDataFail:
					DisplayLoadFail();
					while (true)
					{
						userInput = GameManager.Input();
						if (Utils.IsVaildInput(0, 1, userInput)) { break; }
						else { WrongInputMessage(); }
					}
					if (userInput == 1) { _state = StartSceneState.CreateCharacter; }
					break;
			}
			

		}
	}
}
