using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
	internal class Shop
	{
		private List<Item> _items;
		public List<Item> Items { get { return _items; } }

		public void TestInit()
		{
			_items = new List<Item>();
			_items.Add(new Weapon("나무칼", "나무로 만든 칼입니다.", 1, 100));
			_items.Add(new Weapon("돌칼", "돌로 만든 칼입니다.", 3, 300));
			_items.Add(new Weapon("구리칼", "구리로 만든 칼입니다.", 5, 500));
			_items.Add(new Armor("천갑옷", "나무로 만든 갑옷입니다.", 1, 100));
			_items.Add(new Armor("나무갑옷", "돌로 만든 갑옷입니다.", 3, 300));
			_items.Add(new Armor("구리갑옷", "구리로 만든 갑옷입니다.", 5, 500));
		}
		public int GetShopItemSize()
		{
			return _items.Count;
		}

		public string ShowShopItemList()
		{
			StringBuilder sb = new StringBuilder();

            foreach (Item item in _items)
            {
				sb.AppendLine($" - {item.Info()} | {item.Price}");
            }
			return sb.ToString();
        }
		public string ShowShopItemList(Inventory inventory)
		{
			StringBuilder sb = new StringBuilder();
			for(int i = 0; i < _items.Count; i++)
			{
				if (inventory.HasItem(_items[i]))
					sb.AppendLine($" - {i + 1} {_items[i].Info()} | 구매완료");
				else
					sb.AppendLine($" - {i + 1} {_items[i].Info()} | {_items[i].Price}");
			}

			return sb.ToString();
		}

	}
}
