using using System;
using System.Collections.Generic;
using System.Text;

namespace B24_Ex02_MemoryGameLogic
{
    public class GameLogicManager
    {
        private readonly int r_NumOfRows;
        private readonly int r_NumOfCols;
        private ComputerPlayerMemoryAndLogic m_ComputerPlayerMemoryAndLogic;
        private PlayersCircularQueue m_PlayersCircularQueue;
        private readonly int[,] r_GameBoardInInts;
        private bool[,] m_GameBoardRevealed;
        private int m_NumOfRevealedPairs;
        private int m_FirstCardValue;
        private Tuple<int, int> m_FirstCardIndexes;

        private GameLogicManager(int i_MaxNumberOfCardsToRemember, List<Tuple<string, bool>> i_Players, int i_NumOfRows, int i_NumOfCols)
        {
            r_NumOfRows = i_NumOfRows;
            r_NumOfCols = i_NumOfCols;
            m_ComputerPlayerMemoryAndLogic = new ComputerPlayerMemoryAndLogic(i_MaxNumberOfCardsToRemember, i_NumOfRows, i_NumOfCols);
            m_PlayersCircularQueue = new PlayersCircularQueue(i_Players);
            r_GameBoardInInts = new int[r_NumOfRows, r_NumOfCols];
            m_GameBoardRevealed = new bool[r_NumOfRows, r_NumOfCols];
            m_NumOfRevealedPairs = 0;
            m_FirstCardValue = -1;
            m_FirstCardIndexes = new Tuple<int, int>(-1, -1);
            InitGameBoards();
        }

        public static Tuple<eBoardValidationResult, GameLogicManager> CreateGameLogic(int i_MaxNumberOfCardsToRemember, Player[] i_Players, int i_NumOfRows, int i_NumOfCols)
        {
            eBoardValidationResult validationResult = ValidateBoardSize(i_NumOfRows, i_NumOfCols);
            GameLogic gameLogic = null;
            Tuple<eBoardValidationResult, GameLogic> gameLogicResult = null;
            const bool k_IsValidBoardSize = (validationResult == eBoardValidationResult.Valid);

            if(k_IsValidBoardSize)
            {
                gameLogic = new GameLogic(i_MaxNumberOfCardsToRemember, i_Players, i_NumOfRows, i_NumOfCols);
            }

            gameLogicResult = new Tuple<eBoardValidationResult, GameLogic>(validationResult, gameLogic);

            return gameLogicResult;
        }

        public enum eCardRevealStatus
        {
            Valid,
            AlreadyRevealed,
            Match,
            NoMatch,
            RowIndexOutOfRange,
            ColIndexOutOfRange,
            BothIndexesOutOfRange,
            NotInitialized
        }

        public enum eBoardValidationResult
        {
            Valid,
            OddNumOfTotalCards,
            NotInitialized
        }

        private eBoardValidationResult ValidateBoardSize(int i_NumOfRows, int i_NumOfCols)
        {
            eBoardValidationResult validationResult = eBoardValidationResult.Valid;

            if ((i_NumOfRows * i_NumOfCols) % 2 != 0)
            {
                validationResult = eBoardValidationResult.OddNumOfTotalCards;
            }

            return validationResult;
        }

        private void InitGameBoards()
        {
            List<int> cardValues = new List<int>();
            int numOfPairs = (r_NumOfRows * r_NumOfCols) / 2;
            int currentCardValue;

            for (int i = 0; i < numOfPairs; i++)
            {
                cardValues.Add(i);
                cardValues.Add(i);
            }

            Random random = new Random();
            for (int i = 0; i < r_NumOfRows; i++)
            {
                for (int j = 0; j < r_NumOfCols; j++)
                {
                    int randomIndex = random.Next(0, cardValues.Count);
                    currentCardValue = cardValues[randomIndex];
                    r_GameBoardInInts[i, j] = currentCardValue;
                    cardValues.RemoveAt(randomIndex);
                    m_GameBoardRevealed[i, j] = false;
                }
            }
        }

        public int[,] GameBoardInInts
        {
            get
            {
                return r_GameBoardInInts;
            }
        }

        public bool[,] GameBoardRevealed
        {
            get
            {
                return m_GameBoardRevealed;
            }
        }

        public Player CurrentPlayer
        {
            get
            {
                return m_PlayersCircularQueue.CurrentPlayer;
            }
        }

        public Tuple<int, int> GetNextComputerMove()
        {
            return m_ComputerPlayerMemoryAndLogic.GetNextComputerMove();
        }

        public eCardRevealStatus MakeMove(int i_Row, int i_Col)
        {
            eCardRevealStatus cardStatus = eCardRevealStatus.Valid;
            eCardRevealStatus validationResult = validateCardIndexes(i_Row, i_Col);
            const bool k_IsValidCardIndexes = (validationResult == eCardRevealStatus.Valid);
            bool isCardRevealed = false;
            
            if(k_IsValidCardIndexes)
            {
                isCardRevealed = RevealCard((i_Row, i_Col), out cardStatus);
                if(!isCardRevealed)
                {
                    cardStatus = eCardRevealStatus.AlreadyRevealed
                }
            }
            
            return cardStatus;
        }

        private eCardRevealStatus validateCardIndexes(int i_Row, int i_Col)
        {
            eCardRevealStatus validationResult = eCardRevealStatus.Valid;
            const bool k_IsValidRow = (i_Row >= 0 && i_Row < r_NumOfRows);
            const bool k_IsValidCol = (i_Col >= 0 && i_Col < r_NumOfCols);

            if (!k_IsValidRow)
            {
                validationResult = eCardRevealStatus.RowIndexOutOfRange;
            }
            else if (!k_IsValidRow)
            {
                validationResult = eCardRevealStatus.ColIndexOutOfRange;
            }

            return validationResult;
        }

        private bool RevealCard(int i_Row, int i_Col, out eCardRevealStatus o_CardStatus)
        {
            o_CardStatus = eCardRevealStatus.Valid;
            const bool k_IsCardAlreadyRevealed = m_GameBoardRevealed[i_Row, i_Col];
            const bool k_IsFirstCard = (m_FirstCardValue == -1);
            int cardValue = r_GameBoardInInts[i_Row, i_Col];
            Tuple<int, int> cardIndexes = new Tuple<int, int>(i_Row, i_Col);

            if (!k_IsCardAlreadyRevealed)
            {
                m_GameBoardRevealed[i_Row, i_Col] = true;
                m_ComputerPlayerMemoryAndLogic.AddCardToMemory(cardValue, i_Row, i_Col);
                if(k_IsFirstCard)
                {
                    m_FirstCardValue = cardValue;
                    m_FirstCardIndexes = cardIndexes;
                }
                else
                {
                    handleEndTurn(cardValue, i_Row, i_Col, out o_CardStatus);
                }
            }

            return !k_IsCardAlreadyRevealed;
        }

        private void handleEndTurn(int i_CardValue, int i_Row, int i_Col, out eCardRevealStatus o_CardStatus)
        {
            o_CardStatus = eCardRevealStatus.Valid;
            const bool k_IsMatchedPair == (m_FirstCardValue == i_CardValue);
            Tuple<int, int> secondCardIndexes = new Tuple<int, int>(i_Row, i_Col);
            Player currentPlayer = m_PlayersCircularQueue.CurrentPlayer;

            if(k_IsMatchedPair)
            {
                m_NumOfRevealedPairs++;
                currentPlayer.Score++;
                o_CardStatus = eCardRevealStatus.Match;
            }
            else
            {
                m_GameBoardRevealed[i_Row, i_Col] = false;
                m_GameBoardRevealed[m_FirstCardIndexes.Item1, m_FirstCardIndexes.Item2] = false;
                o_CardStatus = eCardRevealStatus.NoMatch;
            }

            m_FirstCardValue = -1;
            m_FirstCardIndexes.Item1 = -1;
            m_FirstCardIndexes.Item2 = -1;
            m_ComputerPlayerMemoryAndLogic.HandleEndTurn(k_IsMatchedPair, i_CardValue, m_FirstCardIndexes, secondCardIndexes);
            m_PlayersCircularQueue.MoveToNextPlayer();
        }

        public bool IsGameOver()
        {
            return m_NumOfRevealedPairs == (r_NumOfRows * r_NumOfCols) / 2;
        }

        public List<Player> GetWinners()
        {
            List<Player> winners = new List<Player>();
            int winnerScore = 0;

            foreach(Player player in m_PlayersCircularQueue.Players)
            {
                if(player.Score > winnerScore)
                {
                    winner = player;
                    winnerScore = player.Score;
                }
            }

            winners = getAllWinners(winner);

            return winners;
        }

        private List<Player> getAllWinners(i_winner)
        {
            List<Player> winners = new List<Player>();

            foreach(Player player in m_PlayersCircularQueue.Players)
            {
                if(player.Score == i_winner.Score)
                {
                    winners.Add(player);
                }
            }

            return winners;
        }
    }
}