using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage
{
    public int stageId { get; private set; } // 스테이지 고유 번호
    public string stageName; // 스테이지 이름
    public bool isStageClear { get; private set; } // 스테이지 클리어 상태
    public int totalWaveNum;  // 총 wave 수
    public Vector3 playerPoint; // 플레이어 초기 위치
    public int[] spawnPointNums; // 적 AI를 소환할 위치

    // Stage 초기화
    public void InitStage(int _stageId)
    {
        isStageClear = false;

        // TODO. 나중에 스테이지 ID 별로 정보를 불러오는 식으로 바꿔야 함
        SetStage(_stageId, "First Stage!!", 1, new Vector3(0, 0, 0), new int[] { 1, 2, 3 });
    }

    // Stage 정보 세팅
    public void SetStage(int _stageId, string _stageName, int _totalWaveNum, Vector3 _playerPoint, int[] _spawnPointNums)
    {
        stageId = _stageId;
        stageName = _stageName;
        totalWaveNum = _totalWaveNum;
        playerPoint = _playerPoint;
        spawnPointNums = _spawnPointNums;
    }

    // 스테이지 클리어 처리
    public void ClearStage(int rewardGold)
    {
        isStageClear = true;

        UIManager.instance.UpdateStageClearText(stageName, rewardGold);

        // clear UI 활성화
        UIManager.instance.SetActiveStageClearUI(true);
    }
}
