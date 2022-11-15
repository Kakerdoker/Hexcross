using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex_Handler : MonoBehaviour
{

    public int X;
    public int Y;
    public bool hexState;
    public bool isStateKnown;
    public int spriteNum = 2;
    Hexgrid_Handler hagrid;
    Pan_And_Zoom panAndZoom;
    UI_Stats_Handler uiStats;
    Sound_Handler soundHandler;
    GameObject lightHover;

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        hagrid = GameObject.Find("!SCRIPT HOLDER!").GetComponent<Hexgrid_Handler>();
        panAndZoom = GameObject.Find("!SCRIPT HOLDER!").GetComponent<Pan_And_Zoom>();
        uiStats = GameObject.Find("!SCRIPT HOLDER!").GetComponent<UI_Stats_Handler>();
        soundHandler = GameObject.Find("!SCRIPT HOLDER!").GetComponent<Sound_Handler>();
        lightHover = gameObject.transform.GetChild(0).gameObject;
    }

    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;

    public void setSprite(int spriteNum){
        this.spriteNum = spriteNum;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[spriteNum];
    }

    void makeGuessNotPerfect(){
        if(!isStateKnown){
            hagrid.isPerfect = false;
        }
    }

    void checkIfHexIsUnknown(){
        if(!isStateKnown){
            hagrid.anyUnknownHexes = true;
        }
    }

    public void guessHexIsTrue(){
        checkIfHexIsUnknown();
        if(hexState){
            SetSpriteYes();
        }
        else{
            makeGuessNotPerfect();
            SetSpriteNoIncorrect();
        }
    }
    public void guessHexIsFalse(){
        checkIfHexIsUnknown();
        if(!hexState){
            SetSpriteNo();
        }
        else{
            makeGuessNotPerfect();
            SetSpriteYesIncorrect();   
        }
    }

    public void SetSpriteYes(){
        if(!isStateKnown){
            spriteRenderer.sprite = sprites[0];
            spriteNum = 0;
            isStateKnown = true;
            uiStats.correctGuess();
        }  
    }

    public void SetSpriteYesIncorrect(){
        if(!isStateKnown){
            spriteRenderer.sprite = sprites[1];
            spriteNum = 1;
            isStateKnown = true;
            uiStats.wrongGuess();
        }  
    }
    
    public void SetSpriteMaybe(){
        if(!isStateKnown){
            spriteRenderer.sprite = sprites[2];
        }  
    }
    public void SetSpriteNo(){
        if(!isStateKnown){
            spriteRenderer.sprite = sprites[3];
            spriteNum = 3;
            isStateKnown = true;
            uiStats.correctGuess();
        }  
    }
    public void SetSpriteNoIncorrect(){
        if(!isStateKnown){
            spriteRenderer.sprite = sprites[4];
            spriteNum = 4;
            isStateKnown = true;
            uiStats.wrongGuess();
        }  
    }
    public void SetSpriteHoldAndDrag(){
        if(!isStateKnown && spriteRenderer.sprite == sprites[2] && !uiStats.isPaused){
            spriteRenderer.sprite = sprites[6];
        }  
    }
    public void enableHoverLight(){
        lightHover.SetActive(true);
    }
    public void disableHoverLight(){
        lightHover.SetActive(false);
    }    

    void ifBlockedClearLineIfNotSelectHexes(){
        hagrid.hexX = hagrid.highlightedX = X; hagrid.hexY = hagrid.highlightedY = Y;

        if(panAndZoom.blockedPlacement){
            hagrid.deselectEntireLineOnPreviousAxisIfAxisWasChanged();
            SetSpriteMaybe();
        }
        else if(hagrid.draggedSquareOrigin == "" || hagrid.draggedSquareOrigin == gameObject.name){
            hagrid.selectDraggedOverHexes();
            hagrid.draggedSquareOrigin = gameObject.name;
        }
    }

    void OnMouseOver(){
        hagrid.mouseOverAnyHex = true;

        if(!Input.GetMouseButton(0) && !Input.GetMouseButton(1)){
            panAndZoom.hexPosition = gameObject.transform.position;

            hagrid.UndrawThreePreviousGuidingLines();
            hagrid.hexX = X; hagrid.hexY = Y;

            if(!uiStats.isPaused){hagrid.DrawThreeGuidingLines();}
        }
    }

    void OnMouseDown(){
        if(!uiStats.isPaused){soundHandler.playHoverButNotIfYouWon();}
        hagrid.originX = X;
        hagrid.originY = Y;
    }

    void OnRightMouseDown(){
        if(!uiStats.isPaused){soundHandler.playHoverButNotIfYouWon();}
        hagrid.originX = X;
        hagrid.originY = Y;
    }
    
    void OnMouseExit(){
        hagrid.mouseOverAnyHex = false;
        panAndZoom.checkClosestAxisAndHowManyHexesAreSelectedOnThatAxis();
        hagrid.UndrawThreePreviousGuidingLines();
    }

    void OnMouseUp(){
        ifBlockedClearLineIfNotSelectHexes();
    }

    void OnMouseDrag(){
        ifBlockedClearLineIfNotSelectHexes();
    }

    void OnRightMouseDrag(){
        ifBlockedClearLineIfNotSelectHexes();
    }

    void OnRightMouseUp(){
        ifBlockedClearLineIfNotSelectHexes();
    }
}
