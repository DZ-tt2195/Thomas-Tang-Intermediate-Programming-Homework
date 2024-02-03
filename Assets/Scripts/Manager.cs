using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static Manager instance; //static reference for the manager
    Canvas canvas; //the canvas
    [SerializeField] TMP_Text textBox; //text for data

    [SerializeField] Collectible jewelPrefab; //jewel
    [SerializeField] Collectible spikePrefab; //spike

    [HideInInspector] public int missedJewels = 0; //number of jewels that missed

    int difficulty = 1; //current difficulty
    List<Collectible> toDrop = new(); //the collectibles to be dropped

    private void Awake()
    {
        instance = this; //set instance
        canvas = FindObjectOfType<Canvas>(); //find the canvas
    }

    private void Start()
    {
        Application.targetFrameRate = 60; //cap at 60fps
        DropCollectible(); //immediately drop a collectible
    }

    /// <summary>
    /// generates a random position for a collectible to appear from above the screen
    /// </summary>
    /// <returns>a random vector3</returns>
    Vector3 RandomSpawn()
    {
        return new Vector3(Random.Range
        (Screen.width / -2, Screen.width / 2), Screen.height / 2 + 100, 0);
    }

    /// <summary>
    /// drop a collectible from the top of the screen
    /// </summary>
    void DropCollectible()
    {
        difficulty = Player.instance.score < 5 ? 0 : Player.instance.score / 5; //change difficulty based on score, minimum 0

        if (toDrop.Count == 0) //if there are no collectibles to drop
        {
            for (int i = 0; i < difficulty*2; i++) //add 2 spikes per difficulty
            {
                Collectible newSpike = Instantiate(spikePrefab, canvas.transform);
                newSpike.transform.localPosition = RandomSpawn();
                toDrop.Add(newSpike);
            }

            for (int i = 0; i < 2; i++)
            {
                Collectible newJewel = Instantiate(jewelPrefab, canvas.transform); //add 2 jewels
                newJewel.transform.localPosition = RandomSpawn();
                toDrop.Insert(Random.Range(0, toDrop.Count - 1), newJewel); //put it randomly into the list
            }
        }

        Collectible next = toDrop[^1]; //get the last collectible in the list and drop it
        toDrop.RemoveAt(toDrop.Count - 1);
        next.StartMoving(Mathf.Max(0.6f, 3f - difficulty * 0.5f));
        //will take 3 seconds for it to hit the ground. each difficulty speeds this up by 0.5f. max is 0.6f

        Invoke(nameof(DropCollectible), Mathf.Max(0.2f, 1f - difficulty * 0.2f));
        //will take 1 second to spawn the next collectible. each difficulty speeds this up by 0.2f. max is 0.2f
    }

    private void Update()
    {
        textBox.text = $"Score: {Player.instance.score} " +
            $"\nMissed: {missedJewels}" +
            $"\nDifficulty: {difficulty}";
        //update textbox;
    }
}
