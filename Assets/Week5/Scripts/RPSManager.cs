using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ThomasTang.Week5
{
    public enum Choices { Rock, Paper, Scissor, None };

    public class RPSManager : MonoBehaviour
    {
        public static RPSManager instance;
        [SerializeField] PlayerCharacter player; //the player
        [SerializeField] EnemyCharacter enemy; //the ai enemy
        [SerializeField] TMP_Text instructions; //instructions in the top left

        private void Awake() //this is a singleton
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this.gameObject);
        }

        private void Start() //set health and begin the game
        {
            StartCoroutine(NextRound());
        }

        IEnumerator NextRound()
        {
            instructions.text = "Pick an option."; //have enemy and player choose something
            yield return enemy.MakeChoice();
            yield return player.MakeChoice();

            yield return new WaitForSeconds(1f); //wait 1 second for dramatic effect

            player.RevealChoice(); //reveal choices
            enemy.RevealChoice();

            if (player.info.currentChoice == enemy.info.currentChoice) //if the choices are the same
            {
                instructions.text = "Nothing happened.";
            }
            else 
            {
                switch (player.info.currentChoice)
                {
                    case Choices.Rock:
                        switch (enemy.info.currentChoice)
                        {
                            case Choices.Paper: //rock loses to paper
                                instructions.text = "You've chosen poorly.";
                                player.info.health--;
                                break;
                            case Choices.Scissor: //rock beats scissors
                                instructions.text = "You got lucky.";
                                enemy.info.health--;
                                break;
                        }
                        break;
                    case Choices.Paper:
                        switch (enemy.info.currentChoice)
                        {
                            case Choices.Scissor: //paper loses to scissors
                                instructions.text = "You've chosen poorly.";
                                player.info.health--;
                                break;
                            case Choices.Rock: //paper beats rock
                                instructions.text = "You got lucky.";
                                enemy.info.health--;
                                break;
                        }
                        break;
                    case Choices.Scissor:
                        switch (enemy.info.currentChoice)
                        {
                            case Choices.Rock: //scissors loses to rock
                                instructions.text = "You've chosen poorly.";
                                player.info.health--;
                                break;
                            case Choices.Paper: //scissors beats paper
                                instructions.text = "You got lucky.";
                                enemy.info.health--;
                                break;
                        }
                        break;
                }
            }

            if (player.info.health == 0) //you lose
            {
                instructions.text = "Git gud.";
            }
            else if (enemy.info.health == 0) //enemy loses
            {
                instructions.text = "You're the best at luck games.";
            }
            else //begin the next round
            {
                yield return new WaitForSeconds(1f);
                StartCoroutine(NextRound());
            }
        }
    }
}