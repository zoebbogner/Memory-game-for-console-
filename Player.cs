using using System;
using System.Collections.Generic;
using System.Text;

namespace B24_Ex02_MemoryGameLogic
{
    public class Player
    {
        private readonly string r_Name;
        private int m_Score;
        private bool m_IsComputer;

        public Player(string i_Name, bool i_IsComputer)
        {
            r_Name = i_Name;
            m_Score = 0;
            m_IsComputer = i_IsComputer;
        }

        public string Name
        {
            get
            {
                return r_Name;
            }
        }

        public int Score
        {
            get
            {
                return m_Score;
            }

            set
            {
                m_Score = value;
            }
        }
    }
}