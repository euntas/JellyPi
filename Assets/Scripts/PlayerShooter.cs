﻿using UnityEngine;

// 주어진 Gun 오브젝트를 쏘거나 재장전
// 알맞은 애니메이션을 재생하고 IK를 사용해 캐릭터 양손이 총에 위치하도록 조정
public class PlayerShooter : MonoBehaviour
{
    public Gun gun; // 사용할 총
    public Transform gunPivot; // 총 배치의 기준점
    public Transform leftHandMount; // 총의 왼쪽 손잡이, 왼손이 위치할 지점
    public Transform rightHandMount; // 총의 오른쪽 손잡이, 오른손이 위치할 지점
    private PlayerInput playerInput; // 플레이어의 입력
    private Animator playerAnimator; // 애니메이터 컴포넌트

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();

        // TODO. 나중에 무기 들 때 설정으로 바꾸기
        playerAnimator.SetInteger("WeaponType_int", 4);
        playerAnimator.SetBool("Shoot_b", false);
        playerAnimator.SetBool("Reload_b", false);
    }

    private void OnEnable()
    {
        // 슈터가 활성화될 때 총도 함께 활성화
        gun.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        // 슈터가 비활성화될 때 총도 함께 비활성화
        gun.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // 입력을 감지하고 총을 발사하거나 재장전
        if(playerInput.fire)
        {
            // 발사 입력 감지 시 총 발사
            playerAnimator.SetBool("Shoot_b", true);
            gun.Fire();
        }
        else if(playerInput.reload)
        {
            // 재장전 입력 감지 시 재장전
            if(gun.Reload())
            {
                // 재장전 성공 시에만 재장전 애니메이션 재생
                playerAnimator.SetBool("Reload_b", true);
            }
        }
        else
        {
            // 애니메이션 변수 초기화
            playerAnimator.SetBool("Shoot_b", false);
            playerAnimator.SetBool("Reload_b", false);
        }

        // 남은 탄알 UI 갱신
        UpdateUI();
    }

    // 탄알 UI 갱신
    private void UpdateUI()
    {
        if(gun != null && UIManager.instance != null)
        {
            UIManager.instance.UpdateAmmoText(gun.magAmmo, gun.ammoRemain);
        }
    }

    // 애니메이터의 IK 갱신
    private void OnAnimatorIK(int layerIndex)
    {
        // 총의 기준점 gunPivot을 3d 모델의 오른쪽 팔꿈치 위치로 이동
        gunPivot.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

        // IK를 사용하여 왼손의 위치와 회전을 총의 왼쪽 손잡이에 맞춤
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);

        // IK를 사용하여 오른손의 위치와 회전을 총의 오른쪽 손잡이에 맞춤
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
    }
}
