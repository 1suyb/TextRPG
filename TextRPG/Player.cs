using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TextRPG
{
	public class Player
	{

		private Playable _character;
		private Inventory _inventory;
		private Equipment[] equips = new Equipment[2];
		[JsonInclude]
		public Playable Character { get { return _character; } private set { _character = value; } }
		[JsonInclude]
		public Inventory Inventory { get { return _inventory; } private set { _inventory = value; } }
		[JsonInclude]
		public Equipment[] Equips { get { return equips; } private set { equips = value; } }

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
			List<Equipment> items, int gold) 
		{
			_character = new Warrior(Class.Warrior,level, name, maxHp, hp, defaultAttack, attack, defaultDefense, defense, maxExp, exp);
			_inventory = new Inventory(items, gold);
		}
		[JsonConstructor]
		public Player(Playable character, Inventory inventory, Equipment[] equips)
		{
			Character = character;
			Inventory = inventory;
			Equips = equips;
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
			Equipment item = _inventory.Items[index];
			if (item.IsWorn) { TakeOffEquipment(item); }
			else { WearEuipment(item); }
		}
		private void TakeOffEquipment(Equipment equipment)
		{
			if (equips[(int)equipment.EquipmentType] == equipment)
			{
				equipment.TakeOff();
				equips[(int)equipment.EquipmentType] = null;
				if(equipment.EquipmentType == EquipType.Weapon)
					_character.AddAttack(-equipment.Val);
				else 
					_character.AddDefense(-equipment.Val);
			}
		}
		private void WearEuipment(Equipment equipment)
		{
			equipment.Wear();
			if(equips[(int)equipment.EquipmentType] != null) { TakeOffEquipment(equipment); }
			equips[(int)equipment.EquipmentType] = equipment;
			if(equipment.EquipmentType == EquipType.Weapon) 
				_character.AddAttack(equipment.Val);
			else 
				_character.AddDefense(equipment.Val);
		}
		public int PurchaseItem(Equipment item)
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
			Equipment item = _inventory.Items[index];
			if (item.GetType().GetInterfaces()[0] == typeof(IPurchasable))
			{
				if (equips[(int)item.EquipmentType] == item) TakeOffEquipment(item);
				_inventory.AddGold((int)(item.Price * 0.85f));
				_inventory.RemoveItem(index);
			}
		}
	}
}
