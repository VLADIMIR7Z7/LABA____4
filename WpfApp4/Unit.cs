using System;

namespace WizardChessUI
{
    [Serializable]
    public class Unit
    {
        public string Name { get; set; } = string.Empty;
        public int Health { get; set; }
        public int Damage { get; set; }
        public bool IsAlive { get; set; }

        public virtual void Attack(Unit target)
        {
            if (IsAlive && target != null)
            {
                target.TakeDamage(Damage);
                Console.WriteLine($"{Name} атакует {target.Name} на {Damage} урона!");
            }
        }

        public void TakeDamage(int amount)
        {
            Health -= amount;
            if (Health <= 0)
            {
                IsAlive = false;
                Console.WriteLine($"{Name} погиб.");
            }
        }

        public override string ToString()
        {
            return $"{Name}, HP: {Health}, Damage: {Damage}, Alive: {IsAlive}";
        }
    }
}