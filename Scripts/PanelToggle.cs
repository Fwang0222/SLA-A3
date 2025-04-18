using UnityEngine;

public class PanelToggle : MonoBehaviour
{
    public GameObject panel;
    
    // 需要暂停的所有组件
    private MonoBehaviour[] pauseableComponents;
    private bool[] wasComponentEnabled;
    
    void Start()
    {
        // 获取玩家对象上所有需要暂停的脚本组件
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            pauseableComponents = player.GetComponents<MonoBehaviour>();
            wasComponentEnabled = new bool[pauseableComponents.Length];
        }
    }

    public void TogglePanel()
    {
        bool willActivate = !panel.activeSelf;
        panel.SetActive(willActivate);
        
        if (willActivate)
        {
            // 暂停游戏 - 记录并禁用所有相关组件
            Time.timeScale = 0f;
            
            if (pauseableComponents != null)
            {
                for (int i = 0; i < pauseableComponents.Length; i++)
                {
                    if (pauseableComponents[i] != null && pauseableComponents[i].enabled)
                    {
                        wasComponentEnabled[i] = true;
                        pauseableComponents[i].enabled = false;
                    }
                }
            }
        }
        else
        {
            // 恢复游戏 - 重新启用之前活跃的组件
            Time.timeScale = 1f;
            
            if (pauseableComponents != null)
            {
                for (int i = 0; i < pauseableComponents.Length; i++)
                {
                    if (pauseableComponents[i] != null && wasComponentEnabled[i])
                    {
                        pauseableComponents[i].enabled = true;
                    }
                }
            }
        }
    }
}