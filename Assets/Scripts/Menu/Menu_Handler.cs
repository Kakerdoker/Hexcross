using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Menu_Handler : MonoBehaviour
{

    public void createMenuButtonOrSentence(int x, int y, string buttonName, bool isButton){
        for(int i = 0; i < buttonName.Length; i++){
            numberArray[x+i][y].setButtonType(buttonName[i],buttonName, isButton);
        }       
    }
    public void createMenuButtonOrSentence(int x, int y, string buttonName, bool isButton, string diffButtonName){
        for(int i = 0; i < buttonName.Length; i++){
            numberArray[x+i][y].setButtonType(buttonName[i],diffButtonName, isButton);
        }       
    }

    public void clearAllButtons(){
        for(int i = 0; i < 33; i++){
            for(int j = 0; j < 11; j++){
                numberArray[i][j].clearItself();
            }
        }
    }

    public void enableAllNumberBoxColliders(int isEnabled){
        for(int i = 0; i < 33; i++){
            for(int j = 0; j < 11; j++){
                numberBoxColliderArray[i][j].size = new Vector2(0.68f*isEnabled, 0.85f);
            }
        }
    }

    void Start()
    {
        setNumbers();
    }

    float widthLength;
    float heightLength;
    float pytaGorasa;
    int previousX = 0, previousY = 0;

    void hideNumbersInPreviousCircle(){
        for(int i = 0; i < 33; i++){
            for(int j = 0; j < 11; j++){
                widthLength = Mathf.Abs(i - previousX)*0.5365f;
                heightLength = Mathf.Abs(j - previousY)*0.92f;
                pytaGorasa = Mathf.Sqrt(widthLength*widthLength + heightLength*heightLength);
                if(pytaGorasa < 5.5f){
                    numberArray[i][j].hideLetter();
                }
            }
        }
    }

    void revealNumbersInACircle(){
        for(int i = 0; i < 33; i++){
            for(int j = 0; j < 11; j++){
                widthLength = Mathf.Abs(i - x)*0.5365f;
                heightLength = Mathf.Abs(j - y)*0.92f;
                pytaGorasa = Mathf.Sqrt(widthLength*widthLength + heightLength*heightLength);
                if(pytaGorasa < 5.5f){
                    numberArray[i][j].revealLetter();
                }
            }
        }
    }

    public int x = 0, y = 0;
    public void drawCircle(){
        hideNumbersInPreviousCircle();
        revealNumbersInACircle();
        previousX = x; previousY = y;
    }
    
    System.Random random = new System.Random(Environment.TickCount);

    void placeNumberPrefab(int x, int y){
        numberPrefab = Instantiate(numberObject);
        numberPrefab.name = "number-"+x.ToString()+"-"+y.ToString();
        numberPrefab.transform.position = new Vector2(-8.58f+(x*0.5365f),4.60f-(y*0.92f));
        numberPrefab.transform.SetParent(canvasObject.transform);

        numberArray[x].Add(numberPrefab.GetComponent<Menu_Number_Handler>());
        numberArray[x][y].construct(x,y,(0.6f+((float)random.NextDouble()*0.4f))%1f, 3f+(float)random.NextDouble()*3f);

        numberBoxColliderArray[x].Add(numberPrefab.GetComponent<BoxCollider2D>());
    }

    public GameObject numberObject;
    public GameObject canvasObject;
    GameObject numberPrefab;
    List<List<Menu_Number_Handler>> numberArray = new List<List<Menu_Number_Handler>>();
    List<List<BoxCollider2D>> numberBoxColliderArray = new List<List<BoxCollider2D>>();

    protected void setNumbers(){

        for(int i = 0; i < 33; i++){
            numberArray.Add(new List<Menu_Number_Handler>());
            numberBoxColliderArray.Add(new List<BoxCollider2D>());
            for(int j = 0; j < 11; j++){
                placeNumberPrefab(i, j);
            }
        }

    }

    GameObject previousNumber;
    public int layerMask = 1;

    void drawRayFromMouseAndSendOnMouseMessagesToTouchedGameObjects(){ //because you need to write unity function clones if you want 1 thing differently...
        GameObject numberGameObject;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity,layerMask);
        
        if(hit.collider != null)
        {
            numberGameObject = hit.collider.gameObject;

            if(Input.GetMouseButtonDown(0)){
                numberGameObject.SendMessage("OnRaycastMouseDown",SendMessageOptions.DontRequireReceiver);
            }
            if(previousNumber != null && previousNumber.name != numberGameObject.name){
                previousNumber.SendMessage("OnRaycastMouseExit",SendMessageOptions.DontRequireReceiver);
                numberGameObject.SendMessage("OnRaycastMouseEnter",SendMessageOptions.DontRequireReceiver);
            }
            previousNumber = numberGameObject;
        }
    }

    void Update()
    {
        drawCircle();
        drawRayFromMouseAndSendOnMouseMessagesToTouchedGameObjects();
    }

}
