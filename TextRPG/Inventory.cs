using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TextRPG
{
	public class Inventory
	{
		private List<Equipment> _items;
		[JsonInclude]
		public List<Equipment> Items { get { return _items; } init { _items = value; } }
		private int _gold;
		[JsonInclude]
		public int Gold {get { return _gold; } init { _gold = value; } }

		[JsonConstructor]
		public Inventory(List<Equipment> items, int gold) 
		{
			Items = items;
			Gold = gold;
		}
		public Inventory()
		{
			_items = new List<Equipment>();
			_gold = 1500;
		}
		public string ShowGold()
		{
			return $"Gold : {Gold}";
		}
		public string ShowItemList(bool lineNumber = false)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < _items.Count; i++) {
				if (lineNumber) { sb.AppendLine($" - {i + 1} {_items[i].Info()}"); }
				else { sb.AppendLine($" - {_items[i].Info()}"); }
			}
			return sb.ToString();
		} 
		public bool HasItem(Item item)
		{
			foreach(Item i in _items)
			{
				if (i == item) return true;
			}
			return false;
		}
		public bool IsEnoughGold(int gold)
		{
			if (_gold - gold < 0) { return false; }
			else { return true; }
		}
		public bool UseGold(int gold)
		{
            if (IsEnoughGold(gold))
			{
				_gold -= gold;
				return true;
			}
			else { return false; }
        }
		public void AddItem(Equipment item)
		{
			_items.Add(item);
		}
		public void AddGold(int gold) { _gold += gold; }
		public void RemoveItem(int index)
		{
			_items.RemoveAt(index);
		}
	}
}
