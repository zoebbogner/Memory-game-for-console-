using using System;
using System.Collections.Generic;
using System.Text;

namespace B24_Ex02_MemoryGameUI
{
    public class ConsoleUIMessages
    {
        public static void PrintWelcomeMessage()
        {
            Console.WriteLine(@"Welcome to the Memory Game!
The rules are simple:
1. You will be presented with a board of cards.
2. Each card has an image on the other side.
3. Your goal is to find all the pairs in the board.
4. You can only flip two cards at a time.
5. If the cards you flipped are not a pair, they will be flipped back.
6. If the cards you flipped are a pair, they will be stay flipped on the board.
7. The game ends when you find all the pairs.
8. Good luck!");

            Console.WriteLine();
            Console.WriteLine("Please type the first player's name : ");
        }

        public static void PrintOptionsSecondPlayerChoiceMessage()
        {
            Console.WriteLine(@"If you want to play against the computer, type 'Y'.
If you want to play against another player, type 'N'.");
        }

        public static void PrintSecondHumanPlayerNameMessage()
        {
            Console.WriteLine("Please type the second player's name : ");
        }

        public static void ChooseLevelOfDifficultyMessage()
        {
            Console.WriteLine(@"Please choose the level of difficulty :
1. Easy
2. Medium
3. Hard");
        }

        public static void PrintBoardSizeMessage()
        {
            Console.WriteLine("Please type the size of the board in the next two lines (total cards in the board must be even and between 4x4 to 6x6)");
        }

        public static void PrintNumberOfRowsInBoardMessage()
        {
            Console.WriteLine("Please type the number of rows in the board : ");
        }

        public static void PrintNumberOfColsInBoardMessage()
        {
            Console.WriteLine("Please type the number of columns in the board : ");
        }

        public static void PrintInvalidBoardSizeMessage()
        {
            Console.WriteLine("Invalid board size, make sure that the total cards in the board is even and between 16 and 36.");
        }

        public static void PrintPlayerTurnMessage(string i_PlayerName)
        {
            Console.WriteLine("{0}, it's your turn!", i_PlayerName);
        }

        public static void PrintCardRevealMessage()
        {
            Console.WriteLine("Please type the column and row indexes of the card you want to reveal (example A1)");
        }

        public static void PrintInvalidRowIndexMessage(int i_NumOfRows)
        {
            Console.WriteLine("Invalid row index, please type a valid row index (1-{0})", i_NumOfRows);
        }

        public static void PrintInvalidColIndexMessage(int i_NumOfCols)
        {
            char lastColumnLetter = (char)('A' + i_NumOfCols - 1);

            Console.WriteLine("Invalid column index, please type a valid column index (A-{0})", lastColumnLetter);
        }

        public static void PrintCardAlreadyRevealedMessage()
        {
            Console.WriteLine("This card is already revealed, please choose another card.");
        }

        public static void PrintCardValueMessage(int i_CardValue)
        {
            Console.WriteLine("The card value is {0}", i_CardValue);
        }

        public static void PrintMatchMessage()
        {
            Console.WriteLine("It's a match!");
        }

        public static void PrintNoMatchMessage()
        {
            Console.WriteLine("It's not a match, maybe next time!");
        }

        public static void PrintCurrentScoreMessage(string i_PlayerName, int i_PlayerScore)
        {
            Console.WriteLine("{0}, your current score is {1}", i_PlayerName, i_PlayerScore);
        }

        public static void PrintWinnerMessage(string i_WinnerName)
        {
            Console.WriteLine("The winner is {0}!", i_WinnerName);
        }

        public static void PrintTieMessage(List<string> i_WinnersNames)
        {
            Console.Write("It's a tie between : ");

            foreach (string winnerName in i_WinnersNames)
            {
                Console.Write("{0} ", winnerName);
            }
        }

        public static void PrintPlayAgainMessage()
        {
            Console.WriteLine("Do you want to play again? (Y/N)");
        }

        public static void PrintGoodbyeMessage()
        {
            Console.WriteLine("Goodbye!");
        }
    }
}