using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{    
    private CurrencyType currencyType;

    public LevelManager levelManager;
    private CurrencyManager currencyManager;

    public List<Button> buyButtons; // Buttons to buy each implant
    public List<Button> useButtons; // Buttons to use each implant
    public List<Implant> availableImplants; // List of available implants

    private void Start()
    {
        currencyManager = gameObject.GetComponent<CurrencyManager>();
        for (int i = 0; i < availableImplants.Count; i++)
        {
            int index = i; // Capture the current index
            buyButtons[i].onClick.AddListener(() => BuyImplant(index));
            useButtons[i].onClick.AddListener(() => UseImplant(index));
        }
    }

    private void BuyImplant(int index)
    {
        Implant selectedImplant = availableImplants[index];

        if (currencyManager.HasEnoughCurrency(selectedImplant.currencyType, selectedImplant.cost))
        {
            currencyManager.DeductCurrency(selectedImplant.currencyType, selectedImplant.cost);
            levelManager.BuyImplant(selectedImplant);
            AudioManager.Instance.PlayEffect("Buy");
            Debug.Log($"{selectedImplant.name} implant bought for {selectedImplant.cost} "+" "+selectedImplant.currencyType);

        }
        else
        {
            Debug.Log("Not enough coins!");
            AudioManager.Instance.PlayEffect("NoBuy");

        }
    }

    private void UseImplant(int index)
    {
        Implant selectedImplant = availableImplants[index];

        if (levelManager.boughtImplants.Contains(selectedImplant))
        {
            levelManager.UseImplant(selectedImplant);
            Debug.Log($"{selectedImplant.name} implant used for {selectedImplant.duration} seconds!");
        }
        else
        {
            Debug.Log($"No {selectedImplant.name} implant available!");
        }
    }
}