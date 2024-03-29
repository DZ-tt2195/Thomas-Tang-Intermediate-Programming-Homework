using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum TypeOfCollectible { Jewel, Spike}; 
    [SerializeField] TypeOfCollectible thisType; //the type of collectible this is

    Vector3 startingPos; //where this started;
    Vector3 endingPos; //where this ends;

    float movementTimer = 0f; //how long this has been moving for
    float movementDuration; //how long it should take for this to move

    /// <summary>
    /// make this collectible start moving
    /// </summary>
    /// <param name="time">how long before this hits the ground</param>
    public void StartMoving(float time)
    {
        startingPos = transform.localPosition; //remember the starting position
        endingPos = new Vector3(startingPos.x, Screen.height/-2 - 100); //ending position is just below the bottom of the screen
        movementDuration = time; //set the movement timer
    }

    private void Update()
    {
        if (movementDuration > 0) //if movement timer is above 0
        {
            //use the ease in function from https://easings.net/
            movementTimer = Mathf.Min(1, movementTimer + (Time.deltaTime / movementDuration)); //slowed down by movement duration
            transform.localPosition = startingPos + (endingPos - startingPos) * (1 - Mathf.Cos(movementTimer * Mathf.PI / 2)); //set position
        }

        if (this.transform.localPosition == endingPos) //if this reaches the target
        {
            if (thisType == TypeOfCollectible.Jewel) //you missed a jewel
                Manager.instance.missedJewels++;

            Destroy(this.gameObject); //destroy this
        }
    }
}
