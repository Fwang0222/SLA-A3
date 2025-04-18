using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System; // 用于DateTime

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("UI References")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timeText;      // 显示当前时间的UI文本
    [SerializeField] private List<GameObject> heartIcons;
    [SerializeField] private int winScore = 10;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip scoreSound;
    private AudioSource audioSource;

    private int currentScore = 0;
    private float startTime;          // 游戏开始时间
    private float currentTime;        // 当前游戏时间（秒）
    private bool isGameActive = true; // 游戏是否在进行中

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        startTime = Time.time; // 记录游戏开始时间
    }

    void Update()
    {
        if (isGameActive)
        {
            currentTime = Time.time - startTime;
            UpdateTimeDisplay();
        }
    }

    public void AddScore(int points)
    {
        currentScore += points;
        UpdateScoreDisplay();

        if (scoreSound != null)
            audioSource.PlayOneShot(scoreSound);

        if (currentScore >= winScore)
        {
            isGameActive = false; // 停止计时
            GameUIManager uiManager = FindObjectOfType<GameUIManager>();
            uiManager?.ShowWinPopup();
        }
    }

    // 更新UI显示
    private void UpdateTimeDisplay() => timeText.text = $"Time: {currentTime:F1}s";
    private void UpdateScoreDisplay() => scoreText.text = $"{currentScore}/{winScore}";
    public void UpdateHealthDisplay(int health)
    {
        for (int i = 0; i < heartIcons.Count; i++)
            heartIcons[i].SetActive(i < health);
    }
}