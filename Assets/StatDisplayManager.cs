using UnityEngine;

public class StatBars : MonoBehaviour
{
    [Header("Pixel Prefab (1x1 white pixel)")]
    [SerializeField] private GameObject pixelPrefab;

    [Header("Stat Box SpriteRenderer")]
    [SerializeField] private SpriteRenderer statBoxRenderer;

    [Header("Stat Values (0–10)")]
    [Range(0, 10)] public int atk;
    [Range(0, 10)] public int def;
    [Range(0, 10)] public int spd;

    [Header("Pixel Visual Settings")]
    [SerializeField] private float pixelSize = 0.12f;
    [SerializeField] private float pixelSpacing = 0.02f;

    // --- Internal transforms ---
    private Transform atkBarParent;
    private Transform defBarParent;
    private Transform spdBarParent;


    void Awake()
    {
        // Create bar parents if not already in scene
        atkBarParent = CreateSubParent("ATK_Bar");
        defBarParent = CreateSubParent("DEF_Bar");
        spdBarParent = CreateSubParent("SPD_Bar");
    }


    void Start()
    {
        AutoAlignBars();
        RebuildAll();
    }


    // ---------------------------
    // Create empty children
    // ---------------------------
    private Transform CreateSubParent(string name)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(transform);
        return obj.transform;
    }


    // ---------------------------
    // Auto placement based on sprite
    // ---------------------------
    private void AutoAlignBars()
    {
        if (!statBoxRenderer) return;

        // Get pixel-correct bounds
        Bounds b = statBoxRenderer.bounds;

        float top = b.max.y;
        float bottom = b.min.y;
        float height = b.size.y;

        // 3 rows inside the box → divide vertically
        float rowHeight = height / 3f;

        // Center positions for rows
        float atkY = top - rowHeight * 0.5f;
        float defY = top - rowHeight * 1.5f;
        float spdY = top - rowHeight * 2.5f;

        float xStart = b.min.x + 0.2f;   // small horizontal padding

        atkBarParent.position = new Vector3(xStart, atkY, 0f);
        defBarParent.position = new Vector3(xStart, defY, 0f);
        spdBarParent.position = new Vector3(xStart, spdY, 0f);
    }


    // ---------------------------
    // Bar Construction
    // ---------------------------
    public void UpdateStats(int newAtk, int newDef, int newSpd)
    {
        atk = Mathf.Clamp(newAtk, 0, 10);
        def = Mathf.Clamp(newDef, 0, 10);
        spd = Mathf.Clamp(newSpd, 0, 10);

        RebuildAll();
    }


    private void RebuildAll()
    {
        RebuildBar(atkBarParent, atk);
        RebuildBar(defBarParent, def);
        RebuildBar(spdBarParent, spd);
    }


    private void RebuildBar(Transform barParent, int statValue)
    {
        // remove prior pixels
        foreach (Transform t in barParent)
            Destroy(t.gameObject);

        int count = Mathf.Clamp(statValue, 0, 10);

        for (int i = 0; i < count; i++)
        {
            GameObject pix = Instantiate(pixelPrefab, barParent);

            pix.transform.localScale = new Vector3(pixelSize, pixelSize, 1f);

            pix.transform.localPosition = new Vector3(
                i * (pixelSize + pixelSpacing),
                0f,
                0f
            );
        }
    }
}


