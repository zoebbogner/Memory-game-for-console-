using using System;
using System.Collections.Generic;
using System.Text;

namespace B24_Ex02_MemoryGameLogic
{
    public class ComputerPlayerMemoryAndLogic
    {
        private Dictionary<int, List<Tuple<int, int>>> m_Memory;
        private int m_MaxNumberOfCardsToRemember;
        private bool m_IsFirstTurn;
        private Tuple<int, int> m_FirstCardIndexes;
        private Tuple<int, int> m_SecondCardIndexes;
        private List<Tuple<int, int>> m_RemainingCardIndexes;
        
        public ComputerPlayerMemoryAndLogic(int i_MaxNumberOfCardsToRemember, int i_NumOfRows, int i_NumOfCols)
        {
            m_Memory = new Dictionary<int, List<Tuple<int, int>>>();
            m_MaxNumberOfCardsToRemember = i_MaxNumberOfCardsToRemember;
            m_FirstCardValue = new Tuple<int, int>(-1, -1);
            m_SecondCardValue = new Tuple<int, int>(-1, -1);
            setRemainingCardIndexes(i_NumOfRows, i_NumOfCols);
            m_IsFirstTurn = true;
        }

        private void setRemainingCardIndexes(int i_NumOfRows, int i_NumOfCols)
        {
            Tuple<int, int> currentCardIndexes;

            m_RemainingCardIndexes = new List<Tuple<int, int>>();
            for(int i = 0; i < i_NumOfRows; i++)
            {
                for(int j = 0; j < i_NumOfCols; j++)
                {
                    currentCardIndexes = new Tuple<int, int>(i, j);
                    m_RemainingCardIndexes.Add(currentCardIndexes);
                }
            }
        }

        public Tuple<int, int> GetNextComputerMove()
        {
            Tuple<int, int> nextComputerMove;

            if(m_IsFirstTurn)
            {
                nextComputerMove = getFirstCardMove();
            }
            else
            {
                nextComputerMove = getSecondCardMove();
            }

            m_IsFirstTurn = !m_IsFirstTurn;

            return nextComputerMove;
        }

        public void AddCardToMemory(int i_CardValue, int i_Row, int i_Col)
        {
            const bool k_IsCardInMemory = isCardInMemory(i_CardValue);
            const bool k_IsMemoryFull = isMemoryFull();
            List<Tuple<int, int>>() cardLocations = new List<Tuple<int, int>>();
            Tuple<int, int> cardLocation = new Tuple<int, int>(i_Row, i_Col);
            
            if(!k_IsCardInMemory)
            {
                if(k_IsMemoryFull)
                {
                    removeACardFromMemory();
                }

                m_Memory.Add(i_CardValue, cardLocations);
            }
            
            addToExistingCardInMemory(i_CardValue, cardLocation);
            m_RemainingCardIndexes.Remove(cardLocation);
        }

        public void HandleEndTurn(bool i_IsMatch, int i_CardValue, Tuple<int, int> i_FirstCardIndexes, Tuple<int, int> i_SecondCardIndexes)
        {
            if(i_IsMatch)
            {
                handleCardMatch(i_FirstCardIndexes, i_SecondCardIndexes, i_CardValue);
            }
            else
            {
                handleCardMismatch(i_FirstCardIndexes, i_SecondCardIndexes, i_CardValue);
            }
            
            m_FirstCardIndexes = new Tuple<int, int>(-1, -1);
            m_SecondCardIndexes = new Tuple<int, int>(-1, -1);
        }


        private Tuple<int, int> getFirstCardMove()
        {
            int cardValueToFind;
            const bool k_FindPair = true
            const bool k_IsThereAPairInMemory = findRandomCardInMemoryWithOrWithoutPair(k_FindPair, out cardValueToFind);

            if(k_IsThereAPairInMemory)
            {
                setTwoCardMovesFromMemoryWhenExists(cardValueToFind);
            }
            else
            {
                m_FirstCardIndexes = getRandomCardMove();
            }

            return m_FirstCardIndexes;
        }

        private void setTwoCardMovesFromMemoryWhenExists(int i_CardValue)
        {
            List<Tuple<int, int>> cardLocations = m_Memory[i_CardValue];

            m_FirstCardValue = cardLocations[0];
            m_SecondCardIndexes = cardLocations[1];
        }

        private Tuple<int, int> getRandomCardMove()
        {
            Random random = new Random();
            int remainingIndexesListSize = m_RemainingCardIndexes.Count;
            int randomIndex = random.Next(0, remainingIndexesListSize);
            Tuple<int, int> randomCardMove = m_RemainingCardIndexes[randomIndex];

            return randomCardMove;
        }

        private Tuple<int, int> getSecondCardMove()
        {
            const bool k_IsPartOfPair = isPartOfPair();

            if(!k_IsPartOfPair)
            {
                m_SecondCardIndexes = getRandomCardMove();
            }

            return m_SecondCardIndexes;
        }

        private bool isPartOfPair()
        {
            const bool k_PartOfPreKnownPair = (m_SecondCardIndexes.Item1 != -1) && (m_SecondCardIndexes.Item2 != -1);
            bool isPartOfFirstCardPair = false, isTheFirstInTheMemory = false;

            foreach(KeyValuePair<int, List<Tuple<int, int>>> cardData in m_Memory)
            {
                isPartOfFirstCardPair = (cardData.Value.Count == 2) &&
                                        (cardData.Value.Contains(m_FirstCardIndexes));
                if(isPartOfFirstCardPair)
                {
                    isTheFirstInTheMemory = (cardData.Value[0].Item1 == m_FirstCardIndexes.Item1) &&
                                            (cardData.Value[0].Item2 == m_FirstCardIndexes.Item2);
                    m_SecondCardIndexes = isTheFirstInTheMemory ? cardData.Value[1] : cardData.Value[0];
                    break;
                }
            }

            return k_PartOfPreKnownPair || isPartOfFirstCardPair;
        }

        private bool isCardInMemory(int i_CardValue)
        {
            bool foundCardInMemory = false;

            foreach(KeyValuePair<int, List<Tuple<int, int>>> cardData in m_Memory)
            {
                foundCardInMemory = (cardData.Key == i_CardValue);
                if(foundCardInMemory)
                {
                    break;
                }
            }
            
            return foundCardInMemory;
        }

        private void removeACardFromMemory()
        {   
            int cardValueToRemove;
            const bool k_FindPair = true;
            const bool foundCardToRemove = findRandomCardInMemoryWithOrWithoutPair(!k_FindPair, out cardValueToRemove);

            if(foundCardToRemove)
            {
                m_Memory.Remove(cardValueToRemove);
            }
            else
            {
                removeOldestCardFromMemory();
            }
        }

        private bool findRandomCardInMemoryWithOrWithoutPair(bool i_IsCardWithPair, out int o_CardValue)
        {
            bool foundCard = false;
            bool isCurrentCardWithPair;
            o_CardValue = -1;

            foreach(KeyValuePair<int, List<Tuple<int, int>>> cardData in m_Memory)
            {
                isCurrentCardWithPair = (cardData.Value.Count == 2);
                foundCard = (isCurrentCardWithPair == i_IsCardWithPair);
                if(foundCard)
                {
                    o_CardValue = cardData.Key;
                    break;
                }
            }

            return foundCard;
        }

        private void removeOldestCardFromMemory()
        {
            int oldestCardKey = m_Memory.Keys.First();
            m_Memory.Remove(oldestCardKey);
        }

        private void addToExistingCardInMemory(int i_CardValue, Tuple<int, int> i_CardLocation)
        {
            const bool k_AreIndexesInMemory = areCardIndexesInMemory(i_CardValue, i_CardLocation);

            if(!k_AreIndexesInMemory)
            {
                m_Memory[i_CardValue].Add(i_CardLocation);
            }
        }

        private bool areCardIndexesInMemory(int i_CardValue, Tuple<int, int> i_CardLocation)
        {
            List<Tuple<int, int>> cardPairsOfIndexes = m_Memory[i_CardValue];
            bool areIndexesInMemory;
            int rowIndex = i_CardLocation.Item1;
            int colIndex = i_CardLocation.Item2;

            foreach(Tuple<int, int> cardLocation in i_CardLocations)
            {
                areIndexesInMemory = (cardLocation.Item1 == rowIndex) && (cardLocation.Item2 == colIndex);
                if (areIndexesInMemory)
                {
                    break;
                }
            }

            return areIndexesInMemory;
        }

        private bool isMemoryFull()
        {
            return m_Memory.Count == m_MaxNumberOfCardsToRemember;
        }

        private void handleCardMatch(Tuple<int, int> i_FirstCardIndexes, Tuple<int, int> i_SecondCardIndexes, int i_CardValue)
        {
            m_Memory.Remove(i_CardValue);
        }

        private void handleCardMismatch(Tuple<int, int> i_FirstCardIndexes, Tuple<int, int> i_SecondCardIndexes, int i_CardValue)
        {
            m_RemainingCardIndexes.Add(i_FirstCardIndexes);
            m_RemainingCardIndexes.Add(i_SecondCardIndexes);
        }
    }
}