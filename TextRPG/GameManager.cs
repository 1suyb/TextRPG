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
		Dungeon = 4,
		Rest = 5,
	}
	public class GameManager
	{
		private GameState _state;
		private Player _player;
		private Shop _shop;


		public GameState State { get {return _state; } set { _state = value; } }
		private MainScene _mainScene;
		private StatusScene _statusScene;
		private InventoryScene _inventoryScene;
		private ShopScene _shopScene;
		private DungeonScene _dungeonScene;
		private RestScene _restScene;

		public GameManager()
		{
			_state = GameState.Start;
			_player = new Player();
			_shop = new Shop();
			_shop.TestInit();

			_mainScene = new MainScene(this);
			_statusScene = new StatusScene(this, _player);
			_inventoryScene = new InventoryScene(this,_player);
			_shopScene = new ShopScene(this,_shop,_player);
			_dungeonScene = new DungeonScene(this,_player);
			_restScene = new RestScene(this,_player);
			
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
						_mainScene.PlayScene();
						break;
					case GameState.Status:
						_statusScene.PlayScene();
						break;
					case GameState.Inventory:
						_inventoryScene.PlayScene();
						break;
					case GameState.Shop:
						_shopScene.PlayScene();
						break;
					case GameState.Dungeon:
						_dungeonScene.PlayScene();
						break;
					case GameState.Rest:
						_restScene.PlayScene();
						break;

				}
			}
		}
		public static int Input()
		{
			string? input = "";
			int inputnum = 0;
			while (true)
			{
				input = Console.ReadLine();
				if(int.TryParse(input,out inputnum)) { return inputnum; }
				else { Console.WriteLine("잘못된 입력입니다. 숫자로 입력하십시오."); }
			}
		}
		
		public void StartScene()
		{
			Console.WriteLine("어서오세요 TextRPG입니다");
			Console.WriteLine("이름을 설정해주세요");
			_player.SetName(Console.ReadLine());
			_state = GameState.Main;
		}
	}
}
