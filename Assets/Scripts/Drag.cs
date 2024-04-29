using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    public static Drag Instance;

    public event EventHandler<OnDragEventArgs> OnDrag;
    public class OnDragEventArgs : EventArgs
    {
        public Vector2 dragStartPosition;
        public Vector2 dragDirection;

        public Vector2 GetDragStartPositionInScreenSpace()
        {
            return dragStartPosition;
        }
        public Vector2 GetDragStartPositionInWorldSpace(Camera camera)
        {
           Vector2 dragStartWorldPosition = camera.ScreenToWorldPoint(dragStartPosition);
            return dragStartWorldPosition;
        }

        public Vector2 GetDragDistanceInScreenSpace()
        {
            Vector2 dragStart = dragStartPosition;
            Vector2 currentMousePosition = Input.mousePosition;
            return currentMousePosition - dragStart;
        }

        public Vector2 GetDragDistacneInWorldSpace(Camera camera)
        {
            Vector2 dragStartWorldPosition = camera.ScreenToWorldPoint(dragStartPosition);
            Vector2 currentMouseWorldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            return currentMouseWorldPosition - dragStartWorldPosition;
        }
        public Vector2 GetDragDirection()
        {
            return dragDirection;
        }
    }

    public event EventHandler<OnDynamicDragEventArgs> OnDynamicDrag;
    public class OnDynamicDragEventArgs : EventArgs
    {
        public Vector2 mousePositionDown;
        public Vector2 dynamicDragStartPosition;
        public Vector2 dynamicDragDirection;

        public Vector2 GetDynamicDragStartPositionInScreenSpace()
        {
            return dynamicDragStartPosition;
        }
        public Vector2 GetDynamicDragStartPositionInWorldSpace(Camera camera)
        {
            Vector2 dynamicDragStartWorldPosition = camera.ScreenToWorldPoint(dynamicDragStartPosition);
            return dynamicDragStartWorldPosition;
        }

        public Vector2 GetDynamicDragDistanceInScreenSpace()
        {
            Vector2 dynamicDragStart = mousePositionDown;
            Vector2 currentMousePosition = Input.mousePosition;
            return currentMousePosition - dynamicDragStart;
        }

        public Vector2 GetDynamicDragDistacneInWorldSpace(Camera camera)
        {
            Vector2 dynamicDragStartWorldPosition = camera.ScreenToWorldPoint(mousePositionDown);
            Vector2 currentMouseWorldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            return currentMouseWorldPosition - dynamicDragStartWorldPosition;
        }
        public Vector2 GetDynamicDragDirection()
        {
            return dynamicDragDirection;
        }
    }

    public event EventHandler<OnDragStartEventArgs> OnDragStart;
    public class OnDragStartEventArgs : EventArgs
    {
        public Vector2 dragStartPosition;

        public Vector2 GetMouseStartPositionInScreenSpace()
        {
            return dragStartPosition;
        }
        public Vector2 GetMouseStartPositionInWorldSpace(Camera camera)
        {
            Vector2 dragStartWorldPosition = camera.ScreenToWorldPoint(dragStartPosition);
            return dragStartWorldPosition;
        }
    }

    public event EventHandler<OnDragEndEvenArgs> OnDragEnd;

    public event EventHandler<OnZoomInOutEventArgs> OnZoomInOut;
    public class OnZoomInOutEventArgs : EventArgs
    {
        public float zoomDiffrence;

        public float GetZoomDiffrence()
        {
            return zoomDiffrence;
        }
    }
    public class OnDragEndEvenArgs : EventArgs
    {
        public Vector2 dragStartPosition;
        public Vector2 dragEndPosition;
        public Vector2 dragDirection;

        public Vector2 GetDragStartPositionInScreenSpace()
        {
            return dragStartPosition;
        }

        public Vector2 GetDragStartPositionInWordSpace(Camera camera)
        {
            Vector2 dragStartWorldPosition = camera.ScreenToWorldPoint(dragStartPosition);
            return dragStartWorldPosition;
        }

        public Vector2 GetDragEndPositionInScreenSpace()
        {
            return dragEndPosition;
        }

        public Vector2 GetDragEndPositionInWorldSpace(Camera camera)
        {
            Vector2 dragEndWorldPosition = camera.ScreenToWorldPoint(dragEndPosition);
            return dragEndWorldPosition;
        }

        public Vector2 GetDragDistanceInScreenSpace()
        {
            Vector2 dragStart = dragStartPosition;
            Vector2 dragEnd = dragEndPosition;
            return dragEnd - dragStart;
        }

        public Vector2 GetDragDistacneInWorldSpace(Camera camera)
        {
            Vector2 dragStartWorldPosition = camera.ScreenToWorldPoint(dragStartPosition);
            Vector2 dragEndWorldPosition = camera.ScreenToWorldPoint(dragEndPosition);
            return dragEndWorldPosition - dragStartWorldPosition;
        }

        public Vector2 GetDragDirection()
        {
            return dragDirection;
        }
    }

    Vector2 mousePositionDown;
    Vector2 dragStartPosition;
    Vector2 dragEndPosition;

    bool dragStarted = false;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePositionDown = Input.mousePosition;
            dragStartPosition = mousePositionDown;
            dragStarted = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragEndPosition = Input.mousePosition;
            if (dragStarted)
            {
                Vector2 dragDirection = dragStartPosition - dragEndPosition;
                OnDragEnd?.Invoke(this, new OnDragEndEvenArgs { dragStartPosition = dragStartPosition, dragEndPosition = dragEndPosition, dragDirection = dragDirection.normalized });
            }
            dragStarted = false;
        }

        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

            float prevMagnitute = (touch1PrevPos - touch2PrevPos).magnitude;
            float currentMagnitute = (touch1.position - touch2.position).magnitude;

            float Diffrence = currentMagnitute - prevMagnitute;

            OnZoomInOut?.Invoke(this , new OnZoomInOutEventArgs { zoomDiffrence = Diffrence });
        }

    }

    private void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 currentMousePoistion = Input.mousePosition;
            Vector2 dragDirection = dragStartPosition - currentMousePoistion;

            if (dragDirection != Vector2.zero)
            {
                if (!dragStarted)
                {
                    dragStarted = true;
                    OnDragStart?.Invoke(this, new OnDragStartEventArgs { dragStartPosition = dragStartPosition });
                }
                OnDrag?.Invoke(this , new OnDragEventArgs { dragDirection = dragDirection.normalized,  dragStartPosition = dragStartPosition });
            }
            //mousePositionDown = Input.mousePosition;
        }

        // For Dynamic Drag Requirement
        if (Input.GetMouseButton(0))
        {
            Vector2 currentMousePoistion = Input.mousePosition;
            Vector2 dragDirection = mousePositionDown - currentMousePoistion;

            if (dragDirection != Vector2.zero)
            {
                OnDynamicDrag?.Invoke(this , new OnDynamicDragEventArgs {mousePositionDown = mousePositionDown , dynamicDragDirection = dragDirection , dynamicDragStartPosition = dragStartPosition});
            }
            mousePositionDown = Input.mousePosition;
        }
    }

    public Vector2 GetWorldSpaceMousePosition(Camera camera)
    {
        return camera.ScreenToWorldPoint(Input.mousePosition);
    }
}
