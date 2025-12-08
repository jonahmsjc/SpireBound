using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ShopManagerScript : MonoBehaviour
{
    public TextMeshProUGUI CurrentFunds;
    public int statCost = 25;
    public int dashAttackCostGold = 100;
    public int companionCostGold = 200;

    void Start()
    {
        UpdateFundsUI();
    }

    void Update()
    {
        UpdateFundsUI();
    }

    void UpdateFundsUI()
    {
        if (CurrencyManagerScript.Instance == null) return;

        int gold = CurrencyManagerScript.Instance.Gold;
        int exp = CurrencyManagerScript.Instance.Exp;
        float atk = CurrencyManagerScript.Instance.Atk;
        float def = CurrencyManagerScript.Instance.Def;
        float spd = CurrencyManagerScript.Instance.Spd;

        CurrentFunds.text =
            "Gold : " + gold +
            "\nExp : " + exp +
            "\nAtk : " + atk.ToString("0.0") +
            "\nDef : " + def.ToString("0.0") +
            "\nSpd : " + spd.ToString("0.0");
    }

    bool CanPurchase(float currentStat)
    {
        if (CurrencyManagerScript.Instance == null) return false;

        if (currentStat >= 10f)
        {
            Debug.Log("Stat is already maxed out.");
            return false;
        }

        if (CurrencyManagerScript.Instance.Exp < statCost)
        {
            Debug.Log("Not enough EXP to purchase this stat point.");
            return false;
        }

        return true;
    }

    public void OnClick_AddAtk()
    {
        if (CurrencyManagerScript.Instance == null) return;

        float currentAtk = CurrencyManagerScript.Instance.Atk;
        if (!CanPurchase(currentAtk)) return;

        CurrencyManagerScript.Instance.Exp -= statCost;
        CurrencyManagerScript.Instance.addAtk();
        UpdateFundsUI();
    }

    public void OnClick_AddDef()
    {
        if (CurrencyManagerScript.Instance == null) return;

        float currentDef = CurrencyManagerScript.Instance.Def;
        if (!CanPurchase(currentDef)) return;

        CurrencyManagerScript.Instance.Exp -= statCost;
        CurrencyManagerScript.Instance.addDef();
        UpdateFundsUI();
    }

    public void OnClick_AddSpd()
    {
        if (CurrencyManagerScript.Instance == null) return;

        float currentSpd = CurrencyManagerScript.Instance.Spd;
        if (!CanPurchase(currentSpd)) return;

        CurrencyManagerScript.Instance.Exp -= statCost;
        CurrencyManagerScript.Instance.addSpd();
        UpdateFundsUI();
    }

    public void OnClick_BuyCompanion()
    {
        if (CurrencyManagerScript.Instance == null) return;

        if (CurrencyManagerScript.Instance.CompanionUnlocked)
        {
            Debug.Log("Companion already purchased.");
            return;
        }

        if (CurrencyManagerScript.Instance.Gold < companionCostGold)
        {
            Debug.Log("Not enough gold to buy companion.");
            return;
        }

        CurrencyManagerScript.Instance.Gold -= companionCostGold;
        CurrencyManagerScript.Instance.UnlockCompanion();
        Debug.Log("Companion purchased.");
    }

    public void OnClick_BuyDashAttack()
    {
        if (CurrencyManagerScript.Instance == null) return;

        if (CurrencyManagerScript.Instance.DashAttackUnlocked)
        {
            Debug.Log("Dash attack already purchased.");
            return;
        }

        if (CurrencyManagerScript.Instance.Gold < dashAttackCostGold)
        {
            Debug.Log("Not enough gold to buy dash attack.");
            return;
        }

        CurrencyManagerScript.Instance.Gold -= dashAttackCostGold;
        CurrencyManagerScript.Instance.UnlockDashAttack();
        Debug.Log("Dash attack purchased.");
    }

    public void OnClick_NextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level2");
    }
}

