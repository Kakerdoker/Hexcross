using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu_Number_Handler : MonoBehaviour
{
    public int X, Y;
    public string buttonType = "";
    public string buttonLetter = "0";
    public bool isButton = false;
    public TextMeshProUGUI textMesh;
    public Sound_Handler soundHandler;
    public Menu_Handler menuHandler;
    public Menu_Button_Handler menuButtonHandler;
    public Save_And_Load saveAndLoad;

    Color colorTurnedOff, colorSlightlyOn, colorFullyOn;

    void Start()
    {
        menuHandler = GameObject.Find("!SCRIPT HOLDER!").GetComponent<Menu_Handler>();
        soundHandler = GameObject.Find("!SCRIPT HOLDER!").GetComponent<Sound_Handler>();
        menuButtonHandler = GameObject.Find("!SCRIPT HOLDER!").GetComponent<Menu_Button_Handler>();
        saveAndLoad = GameObject.Find("!SCRIPT HOLDER!").GetComponent<Save_And_Load>();
    }

    float time = 0f, flickTime;
    bool isFlickering= false;

    void flicker(){
        if(isFlickering){
            time += Time.deltaTime;
            if(time > flickTime+0.05f){
                time = 0f;
            }
            else if(time > flickTime){
                textMesh.color = colorTurnedOff;
            }
        }
    }

    void Update(){
        flicker();
    }

    public void construct(int x, int y, float brightness, float flickTime){
        X = x; Y = y;
        setColors(brightness, flickTime);
    }

    public void clearItself(){
        buttonLetter = "0";
        buttonType = "";
        isButton = false;
    }    
    
    public void setColors(float brightness, float flickTime){
        colorTurnedOff = new Color(0f, 0.008f, 0.063f, 1f);
        colorSlightlyOn = new Color(0f, 0.015f*brightness, 0.102f*brightness, 1f);
        colorFullyOn = new Color(0, 0.11f*brightness, 0.855f*brightness, 1f);
        if(brightness < 0.63f){
            isFlickering = true;
            this.flickTime = flickTime;
        }
    }

    public void setButtonType(char letter, string type, bool isButton){ 
        buttonType = type;
        buttonLetter = letter.ToString();
        this.isButton = isButton;
        
    }

    public void revealLetter(){
        if(textMesh.text != "+" && time <= flickTime){
            if(buttonLetter != "0" || buttonType == "number"){
                textMesh.text = buttonLetter;
                textMesh.color = colorFullyOn; 
            }
            else{
                textMesh.color = colorSlightlyOn; 
            }
        }
    }

    public void hideLetter(){
        if(textMesh.text != "+" || buttonType == "number"){
            textMesh.text = "0";
            textMesh.color = colorTurnedOff;
        };
    }

    void OnRaycastMouseEnter(){
        menuHandler.x = X;
        menuHandler.y = Y;
        if(isButton){
            soundHandler.playHover();
        }
        textMesh.text = "+";
        textMesh.color = colorFullyOn;
    }

    void OnRaycastMouseExit(){
        textMesh.text = buttonLetter;
        if(buttonLetter == "0"){
            textMesh.color = colorSlightlyOn;
        }
    }

    void OnRaycastMouseDown(){
        if(isButton){
            menuButtonHandler.doAnActionDependingOnWhatButtonWasPressed(buttonType);
        }
    }
}
