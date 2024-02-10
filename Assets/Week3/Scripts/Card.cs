using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    public enum FoodType { Drink, Dessert, Meat, Vegetable};
    public FoodType myType { get; private set; }

    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text typeText;
    [SerializeField] TMP_Text instructionsText;

    public Button button;

    /// <summary>
    /// get card information
    /// </summary>
    /// <param name="property">card property</param>
    public void AssignInfo(CardProperty property)
    {
        //get information from the property
        this.name = property.cardName;
        titleText.text = property.cardName;
        instructionsText.text = property.instructions;

        myType = property.type;
        typeText.text = myType.ToString();

        Image image = GetComponent<Image>();
        switch (myType)
        {
            case FoodType.Drink: //color drinks light blue
                image.color = new Color(0.6f, 0.6f, 1);
                break;
            case FoodType.Dessert: //color desserts white
                image.color = Color.white;
                break;
            case FoodType.Meat: //color meats light red
                image.color = new Color(1, 0.3f, 0.3f);
                break;
            case FoodType.Vegetable: //color vegetables green
                image.color = Color.green;
                break;
        }

        button.onClick.AddListener(PlayCard);
    }

    /// <summary>
    /// move this card into play and do its instructions
    /// </summary>
    public void PlayCard()
    {
        FoodManager.instance.MoveToPlayArea(this);
        switch (this.name)
        {
            case "Bacon": //free 2 points
                FoodManager.instance.GainPoints(2, this);
                break;
            case "Salad": //1 point per meat and vegetable
                FoodManager.instance.GainPoints(FoodManager.instance.TypeInPlay(FoodType.Meat) + FoodManager.instance.TypeInPlay(FoodType.Vegetable), this);
                break;
            case "Burger": //3 points if there's 1+ vegetable
                FoodManager.instance.GainPoints(FoodManager.instance.TypeInPlay(FoodType.Vegetable) >= 1 ? 3 : 0, this);
                break;
            case "Water": //3 points if there's 1+ meat
                FoodManager.instance.GainPoints(FoodManager.instance.TypeInPlay(FoodType.Meat) >= 1 ? 3 : 0, this);
                break;
            case "Coffee": //if first card, +3 points
                FoodManager.instance.GainPoints(FoodManager.instance.numCardsPlayed == 1 ? 3 : 0, this);
                break;
            case "French Fries": //check for next card
                FoodManager.instance.checkForNextCard = this;
                break;
            case "Ice Cream": //check for next card
                FoodManager.instance.checkForNextCard = this;
                break;
            case "Cake": //1 point per type
                int hasMeat = FoodManager.instance.TypeInPlay(FoodType.Meat) >= 1 ? 1 : 0;
                int hasVegetable = FoodManager.instance.TypeInPlay(FoodType.Vegetable) >= 1 ? 1 : 0;
                int hasDessert = FoodManager.instance.TypeInPlay(FoodType.Dessert) >= 1 ? 1 : 0;
                int hasDrink = FoodManager.instance.TypeInPlay(FoodType.Drink) >= 1 ? 1 : 0;
                FoodManager.instance.GainPoints(hasMeat + hasVegetable + hasDessert + hasDrink, this);
                break;
            default:
                Debug.LogError($"{this.name} not recognized");
                break;
        }
    }

    /// <summary>
    /// handle cards that care about the next card played
    /// </summary>
    public void ResolveNext(Card nextCard)
    {
        switch (this.name)
        {
            case "French Fries": //if the next card is a meat, +3 points
                FoodManager.instance.GainPoints(nextCard.myType == FoodType.Meat ? 3 : 0, this);
                break;
            case "Ice Cream": //if the next card is a dessert, +3 points
                FoodManager.instance.GainPoints(nextCard.myType == FoodType.Dessert ? 3 : 0, this);
                break;
            default:
                Debug.LogError($"{this.name} not recognized");
                break;
        }
    }
}
