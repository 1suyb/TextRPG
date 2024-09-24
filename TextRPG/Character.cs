using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

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
		public float DefaultAttack { get; }
		public float Attack { get; }
		public float DefaultDefense {  get; }
		public float Defense { get; }

		public void TakeDamage(int Demage);
	}

	public class Playable : ICharacter
	{
		protected Class _class;
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
		public float DefaultAttack { get; protected set; }
		public float Attack { get; protected set; }
		public float DefaultDefense { get; protected set; }
		public float Defense { get; protected set; }

		public float MaxExp { get; protected set; }
		public float Exp { get; protected set; }

		public Playable() {
			Level = 1;
			MaxExp = 1;
			Exp = 0;
		}
		public Playable(string name)
		{
			Level = 1;
			MaxExp = 1;
			Exp = 0;
			Name = name;
		}
		public Playable(int level, string name, int maxHp, int hp, 
			float defaultAttack, float attack, 
			float defaultDefense, float defense,
			float maxExp, float exp)
		{
			Level = level;
			Name = name;
			MaxHp = maxHp;
			HP = hp;
			DefaultAttack = defaultAttack;
			Attack = attack;
			DefaultDefense = defaultDefense;
			Defense = defense;
			MaxExp = maxExp;
			Exp = exp;
		}

		public virtual void AddExp(int exp)
		{
			Exp+=exp;
			if(Exp == MaxExp)
			{
				LevelUp();
			}
		}
		public virtual void TakeDamage(int Demage)
		{
			HP -= Demage;
		}
		public virtual string ShowDetailStatus()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine($"Lv {Level}");
			sb.AppendLine($"EXP : {Exp} /{MaxExp}");
			sb.AppendLine($"Class : {Class}");
			sb.AppendLine($"Name : {Name}");
			sb.AppendLine($"Attack : {Attack} (+{Attack-DefaultAttack})");
			sb.AppendLine($"Defense : {Defense} (+{Defense -DefaultDefense})");
			sb.AppendLine($"HP : {HP}");
			return sb.ToString();
		}
		public virtual void LevelUp()
		{
			Level += 1;
			DefaultAttack += 0.5f;
			Attack += 0.5f;
			DefaultDefense += 1f;
			Defense += 1f;
			MaxExp += 1;
			Exp = 0;
		}
		public void SetName(string name)
		{
			Name = name; 
		}
		public void AddAttack(float attack) { this.Attack += attack; }
		public void AddDefense(float defense) { this.Defense += defense; }
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

		public Warrior(Warrior warrior)
		{
			_class = warrior._class;
			Level = warrior.Level;
			Name = warrior.Name;
			MaxHp = warrior.MaxHp;
			HP = warrior.HP;
			DefaultAttack = warrior.DefaultAttack;
			Attack = warrior.Attack;
			DefaultDefense = warrior.DefaultDefense;
			Defense = warrior.Defense;
			MaxExp = warrior.MaxExp;
			Exp = warrior.Exp;
			MaxEnergy = warrior.MaxEnergy;
			Energy = warrior.Energy;
		}
		public Warrior() : base()
		{
			_class = TextRPG.Class.Warrior;
			Attack = 10f;
			DefaultAttack = 10f;
			Defense = 5f;
			DefaultDefense = 5f;
			MaxHp = 100;
			HP = 100;
			MaxEnergy = 30;
			Energy = 30;
			
		}
		public Warrior(int level, string name, int maxHp, int hp,
			float defaultAttack, float attack,
			float defaultDefense, float defense,
			float maxExp, float exp,
			int maxEnergy, int energy) : base(level, name, maxHp, hp, defaultAttack, attack, defaultDefense, defense, maxExp, exp)
		{
			MaxEnergy = maxEnergy;
			Energy = energy;
		}
	}
}
