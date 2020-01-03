using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private RectTransform rect_Background;
    [SerializeField] private RectTransform rect_Joystick;

    private float radius;

    [SerializeField] private GameObject go_Player;

    public bool isTouch = false;

    public string moveAxisName = "Vertical"; // 앞뒤 움직임을 위한 입력축 이름
    public string rotateAxisName = "Horizontal"; // 좌우 회전을 위한 입력축 이름
    public string fireButtonName = "Fire1";
    public string reloadButtonName = "Reload";

    public float horizontal { get; private set; } // 감지된 움직임 입력값
    public float vertical { get; private set; } // 감지된 회전 입력값
    public bool fire { get; private set; } // 감지된 발사 입력값
    public bool reload { get; private set; } // 감지된 재장전 입력값

    // Start is called before the first frame update
    void Start()
    {
        radius = rect_Background.rect.width * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))

        {
            UIManager.instance.SetActiveSettingUI(true);
        }

        if (!isTouch)
        {
        }

        // 게임오버 상태에서는 사용자 입력을 감지하지 않는다
        //if (GameManager.instance != null && GameManager.instance.isGameover)
        //{
        //    fire = false;
        //    reload = false;
        //    return;
        //}

        //// fire에 관한 입력 감지
        //fire = Input.GetButton(fireButtonName);
        //// reload에 관한 입력 감지
        //reload = Input.GetButtonDown(reloadButtonName);
    }

    public void OnPointerDown(BaseEventData inputData)
    {
        PointerEventData eventData = (PointerEventData)inputData;
        isTouch = true;
    }

    public void OnPointerUp(BaseEventData inputData)
    {
        PointerEventData eventData = (PointerEventData)inputData;
        isTouch = false;
        rect_Joystick.localPosition = Vector3.zero;
    }

    public void OnDrag(BaseEventData inputData)
    {
        PointerEventData eventData = (PointerEventData)inputData;
        Vector2 value = eventData.position - (Vector2)rect_Background.position;

        value = Vector2.ClampMagnitude(value, radius);
        rect_Joystick.localPosition = value;

        value = value.normalized;

        horizontal = value.x;
        vertical = value.y;
    }

    public void SetFire(bool isFire)
    {
        fire = isFire;
    }

    public void SetReload(bool isReload)
    {
        reload = isReload;
    }
}
