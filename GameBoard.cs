using using System;
using System.Collections.Generic;
using System.Text;

namespace B24_Ex02_MemoryGameUI
{
    public class GameBoard
    {
        private GameCard[,] m_GameBoard;
    
        public GameBoard(int[,] i_GameBoardInInts)
        {
            int numOfRows = i_GameBoardInInts.GetLength(0);
            int numOfCols = i_GameBoardInInts.GetLength(1);
            createGameBoardFromInts(numOfRows, numOfCols, i_GameBoardInInts);
        }

        private void createGameBoardFromInts(int i_NumOfRows, int i_NumOfCols, int[,] i_GameBoardInInts)
        {
            int currentCardInGameBoardInInt;
            GameCard currentParsedCardInGameBoard;

            m_GameBoard = new GameCard[i_NumOfRows, i_NumOfCols];
            for (int i = 0; i < i_NumOfRows; i++)
            {
                for (int j = 0; j < i_NumOfCols; j++)
                {
                    currentCardInGameBoardInInt = i_GameBoardInInts[i, j];
                    currentParsedCardInGameBoard = GameCard.intToGameCard(currentCardInGameBoardInInt);
                    m_GameBoard[i, j] = currentParsedCardInGameBoard;
                }
            }
        }

        public GameCard GetCard[int i_Row, int i_Col]
        {
            get
            {
                return m_GameBoard[i_Row, i_Col];
            }
        }

        public void DisplayBoard(bool[,] i_GameBoardRevealed)
        {
            int numOfRows = m_GameBoard.GetLength(0);
            int numOfCols = m_GameBoard.GetLength(1);
            int row, col, currentRowIndexInUI;
            string currentColumn = string.Empty;
            string newLine = Environment.NewLine;
            StringBuilder gameBoardString = new StringBuilder();
            bool isCurrentCardRevealed;

            addColumnLettersAsIndexesLine(gameBoardString, numOfCols);
            gameBoardString.Append(newLine);
            for(row = 0; row < numOfRows; row++)
            {
                addEqualSignsLine(gameBoardString, numOfCols);
                currentRowIndexInUI = row + 1;
                currentColumn = string.Format("{0} ", currentRowIndexInUI);
                gameBoardString.Append(currentColumn);
                for(col = 0; col < numOfCols; col++)
                {
                    isCurrentCardRevealed = i_GameBoardRevealed[row, col];
                    addCardAccordingToState(gameBoardString, row, col, isCurrentCardRevealed);
                }

                gameBoardString.Append("|");
                gameBoardString.Append(newLine);
            }

            addEqualSignsLine(gameBoardString, numOfCols);
        }

        private void addColumnLettersAsIndexesLine(StringBuilder i_GameBoardString, int i_NumOfCols)
        {
            int col;
            char currentColumnIndexAsChar = 'A';
            string currentColumn = string.Empty;
            
            i_GameBoardString.Append("  ");
            for(col = 0; col < i_NumOfCols; col++)
            {
                currentColumnIndexAsChar = (char)('A' + col);
                currentColumn = string.Format(" {0}  ", currentColumnIndexAsChar);
                i_GameBoardString.Append(currentColumn);
            }
        }

        private void addEqualSignsLine(StringBuilder i_GameBoardString, int i_NumOfCols)
        {

            i_GameBoardString.Append("  ");
            for(int i = 0; i < i_NumOfCols; i++)
            {
                i_GameBoardString.Append("====");
            }

            i_GameBoardString.Append("=");
            i_GameBoardString.Append(newLine);
        }

        private void addCardAccordingToState(StringBuilder i_GameBoardString, int i_Row, int i_Col, bool i_IsCardRevealed)
        {
            string cardString = " ";

            if(i_IsCardRevealed)
            {
                cardString = m_GameBoard[i_Row, i_Col].ToString();
            }

            i_GameBoardString.Append("| ");
            i_GameBoardString.Append(cardString);
            i_GameBoardString.Append(" ");
        }
    }
}