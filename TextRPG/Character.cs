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
		[JsonInclude]
		public Class JobClass { get { return _class; } protected set { _class = value; } }
		[JsonInclude]
		public int Level { get; protected set; }
		[JsonInclude]
		public string Name { get; protected set; }
		[JsonInclude]
		public int MaxHp { get; protected set; }
		protected int _hp;
		[JsonInclude]
		public int HP { get { return _hp; }
			set
			{
				if (value > MaxHp) { _hp = MaxHp; }
				else if (value <= 0) { _hp = 0; }
				else { _hp = value; }
			}
		}
		[JsonInclude]
		public float DefaultAttack { get; protected set; }
		[JsonInclude]
		public float Attack { get; protected set; }
		[JsonInclude]
		public float DefaultDefense { get; protected set; }
		[JsonInclude]
		public float Defense { get; protected set; }
		[JsonInclude]
		public float MaxExp { get; protected set; }
		[JsonInclude]
		public float Exp { get;  protected set; }

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
		public Playable(Class jobClass, int level, string name, int maxHp, int hp, 
			float defaultAttack, float attack, 
			float defaultDefense, float defense,
			float maxExp, float exp)
		{
			JobClass = jobClass;
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
			sb.AppendLine($"Class : {JobClass.ToString()}");
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
		[JsonInclude]
		public int MaxEnergy { get; private set; }

		protected int _energy;
		[JsonInclude]
		public int Energy
		{
			get { return _energy; }
			 set
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
		public Warrior(Class cl, int level, string name, int maxHp, int hp,
			float defaultAttack, float attack,
			float defaultDefense, float defense,
			float maxExp, float exp) : base(cl, level, name, maxHp, hp, defaultAttack, attack, defaultDefense, defense, maxExp, exp)
		{
		}
	}
}
