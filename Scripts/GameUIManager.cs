using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    [Header("Popups")]
    public GameObject winPopup;
    public GameObject losePopup;

    [Header("Win Popup Buttons")]
    public Button winHomeButton;
    public Button winRetryButton;    // 新增：胜利弹窗的 Retry 按钮
    public Button winSwitchSceneButton; // 新增：胜利弹窗的切换场景按钮

    [Header("Lose Popup Buttons")]
    public Button loseHomeButton;
    public Button loseRetryButton;

    [Header("Scene Settings")]
    public string switchSceneName = "Level2"; // 可自定义切换的场景名

    void Start()
    {
        // 初始隐藏所有弹窗
        if (winPopup != null) winPopup.SetActive(false);
        if (losePopup != null) losePopup.SetActive(false);

        // 绑定 WinPanel 按钮
        if (winHomeButton != null)
        {
            winHomeButton.onClick.RemoveAllListeners();
            winHomeButton.onClick.AddListener(ReturnToMainMenu);
        }

        if (winRetryButton != null)
        {
            winRetryButton.onClick.RemoveAllListeners();
            winRetryButton.onClick.AddListener(RetryGame);
        }

        if (winSwitchSceneButton != null)
        {
            winSwitchSceneButton.onClick.RemoveAllListeners();
            winSwitchSceneButton.onClick.AddListener(SwitchScene);
        }

        // 绑定 LosePanel 按钮
        if (loseHomeButton != null)
        {
            loseHomeButton.onClick.RemoveAllListeners();
            loseHomeButton.onClick.AddListener(ReturnToMainMenu);
        }

        if (loseRetryButton != null)
        {
            loseRetryButton.onClick.RemoveAllListeners();
            loseRetryButton.onClick.AddListener(RetryGame);
        }
    }

    public void ShowWinPopup()
    {
        if (winPopup != null)
        {
            winPopup.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ShowLosePopup()
    {
        if (losePopup != null)
        {
            losePopup.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void OnRestartButtonClicked()
    {
        RetryGame();
    }

    void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    void RetryGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 新增：切换场景方法
    void SwitchScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(switchSceneName);
    }
}