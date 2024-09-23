﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
	public class Player
	{
		private Playable _character;
		private Inventory _inventory;

		public Player()
		{
			_character = new Playable();
			_inventory = new Inventory();
			_inventory.TestInit();
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
			return _inventory.ShowItemList(true	);
		}
		public int GetInventorySize()
		{
			return _inventory.Items.Count;
		}
		public void WearEquipment(int index)
		{
			if (_inventory.Items[index-1].GetType() == typeof(Equipment)) 
			{
				Weapon weapon = (Weapon) _inventory.Items[index-1];
				this._character.AddAttack(weapon.Wear());
				Console.WriteLine($"{weapon.Name} 장착 성공");
			}
			if (_inventory.Items[index - 1].GetType() == typeof(Armor))
			{
				Armor armor = (Armor)_inventory.Items[index - 1];
				this._character.AddAttack(armor.Wear());
				Console.WriteLine($"{armor.Name} 장착 성공");
			}
		}
	}
}
