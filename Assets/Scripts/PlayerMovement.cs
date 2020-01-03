﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 앞뒤 움직임의 속도
    public float rotateSpeed = 180f; // 좌우 회전 속도


    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디
    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터

    private void Start()
    {
        // 사용할 컴포넌트들의 참조를 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();

        // TODO. 나중에 무기 들 때 설정으로 바꾸기
        playerAnimator.SetInteger("WeaponType_int", 4);
        playerAnimator.SetBool("Shoot_b", false);
        playerAnimator.SetBool("Reload_b", false);

    }

    // FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
    private void FixedUpdate()
    {
        // 물리 갱신 주기마다 움직임, 회전, 애니메이션 처리 실행
        
        if (playerInput.isTouch)
        {
            // 움직임 실행
            Move();
            Rotate();
            playerAnimator.SetBool("Static_b", false);
            playerAnimator.SetFloat("Speed_f", new Vector2(playerInput.horizontal, playerInput.vertical).magnitude);

        }
        else
        {
            playerAnimator.SetFloat("Speed_f", 0f);
            playerAnimator.SetBool("Static_b", true);
        }
        
        
    }

    // 입력값에 따라 캐릭터를 앞뒤로 움직임
    private void Move()
    {
        // 상대적으로 이동할 거리 계산
        Vector3 moveDistance = transform.forward * moveSpeed * Time.deltaTime;

        // 리지드바디를 이용해 게임 오브젝트 위치 변경
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    // 입력값에 따라 캐릭터를 좌우로 회전
    private void Rotate()
    {
        // 상대적으로 회전할 수치 계산
        float turn = playerInput.horizontal * rotateSpeed * Time.deltaTime;
        // 리지드바디를 이용해 게임 오브젝트 회전 변경
        playerRigidbody.MoveRotation(Quaternion.Euler(0, Mathf.Atan2(playerInput.horizontal, playerInput.vertical) * Mathf.Rad2Deg, 0));
    }
}