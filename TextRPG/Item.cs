using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TextRPG
{
	public abstract class Item: IPurchasable
	{
		public abstract string Name { get; set; }
		public abstract string Description { get; set; }

		public abstract int Price { get;  set; }

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

	public enum EquipType
	{
		Weapon = 0,
		Armor = 1,
	}
	public class Equipment : Item
	{
		private EquipType _equipType;
		private string _name;
		private string _description;
		private float _value;
		private bool _isWorn;

		[JsonInclude]
		public EquipType EquipmentType { get { return _equipType; } set { _equipType = value; } }

		[JsonInclude]
		public override string Name { get { return _name; } set { _name = value; } }

		[JsonInclude]
		public override string Description { get { return _description; } set { _description = value; } }

		[JsonInclude]
		public float Val { get { return _value; } set { _value = value; } }

		[JsonInclude]
		public override int Price { get; set; }

		[JsonInclude]
		public bool IsWorn { get { return _isWorn; } set { _isWorn = value; } }

		[JsonConstructor]
		public Equipment(EquipType equipmentType, string name, string description, float val, int price, bool isWorn)
		{
			EquipmentType = equipmentType;
			Name = name;
			Description = description;
			Val = val;
			Price = price;
			IsWorn = isWorn;
		}

		public Equipment(EquipType type, string name, string description, int val, int price)
		{
			EquipmentType = type;
			Name = name;
			Description = description;
			Val = val;
			IsWorn = false;
			Price = price;
		}
		public Equipment()
		{
			EquipmentType = EquipType.Weapon;
			Name = "";
			Description = "";
			Val = 0;
			IsWorn = false;
			Price = 0;
		}
		public virtual float Wear()
		{
			_isWorn = true;
			return Val;
		}
		public virtual void TakeOff()
		{
			_isWorn = false;
		}

		public override string Info() 
		{
			string detail = base.Info();
			if (IsWorn) { detail = $"[E] {detail}"; }
			string[] details = detail.Split("|");
			if(_equipType == EquipType.Weapon) { detail = $"{details[0]} | 공격력 : {Val} | {details[1]}"; }
			if (_equipType == EquipType.Armor) { detail = $"{details[0]} | 방어력 : {Val} | {details[1]}"; }
			return detail ;
		}
	}
	
	public interface IPurchasable
	{
		public int Price { get; }
	}
}
