using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
	internal class Inventory
	{
		private List<Item> _items;
		public List<Item> Items {  get { return _items; } }
		public int Gold { get; private set; }

		public void TestInit()
		{
			_items = new List<Item>();
			_items.Add(new Weapon("나무칼", "나무로 만든 칼입니다.", 1));
			_items.Add(new Weapon("돌칼", "돌로 만든 칼입니다.", 3));
			_items.Add(new Weapon("구리칼", "구리로 만든 칼입니다.", 5));
			_items.Add(new Armor("천갑옷", "나무로 만든 갑옷입니다.", 1));
			_items.Add(new Armor("나무갑옷", "돌로 만든 갑옷입니다.", 3));
			_items.Add(new Armor("구리갑옷", "구리로 만든 갑옷입니다.", 5));
			Gold = 1500;
		}

		public string ShowItemList()
		{
			StringBuilder sb = new StringBuilder();
			foreach(Item item in Items)
			{
				sb.AppendLine(item.Info());
			}
			return sb.ToString();
		}
	}
}
