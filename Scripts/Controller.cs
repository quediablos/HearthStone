using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace HearthStone
{
    /// <summary>
    /// This is where the logic of the game runs.
    /// </summary>
    public class Controller : MonoBehaviour
    {
        private static string TEXT_PLAYER_STATS = "Player: {0}\nHealth: {1}\nMana: {2}\nCards in deck: {3}";
        private static string TEXT_TURN = "Turn for player {0}";
        private static int SCENE_GAME_OVER = 1;
        private static int SCENE_IN_GAME = 0;

        private Player player1;
        private Player player2;
        private Player playerActive;
        private Player playerPassive;

        public Button[] buttonsP1;
        public Button[] buttonsP2;

        public Text textStatsP1;
        public Text textStatsP2;
        public Text textTurn;

        public static string textWinner;

        void Start()
        {
            player1 = new Player();
            player2 = new Player();
            playerActive = player1;
            playerPassive = player2;

            player1.onStartHand();
        }

        void Update()
        {

        }

        /// <summary>
        /// Rendering is done here.
        /// </summary>
        private void LateUpdate()
        {
            renderButtons();
            renderPlayerStats();
        }

        private void startGame()
        {

        }

        private void renderButtons()
        {
            for (int i=0; i<Player.SIZE_HAND; i++)
            {
                string textButton1, textButton2;

                if (i < player1.hand.Count)
                    textButton1 = "Card -> " + player1.hand[i].manaCost;
                else
                    textButton1 = null;

                if (i < player2.hand.Count)
                    textButton2 = "Card -> " + player2.hand[i].manaCost;
                else
                    textButton2 = null;

                buttonsP1[i].GetComponentInChildren<Text>().text = textButton1;
                buttonsP2[i].GetComponentInChildren<Text>().text = textButton2;
            }
        }

        private void renderPlayerStats()
        {
            textStatsP1.text = string.Format(TEXT_PLAYER_STATS, 1, player1.health, player1.mana, player1.deck.cardsUnused.Count);
            textStatsP2.text = string.Format(TEXT_PLAYER_STATS, 2, player2.health, player2.mana, player2.deck.cardsUnused.Count);
            textTurn.text = string.Format(TEXT_TURN, playerActive.Equals(player1) ? 1 : 2);
        }

        /// <summary>
        /// Skips the hand from one player to another.
        /// </summary>
        /// <param name="playerSkipping"></param>
        private void skip(Player playerSkipping)
        {
            playerActive = playerPassive;
            playerPassive = playerSkipping;

            playerActive.onStartHand();

            //Check if active player is alive because it might have taken damage due to empty deck.
            if (!playerActive.isAlive())
                gameOver(playerActive);

            //Check if the player can make another move.
            if (!playerActive.canMakeMoreMovesInCurrentHand())
            {
                //Skip otherwise, skipping is a recursive process. It might loop until one of the players can finally make a move.
                skip(playerActive);
            }
        }

        /// <summary>
        /// When a card button is clicked...
        /// </summary>
        /// <param name="button"></param>
        public void onClickCardButton(Button button)
        {
            ButtonController bController = button.GetComponent<ButtonController>();

            Player playerOfButton = bController.playerNo == 1 ? player1 : player2;

            if (!playerOfButton.Equals(playerActive))
                return;

            //Check if the button corresponds to a card in hand.
            if (bController.index >= playerActive.hand.Count)
                return;

            Card cardToBeUsed = playerActive.hand[bController.index];

            //Check if player has enough mana to use the card.
            if (!playerActive.canUseCard(cardToBeUsed))
                return;

            //Inflict damage.
            playerActive.useCard(playerPassive, cardToBeUsed);

            //Check if opponent died.
            if (!playerPassive.isAlive())
            {
                gameOver(playerPassive);
            }

            //Check if the player can make another move.
            if (!playerActive.canMakeMoreMovesInCurrentHand())
            {
                skip(playerActive);
            }
                
        }

        private void gameOver(Player loser)
        {
            textWinner = "Winner is player " + (loser.Equals(player1) ? 2 : 1);
            SceneManager.LoadScene(SCENE_GAME_OVER);
        }

        /// <summary>
        /// When active player clicks on skip button...
        /// </summary>
        /// <param name="button"></param>
        public void onClickSkip(Button button)
        {
            ButtonController bController = button.GetComponent<ButtonController>();

            Player playerOfButton = bController.playerNo == 1 ? player1 : player2;

            if (!playerOfButton.Equals(playerActive))
                return;

            skip(playerActive);
        }
    }
}

