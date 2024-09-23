using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
	public enum Class
	{
		Warrior,
		Archer,
		Wizard,
	}
	public interface ICharacter
	{
		public string Name { get; }
		public int Level {  get; }
		public int MaxHp {  get; }
		public int HP { get; }
		public int DefaultAttack { get; }
		public int Attack { get; }
		public int DefaultDefense {  get; }
		public int Defense { get; }

		public void TakeDamage(ICharacter enemy);
	}

	public class Playable : ICharacter
	{
		private Class _class;
		public string? Class { get { return Enum.GetName(typeof(Class), _class); } }
		public int Level { get; protected set; }
		public string Name { get; protected set; }

		public int MaxHp { get; protected set; }

		protected int _hp;
		public int HP { get { return _hp; } 
			protected set 
			{
				if (value > MaxHp) { _hp = MaxHp; }
				else if (value <= 0) { _hp = 0; }
				else { _hp = value; }
			} 
		}
		public int DefaultAttack { get; protected set; }
		public int Attack { get; protected set; }
		public int DefaultDefense { get; protected set; }
		public int Defense { get; protected set; }
		public virtual void TakeDamage(ICharacter enemy)
		{
			HP -= enemy.Attack;
		}
		public virtual string ShowDetailStatus()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine($"Lv {Level}");
			sb.AppendLine($"Class : {Class}");
			sb.AppendLine($"Name : {Name}");
			sb.AppendLine($"Attack : {Attack} (+{Attack-DefaultAttack})");
			sb.AppendLine($"Defense : {Defense} (+{Defense -DefaultDefense})");
			sb.AppendLine($"HP : {HP}");
			return sb.ToString();
		}
		public void SetName(string name)
		{
			Name = name; 
		}
		public void AddAttack(int attack) { this.Attack += attack; }
		public void AddDefense(int defense) { this.Defense += defense; }
		public void RecoveryHP(int hp) { HP += hp; }
	}
	public class Warrior : Playable
	{
		public int MaxEnergy { get; }

		protected int _energy;
		public int Energy
		{
			get { return _energy; }
			protected set
			{
				if (value > MaxEnergy) { _energy = MaxEnergy; }
				else if (value <= 0) { _energy = 0; }
				else { _energy = value; }
			}
		}
		public override string ShowDetailStatus()
		{
			StringBuilder sb = new StringBuilder(base.ShowDetailStatus());
			sb.AppendLine($"Energy : {Energy}");
			return sb.ToString() ;
		}
	}
}
