﻿using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Grid = Microsoft.Maui.Controls.Grid;
namespace MyExcelMAUIApp
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        private Entry _selectedEntry;
        private string _selectedCellText;

        public string SelectedCellText
        {
            get => _selectedCellText;
            set
            {
                if (_selectedCellText != value)
                {
                    _selectedCellText = value;
                    OnPropertyChanged(nameof(SelectedCellText));


                    // Update the selected cell's text whenever SelectedCellText changes
                    if (_selectedEntry != null)
                    {
                        _selectedEntry.Text = _selectedCellText;
                    }
                }
            }
        }

        const int CountColumn = 10; //20
        const int CountRow = 20;  //50
        public MainPage()
        {
            InitializeComponent();
            BindingContext = this; // Set the BindingContext for data binding
            CreateGrid();
        }

        // Notify when a property changes
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CreateGrid()
        {
            AddColumnsAndColumnLabels();
            AddRowsAndCellEntries();
        }


        #region  Add Columns And Column Labels
        private void AddColumnsAndColumnLabels()
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(20) });

            for (int col = 1; col <= CountColumn; col++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(100)} );

                if (col > 0)
                {
                    var label = new Label
                    {
                        Text = GetColumnName(col),
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center
                    };
                    Grid.SetRow(label, 0);
                    Grid.SetColumn(label, col);
                    grid.Children.Add(label);
                }
            }
        }
        #endregion

        #region Add Rows And Cell Entries
        private void AddRowsAndCellEntries()
        {
            for (int row = 0; row < CountRow; row++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                var label = new Label
                {
                    Text = (row + 1).ToString(),
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                Grid.SetRow(label, row + 1);
                Grid.SetColumn(label, 0);
                grid.Children.Add(label);


                for (int col = 1; col <= CountColumn; col++)
                {
                    var entry = new Entry
                    {
                        Text = "",
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Fill
                    };

                    entry.TextChanged += Entry_TextChanged;
                    entry.Focused += Entry_Focused;
                    entry.Unfocused += Entry_Unfocused;


                    Grid.SetRow(entry, row + 1);
                    Grid.SetColumn(entry, col);
                    grid.Children.Add(entry);
                }
            }
        }
        #endregion


        #region ColName Focuse Unfocuse Entry_Text
        private string GetColumnName(int colIndex)
        {
            int dividend = colIndex;
            string columnName = string.Empty;
            while (dividend > 0)
            {
                int modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo) + columnName;
                dividend = (dividend - modulo) / 26;
            }
            return columnName;
        }

        private void Entry_Focused(object sender, FocusEventArgs e)
        {
            if (sender is Entry entry)
            {
                _selectedEntry = entry; // Store the selected Entry reference
                // Set SelectedCellText to the focused cell's text
                SelectedCellText = entry.Text;

                // Keep a reference to the focused entry for later use
                //entry.BindingContext = entry;
            }
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {

            _selectedEntry = null; // Clear the selected Entry reference

            //if (sender is Entry entry && entry.BindingContext is Entry boundEntry)
            //{
            //    // Set the unfocused cell's text to match SelectedCellText
            //    boundEntry.Text = SelectedCellText;

            //    // Clear the BindingContext to avoid holding a reference
            //    boundEntry.BindingContext = null;
            //}

        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Entry entry)
            {
                textInput.Text = entry.Text;
            }
        }
        #endregion


        #region Buttons Group
        private void CalculateButton_Clicked(object sender, EventArgs e)
        {
            // Обробка кнопки "Порахувати"
        }
        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            // Обробка кнопки "Зберегти"
        }
        private void ReadButton_Clicked(object sender, EventArgs e)
        {
            // Обробка кнопки "Прочитати"
        }
        private async void ExitButton_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Підтвердження", "Ви дійсно хочете вийти?", "Так", "Ні");
            if (answer)
            { 
                System.Environment.Exit(0);
            }
        }
        private async void HelpButton_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Довідка", "Лабораторна робота 1. Студента Василя Іваненка", "OK");
        }
        #endregion



        #region Delete Buttons
        private void DeleteRowButton_Clicked(object sender, EventArgs e)
        {
            if (grid.RowDefinitions.Count > 1)
            {
                int lastRowIndex = grid.RowDefinitions.Count - 1;
                grid.RowDefinitions.RemoveAt(lastRowIndex);

                grid.Children.RemoveAt(lastRowIndex * (CountColumn + 1)); // Remove label

                for (int col = 0; col < CountColumn; col++)
                {
                    grid.Children.RemoveAt((lastRowIndex * CountColumn) + col + 1); // Remove entry
                }
            }
        }

        private void DeleteColumnButton_Clicked(object sender, EventArgs e)
        {
            if (grid.ColumnDefinitions.Count > 1)
            {
                int lastColumnIndex = grid.ColumnDefinitions.Count - 2;
                grid.ColumnDefinitions.RemoveAt(lastColumnIndex);
                grid.Children.RemoveAt(lastColumnIndex); // Remove label
                for (int row = 0; row < CountRow; row++)
                {
                    grid.Children.RemoveAt(row * (CountColumn + 1) + lastColumnIndex + 1); // Remove entry
                }
            }
        }
        #endregion


        #region Add Buttons
        private void AddRowButton_Clicked(object sender, EventArgs e)
        {
            int newRow = grid.RowDefinitions.Count;

            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var label = new Label
            {
                Text = newRow.ToString(),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            Grid.SetRow(label, newRow);
            Grid.SetColumn(label, 0);
            grid.Children.Add(label);
            // Add entry cells for the new row
            for (int col = 0; col < CountColumn; col++)
            {
                var entry = new Entry
                {
                    Text = "",
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Fill
                };
                entry.Unfocused += Entry_Unfocused;
                Grid.SetRow(entry, newRow);
                Grid.SetColumn(entry, col + 1);
                grid.Children.Add(entry);
            }
        }
        private void AddColumnButton_Clicked(object sender, EventArgs e)
        {
            int newColumn = grid.ColumnDefinitions.Count;
   

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });

            var label = new Label
            {
                Text = GetColumnName(newColumn),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            Grid.SetRow(label, 0);
            Grid.SetColumn(label, newColumn);
            grid.Children.Add(label);

            for (int row = 0; row < CountRow; row++)
            {
                var entry = new Entry
                {
                    Text = "",
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Fill
                };
                entry.Unfocused += Entry_Unfocused;
                Grid.SetRow(entry, row + 1);
                Grid.SetColumn(entry, newColumn);
                grid.Children.Add(entry);
            }
        }
        #endregion


    }
}