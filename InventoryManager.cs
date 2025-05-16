using TMPro; // Required for TextMeshPro
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int woodCount = 0;
    public int stoneCount = 0;

    public TMP_Text woodText; 
    public TMP_Text stoneText;

    void Start()
    {
        if(woodText == null)
            woodText = GameObject.Find("WoodText").GetComponent<TMP_Text>();
        
        if(stoneText == null)
            stoneText = GameObject.Find("StoneText").GetComponent<TMP_Text>();

        UpdateUI();
    }

    public void AddWood(int amount)
    {
        woodCount += amount;
        UpdateUI();
    }

    public void AddStone(int amount)
    {
        stoneCount += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (woodText != null) woodText.text = "Wood: " + woodCount;
        if (stoneText != null) stoneText.text = "Stone: " + stoneCount;
    }
}
