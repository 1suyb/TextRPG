using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
	public abstract class Item: IPurchasable
	{
		public abstract string Name { get; }
		public abstract string Description { get; }

		public abstract int Price { get; protected set; }

		public virtual string Info()
		{
			string detail = $"{Name} | {Description} ";
			return detail ;
		}

		public static bool operator ==(Item left, Item right)
		{
			if (left.Name == right.Name) { return true; }
			else { return false; }
		}
		public static bool operator !=(Item left, Item right)
		{
			if (left.Name == right.Name) { return false; }
			else { return true; }
		}

	}

	public abstract class Equipment : Item
	{
		protected bool _isWorn;
		public bool IsWorn { get { return _isWorn; } }

		public virtual void Wear(ICharacter chr)
		{
			_isWorn = true;
		}
		public virtual void TakeOff(ICharacter chr)
		{
			_isWorn = false;
		}

		public override string Info() 
		{
			string detail = base.Info();
			if (IsWorn) { detail = $"[E] {detail}"; }
			return detail ;
		}
	}
	public class Weapon : Equipment
	{
		private string _name;
		private string _description;
		private int _attack;
		public override string Name { get { return _name; } }
		public override string Description { get { return _description; } }
		public int Attack { get { return _attack; } }
		public override int Price { get; protected set; }

		public Weapon(string name, string description, int attack, int price)
		{
			_name = name;
			_description = description;
			_attack = attack;
			_isWorn = false;
			Price = price;
		}
		public override string Info() 
		{
			string[] details = base.Info().Split("|");
			return $"{details[0]} | 공격력 : {Attack} | {details[1]}";
		}

	}
	public class Armor : Equipment 
	{
		private string _name;
		private string _description;
		private int _defense;
		public int Defense { get { return _defense; } }
		public override string Name { get { return _name; } }
		public override string Description { get { return _description; } }
		public override int Price { get; protected set; }

		public Armor(string name, string description, int defense, int price)
		{
			_name = name;
			_description = description;
			_defense = defense;
			_isWorn = false;
			Price = price;
		}
		public override string Info()
		{
			string[] details = base.Info().Split("|");
			return $"{details[0]} | 방어력 : {Defense} | {details[1]}";
		}
	}

	public interface IPurchasable
	{
		public int Price { get; }
	}
}
