using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pan_And_Zoom : MonoBehaviour
{
    Sound_Handler soundHandler;
    UI_Stats_Handler uiStats;
    Pan_And_Zoom panAndZoom;
    GameObject numberObject;
    TextMeshProUGUI numberText;

    void setPrefs(int hexSize){

    }

    void setParameters(){
        hexgridHandler = gameObject.GetComponent<Hexgrid_Handler>();
        soundHandler = gameObject.GetComponent<Sound_Handler>();
        uiStats = gameObject.GetComponent<UI_Stats_Handler>();
        numberObject = GameObject.Find("amountSelectedNumber");
        numberText = numberObject.GetComponent<TextMeshProUGUI>();

        PlayerPrefs.GetInt("size");

        if(PlayerPrefs.GetInt("newGame") == 1){

        }

        zoomOutMax = PlayerPrefs.GetInt("size")+1f;
        zoomOutMin = zoomOutMax <= 4f ? 1f : 1f;
        borderX = PlayerPrefs.GetInt("size");
        borderY = PlayerPrefs.GetInt("size")*0.76f;
    }

    Hexgrid_Handler hexgridHandler;
    void Start(){
        setParameters();
    }

    Vector3 touchStart, touchDrag, draggedLine;
    Vector3 leftClickStart;
    float zoomOutMax, borderX, borderY, zoomOutMin, angle;

    public int howManyHexesSelected = 1;
    private int lastAmountOfHexesSelected = 1;
    public int closestAxis = 999;
    public float gapToHexEdge = 0f;

    public bool blockedPlacement = false;

    int howManySelected(float differencePerNewHex, float differenceFromCenter, float draggedLine){
        return (int)Mathf.Floor((Mathf.Abs(draggedLine)+differenceFromCenter)/differencePerNewHex) + 1;
    }

    void checkHowManyAreSelectedFromTopToBottom(bool reverse){
        if(leftClickStart.y <= hexPosition.y != reverse){
                howManyHexesSelected = howManySelected(0.76f, -Mathf.Abs(leftClickStart.y-hexPosition.y),draggedLine.y);
        }
        else{
                howManyHexesSelected = howManySelected(0.76f, Mathf.Abs(leftClickStart.y-hexPosition.y),draggedLine.y);
        }   
    }

    void checkHowManyAreSelectedFromRightToLeft(bool reverse){
        if(leftClickStart.x <= hexPosition.x != reverse){
                howManyHexesSelected = howManySelected(1f, -Mathf.Abs(leftClickStart.x-hexPosition.x),draggedLine.x);
        }
        else{
                howManyHexesSelected = howManySelected(1f, Mathf.Abs(leftClickStart.x-hexPosition.x),draggedLine.x);
        }       
    }

    public Vector2 hexPosition;
    public void checkClosestAxisAndHowManyHexesAreSelectedOnThatAxis(){
        if(angle >= 0f && angle  <= 62.5f){
            checkHowManyAreSelectedFromTopToBottom(false);
            closestAxis = 0;
        }
        else if(angle >= 62.5f && angle <= 117.5f){
            checkHowManyAreSelectedFromRightToLeft(false);
            closestAxis = 1;
        }
        else if(angle >= 117.5f && angle  <= 180f){
            checkHowManyAreSelectedFromTopToBottom(true);
            closestAxis = 2;
        }
        else if(angle >= 180f && angle  <= 242.5f){
            checkHowManyAreSelectedFromTopToBottom(true);
            closestAxis = 3;
        }
        else if(angle >= 242.5f && angle  <= 297.5f){
            checkHowManyAreSelectedFromRightToLeft(true);
            closestAxis = 4;
        }
        else if(angle >= 297.5f && angle  <= 360f){
            checkHowManyAreSelectedFromTopToBottom(false);
            closestAxis = 5;
        }
        howManyHexesSelected = howManyHexesSelected < 1 ? 1 : howManyHexesSelected;
    }

    public void calculateVariablesForDragSelectingUsindLeftMouseButton(){
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)){
            leftClickStart = pointer;
        }
        else if(Input.GetMouseButton(0) || Input.GetMouseButton(1)){
            calculateAngleAndLengthOfDraggedLine();
            checkClosestAxisAndHowManyHexesAreSelectedOnThatAxis();
            //Debug.DrawRay(new Vector3(0,0,0), draggedLine, Color.green);
        }
    }

    void calculateAngleAndLengthOfDraggedLine(){
        touchDrag = pointer;
        touchDrag.z = leftClickStart.z = 0;
        draggedLine = touchDrag - leftClickStart;
        angle = Vector2.Angle(new Vector3(0f, 10f, 0f), draggedLine);
        if(draggedLine.x < 0f){
            angle = 360f - angle;
        }
    }

    void panCameraUsingMiddleMouseButton(){
        if(Input.GetMouseButtonDown(2)){
            touchStart = pointer;
        }
        
        if(Input.GetMouseButton(2)){
            Vector3 direction = touchStart - pointer;
            Camera.main.transform.position += direction;
            Camera.main.transform.position = new Vector3(Mathf.Clamp(Camera.main.transform.position.x,borderX*-1f, borderX), Mathf.Clamp(Camera.main.transform.position.y,borderY*-1f, borderY), -10f);
        }
    }

    void zoomUsingScroll(){
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - Input.GetAxis("Mouse ScrollWheel")*8f, zoomOutMin, zoomOutMax);
    }

    void blockAndUnblockPlacementDependingOnMouseButtonInputs(){
        if(((Input.GetMouseButton(0) && Input.GetMouseButtonDown(1)) || (Input.GetMouseButton(1) && Input.GetMouseButtonDown(0))) && !blockedPlacement || uiStats.isPaused){
            hexgridHandler.amountOfHoldAndDragHexes = 0;
            hexgridHandler.lastAmountOfHoldAndDragHexes = 0;
            
            blockedPlacement = true;
            closestAxis = 9;
            if(!uiStats.win && hexgridHandler.draggedSquareOrigin != "" && !uiStats.isPaused){soundHandler.playCancel();}
            hexgridHandler.draggedSquareOrigin = "";
            hexgridHandler.unhighlightNumbersAfterStoppingDragging();
        }
        else if(!Input.GetMouseButton(0) && !Input.GetMouseButton(1)){            
            blockedPlacement = false;
        }
    }

    void makeSelectedAmountNumberFollowMouse(){
        numberObject.SetActive((Input.GetMouseButton(0) || Input.GetMouseButton(1)) && !blockedPlacement && !uiStats.win && hexgridHandler.draggedSquareOrigin != "" && !uiStats.isPaused);
        numberText.text = hexgridHandler.amountOfHoldAndDragHexes.ToString();
        numberObject.transform.position = new Vector3(pointer.x,pointer.y, 1f);
    }

    Vector3 pointer;
	void Update(){
        pointer = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        calculateVariablesForDragSelectingUsindLeftMouseButton();
        if(!uiStats.isPaused){
            panCameraUsingMiddleMouseButton();
            zoomUsingScroll();
        }
        blockAndUnblockPlacementDependingOnMouseButtonInputs();
        makeSelectedAmountNumberFollowMouse();
        
	}


}
