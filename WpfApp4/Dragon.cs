using System;

namespace WizardChessUI
{
    [Serializable]
    public class Dragon : Unit
    {
        public Dragon(string name, int health, int damage)
        {
            Name = name;
            Health = health;
            Damage = damage;
            IsAlive = true;
        }

        public override void Attack(Unit target)
        {
            if (IsAlive && target != null)
            {
                target.TakeDamage(Damage);
                Console.WriteLine($"{Name} стреляет шаром в {target.Name} на {Damage} урона!");
            }
        }
    }
}