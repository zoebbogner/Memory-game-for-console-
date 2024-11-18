using using System;
using System.Collections.Generic;
using System.Text;

namespace B24_Ex02_MemoryGameUI
{
    public class ConsoleUI
    {
        private readonly GameLogicManager m_GameLogicManager;
        private GameBoard m_GameBoard;
        private bool m_IsFirstTurn;

        private ConsoleUI(GameLogicManager i_GameLogicManager)
        {
            m_GameLogicManager = i_GameLogicManager;
            m_GameBoard = new GameBoard(m_GameLogicManager.GameBoardInInts);
            m_IsFirstTurn = true;
        }

        public static ConsoleUI CreateConsoleUI()
        {
            string input;
            string firstPlayerName, secondPlayerName;
            bool isAgainstComputer;
            int numOfRows, numOfCols, maxNumberOfCardsToRemember;
            Tuple<eBoardValidationResult, GameLogicManager> gameLogicResult = new Tuple<eBoardValidationResult, GameLogicManager>(eBoardValidationResult.NotInitialized, null);
            Tuple<string, bool> player;
            List<Tuple<string, bool>> players = new List<Tuple<string, bool>>();

            ConsoleUIMessages.PrintWelcomeMessage();
            firstPlayerName = Console.ReadLine();
            player = new Tuple<string, bool>(firstPlayerName, false);
            players.Add(player);
            ConsoleUIMessages.PrintOptionsSecondPlayerChoiceMessage();
            input = Console.ReadLine();
            isAgainstComputer = input.Equals("Y");
            secondPlayerName = "Computer";
            if(!isAgainstComputer)
            {
                ConsoleUIMessages.PrintSecondHumanPlayerNameMessage();
                secondPlayerName = Console.ReadLine();
            }

            player = new Tuple<string, bool>(secondPlayerName, isAgainstComputer);
            players.Add(player);

            if(isAgainstComputer)
            {
                ConsoleUIMessages.ChooseLevelOfDifficultyMessage();
                input = Console.ReadLine();
                maxNumberOfCardsToRemember = getLevelOfDifficulty(input);
            }

            ConsoleUIMessages.PrintBoardSizeMessage();
            gameLogicResult = tryToCreateGameLogicManager(players, maxNumberOfCardsToRemember);
            while(gameLogicResult.Item1 != eBoardValidationResult.Valid)
            {
                ConsoleUIMessages.PrintInvalidBoardSizeMessage();
                gameLogicResult = tryToCreateGameLogicManager(players, maxNumberOfCardsToRemember);
            }

            return new ConsoleUI(gameLogicResult.Item2);
        }

        private int getLevelOfDifficulty(string i_LevelOfDifficulty)
        {
            int levelOfDifficulty = 0;

            switch(i_LevelOfDifficulty)
            {
                case "1":
                    levelOfDifficulty = 3;
                    break;
                case "2":
                    levelOfDifficulty = 6;
                    break;
                case "3":
                    levelOfDifficulty = 9;
                    break;
            }

            return levelOfDifficulty;
        }

        private Tuple<eBoardValidationResult, GameLogicManager> tryToCreateGameLogicManager(List<Tuple<string, bool>>, int i_MaxNumberOfCardsToRemember)
        {
            int numOfRows, numOfCols;
            Tuple<eBoardValidationResult, GameLogicManager> gameLogicResult = new Tuple<eBoardValidationResult, GameLogicManager>(eBoardValidationResult.NotInitialized, null);
            bool isValidBoardSize = false;

            ConsoleUIMessages.PrintNumberOfRowsInBoardMessage();
            numOfRows = int.Parse(Console.ReadLine());
            ConsoleUIMessages.PrintNumberOfColsInBoardMessage();
            numOfCols = int.Parse(Console.ReadLine());
            isValidBoardSize = (numOfCols <= 6 && numOfCols >= 4) && (numOfRows <= 6 && numOfRows >= 4);
            if(isValidBoardSize)
            {
                gameLogicResult = GameLogicManager.CreateGameLogic(i_MaxNumberOfCardsToRemember, i_Players, numOfRows, numOfCols);
            }

            return gameLogicResult;
        }

        public void RunGame()
        {
            bool isGameOver = false;
            string input;
            Tuple<int, int> cardIndexes;
            eCardRevealStatus cardRevealStatus = eCardRevealStatus.Valid;
            Player currentPlayer;
            bool isCurrentPlayerHuman, isCardValid;
            string currentPlayerName;
            List<Player> winners;

            while(!isGameOver)
            {
                Console.Clear();
                m_GameBoard.DisplayBoard(m_GameLogicManager.GameBoardRevealed);
                currentPlayer = m_GameLogicManager.CurrentPlayer();
                currentPlayerName = currentPlayer.Name;
                isCurrentPlayerHuman = !currentPlayer.IsComputer;
                if(isCurrentPlayerHuman)
                {
                    ConsoleUIMessages.PrintPlayerTurnMessage(currentPlayerName);
                    makePlayerMove();
                }
                else
                {
                    cardIndexes = m_GameLogicManager.MakeComputerMove();
                    m_GameLogicManager.MakeMove(cardIndexes.Item1, cardIndexes.Item2);
                }

                Console.Clear();
                m_GameBoard.DisplayBoard(m_GameLogicManager.GameBoardRevealed);
                if(!m_IsFirstTurn)
                {
                    if(m_GameLogicManager.IsMatch())
                    {
                        ConsoleUIMessages.PrintMatchMessage();
                    }
                    else
                    {
                        ConsoleUIMessages.PrintNoMatchMessage();
                    }

                    System.Threading.Thread.Sleep(2000);
                }

                m_IsFirstTurn = !m_IsFirstTurn;
                isGameOver = m_GameLogicManager.IsGameOver();

            }

            if(isGameOver)
            {
                winners = m_GameLogicManager.GetWinners();
                if(winners.Count == 1)
                {
                    ConsoleUIMessages.PrintWinnerMessage(winners[0].Name);
                }
                else
                {
                    ConsoleUIMessages.PrintTieMessage(winners);
                }
            }

            ConsoleUIMessages.PrintPlayAgainMessage();
            input = Console.ReadLine();
            if(input == "Y")
            {
                RunGame();
            }
            
            ConsoleUIMessages.PrintGoodbyeMessage();
        }

        private void makePlayerMove()
        {
            Tuple<int, int> indexes;
            int row, col;
            eCardRevealStatus cardRevealStatus = eCardRevealStatus.NotInitialized;
            string indexesInString;
            bool isValidInput = false;

            while(cardRevealStatus != eCardRevealStatus.Valid)
            {
                ConsoleUIMessages.PrintCardRevealMessage();
                while(!isValidInput)
                {
                    indexesInString = Console.ReadLine();
                    if(indexesInString == "Q")
                    {
                        ConsoleUIMessages.PrintGoodbyeMessage();
                        Environment.Exit(0);
                    }
                    isValidInput = validateCardInput(indexesInString);
                    if(!isValidInput)
                    {
                        ConsoleUIMessages.PrintCardRevealMessage();
                    }
                }

                indexes = parseCardIndexes(indexesInString);
                row = indexes.Item1;
                col = indexes.Item2;
                cardRevealStatus = m_GameLogicManager.MakeMove(row, col);
                switch(cardRevealStatus)
                {
                    case eCardRevealStatus.RowIndexOutOfRange:
                        ConsoleUIMessages.PrintInvalidRowIndexMessage(m_GameLogicManager.NumOfRows);
                        break;
                    case eCardRevealStatus.ColIndexOutOfRange:
                        ConsoleUIMessages.PrintInvalidColIndexMessage(m_GameLogicManager.NumOfCols);
                        break;
                    case eCardRevealStatus.AlreadyRevealed:
                        ConsoleUIMessages.PrintCardAlreadyRevealedMessage();
                        break;
                    case eCardRevealStatus.Valid:
                        break;
                }
            }
        }

        private bool validateCardInput(string i_Indexes)
        {
            bool isValid = true;
            int row, col;
            bool isLengthValid = i_Indexes.Length == 2;
            bool isFirstLetterValid = i_Indexes[0] >= 'A' && i_Indexes[0] <= 'Z';
            bool isSecondLetterValid = i_Indexes[1] >= '1' && i_Indexes[1] <= '9';

            if(!isLengthValid || !isFirstLetterValid || !isSecondLetterValid)
            {
                isValid = false;
            }

            return isValid;
        }

        private Tuple<int, int> parseCardIndexes(string i_Indexes)
        {
            int row = i_Indexes[0] - '1';
            int col = i_Indexes[1] - 'A';
            Tuple<int, int> indexes = new Tuple<int, int>(row, col);

            return indexes;
        }
    }
}