using System;
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
		private Weapon? _weapon;
		private Armor? _armor;
		public Inventory Inventory { get { return _inventory; } }

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
			weapon.TakeOff();
			this._weapon = null;
			_character.AddAttack(-weapon.Attack);
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
			armor.TakeOff();
			_armor = null;
			_character.AddDefense(-armor.Defense);
		}
		private void WearEuipment(Armor armor)
		{
			armor.Wear();
			if(_armor != null) 
			{ TakeOffEquipment(_armor); }
			_armor = armor;
			_character.AddDefense(armor.Defense);
		}

		public void PurchaseItem(Item item)
		{
			if (_inventory.UseGold(item.Price))
			{
				_inventory.AddItem(item);
				_inventory.ShowItemList();
			}
			else { Console.WriteLine("골드가 부족합니다."); }
			
		}
		public void SellItem(int index)
		{
			if (_inventory.Items[index - 1].GetType().GetInterfaces()[0] == typeof(IPurchasable))
			{
				_inventory.AddGold((int)(_inventory.Items[index - 1].Price * 0.85f));
				_inventory.RemoveItem(index - 1);
			}
		}
	}
}
