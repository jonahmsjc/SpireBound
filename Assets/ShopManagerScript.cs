using UnityEngine;
using TMPro;

public class ShopManagerScript : MonoBehaviour
{
    int gold;
    int exp;

    public TextMeshProUGUI CurrentFunds;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gold = CurrencyManagerScript.Instance.Gold;
        exp = CurrencyManagerScript.Instance.Exp;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentFunds.text = "Gold : " + gold.ToString() + "\nExp : " + exp.ToString();
    }
}
