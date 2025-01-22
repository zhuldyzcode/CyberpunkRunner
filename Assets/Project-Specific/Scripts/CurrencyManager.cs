using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class CurrencyManager : MonoBehaviour
{
    private int coinsCollected;
    private int cryptoCoins;
    private float currentExchangeRate;

    [SerializeField] private TextMeshProUGUI nOfCoinsCollectedShop;
    [SerializeField] private TextMeshProUGUI nOfCryptoCoinsShop;
    [SerializeField] private TextMeshProUGUI nOfCoinsCollected;
    [SerializeField] private TextMeshProUGUI nOfCryptoCoins;


    [SerializeField] private TextMeshProUGUI exchangeRateText;
    [SerializeField] private Slider exchangeSlider;
    [SerializeField] private TextMeshProUGUI sliderValueText;

    private void Awake()
    {
        RetrieveDataFromPlayerPrefs();
    }

    public void GenerateExchangeRate()
    {
        currentExchangeRate = Mathf.FloorToInt(Random.Range(10f, 50f));
        UpdateCoins();
        UpdateSlider();
    }
    private void UpdateSlider()
    {
        if (exchangeSlider != null)
        {
            exchangeSlider.maxValue = Mathf.FloorToInt(coinsCollected / currentExchangeRate);
            if (exchangeSlider.maxValue<= 0){
                exchangeSlider.maxValue = 0;
            }
            exchangeSlider.value = 0;
        }
    }

    public void OnSliderValueChanged(float value)
    {
        sliderValueText.text = Mathf.FloorToInt(exchangeSlider.value).ToString();

    }

    public void ExchangeCoinsForCryptoCoins()
    {
        int amountToExchange = Mathf.FloorToInt(exchangeSlider.value);
        int requiredCoins = Mathf.FloorToInt(amountToExchange * currentExchangeRate);

        if (HasEnoughCurrency(CurrencyType.Coin, requiredCoins))
        {
            DeductCurrency(CurrencyType.Coin, requiredCoins);
            cryptoCoins += amountToExchange;
            SaveDataToPlayerPrefs();
        }
        UpdateCoins();
        UpdateSlider();
    }
    private void RetrieveDataFromPlayerPrefs()
    {
        coinsCollected = PlayerPrefs.GetInt("CoinsCollected", 0);
        cryptoCoins = PlayerPrefs.GetInt("CryptoCoins", 0);
        UpdateCoins();
    }

    public void SaveDataToPlayerPrefs()
    {
        PlayerPrefs.SetInt("CoinsCollected", coinsCollected);
        PlayerPrefs.SetInt("CryptoCoins", cryptoCoins);
        PlayerPrefs.Save();
    }
    public void HandleCoinCollected()
    {
        coinsCollected++;
        UpdateCoins();
        SaveDataToPlayerPrefs();
    }
    private void UpdateCoins()
    {
        nOfCoinsCollected.text = nOfCoinsCollectedShop.text = coinsCollected.ToString();
        nOfCryptoCoins.text = nOfCryptoCoinsShop.text = cryptoCoins.ToString();
        exchangeRateText.text = $"1 CryptoCoin = "+currentExchangeRate+ " Coins";
    }

    public bool HasEnoughCurrency(CurrencyType currencyType, int cost)
    {
        switch (currencyType)
        {
            case CurrencyType.Coin:
                return coinsCollected >= cost;
            case CurrencyType.CryptoCoin:
                return cryptoCoins >= cost;
            default:
                return false;
        }
    }

    public void DeductCurrency(CurrencyType currencyType, int cost)
    {
        if (HasEnoughCurrency(currencyType, cost))
        {
            switch (currencyType)
            {
                case CurrencyType.Coin:
                    coinsCollected -= cost;
                    break;
                case CurrencyType.CryptoCoin:
                    cryptoCoins -= cost;
                    break;
            }
            SaveDataToPlayerPrefs();
            UpdateCoins();
        }
    }
}