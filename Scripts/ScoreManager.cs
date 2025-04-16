using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("UI References")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private List<GameObject> heartIcons; // 存放3个爱心对象的列表

    private int currentScore = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddScore(int points)
    {
        currentScore += points;
        if (scoreText != null) scoreText.text = "SCORE: " + currentScore;
    }

    public void UpdateHealthDisplay(int health)
    {
        // 遍历所有爱心，根据当前血量显示/隐藏
        for (int i = 0; i < heartIcons.Count; i++)
        {
            heartIcons[i].SetActive(i < health);
        }
    }
}