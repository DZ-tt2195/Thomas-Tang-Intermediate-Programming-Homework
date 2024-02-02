using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum TypeOfCollectible { Jewel, Spike};
    [SerializeField] TypeOfCollectible thisType;

    Vector3 startingPos; //where this started;
    Vector3 endingPos; //where this ends;

    float movementTimer = 0f; //how long this has been moving for
    float movementDuration; //how long it should take for this to move

    public void StartMoving(float time)
    {
        startingPos = transform.localPosition;
        endingPos = new Vector3(startingPos.x, Screen.height/-2 - 100);
        movementDuration = time; //set the movement timer
    }

    private void Update()
    {
        if (movementDuration > 0) //if movement timer is above 0
        {
            //use the ease in function from https://easings.net/
            movementTimer = Mathf.Min(1, movementTimer + (Time.deltaTime / movementDuration));
            transform.localPosition = startingPos + (endingPos - startingPos) * (1 - Mathf.Cos(movementTimer * Mathf.PI / 2));
        }

        if (this.transform.localPosition == endingPos) //if this reaches the target
        {
            if (thisType == TypeOfCollectible.Jewel) //missed a jewel
                Manager.instance.missedJewels++;

            Destroy(this.gameObject); //destroy this
        }
    }
}
