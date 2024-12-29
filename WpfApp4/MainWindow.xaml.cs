using System;
using System.Linq;
using System.Windows;

namespace WizardChessUI
{
    public partial class MainWindow : Window
    {
        private GameManager gameManager;

        public MainWindow()
        {
            InitializeComponent();
            gameManager = new GameManager();
            InitializeAvailableUnitsList();
           
        }

        private void InitializeAvailableUnitsList()
        {
            var availableUnits = gameManager.GetAvailableUnits();
            listBoxAvailableUnits.Items.Clear();
            foreach (var unit in availableUnits)
            {
                listBoxAvailableUnits.Items.Add($"{unit.Name} (HP: {unit.Health}, Damage: {unit.Damage})");
            }
        }

        private void UpdatePlayerTeamList()
        {
            var playerTeam = gameManager.GetPlayerTeam();
            listBoxPlayerTeam.Items.Clear();
            foreach (var unit in playerTeam)
            {
                listBoxPlayerTeam.Items.Add($"{unit.Name} (HP: {unit.Health}, Damage: {unit.Damage})");
            }
        }



        private void buttonAddToTeam_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = listBoxAvailableUnits.SelectedIndex;
            if (selectedIndex >= 0)
            {
                var success = gameManager.AddUnitToPlayerTeam(selectedIndex);
                if (success)
                {
                    UpdatePlayerTeamList();
                    UpdatePlayerMoneyDisplay(); // Обновляем отображение монет
                    var selectedUnit = gameManager.GetAvailableUnits()[selectedIndex];
                    textBlockSelectedUnit.Text = $"Выбранный юнит: {selectedUnit.Name} (HP: {selectedUnit.Health}, Damage: {selectedUnit.Damage})";
                }
                else
                {
                    MessageBox.Show("Недостаточно монет или юнит уже в команде.");
                }
            }
        }

        private void UpdatePlayerMoneyDisplay()
        {
            textBlockPlayerMoney.Text = $"Монеты: {gameManager.GetPlayerMoney()}";
        }

        private void buttonStartBattle_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxPlayerTeam.Items.Count == 0)
            {
                MessageBox.Show("Выберите хотя бы одного юнита для начала боя.");
                return;
            }

            listBoxBattleLog.Items.Clear();
            var initialLog = "Бой начался!";
            listBoxBattleLog.Items.Add(initialLog);
            MessageBox.Show(initialLog);

            while (gameManager.GetPlayerTeam().Any(c => c.IsAlive) && gameManager.GetEnemyTeam().Any(c => c.IsAlive))
            {
                var logEntry = gameManager.ExecuteNextTurn();
                listBoxBattleLog.Items.Add(logEntry);
                if (logEntry.Contains("Бой завершён."))
                {
                    MessageBox.Show(logEntry);
                    gameManager.StartOver(); // Сброс состояния игры
                    UpdatePlayerTeamList(); // Обновляем список юнитов
                    UpdatePlayerMoneyDisplay(); // Обновляем отображение монет
                    listBoxBattleLog.Items.Clear(); // Очищаем журнал боя
                    textBlockSelectedUnit.Text = "Выбранный юнит: "; // Сбрасываем выбранного юнита
                    return; // Выходим из метода
                }
            }

            UpdatePlayerTeamList();
        }

        private void buttonStartOver_Click(object sender, RoutedEventArgs e)
        {
            gameManager.StartOver();
            
            UpdatePlayerTeamList();
            listBoxBattleLog.Items.Clear();
            textBlockSelectedUnit.Text = "Выбранный юнит: ";
        }
    }

}