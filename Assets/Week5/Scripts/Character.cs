using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace ThomasTang.Week5
{
    public struct CharacterInfo //store information about characters
    {
        public int health; //current health
        public Choices currentChoice; //whether to use rock/paper/scissor

        public CharacterInfo(int health)
        {
            this.health = health;
            currentChoice = Choices.None;
        }
    }

    public class Character : MonoBehaviour
    {
        public CharacterInfo info = new(3); //start with 3 health
        [SerializeField] TMP_Text healthText;
        [SerializeField] TMP_Text choiceText;

        public virtual IEnumerator MakeChoice()
        {
            choiceText.text = ""; //blank choice text
            yield return null;
        }

        public void RevealChoice() //reveal the choice
        {
            choiceText.text = info.currentChoice.ToString();
        }

        private void Update()
        {
            healthText.text = $"Health: {info.health}";
        }
    }
}