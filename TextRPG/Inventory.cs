using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
	public class Inventory
	{
		private List<Item> _items;
		public List<Item> Items {  get { return _items; } }
		private int _gold;
		public int Gold {get { return _gold; }}

		public void TestInit()
		{
			_items = new List<Item>();
			_items.Add(new Weapon("나무칼", "나무로 만든 칼입니다.", 1, 100));

			_items.Add(new Armor("천갑옷", "나무로 만든 갑옷입니다.", 1, 100));

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
		public void AddItem(Item item)
		{
			_items.Add(item);
		}
	}
}
