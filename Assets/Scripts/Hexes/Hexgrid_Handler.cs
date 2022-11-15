using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Hexgrid_Handler : MonoBehaviour
{
    //cyan! Unity methods!
    //cyan! | | | | | | |
    //cyan! V V V V V V V

    public int hexSize, leftNumbers, topRightNumbers, bottomRightNumbers;

    private int percentOfTrueStates, newGame;

    private Pan_And_Zoom panAndZoom;
    private Sound_Handler soundHandler;
    private UI_Stats_Handler uiStats;
    private Save_And_Load saveAndLoad;
    private SteamManager steamManager;

    private GameObject hexParent;

    void getComponents(){
        panAndZoom = gameObject.GetComponent<Pan_And_Zoom>();
        soundHandler = gameObject.GetComponent<Sound_Handler>();
        uiStats = gameObject.GetComponent<UI_Stats_Handler>();
        saveAndLoad = gameObject.GetComponent<Save_And_Load>();
        steamManager = GameObject.Find("SteamManager").GetComponent<SteamManager>();
        hexParent = GameObject.Find("HexParent");
    }

    void setPlayerPrefs(){
        percentOfTrueStates = PlayerPrefs.GetInt("percentage");
        hexSize = PlayerPrefs.GetInt("size");
        newGame = PlayerPrefs.GetInt("newGame");
        leftNumbers = PlayerPrefs.GetInt("leftNumbers");
        topRightNumbers = PlayerPrefs.GetInt("topRightNumbers");
        bottomRightNumbers = PlayerPrefs.GetInt("bottomRightNumbers");  
    }

    void buildHexgrid(){
        placeHexagons();
        translateHexagons();
        placeNumbers();
    }

    void Start()
    {  
        getComponents();
        setPlayerPrefs();
        buildHexgrid();
        checkSideNumbersIfLoadedGame();
    }

    void resetSquareOriginOnIdleMouseButtons(){
        if(!Input.GetMouseButton(0) && !Input.GetMouseButton(1)){
            draggedSquareOrigin = "";
        }
    }

    void Update(){
        resetSquareOriginOnIdleMouseButtons();
    }

    //cyan! ^ ^ ^ ^ ^ ^ ^
    //cyan! | | | | | | |
    //cyan! Unity methods!
    //green! Miscelaneous methods.
    //green! | | | | | | | | | |
    //green! V V V V V V V V V V

    public bool isPerfect = true, anyUnknownHexes = false;
    void playConfirmSound(){
        if(anyUnknownHexes){
            if(isPerfect){
                soundHandler.playConfirmPerfect();
            }
            else{
                soundHandler.playConfirmError();
            }
        }
        isPerfect = true;
        anyUnknownHexes = false;
    }

    //green! ^ ^ ^ ^ ^ ^ ^ ^ ^ ^
    //green! | | | | | | | | | |
    //green! Miscelaneous methods.
    //orange! General methods. Used by multiple others or easily could be.
    //orange! | | | | | | | | | | | | | | | | | | | | | | | | | | | | | |
    //orange! V V V V V V V V V V V V V V V V V V V V V V V V V V V V V V

    int getXOfYPositionFromXAndY(int x, int y){
        return x < hexSize ? y : x+y+1-hexSize;
    }
    int getXOfZPositionFromXAndY(int x, int y){
        return x < hexSize ? hexSize - 1 + x - y : 2*(hexSize-1) - y;
    }

    bool notOutOfBounds(int num, int highest){
        return num < highest && num >= 0;
    }

    void changeTheNewHexByAVectorDependingOnIfItCrossedTheMiddleOfTheHexgridArray(){
        hexY += hexX < hexSize-1+hexDifference ? yVectorHexLarger : yVectorHexSmaller;
        hexX += hexX < hexSize-1+hexDifference ? xVectorHexLarger : xVectorHexSmaller;
    }

    //orange! ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^
    //orange! | | | | | | | | | | | | | | | | | | | | | | | | | | | | | |
    //orange! General methods. Used by multiple others or easily could be.
    //blue! Methods relating to the numbers on the sides of the hexogram.
    //blue! | | | | | | | | | | | | | | | | | | | | | | | | | | | | | | |
    //blue! V V V V V V V V V V V V V V V V V V V V V V V V V V V V V V V

    public GameObject numberObject, canvasObject;

    private GameObject numberPrefab;

    private List<List<TextMeshProUGUI>> rightNumbers2DList = new List<List<TextMeshProUGUI>>();
    private List<List<TextMeshProUGUI>> topNumbers2DList = new List<List<TextMeshProUGUI>>();
    private List<List<TextMeshProUGUI>> leftNumbers2DList = new List<List<TextMeshProUGUI>>();

    private int streak = 0;
    private float numPosX, numPosY;
    private bool previousHexState = false;
    
    void checkIfSideNumbersAreSolvedFromTheFront(List<List<bool>> hexGridStates, List<List<Hex_Handler>> hexHandlerArray, List<List<TextMeshProUGUI>> numbers2DList){
        streak = 0;
        previousHexState = false;
        for(int i = hexGridStates[x].Count-1; i >= 0 ; i--){
            if(hexHandlerArray[x][i].isStateKnown){
                if(i == 0 && hexGridStates[x][i]){                        
                    numbers2DList[x][streak].color = new Color32(255, 255, 255, 40);
                }
                else if(hexGridStates[x][i]){
                    previousHexState = true;
                }
                else{
                    if(previousHexState){
                    numbers2DList[x][streak].color = new Color32(255, 255, 255, 40);
                    streak++;
                }
                previousHexState = false;
                }
            }
            else{break;}
        }
    }

    void checkIfSideNumbersAreSolvedFromTheBack(List<List<bool>> hexGridStates, List<List<Hex_Handler>> hexHandlerArray, List<List<TextMeshProUGUI>> numbers2DList){
        streak = 0;
        previousHexState = false;
        for(int i = 0; i < hexGridStates[x].Count; i++){
            if(hexHandlerArray[x][i].isStateKnown){
                if(i == hexGridStates[x].Count-1 && hexGridStates[x][i]){                        
                    numbers2DList[x][streak].color = new Color32(255, 255, 255, 40);
                }
                else if(hexGridStates[x][i]){
                    previousHexState = true;
                }
                else{
                    if(previousHexState){
                        numbers2DList[x][numbers2DList[x].Count - streak-1].color = new Color32(255, 255, 255, 40);
                        streak++;
                    }
                    previousHexState = false;
                }
            }
            else{break;}
        }
    }

    void checkIfNumbersInXLineAreSolved(){
        x = hexX;
        if(notOutOfBounds(x, hexGridStatesX.Count)){
            checkIfSideNumbersAreSolvedFromTheFront(hexGridStatesX, hexHandlerArrayX, leftNumbers2DList);
            checkIfSideNumbersAreSolvedFromTheBack(hexGridStatesX, hexHandlerArrayX, leftNumbers2DList);
        }
    }

    void checkIfNumbersInYLineAreSolved(){
        x = getXOfYPositionFromXAndY(hexX, hexY);
        if(notOutOfBounds(x, hexGridStatesY.Count)){
            checkIfSideNumbersAreSolvedFromTheFront(hexGridStatesY, hexHandlerArrayY, topNumbers2DList);
            checkIfSideNumbersAreSolvedFromTheBack(hexGridStatesY, hexHandlerArrayY, topNumbers2DList);
        }
    }

    void checkIfNumbersInZLineAreSolved(){
        x = getXOfZPositionFromXAndY(hexX, hexY);
        if(notOutOfBounds(x, hexGridStatesZ.Count)){
            checkIfSideNumbersAreSolvedFromTheFront(hexGridStatesZ, hexHandlerArrayZ, rightNumbers2DList);
            checkIfSideNumbersAreSolvedFromTheBack(hexGridStatesZ, hexHandlerArrayZ, rightNumbers2DList);
        }
    }

    void checkIfSideNumbersAreSolvedInThreeXLines(){
        if(leftNumbers == 1){checkIfNumbersInXLineAreSolved();}
        if(topRightNumbers == 1){checkIfNumbersInYLineAreSolved();}
        if(bottomRightNumbers == 1){checkIfNumbersInZLineAreSolved();}
    }

    void checkSideNumbersIfLoadedGame(){
        if(newGame == 0){
            for(int i = 0; i < hexGridStatesX[0].Count; i++){//top line of numbers
                hexX = 0; hexY = i;
                checkIfSideNumbersAreSolvedInThreeXLines();
            }
            for(int i = 0; i < hexGridStatesX[hexGridStatesX.Count-1].Count; i++){//bottom line of numbers
                hexX = hexGridStatesX.Count-1; hexY = i;
                checkIfSideNumbersAreSolvedInThreeXLines();
            }
            for(int i = 0; i < hexGridStatesX.Count; i++){//left line of numbers
                hexX = i; hexY = 0;
                checkIfSideNumbersAreSolvedInThreeXLines();
            }
        }
    }

    void movePositionBeforeAndAfterLeftSideNumberIsPlaced(){
        numPosX += (streak >= 10 ? -0.369f : -0.228f)-0.30f;
        numberPrefab.transform.position = new Vector3(numPosX, numPosY, 0);
        numPosX += streak >= 10 ? -0.1f : 0;
    }

    void placeLeftSideNumberOnScene(int x){
        setNumberGameobjectProperties();
        movePositionBeforeAndAfterLeftSideNumberIsPlaced();
        leftNumbers2DList[x].Add(numberPrefab.GetComponent<TextMeshProUGUI>());
        streak = 0;
    }


    void movePositionBeforeTopSideNumberIsPlaced(){
        numPosX += 0.28f; //27
        numPosY += 0.44f; //40
        numberPrefab.transform.position = new Vector3(numPosX, numPosY, 0);
    }

    void placeTopSideNumberOnScene(int x){
        setNumberGameobjectProperties();
        movePositionBeforeTopSideNumberIsPlaced();
        topNumbers2DList[x].Add(numberPrefab.GetComponent<TextMeshProUGUI>());
        streak = 0;
    }

    void setNumberGameobjectProperties(){
        numberPrefab = Instantiate(numberObject);
        numberPrefab.name = "num|"+numPosX+"|"+numPosY;
        numberPrefab.GetComponent<TextMeshProUGUI>().text = streak.ToString();
        numberPrefab.transform.SetParent(canvasObject.transform);
    }

    void movePositionAfterRightSideNumberIsPlaced(){
        numberPrefab.transform.position = new Vector3(numPosX, numPosY, 0);
        numPosX += 0.28f;
        numPosY -= 0.44f;
    }

    void placeRightSideNumberOnScene(int x){
        setNumberGameobjectProperties();
        movePositionAfterRightSideNumberIsPlaced();
        rightNumbers2DList[x].Add(numberPrefab.GetComponent<TextMeshProUGUI>());
        streak = 0;
    }

    void changeRightSideNumberPositionAfterNewLine(int i){
        if(i < hexSize){
            numPosX = (hexSize*1f)-((i+1)*0.5f)-0.1f;
            numPosY = -0.78f-(i*0.76f);
        }
        else{
            numPosX = (hexSize*1f)-(hexSize*0.5f)-((hexSize-1-i)*-1f);
            numPosY = (hexSize*-0.76f);
        }
    }

   

    void fillOutNumbersOnRightSide(){
        for(int i = 0; i < hexGridStatesZ.Count; i++){
            
            rightNumbers2DList.Add(new List<TextMeshProUGUI>());
            changeRightSideNumberPositionAfterNewLine(i);
            for(int j = hexGridStatesZ[i].Count-1; j >= 0  ; j--){ 
                if(hexGridStatesZ[i][j] == true){
                    streak++;
                }
                else if(streak != 0){
                    placeRightSideNumberOnScene(i);
                }
            }
            if(streak != 0){
                placeRightSideNumberOnScene(i);
            }
        }
    }

    void changeTopSideNumberPositionAfterNewLine(int i){
        if(i < hexSize){
            numPosX = startingPlaceX+(i*1f)+1.07f;
            numPosY = startingPlaceY+0.13f;
        }
        else{
            numPosX = startingPlaceX+((i-hexSize-1)*0.5f)+(hexSize*1f)+1.05f;
            numPosY = startingPlaceY-0.62f+(i-hexSize)*-0.76f;
        }
    }    

    void fillOutNumbersOnTopSide(){
        for(int i = 0; i < hexGridStatesY.Count; i++){

            topNumbers2DList.Add(new List<TextMeshProUGUI>());

            changeTopSideNumberPositionAfterNewLine(i);
            for(int j = hexGridStatesY[i].Count-1; j >= 0  ; j--){ 
                if(hexGridStatesY[i][j] == true){
                    streak++;
                }
                else if(streak != 0){
                    placeTopSideNumberOnScene(i);
                }
            }
            if(streak != 0){
                placeTopSideNumberOnScene(i);
            }
        }
    }

    void changeLeftSideNumberPositionAfterNewLine(int i){
        if(i < hexSize){
            numPosX = startingPlaceX+(i*-0.5f)+0.8f;
            numPosY = startingPlaceY-(0.76f*i)-0.13f;
        }
        else{
            numPosX = startingPlaceX+((i-hexSize+1)*0.5f)+0.8f-(hexSize-1)*0.5f;
            numPosY = startingPlaceY-(0.76f*i)-0.13f;
        }
    }   

    void fillOutNumbersOnLeftSide(){
        for(int i = 0; i < hexGridStatesX.Count; i++){

            leftNumbers2DList.Add(new List<TextMeshProUGUI>());
            changeLeftSideNumberPositionAfterNewLine(i);
            
            for(int j = hexGridStatesX[i].Count-1; j >= 0  ; j--){
                if(hexGridStatesX[i][j] == true){
                    streak++;
                }
                else if(streak != 0){
                    placeLeftSideNumberOnScene(i);
                }
            }
            if(streak != 0){
                placeLeftSideNumberOnScene(i);
            }
        }
    }

    void placeNumbers(){
        if(leftNumbers == 1){fillOutNumbersOnLeftSide();}
        if(topRightNumbers == 1){fillOutNumbersOnTopSide();}
        if(bottomRightNumbers == 1){fillOutNumbersOnRightSide();}
    }

    public TMP_FontAsset normalFont, higlightedFont;

    void highlightANumber(TextMeshProUGUI number){
        if(number.color == new Color32(255,255,255,150)){
            number.color = new Color32(255,255,255,255); 
        }
        if(number.color == new Color32(255, 255, 255, 40)){
            number.color = new Color32(255,255,255, 60);
        }
        number.font = higlightedFont;
        number.fontSize = 0.58f;
    }

    void unhighlightANumber(TextMeshProUGUI number){
        if(number.color == new Color32(255,255,255,255)){
            number.color = new Color32(255,255,255,150);
        }
        if(number.color == new Color32(255,255,255, 60)){
            number.color = new Color32(255,255,255, 40);
        }
        number.font = normalFont;
        number.fontSize = 0.40f;
    }

    void highlightAGuidingLineOfNumbers(int columnOfNumbers, List<List<TextMeshProUGUI>> numbers2DList){
        if(notOutOfBounds(columnOfNumbers, numbers2DList.Count)){
            for(int i = 0; i < numbers2DList[columnOfNumbers].Count; i++){
                highlightANumber(numbers2DList[columnOfNumbers][i]);
            }
        }
    }

    void unhighlightAGuidingLineOfNumbers(int columnOfNumbers, List<List<TextMeshProUGUI>> numbers2DList){
        if(notOutOfBounds(columnOfNumbers, numbers2DList.Count)){
            for(int i = 0; i < numbers2DList[columnOfNumbers].Count; i++){
                unhighlightANumber(numbers2DList[columnOfNumbers][i]);
            }
        }
    }

    public void unhighlightNumbersAfterStoppingDragging(){
        unhighlightAGuidingLineOfNumbers(highlightedX, leftNumbers2DList);
        unhighlightAGuidingLineOfNumbers(getXOfYPositionFromXAndY(highlightedX, highlightedY), topNumbers2DList);
        unhighlightAGuidingLineOfNumbers(getXOfZPositionFromXAndY(highlightedX, highlightedY), rightNumbers2DList);
        highlightedX = -1; highlightedY = -1;
    }

    void unhighlightNumbersInPreviousPosition(){
        if(!Input.GetMouseButton(0) && !Input.GetMouseButton(1)){
            unhighlightAGuidingLineOfNumbers(hexX, leftNumbers2DList);
            unhighlightAGuidingLineOfNumbers(getXOfYPositionFromXAndY(hexX, hexY), topNumbers2DList);
            unhighlightAGuidingLineOfNumbers(getXOfZPositionFromXAndY(hexX, hexY), rightNumbers2DList);
        }
    }

    void highlightThreeGuidingLinesOfNumbers(int x, int y){
        highlightAGuidingLineOfNumbers(x, leftNumbers2DList);
        highlightAGuidingLineOfNumbers(getXOfYPositionFromXAndY(x, y),topNumbers2DList);
        highlightAGuidingLineOfNumbers(getXOfZPositionFromXAndY(x, y),rightNumbers2DList);
    }

    //blue! ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^
    //blue! | | | | | | | | | | | | | | | | | | | | | | | | | | | | | | |
    //blue! Methods relating to the numbers on the sides of the hexogram.
    //red! Methods interacting with hexes.
    //red! | | | | | | | | | | | | | | |
    //red! V V V V V V V V V V V V V V V

    public string draggedSquareOrigin = "";

    public int hexX, hexY, highlightedX = -1, highlightedY = -1, originX, originY;

    public bool mouseOverAnyHex = false;

    private int lastClosestAxis=0, startingDraggingHexX = 0, startingDraggingHexY = 0, previousStartingDraggingHexX, previousStartingDraggingHexY, yVectorHexSmaller, yVectorHexLarger, xVectorHexSmaller, xVectorHexLarger, hexDifference, x;


    bool checkIfHandlerArrayIsNotOutOfBounds(){
        if(hexHandlerArrayX.Count <= hexX || hexX < 0){
            return false;
        }
        else if(hexHandlerArrayX[hexX].Count <= hexY || hexY < 0){
            return false;
        }
        return true;
    }
    void changeHexStateDependingOnMouseUpInput(){
        if(Input.GetMouseButtonUp(0)){
            hexHandlerArrayX[hexX][hexY].guessHexIsTrue();
            steamManager.addStat("hexes", 1);
        }
        else if(Input.GetMouseButtonUp(1)){
            hexHandlerArrayX[hexX][hexY].guessHexIsFalse();
            steamManager.addStat("hexes", 1);
        }
        else{
            hexHandlerArrayX[hexX][hexY].SetSpriteHoldAndDrag();
        }
    }

    public int amountOfHoldAndDragHexes = 0, lastAmountOfHoldAndDragHexes = 0;

    void makeSureAmountDoesntGoBelowOneAndPlaySoundOnChange(){
        if(lastAmountOfHoldAndDragHexes < 1) lastAmountOfHoldAndDragHexes = 1;
        if(amountOfHoldAndDragHexes < 1) amountOfHoldAndDragHexes = 1;

        if(lastAmountOfHoldAndDragHexes != amountOfHoldAndDragHexes){soundHandler.playHoverButNotIfYouWon();}
        lastAmountOfHoldAndDragHexes = amountOfHoldAndDragHexes;
    }

    void selectInALineANumberOfHexesFromAStartingPosition(){
        amountOfHoldAndDragHexes = 0;
        while(amountOfHoldAndDragHexes < panAndZoom.howManyHexesSelected && checkIfHandlerArrayIsNotOutOfBounds()){
            changeHexStateDependingOnMouseUpInput();
            if(Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) && !panAndZoom.blockedPlacement){checkIfSideNumbersAreSolvedInThreeXLines();}
            changeTheNewHexByAVectorDependingOnIfItCrossedTheMiddleOfTheHexgridArray();  
            amountOfHoldAndDragHexes++;
        }
        makeSureAmountDoesntGoBelowOneAndPlaySoundOnChange();
        playConfirmSound();
        saveAndLoad.saveGame();
    }

    void drawLine(Action doSomethingToHoverLight, int startingDraggingHexX, int startingDraggingHexY){
        hexX = startingDraggingHexX; hexY = startingDraggingHexY;
        while(checkIfHandlerArrayIsNotOutOfBounds()){
            doSomethingToHoverLight();
            changeTheNewHexByAVectorDependingOnIfItCrossedTheMiddleOfTheHexgridArray();
        }
    }

    public void UndrawThreePreviousGuidingLines(){
        previousStartingDraggingHexX = startingDraggingHexX; previousStartingDraggingHexY = startingDraggingHexY;
        hexX = previousStartingDraggingHexX; hexY = previousStartingDraggingHexY;

        unhighlightNumbersInPreviousPosition();

        changeHexVectorProperties(1,0,-1,-1,1);
        drawLine(() => hexHandlerArrayX[hexX][hexY].disableHoverLight(), previousStartingDraggingHexX, previousStartingDraggingHexY);

        changeHexVectorProperties(0,1,1,1,0);
        drawLine(() => hexHandlerArrayX[hexX][hexY].disableHoverLight(), previousStartingDraggingHexX, previousStartingDraggingHexY);

        changeHexVectorProperties(-1,-1,0,0,0);
        drawLine(() => hexHandlerArrayX[hexX][hexY].disableHoverLight(), previousStartingDraggingHexX, previousStartingDraggingHexY);
    }

    public void DrawThreeGuidingLines(){
        if(!uiStats.win){
            startingDraggingHexX = hexX; startingDraggingHexY = hexY;

            highlightThreeGuidingLinesOfNumbers(hexX, hexY);

            changeHexVectorProperties(1,0,-1,-1,1);
            drawLine(() => hexHandlerArrayX[hexX][hexY].enableHoverLight(), startingDraggingHexX, startingDraggingHexY);
            changeHexVectorProperties(0,1,1,1,0);
            drawLine(() => hexHandlerArrayX[hexX][hexY].enableHoverLight(), startingDraggingHexX, startingDraggingHexY);
            changeHexVectorProperties(-1,-1,0,0,0);
            drawLine(() => hexHandlerArrayX[hexX][hexY].enableHoverLight(), startingDraggingHexX, startingDraggingHexY);
        }
    }
    

    void getRidOfTheBugThatMakesTheStartingHexRemainInDragAndHoldSprite(){
        if(Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)){
            hexHandlerArrayX[originX][originY].SetSpriteMaybe();
        }
    }

    void deselectInALineTheRestOfHexesUntilOutOfBounds(){
        while(checkIfHandlerArrayIsNotOutOfBounds()){
            if(lastClosestAxis!=panAndZoom.closestAxis){
                changeTheNewHexByAVectorDependingOnIfItCrossedTheMiddleOfTheHexgridArray();
            }
            if(checkIfHandlerArrayIsNotOutOfBounds()){            
                hexHandlerArrayX[hexX][hexY].SetSpriteMaybe();
            }
            if(lastClosestAxis==panAndZoom.closestAxis){
                changeTheNewHexByAVectorDependingOnIfItCrossedTheMiddleOfTheHexgridArray();
            }
        }
        getRidOfTheBugThatMakesTheStartingHexRemainInDragAndHoldSprite();
    }

    void changeHexVectorProperties(int yVectorHexSmaller, int yVectorHexLarger, int xVectorHexSmaller, int xVectorHexLarger, int hexDifference){
        this.yVectorHexSmaller = yVectorHexSmaller;
        this.yVectorHexLarger = yVectorHexLarger;
        this.xVectorHexSmaller = xVectorHexSmaller;
        this.xVectorHexLarger = xVectorHexLarger;
        this.hexDifference = hexDifference;
    }

    void selectHexesDependingOnClosestAxisForLineLengthAndThenDeselectTheRestOfTheLine(){
        switch(panAndZoom.closestAxis){
            case 0:
                changeHexVectorProperties(1,0,-1,-1,1);
            break;
            case 1:
                changeHexVectorProperties(1,1,0,0,0);
            break;
            case 2:
                changeHexVectorProperties(0,1,1,1,0);
            break;
            case 3:
                changeHexVectorProperties(-1,0,1,1,0);
            break;
            case 4:
                changeHexVectorProperties(-1,-1,0,0,0);
            break;
            case 5:
                changeHexVectorProperties(0,-1,-1,-1,1);
            break; 
        }
        if(!uiStats.isPaused){selectInALineANumberOfHexesFromAStartingPosition();}
        deselectInALineTheRestOfHexesUntilOutOfBounds();
    }

    public void deselectEntireLineOnPreviousAxisIfAxisWasChanged(){
        if(lastClosestAxis!=panAndZoom.closestAxis){
            switch(lastClosestAxis){
                case 0:
                    changeHexVectorProperties(1,0,-1,-1,1);
                break;
                case 1:
                    changeHexVectorProperties(1,1,0,0,0);
                break;
                case 2:
                    changeHexVectorProperties(0,1,1,1,0);
                break;
                case 3:
                    changeHexVectorProperties(-1,0,1,1,0);
                break;
                case 4:
                    changeHexVectorProperties(-1,-1,0,0,0);
                break;
                case 5:
                    changeHexVectorProperties(0,-1,-1,-1,1);
                break;
            }  
            deselectInALineTheRestOfHexesUntilOutOfBounds();  

        }
    }

    public void selectDraggedOverHexes(){
        deselectEntireLineOnPreviousAxisIfAxisWasChanged();
        selectHexesDependingOnClosestAxisForLineLengthAndThenDeselectTheRestOfTheLine();
        lastClosestAxis = panAndZoom.closestAxis;
    }

    public bool checkIfHexIsTrueOrFalse(int x, int y, bool leftClickClicked){
        if(hexGridStatesX[x][y] == leftClickClicked){
            return true;
        }
        else{
            return false;
        }
    }

    //red! ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^
    //red! | | | | | | | | | | | | | | |
    //red! Methods interacting with hexes.
    //pink! Methods about creating the hexes.
    //pink! | | | | | | | | | | | | | | | |
    //pink! V V V V V V V V V V V V V V V V

    public List<List<Hex_Handler>> hexHandlerArrayX = new List<List<Hex_Handler>>();
    public List<List<Hex_Handler>> hexHandlerArrayY = new List<List<Hex_Handler>>();
    public List<List<Hex_Handler>> hexHandlerArrayZ = new List<List<Hex_Handler>>();

    public List<List<bool>> hexGridStatesX = new List<List<bool>>();
    public List<List<bool>> hexGridStatesY = new List<List<bool>>();
    public List<List<bool>> hexGridStatesZ = new List<List<bool>>();

    public GameObject hexObject, hoverLightObject;

    private System.Random random = new System.Random(Environment.TickCount);
    private List<List<Save_And_Load.HexProperties>> hexPropertiesArray = new List<List<Save_And_Load.HexProperties>>();
   
    private float startingPlaceX, startingPlaceY;
    private bool hexState;

    void translateTopRightHexStatesFromYtoZ(){

        for(int i = 0; i < hexSize; i++){
            hexGridStatesZ.Add(new List<bool>());
            for(int j = 0;j < hexSize+1+i; j++){
                hexGridStatesZ[i].Add(hexGridStatesY[2*hexSize-1-j][j > hexSize ? i-j+hexSize : i]);
            }
        }
    }

    void translateBottomLeftHexStatesFromYtoZ(){

        for(int i = 0; i < hexSize; i++){
            hexGridStatesZ.Add(new List<bool>());
            for(int j = 0; j < (hexSize*2)-i; j++){
                hexGridStatesZ[hexSize+i].Add(hexGridStatesY[(hexSize*2)-i-j-1][j>hexSize-i ? (2*hexSize)-j : hexSize +i]);
            }
        }
    }

    void translateTopLeftHexStatesFromYtoZ(){
        for(int i = 0; i < hexSize; i++){
            hexGridStatesZ.Add(new List<bool>());
            hexHandlerArrayZ.Add(new List<Hex_Handler>());
            for(int j = 0; j < hexSize+i; j++){
                int x = hexSize*2-2-j;
                int y = j >= hexSize ? i-j+hexSize-1 : i;
                hexGridStatesZ[i].Add(hexGridStatesY[x][y]);
                hexHandlerArrayZ[i].Add(hexHandlerArrayY[x][y]);
            }
        }
    }

    void translateBottomRightHexStatesFromYtoZ(){
        for(int i = 0; i < hexSize-1; i++){
            hexGridStatesZ.Add(new List<bool>());
            hexHandlerArrayZ.Add(new List<Hex_Handler>());
            for(int j = 0; j < hexSize*2-2-i; j++){
                int x = hexSize*2-3-i-j;
                int y = j >= hexSize-1-i ? hexSize+hexSize-2-j : i+hexSize;
                hexGridStatesZ[i+hexSize].Add(hexGridStatesY[x][y]);
                hexHandlerArrayZ[i+hexSize].Add(hexHandlerArrayY[x][y]);
            }
        }     
    }

    void translateHexStatesFromYtoZ(){
        translateTopLeftHexStatesFromYtoZ();
        translateBottomRightHexStatesFromYtoZ();
    }

    void translateTopLeftHexStatesFromXtoY(){

        for(int i = 0; i < hexSize; i++){
            hexGridStatesY.Add(new List<bool>());
            hexHandlerArrayY.Add(new List<Hex_Handler>());
            for(int j = 0; j < hexSize+i; j++){
                int y = j >= hexSize ? hexSize+i-j-1 : i;
                int x = j;
                hexGridStatesY[i].Add(hexGridStatesX[x][y]);
                hexHandlerArrayY[i].Add(hexHandlerArrayX[x][y]);
            }
        }
    }

    void translateBottomRightHexStatesFromXtoY(){
        
        for(int i = 0; i < hexSize-1; i++){
            hexGridStatesY.Add(new List<bool>());
            hexHandlerArrayY.Add(new List<Hex_Handler>());
            for(int j = 0; j < (hexSize*2)-2-i; j++){

                int y = j >= hexSize-i-1 ? hexSize+hexSize-j-2 : hexSize + i;
                int x = j+i+1;
                hexGridStatesY[hexSize+i].Add(hexGridStatesX[x][y]);
                hexHandlerArrayY[hexSize+i].Add(hexHandlerArrayX[x][y]);
            }
        }        
    }

    void translateHexStatesFromXtoY(){
        translateTopLeftHexStatesFromXtoY();
        translateBottomRightHexStatesFromXtoY();
    }


    void placeNewHexOnScene(Vector3 hexPosition, int x, int y){
        GameObject hexPrefab = Instantiate(hexObject);
        hexPrefab.name = "hex-"+x+"-"+y;
        hexPrefab.transform.position = hexPosition;
        hexPrefab.transform.parent = hexParent.transform;

        hexHandlerArrayX[x].Add(hexPrefab.GetComponent<Hex_Handler>());

        hexHandlerArrayX[x][y].X = x;
        hexHandlerArrayX[x][y].Y = y;
        hexHandlerArrayX[x][y].hexState = hexState;

        GameObject hoverLightPrefab = Instantiate(hoverLightObject);        
        hoverLightPrefab.transform.position = hexPosition;
        hoverLightPrefab.transform.SetParent(hexPrefab.transform);
        hoverLightPrefab.SetActive(false);
    }


    void randomizeHexState(){
        hexState = random.Next(100) < percentOfTrueStates;
    }

    void translateHexagons(){
        translateHexStatesFromXtoY();
        translateHexStatesFromYtoZ();
    }

    void createAndPlaceTopHalfOfHexagons(){
        for(int i = 0; i < hexSize-1; i++){
            hexGridStatesX.Add(new List<bool>());
            hexHandlerArrayX.Add(new List<Hex_Handler>());
            for(int j = 0; j < hexSize+i;j++){
                randomizeHexState();
                hexGridStatesX[i].Add(hexState);
                placeNewHexOnScene(new Vector3((j*2-i+2)*0.5f+startingPlaceX,i*-0.76f+startingPlaceY,0f), i, j);
            }
        }
    }

    void createAndPlaceMiddleLineOfHexagons(){
        hexGridStatesX.Add(new List<bool>());
        hexHandlerArrayX.Add(new List<Hex_Handler>());
        for(int i = 0; i < ((hexSize-1)*2)+1; i++){
            randomizeHexState();
            hexGridStatesX[hexSize-1].Add(hexState);
            placeNewHexOnScene(new Vector3((i*2-hexSize+3)*0.5f+startingPlaceX,(hexSize-1)*-0.76f+startingPlaceY,0f), hexSize-1, i);
        }
    }
    void createAndPlaceBottomHalfOfHexagons(){
        for(int i = 0; i < hexSize-1; i++){
            hexGridStatesX.Add(new List<bool>());
            hexHandlerArrayX.Add(new List<Hex_Handler>());
            for(int j = 0; j < (hexSize-1)*2-i;j++){
                randomizeHexState();
                hexGridStatesX[hexSize+i].Add(hexState);
                placeNewHexOnScene(new Vector3((i+j*2-hexSize+4)*0.5f+startingPlaceX,(hexSize+i)*-0.76f+startingPlaceY,0f), hexSize+i, j);
            }
        }
    }

    void placeReadHexOnScene(int x, int y){
        GameObject hexPrefab = Instantiate(hexObject);
        hexPrefab.name = "hex-"+x+"-"+y;
        hexPrefab.transform.position = hexPropertiesArray[x][y].position;
        hexPrefab.transform.parent = hexParent.transform;

        GameObject hoverLightPrefab = Instantiate(hoverLightObject);        
        hoverLightPrefab.transform.position = hexPropertiesArray[x][y].position;
        hoverLightPrefab.transform.SetParent(hexPrefab.transform);
        hoverLightPrefab.SetActive(false);

        hexHandlerArrayX[x].Add(hexPrefab.GetComponent<Hex_Handler>());
        hexHandlerArrayX[x][y].X = x;
        hexHandlerArrayX[x][y].Y = y;
        hexHandlerArrayX[x][y].hexState = hexPropertiesArray[x][y].hexState;
        hexHandlerArrayX[x][y].isStateKnown = hexPropertiesArray[x][y].isStateKnown;
        hexHandlerArrayX[x][y].setSprite(hexPropertiesArray[x][y].spriteNum);
    }

    void readAndPlaceHexesFromArray(){
        for(int i = 0; i < hexPropertiesArray.Count; i++){
            hexGridStatesX.Add(new List<bool>());
            hexHandlerArrayX.Add(new List<Hex_Handler>());
            for(int j = 0; j < hexPropertiesArray[i].Count; j++){
                hexGridStatesX[i].Add(hexPropertiesArray[i][j].state);
                placeReadHexOnScene(i, j);
            }
        }

    }

    void placeHexagons(){
        startingPlaceX = hexSize*-0.5f-0.5f;
        startingPlaceY = (hexSize-1)*0.76f;

        if(newGame == 1){
            createAndPlaceTopHalfOfHexagons();
            createAndPlaceMiddleLineOfHexagons();
            createAndPlaceBottomHalfOfHexagons();
        }
        else{
            hexPropertiesArray = saveAndLoad.getHexProperties();      
            readAndPlaceHexesFromArray();
        }
    }
}

    //pink! ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^
    //pink! | | | | | | | | | | | | | | | |
    //pink! Methods about creating the hexes.




