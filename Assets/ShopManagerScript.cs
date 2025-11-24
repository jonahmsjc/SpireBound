using UnityEngine;
using TMPro;

public class ShopManagerScript : MonoBehaviour
{
    int gold;
    int exp;
    float Spd;
    float Atk;
    float Def;

    public TextMeshProUGUI CurrentFunds;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gold = CurrencyManagerScript.Instance.Gold;
        exp = CurrencyManagerScript.Instance.Exp;
        Spd = CurrencyManagerScript.Instance.Spd;
        Atk = CurrencyManagerScript.Instance.Atk;
        Def = CurrencyManagerScript.Instance.Def;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentFunds.text = "Gold : " + gold.ToString() + "\nExp : " + exp.ToString();
    }
    public void OnClick_AddAtk()
    {
        CurrencyManagerScript.Instance.addAtk();
        Atk = CurrencyManagerScript.Instance.Atk;
    }

    public void OnClick_AddDef()
    {
        CurrencyManagerScript.Instance.addDef();
        Def = CurrencyManagerScript.Instance.Def;
    }

    public void OnClick_AddSpd()
    {
        CurrencyManagerScript.Instance.addSpd();
        Spd = CurrencyManagerScript.Instance.Spd;
    }
}
