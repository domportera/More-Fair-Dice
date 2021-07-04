using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour
{
    List<int> diceRolls;
    int numSides = 6;
    bool includesZero = false;
    public int randomQuantity = 1;
    System.Random rng = new System.Random();
    public bool isFair;

    Button fairButton;

    List<DiceGroup> dice = new List<DiceGroup>();

    // Start is called before the first frame update
    void Start()
    {
        if (isFair)
        {
            CreateDiceRolls(out diceRolls);
        }
    }

    public void ToggleFairness()
    {
        isFair = !isFair;
    }

    int FairRoll(ref List<int> _availableRolls)
    {
        if (_availableRolls.Count == 0)
        {
            CreateDiceRolls(out _availableRolls);
        }

        int roll = _availableRolls[0];
        _availableRolls.RemoveAt(0);

        return roll;
    }

    int UnfairRoll()
    {
        return rng.Next(1, numSides + 1);
    }

    void EnableFairness()
    {
        CreateDiceRolls(out diceRolls);
        isFair = true;
    }

    void DisableFairness()
    {
        isFair = false;
    }

    public List<int> RollQuantity(int _numToRoll)
    {
        List<int> rolls = new List<int>();
        if(!isFair)
        {
            //roll _num unfair
            for(int i = 0; i < _numToRoll; i++)
            {
                rolls.Add(UnfairRoll());
            }
        }
        else
        {
            DiceGroup diceToRoll = null;

            foreach(DiceGroup d in dice)
            {
                if(d.GetQuantity() == _numToRoll)
                {
                    diceToRoll = d;
                    break;
                }
            }

            if(diceToRoll == null)
            {
                diceToRoll = new DiceGroup(_numToRoll);
                dice.Add(diceToRoll);
            }

            for (int i = 0; i < _numToRoll; i++)
            {
                List<int> temp = diceToRoll.dice[i];
                rolls.Add(FairRoll(ref temp));
                diceToRoll.dice[i] = temp;
            }
        }

        return rolls;
    }


    void CreateDiceRolls(out List<int> _die, int _sides = 0, int _quantity = 0)
    {
        if (_sides == 0)
        {
            _sides = numSides;
        }

        if (_quantity == 0)
        {
            _quantity = randomQuantity;
        }

        List<int> possibleRolls = new List<int>();

        for (int i = 0; i < _quantity; i++)
        {
            for (int j = 1; j <= _sides; j++)
            {
                possibleRolls.Add(j);
            }
        }

        Shuffle(ref possibleRolls);
 
        _die = possibleRolls;
    }

    void Shuffle(ref List<int> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            int value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

class DiceGroup
{
    int diceQuantity;
    public List<List<int>> dice;

    public DiceGroup(int _quantity)
    {
        if(_quantity == 0)
        {
            Debug.LogError("Can't start a dice group with 0 dice");
            return;
        }

        diceQuantity = _quantity;

        dice = new List<List<int>>();

        for (int i = 0; i < diceQuantity; i++)
        {
            dice.Add(new List<int>());
        }
    }

    public int GetQuantity()
    {
        return diceQuantity;
    }
}
