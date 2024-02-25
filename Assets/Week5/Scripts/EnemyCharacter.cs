using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThomasTang.Week5
{
    public class EnemyCharacter : Character
    {
        public override IEnumerator MakeChoice()
        {
            yield return base.MakeChoice();
            int chooseAtRandom = Random.Range(0, 3); //choose rock/paper/scissor at random
            info.currentChoice = (Choices)chooseAtRandom; //convert into the enum value
            yield return null;
        }
    }
}