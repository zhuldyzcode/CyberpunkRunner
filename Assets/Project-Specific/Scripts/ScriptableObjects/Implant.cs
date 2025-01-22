using System.Collections;
using UnityEngine;

public enum CurrencyType
{
    Coin,
    CryptoCoin
}

public abstract class Implant : ScriptableObject
{
    public CurrencyType currencyType;
    public int cost; // Cost of the implant
    public float duration; // Duration of the implant effect in seconds

    public abstract void ApplyEffect(GameObject target);
    public abstract void RemoveEffect(GameObject target);

    public IEnumerator ApplyWithDuration(GameObject target, System.Action<GameObject> applyEffect, System.Action<GameObject> removeEffect)
    {
        applyEffect(target);

        yield return new WaitForSeconds(duration);

        removeEffect(target);
        //Destroy(this);
    }
}