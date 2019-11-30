using System.Collections.Generic;
using UnityEngine;

namespace HearthStone
{
    public class Deck
    {
        private static int[] DEFAULT_MANA_SEQUENCE =  { 0, 0, 1, 1, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 5, 5, 6, 6, 7, 8 };
        private static int INITIAL_SIZE = 20;

        public List<Card> cardsUnused;
        public List<Card> cardsTaken;
        private static System.Random random;

        private Deck()
        {
            cardsUnused = new List<Card>(20);
            cardsTaken = new List<Card>(20);

            if (random == null)
                random = new System.Random();
        }

        public static Deck createUponStart()
        {
            Deck deck = new Deck();

            for (int i=0; i<INITIAL_SIZE; i++)
            {
                Card card = new Card(DEFAULT_MANA_SEQUENCE[i]);
                deck.cardsUnused.Add(card);
            }

            return deck;
        }

        /// <summary>
        /// Withdraws a set of cards from the unused set.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public List<Card> withdrawCards(int number)
        {
            //Check if the deck has enough cards.
            if (cardsUnused.Count < number)
                return null;

            List<Card> cardsWithdrawn = new List<Card>(number);
            
            for (int i=1; i<= number; i++)
            {
                int slot = random.Next(0, cardsUnused.Count);

                Card cardToWithdraw = cardsUnused[slot];
                cardsUnused.RemoveAt(slot);
                cardsTaken.Add(cardToWithdraw);
                cardsWithdrawn.Add(cardToWithdraw);
            }

            return cardsWithdrawn;

        }

        /// <summary>
        /// Withdraws a single card from the unused set.
        /// </summary>
        /// <returns></returns>
        public Card withdrawCard()
        {
            List<Card> cards = withdrawCards(1);

            if (cards != null)
                return cards[0];
            else
                return null;
        }


    }
}
