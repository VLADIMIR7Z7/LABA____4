using System;
using System.IO;

namespace WizardChessUI
{
    [Serializable]
    public class GameState
    {
        public UnitCollection PlayerUnits { get; private set; }
        public UnitCollection EnemyUnits { get; private set; }
        public int PlayerMoney { get; private set; }

        public GameState()
        {
            PlayerUnits = new UnitCollection();
            EnemyUnits = new UnitCollection();
            PlayerMoney = 250; // Начальные деньги игрока
        }

        public void DeductMoney(int amount)
        {
            if (PlayerMoney >= amount)
            {
                PlayerMoney -= amount;
            }
            else
            {
                throw new InvalidOperationException("Недостаточно денег.");
            }
        }

        public void ResetMoney()
        {
            PlayerMoney = 250; // Сброс денег
        }

        public void SaveGameState()
        {
            if (PlayerUnits.Units.Count > 0) // Сохраняем только если есть юниты
            {
                using (StreamWriter writer = new StreamWriter("gamestate.txt"))
                {
                    writer.WriteLine(PlayerMoney);
                    writer.WriteLine(PlayerUnits.Units.Count);
                    foreach (var unit in PlayerUnits.Units)
                    {
                        writer.WriteLine($"{unit.Name},{unit.Health},{unit.Damage},{unit.IsAlive}");
                    }
                }
            }
        }

        public static GameState LoadGameState()
        {
            GameState gameState = new GameState();
            if (File.Exists("gamestate.txt"))
            {
                using (StreamReader reader = new StreamReader("gamestate.txt"))
                {
                    gameState.PlayerMoney = int.Parse(reader.ReadLine());
                    int playerUnitCount = int.Parse(reader.ReadLine());
                    for (int i = 0; i < playerUnitCount; i++)
                    {
                        var line = reader.ReadLine();
                        var parts = line.Split(',');
                        var unit = new Unit
                        {
                            Name = parts[0],
                            Health = int.Parse(parts[1]),
                            Damage = int.Parse(parts[2]),
                            IsAlive = bool.Parse(parts[3])
                        };
                        gameState.PlayerUnits.Add(unit);
                    }
                }
            }
            return gameState; // Возвращаем загруженное состояние игры
        }

        public void StartOver()
        {
            if (File.Exists("gamestate.txt"))
            {
                File.Delete("gamestate.txt");
                Console.WriteLine("Старое сохранение удалено. Начинаем новую игру.");
            }
            PlayerUnits = new UnitCollection();
            EnemyUnits = new UnitCollection();
            ResetMoney(); // Сброс денег
        }
    }
}