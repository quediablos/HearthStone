using System.Collections.Generic;
using UnityEngine;

namespace HearthStone
{
    /// <summary>
    /// Represents a player.
    /// </summary>
    public class Player
    {
        private static int MAX_MANA = 10;
        public static int SIZE_HAND = 5;

        public int health;
        public int mana;
        public Deck deck;
        public List<Card> hand;

       
        public Player()
        {
            health = 30;
            mana = 0;

            deck = Deck.createUponStart();
            (hand = new List<Card>(SIZE_HAND)).AddRange(deck.withdrawCards(3));
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

        public bool canUseCard(Card card)
        {
            return mana >= card.manaCost;
        }

        public void useCard(Player opponent, Card card)
        {
            opponent.takeDamage(card.manaCost);

            hand.Remove(card);
            mana -= card.manaCost;
        }

        public bool isAlive()
        {
            return health > 0;
        }

    }
}
