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

    public static UIManager m_instance; // 싱글톤이 할당될 변수

    public GameObject gameoverUI; // 게임 오버시 활성화할 UI
    public GameObject settingUI; // 게임 세팅 메뉴 UI
    public GameObject shopUI; // 게임 상점 UI

    public Text ammoText; // 탄약 표시용 텍스트
    public Text playerHPText; // 플레이어 hp 표시용 텍스트
    public Text waveText; // 적 웨이브 표시용 텍스트
    public Text scoreText; // 점수 표시용 텍스트

    // 게임 오버 UI 활성화
    public void SetActiveGameoverUI(bool active)
    {
        GameManager.instance.PauseGame(active);

        gameoverUI.SetActive(active);
    }

    // 게임 종료시 UI
    public void SetActiveSettingUI(bool active)
    {
        GameManager.instance.PauseGame(active);

        settingUI.SetActive(active);
    }

    // 게임 종료시 UI
    public void SetActiveshopUI(bool active)
    {
        Debug.Log("setactiveshopUI : " + active);
        GameManager.instance.PauseGame(active);

        shopUI.SetActive(active);
    }

    // 게임 재시작
    public void GameRestart()
    {
        SetActiveSettingUI(false);

        SceneManager.LoadScene("Main");
    }

    // 탄약 텍스트 갱신
    public void UpdateAmmoText(int magAmmo, int remainAmmo)
    {
        ammoText.text = magAmmo + "/" + remainAmmo;
    }

    // 플레이어 HP 텍스트 갱신
    public void UpdatePlayerHPText(int currentHP, int maxHP)
    {
        int displayHP = (currentHP < 0) ? 0 : currentHP; 
        playerHPText.text = "HP " + displayHP + "/" + maxHP;
    }

    // 적 웨이브 텍스트 갱신
    public void UpdateWaveText(int waves, int count)
    {
        waveText.text = "Wave : " + waves + "\nEnemy Left : " + count;
    }

    // 점수 텍스트 갱신
    public void UpdateScoreText(int newScore)
    {
        scoreText.text = "Score : " + newScore;
    }
}