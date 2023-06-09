﻿using UnityEngine;
using UnityEngine.UI;

// 주어진 Gun 오브젝트를 쏘거나 재장전
// 알맞은 애니메이션을 재생하고 IK를 사용해 캐릭터 양손이 총에 위치하도록 조정
public class PlayerShooter : MonoBehaviour
{
    public Gun gun; // 사용할 총
    public Transform gunPivot; // 총 배치의 기준점
    private Transform rightHandMount; // 총의 오른쪽 손잡이, 오른손이 위치할 지점
    private PlayerInput playerInput; // 플레이어의 입력
    private Animator playerAnimator; // 애니메이터 컴포넌트

    // Start is called before the first frame update
    void Start()
    {
        // 오른쪽 손잡이 위치
        rightHandMount = transform.Find("Hips_jnt/Spine_jnt/Spine_jnt 1/Chest_jnt/Shoulder_Right_jnt/Arm_Right_jnt/Forearm_Right_jnt/Hand_Right_jnt");

        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();

        SetGunByWeaponType((int)Commons.WeaponType_int.WeaponType_None);
    }

    private void OnEnable()
    {
        // 슈터가 활성화될 때 총도 함께 활성화
        if (gun != null)
        {
            gun.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        // 슈터가 비활성화될 때 총도 함께 비활성화
        if(gun != null)
        {
            gun.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(gun != null)
        {
            // 입력을 감지하고 총을 발사하거나 재장전
            if (playerInput.fire)
            {
                // 발사 입력 감지 시 총 발사
                if (gun.Fire())
                {
                    playerAnimator.SetBool("Shoot_b", true);
                }
            }
            else if (playerInput.reload)
            {
                // 재장전 입력 감지 시 재장전
                if (gun.Reload())
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
        // 총의 기준점 gunPivot을 오른손에 맞춤.
        gunPivot.position = rightHandMount.position;

        // IK를 사용하여 오른손의 위치와 회전을 총의 오른쪽 손잡이에 맞춤
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
    }

    // 해당 weaponType의 총을 선택 
    private void SetGunByWeaponType(int weaponType)
    {
        // player 가 가지고 있는 모든 총을 비활성화
        int childCount = gunPivot.childCount;
        for(int i=0; i<childCount; i++)
        {
            gunPivot.GetChild(i).gameObject.SetActive(false);
        }

        GameObject gunObj;

        // 사용할 총 오브젝트 찾기
        switch (weaponType)
        {
            case (int)Commons.WeaponType_int.WeaponType_Pistol:
                gunObj = transform.Find("Gun Pivot/SA_Wep_Pistol").gameObject;
                break;

            case (int)Commons.WeaponType_int.WeaponType_Shotgun:
                gunObj = transform.Find("Gun Pivot/SA_Wep_Shotgun").gameObject;
                break;

            default:
                gunObj = null;
                break;
        }

        // 총 활성화
        if (gunObj != null)
        {
            gunObj.SetActive(true);

            gun = GetComponentInChildren<Gun>();
            gun.weaponType = weaponType;
            gun.SetGunAbillity(weaponType);

            // 애니메이션 설정
            playerAnimator.SetInteger("WeaponType_int", weaponType);
            playerAnimator.SetBool("Shoot_b", false);
            playerAnimator.SetBool("Reload_b", false);
        }
        else
        {
            gun = null;

            // 애니메이션 설정
            playerAnimator.SetInteger("WeaponType_int", weaponType);
        }
    }

    // 총기 종류 변환
    public void ChangeGun(int weaponType)
    {
        Debug.Log("총 바꾸기 :" + weaponType);
        SetGunByWeaponType(weaponType);
    }
}
