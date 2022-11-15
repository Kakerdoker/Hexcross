using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag_Mouse : MonoBehaviour
{

    bool isRightMouseDragActive = false;
    bool rightMouseDownInPreviousFrame = false;
    bool mouseOver = false;
    bool rightMouseDragActive = false;
    bool startedOnHex = false;

    void OnMouseExit(){
        mouseOver = false;
    }

    void OnMouseOver(){
        if(Input.GetMouseButtonDown(1)){
            startedOnHex = true;
        }
        mouseOver = true;
    }
    
    void Update() {
        if(Input.GetMouseButtonUp(1)){
            startedOnHex = false;
        }

        if (Input.GetMouseButton(1) && (mouseOver || rightMouseDownInPreviousFrame) && startedOnHex)
        {
            if (rightMouseDownInPreviousFrame)
            {
                if (isRightMouseDragActive)
                {
                    gameObject.SendMessage("OnRightMouseDrag",SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    isRightMouseDragActive = true;
                    gameObject.SendMessage("OnRightMouseDown",SendMessageOptions.DontRequireReceiver);
                }
            }
            rightMouseDownInPreviousFrame = true;
        }
        else
        {
            if (isRightMouseDragActive)
            {
                isRightMouseDragActive = false;
                gameObject.SendMessage("OnRightMouseUp",SendMessageOptions.DontRequireReceiver);
            }
            rightMouseDownInPreviousFrame = false;
        }
    }
}
