using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    DiceManager diceManager;

    [SerializeField]
    List<Image> dice1;

    [SerializeField]
    List<Image> dice2;

    [SerializeField]
    List<Image> dice3;

    List<List<Image>> listOfDiceLists = new List<List<Image>>();

    [SerializeField]
    List<Sprite> diceSprites;

    [SerializeField]
    Button fairButton;

    // Start is called before the first frame update
    void Start()
    {
        PopulateListOfDiceLists();
        SetFairButtonProperties();
    }

    public void Roll1()
    {
        SetDiceTexture(diceManager.RollQuantity(1));
    }

    public void Roll2()
    {
        SetDiceTexture(diceManager.RollQuantity(2));
    }
    public void Roll3()
    {
        SetDiceTexture(diceManager.RollQuantity(3));
    }

    public void FairnessToggle()
    {
        diceManager.ToggleFairness();

        if(diceManager.isFair)
        {
            diceManager.randomQuantity++;

            if (diceManager.randomQuantity > 3)
            {
                diceManager.randomQuantity = 1;
            }
        }

        SetFairButtonProperties();
    }

    void DebugRolls(List<int> _rolls)
    {
        string s = "ROLLS: ";

        foreach(int i in _rolls)
        {
            s += i + " ";
        }

        Debug.Log(s);
    }

    void SetDiceTexture(List<int> _rolls)
    {
        List<Image> diceToUse;

        DebugRolls(_rolls);
        switch (_rolls.Count)
        {
            case 1:
                diceToUse = dice1;
                break;
            case 2:
                diceToUse = dice2;
                break;
            case 3:
                diceToUse = dice3;
                break;
            default:
                Debug.LogError("Invalid number of rolls in SetDiceTexture");
                return;
        }

        ToggleVisibleDice(diceToUse);

        for (int i = 0; i < _rolls.Count; i++)
        {
            diceToUse[i].sprite = diceSprites[_rolls[i] - 1];
        }
    }

    void ToggleVisibleDice(List<Image> _diceToUse)
    {
        foreach(List<Image> list in listOfDiceLists)
        {
            if(list != _diceToUse)
            {
                foreach(Image i in list)
                {
                    i.gameObject.SetActive(false);
                }
            }
            else
            {
                foreach (Image i in list)
                {
                    i.gameObject.SetActive(true);
                }
            }
        }
    }

    void SetFairButtonProperties()
    {
        fairButton.GetComponent<Image>().color = diceManager.isFair ? Color.green : Color.red;
        fairButton.GetComponentInChildren<Text>().text = diceManager.isFair ? "Fair - " + diceManager.randomQuantity : "Standard";
    }

    void PopulateListOfDiceLists()
    {
        listOfDiceLists.Add(dice1);
        listOfDiceLists.Add(dice2);
        listOfDiceLists.Add(dice3);
    }
}
