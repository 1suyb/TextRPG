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
			if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
				return true;
			if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
				return false;
			if (left.Name == right.Name) { return true; }
			else { return false; }

		}
		public static bool operator !=(Item left, Item right)
		{
			if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
				return false;
			if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
				return true;

			if (left.Name == right.Name) { return false; }
			else { return true; }
		}

	}

	public abstract class Equipment : Item
	{
		protected bool _isWorn;
		public bool IsWorn { get { return _isWorn; } }

		public virtual float Wear()
		{
			_isWorn = true;
			return 0;
		}
		public virtual void TakeOff()
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
		private float _attack;
		public override string Name { get { return _name; } }
		public override string Description { get { return _description; } }
		public float Attack { get { return _attack; } }
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
		public override float Wear()
		{
			base.Wear();
			return this.Attack;
		}

	}
	public class Armor : Equipment 
	{
		private string _name;
		private string _description;
		private float _defense;
		public float Defense { get { return _defense; } }
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
		public override float Wear()
		{
			base.Wear();
			return this.Defense;
		}
	}

	public interface IPurchasable
	{
		public int Price { get; }
	}
}
