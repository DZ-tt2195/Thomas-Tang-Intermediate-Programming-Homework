using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ThomasTang.Week5
{
    public class PlayerCharacter : Character
    {
        [SerializeField] GameObject buttonBackground;
        [SerializeField] List<Button> listOfButtons = new();

        private void Start()
        {
            buttonBackground.SetActive(false);
            for (int i = 0; i<listOfButtons.Count; i++) //lambda means I can pass a paramater into the method
            {
                int tempValue = i;
                listOfButtons[i].onClick.AddListener(() => GetChoice(tempValue));
            }
        }

        public override IEnumerator MakeChoice()
        {
            yield return base.MakeChoice();
            info.currentChoice = Choices.None;
            buttonBackground.SetActive(true); //wait for player to make a decision

            while (info.currentChoice == Choices.None) 
                yield return null;

            buttonBackground.SetActive(false);
        }

        void GetChoice(int value) //set choice to the button's value
        {
            info.currentChoice = (Choices)value;
        }
    }
}