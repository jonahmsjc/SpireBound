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
        set;
    } = 0;

    [field: SerializeField, Min(0)]
    public int Exp
    {
        get;
        set;
    } = 0;

    [field: SerializeField, Min(0)]
    public float Atk
    {
        get;
        private set;
    } = 0f;

    [field: SerializeField, Min(0)]
    public float Def
    {
        get;
        private set;
    } = 0f;

    [field: SerializeField, Min(0)]
    public float Spd
    {
        get;
        private set;
    } = 0;

    public void addSpd()
    {
        Spd += 0.1f;
    }

    public void addDef()
    {
        Def += 0.1f;
    }

    public void addAtk()
    {
        Atk += 0.1f;
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
