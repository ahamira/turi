using UnityEngine;
using TMPro;

public class FishingThrowManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private RectTransform cursor;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI upgradeText;

    [Header("Settings")]
    [SerializeField] private float speed = 500f;

    private float baseMaxDistance = 100f;
    private int currentLevel = 1;
    private int myGold = 0;

    private float gaugeMinY = -150f;
    private float gaugeMaxY = 150f;
    private float currentY;
    private bool isMovingUp = true;
    private bool isGameActive = false;

    void Start()
    {
        currentY = gaugeMinY;
        UpdateUI();
        resultText.text = "Press THROW Button to Start!";
    }

    void Update()
    {
        if (!isGameActive) return;

        if (isMovingUp)
        {
            currentY += speed * Time.deltaTime;
            if (currentY >= gaugeMaxY) { currentY = gaugeMaxY; isMovingUp = false; }
        }
        else
        {
            currentY -= speed * Time.deltaTime;
            if (currentY <= gaugeMinY) { currentY = gaugeMinY; isMovingUp = true; }
        }

        cursor.anchoredPosition = new Vector2(0, currentY);
    }

    public void OnThrowButtonClicked()
    {
        if (!isGameActive && resultText.text.Contains("Start"))
        {
            isGameActive = true;
            resultText.text = "Timing...!";
            return;
        }

        if (!isGameActive) return;
        isGameActive = false;

        float totalRange = gaugeMaxY - gaugeMinY;
        float currentProgress = currentY - gaugeMinY;
        float powerRatio = currentProgress / totalRange;

        float maxDistance = baseMaxDistance + (currentLevel - 1) * 20f;
        float finalDistance = powerRatio * maxDistance;

        int rarity = 1;
        string fishName = "";

        if (finalDistance < 100f) rarity = 1;
        else if (finalDistance < 150f) rarity = 2;
        else if (finalDistance < 250f) rarity = 3;
        else if (finalDistance < 400f) rarity = 4;
        else if (finalDistance < 550f) rarity = 5;
        else if (finalDistance < 800f) rarity = 6;
        else if (finalDistance < 1100f) rarity = 7;
        else if (finalDistance < 1500f) rarity = 8;
        else if (finalDistance < 2000f) rarity = 9;
        else rarity = 10;

        switch (rarity)
        {
            case 1:
                int r1 = Random.Range(0, 3);
                if (r1 == 0) fishName = "Aji";
                else if (r1 == 1) fishName = "Iwashi";
                else fishName = "Saba";
                break;
            case 2:
                int r2 = Random.Range(0, 2);
                fishName = (r2 == 0) ? "Karei" : "Sayori";
                break;
            case 3:
                int r3 = Random.Range(0, 2);
                fishName = (r3 == 0) ? "Tai" : "Suzuki";
                break;
            case 4:
                int r4 = Random.Range(0, 2);
                fishName = (r4 == 0) ? "Tako" : "Ika";
                break;
            case 5:
                int r5 = Random.Range(0, 2);
                fishName = (r5 == 0) ? "Maguro" : "Buri";
                break;
            case 6:
                fishName = "Kajiki";
                break;
            case 7:
                fishName = "Takaashigani";
                break;
            case 8:
                fishName = "Coelacanth";
                break;
            case 9:
                fishName = "Kraken";
                break;
            case 10:
                fishName = "Poseidon";
                break;
        }

        int baseFishPrice = 0;
        switch (rarity)
        {
            case 1: baseFishPrice = 30; break;
            case 2: baseFishPrice = 60; break;
            case 3: baseFishPrice = 120; break;
            case 4: baseFishPrice = 250; break;
            case 5: baseFishPrice = 500; break;
            case 6: baseFishPrice = 900; break;
            case 7: baseFishPrice = 1500; break;
            case 8: baseFishPrice = 2500; break;
            case 9: baseFishPrice = 4000; break;
            case 10: baseFishPrice = 7000; break;
        }

        int distanceBonus = Mathf.RoundToInt(finalDistance * 0.8f);
        int earnedGold = baseFishPrice + distanceBonus;

        myGold += earnedGold;

        resultText.text = $"[RARITY {rarity}]\n" +
                          $"{fishName}\n" +
                          $"Distance: {finalDistance:F1} m\n" +
                          $"Price: {baseFishPrice}G + Bonus: {distanceBonus}G\n" +
                          $"Total: +{earnedGold} Gold!";

        Invoke("ResetReady", 3.0f);
    }

    void ResetReady()
    {
        resultText.text = "Press THROW Button to Start!";
        UpdateUI();
    }

    public void OnUpgradeButtonClicked()
    {
        int cost = GetUpgradeCost();

        if (myGold >= cost)
        {
            myGold -= cost;
            currentLevel++;
            UpdateUI();
            resultText.text = "Upgrade Success!\nPress THROW Button to Start!";
        }
        else
        {
            resultText.text = "Not enough Gold!\nPress THROW Button to Start!";
        }
    }

    int GetUpgradeCost()
    {
        return currentLevel * 50;
    }

    void UpdateUI()
    {
        goldText.text = $"Gold: {myGold}";
        upgradeText.text = $"Upgrade Rod\nLv.{currentLevel}\n(Cost: {GetUpgradeCost()}G)";
    }
}