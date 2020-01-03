using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }

            return m_instance;
        }
    }

    public GameObject gameoverUI; // 게임 오버시 활성화할 UI
    public GameObject settingUI; // 게임 세팅 메뉴 UI
    public static UIManager m_instance; // 싱글톤이 할당될 변수

    // 게임 오버 UI 활성화
    public void SetActiveGameoverUI(bool active)
    {
        gameoverUI.SetActive(active);
    }

    // 게임 종료시 UI
    public void SetActiveSettingUI(bool active)
    {
        settingUI.SetActive(active);
    }

    // 게임 재시작
    public void GameRestart()
    {
        SetActiveSettingUI(false);

        SceneManager.LoadScene("Main");
    }
}