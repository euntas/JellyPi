using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    private static GameManager m_instance; // 싱글톤이 할당될 static 변수

    private int score = 0; // 현재 게임 점수
    public bool isGameover { get; private set; } // 게임 오버 상태
    public bool isPause { get; private set; } // 일시정지 상태

    public Stage currentStage; // 현재 스테이지
    public EnemySpawner enemySpawner; // 적 스포너

    // 게임 머니 (골드)
    private int gold
    {
        get
        {
            if (!PlayerPrefs.HasKey("Gold"))
            {
                return 0;
            }

            int tmpGold = PlayerPrefs.GetInt("Gold");
            return tmpGold;
        }

        set
        {
            PlayerPrefs.SetInt("Gold", value);
        }
    }

    private void Awake()
    {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (instance != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        isPause = false;

        // 플레이어 캐릭터의 사망 이벤트 발생시 게임 오버
        FindObjectOfType<PlayerHealth>().onDeath += EndGame;

        // 스테이지 로드
        LoadStage(0);

        // gold UI 업데이트
        UIManager.instance.UpdateGoldText(gold);
    }

    private void Update()
    {
    }

    // 게임 오버 처리
    public void EndGame()
    {
        // 게임 오버 상태를 참으로 변경
        isGameover = true;
        // 게임 오버 UI를 활성화
        UIManager.instance.SetActiveGameoverUI(true);
    }

    // 게임 앱 종료
    public void QuitGame()
    {
        Debug.Log("quit game");
        Application.Quit();
    }

    // 게임 일시정지
    public void PauseGame(bool _isPause)
    {
        isPause = _isPause;
        
        if (isPause)
        {
            // 일시정지 활성화
            Time.timeScale = 0;
        }
        else
        {
            // 일시정지 비활성화
            Time.timeScale = 1;
        }
    }

    // 점수를 추가하고 UI 갱신
    public void AddScore(int newScore)
    {
        // 게임 오버가 아닌 상태에서만 점수 증가 가능
        if (!isGameover)
        {
            // 점수 추가
            score += newScore;
            // 점수 UI 텍스트 갱신
            UIManager.instance.UpdateScoreText(score);
        }
    }

    // 스테이지 로드
    public void LoadStage(int _stageId)
    {
        // 스테이지 정보 초기화
        currentStage = new Stage();
        currentStage.InitStage(0);

        // EnemySpawner의 적 스폰 위치 변수 세팅
        Transform[] _spawnPoints = new Transform[currentStage.spawnPointNums.Length];
        
        int index = 0;
        foreach(int pointNum in currentStage.spawnPointNums)
        {
            // 씬에 배치된 Spawn Point 오브젝트 중 pointNum 에 해당하는 것을 찾는다
            GameObject spawnPoint = GameObject.Find("Spawn Point " + pointNum);

            if (spawnPoint != null)
            {
                _spawnPoints[index] = spawnPoint.transform;
                index++;
            }
        }

        if(_spawnPoints[0] != null)
        {
            enemySpawner.spawnPoints = _spawnPoints;
        }

        // 스포너의 상태를 준비완료로 변경
        enemySpawner.isSpawnerReady = true;

        // UI 내용 업데이트
        UIManager.instance.UpdateStageNameText(currentStage.stageName);
        enemySpawner.UpdateUI();
    }
}