using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
	public class Player
	{
		private Playable _character;
		private Inventory _inventory;
		private Weapon _weapon;
		private Armor _armor;
		public Playable Character { get { return _character; } }
		public Inventory Inventory { get { return _inventory; } }

		public Player()
		{
			_character = new Warrior();
			_inventory = new Inventory();
		}
		public Player(int level, string name, int maxHp, int hp,
			float defaultAttack, float attack,
			float defaultDefense, float defense,
			float maxExp, float exp,
			int maxEnergy, int energy, 
			List<Item> items, int gold) 
		{
			_character = new Warrior(level, name, maxHp, hp, defaultAttack, attack, defaultDefense, defense, maxExp, exp, maxEnergy, energy);
			_inventory = new Inventory(items, gold);
		}
		public void SetName(string name)
		{
			_character.SetName(name);
		}
		public string ShowInfo()
		{
			return _character.ShowDetailStatus() + _inventory.ShowGold();
		}
		public string ShowInventoryInfo()
		{
			return _inventory.ShowGold()+"\n"+_inventory.ShowItemList();
		}
		public string ShowItemList()
		{
			return _inventory.ShowItemList(true);
		}
		public int GetInventorySize()
		{
			return _inventory.Items.Count;
		}
		public void Equip(int index)
		{
			index = index - 1;
			if (_inventory.Items[index].GetType() == typeof(Weapon)) 
			{
				Console.WriteLine(_inventory.Items[index].Info());
				Weapon weapon = (Weapon)_inventory.Items[index];
				if (weapon.IsWorn) { this.TakeOffEquipment(weapon); }
				else { this.WearEuipment(weapon); }
			}
			if (_inventory.Items[index].GetType() == typeof(Armor))
			{
				Armor armor = (Armor)_inventory.Items[index];
				this._character.AddAttack(armor.Wear());
				if (armor.IsWorn) { this.TakeOffEquipment(armor); }
				else { this.WearEuipment(armor); }
			}
		}
		private void TakeOffEquipment(Weapon weapon)
		{
			if(_weapon == weapon)
			{
				weapon.TakeOff();
				this._weapon = null;
				_character.AddAttack(-weapon.Attack);
			}
		}
		private void WearEuipment(Weapon weapon)
		{
			weapon.Wear();
			if(_weapon != null) { TakeOffEquipment(_weapon); }
			this._weapon = weapon;
			_character.AddAttack(weapon.Attack);
		}
		private void TakeOffEquipment(Armor armor)
		{
			if(_armor == armor)
			{
				armor.TakeOff();
				_armor = null;
				_character.AddDefense(-armor.Defense);
			}
		}
		private void WearEuipment(Armor armor)
		{
			armor.Wear();
			if(_armor != null) 
			{ TakeOffEquipment(_armor); }
			_armor = armor;
			_character.AddDefense(armor.Defense);
		}
		public int PurchaseItem(Item item)
			// 1 : 골드 부족
			// 2 : 이미 보유한 아이템
			// 0 : 구매 성공
		{
			if (_inventory.HasItem(item))
			{
				return 2;
			}
			if (_inventory.UseGold(item.Price))
			{
				_inventory.AddItem(item);
				return 0;
				
			}
			else { return 1; }
			
		}
		public void SellItem(int index)
		{
			index = index - 1;
			Item item = _inventory.Items[index];
			if (item.GetType().GetInterfaces()[0] == typeof(IPurchasable))
			{
				if (item.GetType() == typeof(Weapon))
				{
					TakeOffEquipment((Weapon)item);
				}
				if (item.GetType() == typeof(Armor))
				{
					TakeOffEquipment((Armor)item);
				}
				_inventory.AddGold((int)(item.Price * 0.85f));
				_inventory.RemoveItem(index);
			}
		}
	}
}
