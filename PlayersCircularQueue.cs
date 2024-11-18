using using System;
using System.Collections.Generic;
using System.Text;

namespace B24_Ex02_MemoryGameLogic
{
    public class PlayersCircularQueue
    {
        private readonly Player[] r_Players;
        private int m_CurrentPlayerIndex;

        public PlayersCircularQueue(List<Tuple<string, bool>> i_Players)
        {
            r_Players = new Player[i_Players.Count];
            InitPlayers(i_Players);
            m_CurrentPlayerIndex = 0;
        }

        private void InitPlayers(List<Tuple<string, bool>> i_Players)
        {
            int currentPlayerIndex = 0;
            Players currentPlayer;
            string currentPlayerName;
            bool currentPlayerIsComputer;

            foreach (Tuple<string, bool> player in i_Players)
            {
                currentPlayerName = player.Item1;
                currentPlayerIsComputer = player.Item2;
                currentPlayer = new Player(currentPlayerName, currentPlayerIsComputer);
                r_Players[currentPlayerIndex] = currentPlayer;
                currentPlayerIndex++;
            }
        }

        public Player[] Players
        {
            get
            {
                return r_Players;
            }
        }

        public Player CurrentPlayer
        {
            get
            {
                return r_Players[m_CurrentPlayerIndex];
            }
        }

        public void MoveToNextPlayer()
        {
            m_CurrentPlayerIndex = (m_CurrentPlayerIndex + 1) % r_Players.Length;
        }
    }
}