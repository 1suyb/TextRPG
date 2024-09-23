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
		public int Gold {
			get { return _gold; }
			set
			{
				if (value < 0) { return; }
				else { _gold = value; }
			}

		}

		public void TestInit()
		{
			_items = new List<Item>();
			_items.Add(new Weapon("나무칼", "나무로 만든 칼입니다.", 1, 100));

			_items.Add(new Armor("천갑옷", "나무로 만든 갑옷입니다.", 1, 100));

			Gold = 1500;
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

		public string PurchaseItem(IPurchasable item)
		{
			if ((Gold - item.Price) < 0) { return "골드가 부족합니다."; }
			else { 
				Gold -= item.Price;
				return "구매 성공";
			}
		}
		
		public bool HasItem(Item item)
		{
			foreach(Item i in _items)
			{
				if (i == item) return true;
			}
			return false;
		}
	}
}
