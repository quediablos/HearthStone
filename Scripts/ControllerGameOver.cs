using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace HearthStone
{
    public class ControllerGameOver : MonoBehaviour
    {
        public Text textWinner;

        // Use this for initialization
        void Start()
        {
            textWinner.text = Controller.textWinner;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

