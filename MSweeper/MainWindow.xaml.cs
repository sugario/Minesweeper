using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using MSweeper.Controls;
using MSweeper.Windows;
using System.Threading;

namespace MSweeper
{
    static class Constats
    {
        public const int GameOver = -1;
        public const int FirstMove = 0;
    }

    public partial class MainWindow : Window
    {
        #region Fields

        public int MinesLeft { get; set; }
       
        private int Turn { get; set; }

        readonly Random rndGenerator = new Random();

        public Settings Settings { get; set; }

        #endregion

        #region Ctor

        public MainWindow()
        {
            InitializeComponent();

            this.Settings = new Settings();

            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

            AddHandler(Cell.CellLeftClickEvent, new RoutedEventHandler(CellLeftClickMethod));
            AddHandler(Cell.CellRightClickEvent, new RoutedEventHandler(CellRightClickMethod));

            StartNewGame();
        }

        #endregion

        #region Right Click Method

        private void CellRightClickMethod(object sender, RoutedEventArgs e)
        {
            var clickedCell = (Cell)e.OriginalSource;

            if (Turn == Constats.GameOver || !clickedCell.IsEnabled) return;

            if(clickedCell.IsFlag)
            {
                clickedCell.DropFlag();

                MinesLeft++;

  
            }
            else
            {
                clickedCell.RaiseFlag();

                MinesLeft--;
            }

            RefreshMineInfoTextBox();
        }

        private void RefreshMineInfoTextBox()
        {
            if(MinesLeft > 99)
            {
                MinesInfo.Text = "99";
            }
            else if(MinesLeft < 0)
            {
                MinesInfo.Text = "00";
            }
            else
            {
                MinesInfo.Text = MinesLeft.ToString("00");
            }
        }

        #endregion

        #region Left Click Method

        private void CellLeftClickMethod(object sender, RoutedEventArgs e)
        {
            var clickedCell = (Cell)e.OriginalSource;

            if(clickedCell.IsFlag || Turn == Constats.GameOver || !clickedCell.IsEnabled)
            {
                return;
            }

            if(Turn == Constats.FirstMove)
            {
                DeployMines(clickedCell);
                TimeClock.Start();
            }

            if (clickedCell.IsMine)
            {
                TriggerGameOver(clickedCell);
            }
            else
            {
                Turn++;
                OpenCell(clickedCell);
            }

            if (Turn == Settings.RowCount * Settings.ColumnCount - Settings.MineCount)
            {
                TimeClock.Stop();
                MessageBox.Show("YOU WON!");
                Turn = Constats.GameOver;
            }
        }

        #endregion

        #region Game functions

        private void StartNewGame()
        {
            Turn = 0;

            AdjustGrid();
            FillCells();

            MinesLeft = Settings.MineCount;
            RefreshMineInfoTextBox();

            TimeClock.Reset();
        }

        private void DeployMines(Cell firstClickCell)
        {
            int forbiddenID = firstClickCell.ID;

            List<int> mineCells = GetListOfMines(forbiddenID);

            for (int i = 0; i < CellCanvas.Rows * CellCanvas.Columns; i++)
            {
                if (mineCells.Contains(i))
                {
                    var cell = (Cell)CellCanvas.Children[i];
                    cell.IsMine = true;
                }
            }
        } 

        private List<int> GetListOfMines(int forbiddenMineID)
        {
            List<int> mineList = new List<int>();

            while (mineList.Count < Settings.MineCount)
            {
                int temp = rndGenerator.Next(0, CellCanvas.Rows * CellCanvas.Columns);

                if (temp == forbiddenMineID) continue;

                if (!mineList.Contains(temp))
                {
                    mineList.Add(temp);
                }
            }

            return mineList;
        }

        private void TriggerGameOver(Cell triggeredCell)
        {
            Turn = Constats.GameOver;

            triggeredCell.TriggerPrimaryExplosion();

            for (int i = 0; i < CellCanvas.Children.Count; i++)
            {
                var cell = (Cell)CellCanvas.Children[i];

                if(cell.ID != triggeredCell.ID && !cell.IsFlag && cell.IsMine)
                {
                    cell.TriggerSecondaryExplosion();
                }
                else if (cell.ID != triggeredCell.ID && cell.IsFlag && !cell.IsMine)
                {
                    cell.MarkWrongFlag();
                }
            }

            TimeClock.Stop();

            MessageBox.Show("GAME OVER!");
        }

        #endregion

        #region Cell functions

        private void AdjustGrid()
        {
            CellCanvas.Children.Clear();

            CellCanvas.Rows = Settings.RowCount;
            CellCanvas.Columns = Settings.ColumnCount;
        }

        private void FillCells()
        {
            for (int i = 0; i < CellCanvas.Rows; i++)
            {
                for (int j = 0; j < CellCanvas.Columns; j++)
                {
                    Cell newCell = new Cell();

                    newCell.ID = i * CellCanvas.Columns + j;

                    newCell.SetValue(Grid.RowProperty, i);
                    newCell.SetValue(Grid.ColumnProperty, j);

                    CellCanvas.Children.Add(newCell);
                }
            }
        }

        private void OpenCell(Cell cell)
        {
            int cellID = cell.ID;

            int bombCount = CountBombsInArea(cell.ID);
            cell.ChangeContentToNumber(bombCount);

            if (bombCount == 0)
            {
                //Thread.Sleep(100);
                ExpandCellsAround(cellID);
            }
        }

        private void ExpandCellsAround(int cellID)
        {
            int row = cellID / CellCanvas.Columns;
            int column = cellID % CellCanvas.Columns;

            for (int i = row - 1; i < row + 2; i++)
            {
                if (i < 0 || i >= CellCanvas.Rows) continue;

                for (int j = column - 1; j < column + 2; j++)
                {
                    if (j < 0 || j >= CellCanvas.Columns) continue;
                    if (i == row && j == column) continue;

                    var cell = (Cell)CellCanvas.Children[i * CellCanvas.Columns + j];

                    cell.RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, 100, MouseButton.Left)
                    {
                        RoutedEvent = UIElement.MouseLeftButtonUpEvent
                    });
                }
            }
        }

        private int CountBombsInArea(int cellID)
        {
            int row = cellID / CellCanvas.Columns;
            int column = cellID % CellCanvas.Columns;

            int counter = 0;

            for (int i = row - 1; i < row + 2; i++)
            {
                if (i < 0 || i >= CellCanvas.Rows) continue;

                for (int j = column - 1; j < column + 2; j++)
                {
                    if (j < 0 || j >= CellCanvas.Columns) continue;
                    if (i == row && j == column) continue;

                    var targetCell = (Cell)CellCanvas.Children[i * CellCanvas.Columns + j];

                    if (targetCell.IsMine)
                    {
                        counter++;
                    }
                }
            }

            return counter;
        }

        #endregion

        #region Menu Button

        private void MenuButton_Tap(object sender, RoutedEventArgs e)
        {
            MenuWindow menu = new MenuWindow();
            menu.ShowDialog();

            switch(menu.Result)
            {
                case MenuDialogResult.NewGame:
                    StartNewGame();
                    break;
                case MenuDialogResult.Quit:
                    this.Close();
                    break;
                case MenuDialogResult.Cancel:
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        #endregion

        #region Settings Button

        private void SettingsButton_Tap(object sender, RoutedEventArgs e)
        {
            SettingsMenu settingsMenu = new SettingsMenu();
            settingsMenu.ShowDialog();

            if (settingsMenu.Result == SettingsDialogResult.OK)
            {
                StartNewGame();
            }
        }

        #endregion
    }
}
