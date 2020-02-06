using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TicTacToe
{
    public class ViewModel : ViewModelBase
    {
        int round;
        string[] tab;
        bool endGame;
        int countX = 0;
        int countO = 0;

        public ViewModel()
        {
            NewGame();

            ScoreX = "X score: " + countX.ToString();
            ScoreO = "O score: " + countO.ToString();

            ClickCommand = new RelayCommand(
                (parameter) =>
                {
                    int param = (int)parameter;

                    if (IsEndabled[param]) // Jezeli przycisk jest mozliwy do klikniecia
                    {
                        round++;

                        WhatTextColor(param); // Ustawia kolor wyswietlanego textu

                        Content[param] = XorO(); // Ustawia text do wyswietlenia (X albo O)

                        IsEndabled[param] = false; // Blokuje przycisk do ponownego klikniecia

                        tab[param] = XorO(); // Wstawia do sztucznej tabeli X albo O

                        CheckForWin(); // Sparzwdza czy ktos wygral
                    }
                    RefreshCanExecutes();
                });

            NewGameCommand = new RelayCommand(
                (obj) =>
                {   
                    NewGame(); // Zeruje wszystkie ustawienia

                    RefreshCanExecutes();
                },
                (obj) =>
                {
                    return endGame;
                });
        }
        
        private void CheckForWin()
        {

            if (round == 9) // Jezeli sa wszystkie pola zajete i nikt jeszcze nie wygral
            {
                endGame = true;

                return;
            }

            if (tab[0].Equals(tab[4]) && tab[4].Equals(tab[8])) // Sprawdza na ukos od lewej
            {
                Win();

                WhatBackgroundColor(0, 4, 8);

                return;
            }

            if (tab[2].Equals(tab[4]) && tab[4].Equals(tab[6])) // Sprawdza na ukos od prawej
            {
                Win();

                WhatBackgroundColor(2, 4, 6);

                return;
            }

            for (int i = 0; i <= 6; i++) 
            {
                // Sprawdza w poziomie
                if ( (i == 0 || i == 3 || i == 6) && ( tab[i].Equals(tab[i + 1]) && tab[i + 1].Equals(tab[i + 2]) ) )
                {
                    Win();

                    WhatBackgroundColor(i, i + 1, i + 2);

                    return;
                }

                // Sprawdza w pionie
                if ( (i == 0 || i == 1 || i == 2) && ( tab[i].Equals(tab[i + 3]) && tab[i + 3].Equals(tab[i + 6]) ) )
                {
                    Win();

                    WhatBackgroundColor(i, i + 3, i + 6);

                    return;
                }
            }
        }

        private void Win()
        {
            endGame = true;

            WhoWin();

            // Blokuje dostep do wszystkich przyciskow
            IsEndabled = new ObservableCollection<bool>(new bool[] { false, false, false, false, false, false, false, false, false }); 
        }

        private void WhoWin()
        {
            if (XorO().Equals("O"))
            {
                countO++;
                ScoreO = "O score: " + countO.ToString();
            }
            else
            {
                countX++;
                ScoreX = "X score: " + countX.ToString();
            }
        }

        private void WhatBackgroundColor(int index1, int index2, int index3)
        {
            if (XorO().Equals("O"))
            {
                BackgroundColor[index1] = "LightGreen";
                BackgroundColor[index2] = "LightGreen";
                BackgroundColor[index3] = "LightGreen";
            }
            else
            {
                BackgroundColor[index1] = "LightCoral";
                BackgroundColor[index2] = "LightCoral";
                BackgroundColor[index3] = "LightCoral";
            }
        }

        private void WhatTextColor(int index)
        {
            if (XorO().Equals("O"))
                TextColor[index] = "Green";
            else
                TextColor[index] = "Red";
        }

        private string XorO()
        {
            return (round % 2 == 0) ? "O" : "X";
        }

        private void NewGame()
        {
            round = 0;
            endGame = false;
            tab = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8" };
            Content = new ObservableCollection<string>(new string[9]);
            TextColor = new ObservableCollection<string>(new string[9]);
            BackgroundColor = new ObservableCollection<string>(new string[] { "White", "White", "White", "White", "White", "White", "White", "White", "White" });
            IsEndabled = new ObservableCollection<bool>(new bool[] { true, true, true, true, true, true, true, true, true });
        }

        public ObservableCollection<string> Content
        {
            private set { SetProperty(ref content, value);}
            get { return content; }
        }

        public ObservableCollection<string> TextColor
        {
            private set { SetProperty(ref textColor, value); }
            get { return textColor; }
        }

        public ObservableCollection<string> BackgroundColor
        {
            private set { SetProperty(ref backgroundColor, value); }
            get { return backgroundColor; }
        }
        public ObservableCollection<bool> IsEndabled
        {
            private set { SetProperty(ref isEnabled, value); }
            get { return isEnabled; }
        }

        public string ScoreX
        {
            private set { SetProperty(ref scoreX, value); }
            get { return scoreX; }
        }

        public string ScoreO
        {
            private set { SetProperty(ref scoreO, value); }
            get { return scoreO; }
        }

        void RefreshCanExecutes()
        {
            ((RelayCommand)ClickCommand).OnCanExecuteChanged();
            ((RelayCommand)NewGameCommand).OnCanExecuteChanged();
        }

        public ICommand ClickCommand { private set; get; }
        public ICommand NewGameCommand { private set; get; }

        public ObservableCollection<string> content;
        public ObservableCollection<string> textColor;
        public ObservableCollection<string> backgroundColor;
        public ObservableCollection<bool> isEnabled;

        public string scoreX;
        public string scoreO;
    }
}
