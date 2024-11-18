using using System;
using System.Collections.Generic;
using System.Text;

namespace B24_Ex02_MemoryGameUI
{
    public class GameCard
    {
        private readonly char r_Letter;

        public GameCard(char i_Letter)
        {
            r_Letter = i_Letter;
        }

        public char getValue
        {
            get
            {
                return r_Letter;
            }
        }

        private static GameCard intToGameCard(int i_Int)
        {
            char letterInGameCard = (char)((int)'A' + i_Int);

            return new GameCard(letterInGameCard);
        }
    }
}