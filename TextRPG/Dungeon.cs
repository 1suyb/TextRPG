using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
	public class Dungeon
	{
		public string Name { get; private set; }
		public int RecommendDefense {  get; private set; }
		public int DefaultReward {  get; private set; }
		public Dungeon(string name, int defense, int reward) 
		{
			Name = name;
			RecommendDefense = defense;
			DefaultReward = reward;
		}
		
		public string Info()
		{
			return $"{Name} 던전 | 방어력 {RecommendDefense} 이상 권장";
		}
		public void SetName(string name)
		{
			Name = name;
		}
		public bool TryDungeon(float defense, float attack, int hp, out int reward, out int spentHp) 
		{
			if (defense < RecommendDefense)
			{
				Random random = new Random();
				int isClear = random.Next(0, 10);
				if (isClear < 4)
				{
					reward = 0;
					spentHp = hp/2;
					return false;
				}
			}
			DungeonClear(defense, attack, out reward, out spentHp);
			return true;
        }
		private void  DungeonClear(float defense,float attack, out int reward, out int spentHp)
		{
			reward = MakeReward(attack);
			spentHp = SpendHP(defense);
		}
		private int MakeReward(float attack)
		{
			Random random = new Random();
			float addRewardRatio = 0.01f*(float)(random.NextDouble() * (attack*2 - attack) + attack);
			return (int)(DefaultReward+DefaultReward*addRewardRatio);
		}
		private int SpendHP(float defense)
		{
			Random random = new Random();
			int hpRange = (int)(RecommendDefense - defense);
			int spentHP = random.Next(20+hpRange, 35+hpRange);
			return spentHP;
		}
	}
	public class DungeonManager
	{
		private List<Dungeon> _dungeons;
		public List<Dungeon> Dungeons;

		public DungeonManager()
		{
			_dungeons = new List<Dungeon>();
			_dungeons.Add(new Dungeon("슬라임", 5, 1000));
			_dungeons.Add(new Dungeon("늑대", 11, 1700));
			_dungeons.Add(new Dungeon("고블린", 17, 2500));
		}

		public string ShowDungeons()
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < _dungeons.Count; i++)
			{
				sb.AppendLine($" - {i + 1} {_dungeons[i].Info()}");
			}
			return sb.ToString();
		}
		public bool TryDeongeon(int index,ICharacter player, out int reward, out int spentHp)
		{
			return _dungeons[index].TryDungeon(player.Defense, player.Attack, player.HP, out reward, out spentHp);
		}
		public int GetDungeonsCount() { return _dungeons.Count; }
	}
}
