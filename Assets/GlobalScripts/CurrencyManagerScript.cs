using UnityEngine;

[DisallowMultipleComponent]
[DefaultExecutionOrder(-1000)]
public class CurrencyManagerScript : MonoBehaviour
{
    public static CurrencyManagerScript Instance
    {
        get;
        private set;
    }

    [field: SerializeField, Min(0)]
    public int Gold
    {
        get;
        private set;
    } = 0;

    [field: SerializeField, Min(0)]
    public int Exp
    {
        get;
        private set;
    } = 0;

    [field: SerializeField, Min(0)]
    public int Atk
    {
        get;
        private set;
    } = 0;

    [field: SerializeField, Min(0)]
    public int Def
    {
        get;
        private set;
    } = 0;

    [field: SerializeField, Min(0)]
    public int Spd
    {
        get;
        private set;
    } = 0;

    public void addSpd()
    {
        Spd++;
    }

    public void addDef()
    {
        Def++;
    }

    public void addAtk()
    {
        Atk++;
    }

    private void Start()
    {
        
    }

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    
}
