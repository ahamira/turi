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

        int earnedGold = Mathf.RoundToInt(finalDistance);
        myGold += earnedGold;

        string rank = (powerRatio > 0.85f) ? "<color=red>PERFECT!!</color>\n" : "GOOD\n";
        resultText.text = rank + $"Distance: {finalDistance:F1} m\n<color=yellow>Get: +{earnedGold} Gold!</color>";

        Invoke("ResetReady", 2.0f); 
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
            resultText.text = "<color=red>Not enough Gold!</color>\nPress THROW Button to Start!";
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
