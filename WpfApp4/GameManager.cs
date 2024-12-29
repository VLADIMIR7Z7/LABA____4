using System;
using System.Collections.Generic;
using System.Linq;

namespace WizardChessUI
{
    public class GameManager
    {
        private const int UnitCost = 50;
        private GameState gameState;
        private List<Unit> availableUnits = new List<Unit>();

        public GameManager()
        {
            gameState = GameState.LoadGameState();
            InitializeAvailableUnits();
            InitializeEnemyTeam();
        }

        private void InitializeAvailableUnits()
        {
            availableUnits.Add(new Unit { Name = "Воин 1", Health = 100, Damage = 20, IsAlive = true });
            availableUnits.Add(new Unit { Name = "Воин 2", Health = 120, Damage = 15, IsAlive = true });
            availableUnits.Add(new Unit { Name = "Воин 3", Health = 90, Damage = 25, IsAlive = true });
            availableUnits.Add(new Dragon("Дракон 1", 150, 30));
            availableUnits.Add(new Dragon("Дракон 2", 140, 35));
        }

        private void InitializeEnemyTeam()
        {
            gameState.EnemyUnits.Add(new Unit { Name = "Враг 1", Health = 80, Damage = 18, IsAlive = true });
            gameState.EnemyUnits.Add(new Unit { Name = "Враг 2", Health = 90, Damage = 22, IsAlive = true });
            gameState.EnemyUnits.Add(new Unit { Name = "Враг 3", Health = 110, Damage = 20, IsAlive = true });
            gameState.EnemyUnits.Add(new Dragon("Вражеский Дракон 1", 160, 25));
            gameState.EnemyUnits.Add(new Dragon("Вражеский Дракон 2", 150, 30));
        }

        public List<Unit> GetAvailableUnits()
        {
            return availableUnits;
        }

        public List<Unit> GetPlayerTeam()
        {
            return gameState.PlayerUnits.GetAliveUnits();
        }

        public List<Unit> GetEnemyTeam()
        {
            return gameState.EnemyUnits.GetAliveUnits();
        }

        public int GetPlayerMoney()
        {
            return gameState.PlayerMoney;
        }
        public bool AddUnitToPlayerTeam(int unitIndex)
        {
            if (unitIndex >= 0 && unitIndex < availableUnits.Count)
            {
                var selectedUnit = availableUnits[unitIndex];

                
                if (gameState.PlayerMoney >= UnitCost)
                {
                    // Создаем новый экземпляр юнита
                    var unitToAdd = new Unit
                    {
                        Name = selectedUnit.Name,
                        Health = selectedUnit.Health,
                        Damage = selectedUnit.Damage,
                        IsAlive = true // Устанавливаем, что юнит жив
                    };

                    gameState.PlayerUnits.Add(unitToAdd);
                    gameState.DeductMoney(UnitCost);
                    gameState.SaveGameState(); // Сохраняем состояние игры
                    return true;
                }
            }
            return false;
        }

        public string ExecuteNextTurn()
        {
            string logEntry = string.Empty;

            var playerUnit = gameState.PlayerUnits.GetAliveUnits().FirstOrDefault();
            var enemyUnit = gameState.EnemyUnits.GetAliveUnits().FirstOrDefault();

            if (playerUnit != null && enemyUnit != null)
            {
                // Игрок атакует врага
                playerUnit.Attack(enemyUnit);
                logEntry = $"{playerUnit.Name} атакует {enemyUnit.Name} и наносит {playerUnit.Damage} урона.";
                if (!enemyUnit.IsAlive)
                {
                    logEntry += $" {enemyUnit.Name} погиб.";
                }

                // Проверка завершения боя
                if (gameState.EnemyUnits.GetAliveUnits().Count == 0)
                {
                    logEntry += " Бой завершён. Вы победили.";
                    return logEntry;
                }

                // Враг атакует игрока
                enemyUnit.Attack(playerUnit);
                logEntry += $"\n{enemyUnit.Name} атакует {playerUnit.Name} и наносит {enemyUnit.Damage} урона.";
                if (!playerUnit.IsAlive)
                {
                    logEntry += $" {playerUnit.Name} погиб.";
                }

                // Проверка завершения боя
                if (gameState.PlayerUnits.GetAliveUnits().Count == 0)
                {
                    logEntry += " Бой завершён. Вы проиграли.";
                    return logEntry;
                }
            }

            return logEntry;
        }

        public void StartOver()
        {
            gameState.StartOver(); // Сброс состояния игры
            InitializeAvailableUnits(); // Переинициализация доступных юнитов
            InitializeEnemyTeam(); // Переинициализация команды врагов
        }
    }
}