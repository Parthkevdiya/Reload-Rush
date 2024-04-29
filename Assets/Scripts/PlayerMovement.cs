using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerHorizontalMovementSpeed;
    [SerializeField] private float playerForwardMovementSpeed;

    [SerializeField] private bool takePushBack = false;
    [SerializeField] private float pushBackTimeMax = 0.5f;
    [SerializeField] private float pushBackTime = 0f;
    private void Start()
    {
        Drag.Instance.OnDynamicDrag += Drag_OnDynamicDrag;
    }

    private void Update()
    {
        if (takePushBack)
        {
            if (pushBackTime > 0)
            {
                pushBackTime -= Time.deltaTime;
                transform.position += new Vector3(0, 0, -playerForwardMovementSpeed * Time.deltaTime);
            }
            else
            {
                takePushBack = false;
            }
        }
        else
        {
            transform.position += new Vector3(0, 0, playerForwardMovementSpeed * Time.deltaTime);
        }
        
    }

    private void Drag_OnDynamicDrag(object sender, Drag.OnDynamicDragEventArgs e)
    {
        Vector2 velocity = e.GetDynamicDragDirection();

        if(velocity.x <= -0.9f || velocity.x >= 0.9f)
        {
            float moveX = -(velocity.x * Time.deltaTime * playerHorizontalMovementSpeed);
            Vector3 movePosition = transform.position + new Vector3(moveX, 0, 0);

            float clampX = Mathf.Clamp(movePosition.x, -4.5f, 4.5f);
            Vector3 clampMovePosition = new Vector3(clampX, movePosition.y, movePosition.z);

            //transform.position = clampMovePosition;
            transform.DOMove(clampMovePosition , 0.01f);
        }
    }

    public void RemoveMovementFromEvent()
    {
        Drag.Instance.OnDynamicDrag -= Drag_OnDynamicDrag;
    }

    public void TakePushBack()
    {
        takePushBack = true;
        float moveYstartValue = transform.localPosition.y;
        transform.DOLocalMoveY(moveYstartValue + 1.5f, pushBackTimeMax/2).OnComplete(() => 
        {
            transform.DOLocalMoveY(moveYstartValue, pushBackTimeMax / 2);
        });
        pushBackTime = pushBackTimeMax;
    }
}
