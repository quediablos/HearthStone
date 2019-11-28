using System.Collections.Generic;
using UnityEngine;

namespace HearthStone
{
    public class Player
    {
        private static int MAX_MANA = 10;
        private static int SIZE_HAND = 5;

        public int health;
        public int mana;
        public Deck deck;
        public List<Card> hand;

        public void onBirth()
        {
            health = 30;
            mana = 0;

            deck = Deck.createUponStart();
            hand.AddRange(deck.withdrawCards(3));
        }

        /// <summary>
        /// Regenerates mana.
        /// </summary>
        private void regenerateteMana()
        {
            if (mana + 1 <= MAX_MANA)
                mana++;
        }


        /// <summary>
        /// Checks if the player can make more moves in the current hand.
        /// </summary>
        /// <returns></returns>
        public bool canMakeMoreMovesInCurrentHand()
        {
            //Check if player has anymore carts.
            if (hand.Count == 0)
                return false;

            //Check if player has sufficient mana to used any card.
            foreach (Card cardInHand in hand)
            {
                if (cardInHand.manaCost <= mana)
                    return true;
            }

            //No hope.
            return false;
        }

        /// <summary>
        /// Applies set of procedures upon starting a new hand.
        /// </summary>
        public void onStartHand()
        {
            regenerateteMana();

            //Withdraw a card from the deck.
            Card cardFromDeck = deck.withdrawCard();

            //Bleeding out: no card left in the deck.
            if (cardFromDeck == null)
            {
                takeDamage(1);
            }
            //Check for overload: hand is not full.
            else if (hand.Count < SIZE_HAND)
            {
                hand.Add(cardFromDeck);
            } 
        }

        public int takeDamage(int damage)
        {
            return health -= damage;
        }


    }
}
