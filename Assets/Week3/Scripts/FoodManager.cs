using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Card;
using TMPro;

[Serializable]
public class CardProperty
{
    public string cardName;
    public string instructions;
    public FoodType type;
}

[Serializable]
public class CardOnScreen
{
    public Vector3 location;
    public Card card;

    public CardOnScreen(Vector3 location)
    {
        this.location = location;
    }
}

public class FoodManager : MonoBehaviour
{
    public static FoodManager instance;
    Canvas canvas;

    [SerializeField] Card cardPrefab;
    [SerializeField] PointsVisual pointsPrefab;
    [SerializeField] List<CardProperty> allCards = new();

    List<CardOnScreen> handPositions = new(); //where cards can be in your hand
    List<CardOnScreen> playPositions = new(); //where cards can be in play

    int currentScore = 0;

    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text instructionsText;

    public Card checkForNextCard = null; //some cards in the game check for what the next card is
    public int numCardsPlayed = 0;

    private void Awake()
    {
        instance = this;
        canvas = FindObjectOfType<Canvas>();
    }

    private void Start()
    {
        for (int i = 0; i < 7; i++) //add 7 positions to hand, spaced by 350 
            handPositions.Add(new CardOnScreen(new Vector3(-1050 + (350 * i), -475, 0)));

        for (int i = 0; i < 5; i++) //add 5 positions to play area, spaced by 350 
            playPositions.Add(new CardOnScreen(new Vector3(-700 + (350 * i), 0, 0)));

        for (int i = 0; i<7; i++) //begin with 7 cards
            GainRandomCard();

        NewRound();
    }

    void GainRandomCard()
    {
        //make a new card
        Card newCard = Instantiate(cardPrefab, canvas.transform);
        newCard.transform.localPosition = new Vector3(0, 200, 0);
        newCard.AssignInfo(allCards[UnityEngine.Random.Range(0, allCards.Count)]);

        foreach (CardOnScreen position in handPositions)
        {
            if (position.card == null || position.card.gameObject == null) //find the next available position in your hand and put the card there
            {
                newCard.transform.localPosition = position.location;
                position.card = newCard;
                return;
            }
            else
            {
            }
        }

        Debug.LogError("failed to add card");
    }

    public void MoveToPlayArea(Card newCard)
    {
        foreach (CardOnScreen next in handPositions) //disable hand
        {
            if (next.card != null) //disable the card
            {
                next.card.button.interactable = false;
                if (next.card == newCard) //if this is the played card, remove it
                    next.card = null;
            }
        }

        playPositions[numCardsPlayed].card = newCard;
        newCard.transform.localPosition = playPositions[numCardsPlayed].location;

        if (checkForNextCard != null)
        {
            checkForNextCard.ResolveNext(newCard);
            checkForNextCard = null;
        }

        numCardsPlayed++;
        if (numCardsPlayed == 5)
        {
            instructionsText.text = $"Card Plays Left: 0";
            Invoke(nameof(ResetCards), 1f);
        }
        else
        {
            CanPlayCards();
        }
    }

    /// <summary>
    /// gain points from a card
    /// </summary>
    /// <param name="points"></param>
    public void GainPoints(int points, Card card)
    {
        currentScore += points;
        PlayerPrefs.SetInt("High Score", Math.Max(PlayerPrefs.GetInt("High Score"), currentScore));
        scoreText.text = $"Score This Round: {currentScore}\nBest Score: {PlayerPrefs.GetInt("High Score")}";

        if (card != null && points > 0)
        {
            PointsVisual pv = Instantiate(pointsPrefab, canvas.transform);
            pv.Setup($"+{points}", card.transform.localPosition);
        }
    }

    /// <summary>
    /// find number of cards in play with a certain type
    /// </summary>
    /// <param name="type">type to search for</param>
    /// <returns>number of cards with that type</returns>
    public int TypeInPlay(FoodType type)
    {
        int answer = 0;
        foreach (CardOnScreen position in playPositions)
        {
            if (position.card == null)
                break;
            if (position.card.myType == type)
                answer++;
        }
        return answer;
    }

    void NewRound()
    {
        //reset everything
        numCardsPlayed = 0;
        checkForNextCard = null;
        GainPoints(-1*currentScore, null); //set score to 0
        CanPlayCards();
    }

    void CanPlayCards()
    {
        instructionsText.text = $"Card Plays Left: {5-numCardsPlayed}";
        foreach (CardOnScreen next in handPositions) //enable hand
        {
            if (next.card != null)
                next.card.button.interactable = true;
        }
    }

    void ResetCards()
    {
        foreach (CardOnScreen next in playPositions) //remove played cards
            Destroy(next.card.gameObject);

        for (int i = 0; i < 5; i++) //draw 5 new cards
            GainRandomCard();

        Invoke(nameof(NewRound), 1f);
    }
}
