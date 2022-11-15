using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Button_Handler : MonoBehaviour
{
    Menu_Handler menuHandler;
    Menu_Animation_Handler menuAnimation;
    Sound_Handler soundHandler;
    Save_And_Load saveAndLoad;
    Camera_Aspect camera;
    SteamManager steamManager;

    void Start(){
        

        menuAnimation = GameObject.Find("tutorial").GetComponent<Menu_Animation_Handler>();
        menuHandler = gameObject.GetComponent<Menu_Handler>();
        soundHandler = gameObject.GetComponent<Sound_Handler>();
        saveAndLoad = gameObject.GetComponent<Save_And_Load>();
        camera = gameObject.GetComponent<Camera_Aspect>();
        steamManager = GameObject.Find("SteamManager").GetComponent<SteamManager>();

        if(PlayerPrefs.GetInt("fristTimeCheck") != 1){
            
            PlayerPrefs.SetInt("fristTimeCheck", 1);
            PlayerPrefs.SetInt("sfxVolumePercent", 100);
            PlayerPrefs.SetInt("musicVolumePercent", 100);
            PlayerPrefs.SetInt("fpsLimit", 60);
        }
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = PlayerPrefs.GetInt("fpsLimit");

        makeFirstMenuButtons();
    }

    public void makeQuickGameButtons(){

        for(int i = 0; i < 8 ; i++){
            if(saveAndLoad.checkIfLevelHadPerfectScore(1+i)){menuHandler.createMenuButtonOrSentence(2,2+i,"+", false, "number");}
        }

        menuHandler.createMenuButtonOrSentence(4,2,"0>", false, "number");
        menuHandler.createMenuButtonOrSentence(4,3,"1>", false);
        menuHandler.createMenuButtonOrSentence(4,4,"2>", false);
        menuHandler.createMenuButtonOrSentence(4,5,"3>", false);
        menuHandler.createMenuButtonOrSentence(4,6,"4>", false);
        menuHandler.createMenuButtonOrSentence(4,7,"5>", false); 
        menuHandler.createMenuButtonOrSentence(4,8,"6>", false);  
        menuHandler.createMenuButtonOrSentence(4,9,"7>", false);

        menuHandler.createMenuButtonOrSentence(6,2,"BROWN0DWARF", true);
        menuHandler.createMenuButtonOrSentence(6,3,"WHITE0DWARF", true);
        menuHandler.createMenuButtonOrSentence(6,4,"RED0DWARF", true);
        menuHandler.createMenuButtonOrSentence(6,5,"BLUE0DWARF", true);
        menuHandler.createMenuButtonOrSentence(6,6,"RED0GIANT", true);
        menuHandler.createMenuButtonOrSentence(6,7,"BLUE0GIANT", true); 
        menuHandler.createMenuButtonOrSentence(6,8,"SUPERGIANT", true);  
        menuHandler.createMenuButtonOrSentence(6,9,"HYPERGIANT", true);
        menuHandler.createMenuButtonOrSentence(28-7,9,"GO0BACK", true);
        scrollTitle ="PLAYGAME-";
    }

    public void makeCustomGameButtons(){
        menuHandler.createMenuButtonOrSentence(2,2,"SIZE", false);

        menuHandler.createMenuButtonOrSentence(7,3,"005", false, "number");
        menuHandler.createMenuButtonOrSentence(5,3,"<", true, "-size");
        menuHandler.createMenuButtonOrSentence(2,3,"<<", true, "--size");
        menuHandler.createMenuButtonOrSentence(11,3,">", true, "+size");
        menuHandler.createMenuButtonOrSentence(13,3,">>", true, "++size");

        menuHandler.createMenuButtonOrSentence(21,2,"BLUE0HEXES", false);

        menuHandler.createMenuButtonOrSentence(23,3,"50%", false, "number");
        menuHandler.createMenuButtonOrSentence(21,3,"<", true, "-percent");
        menuHandler.createMenuButtonOrSentence(18,3,"<<", true, "--percent");
        menuHandler.createMenuButtonOrSentence(27,3,">", true, "+percent");
        menuHandler.createMenuButtonOrSentence(29,3,">>", true, "++percent");

        menuHandler.createMenuButtonOrSentence(9,5,"LEFT0NUMBERS:", false);
        menuHandler.createMenuButtonOrSentence(4,6,"TOP0RIGHT0NUMBERS:", false);
        menuHandler.createMenuButtonOrSentence(1,7,"BOTTOM0RIGHT0NUMBERS:", false);

        menuHandler.createMenuButtonOrSentence(23,5,"ENABLED0", true, "leftNumbers");
        menuHandler.createMenuButtonOrSentence(23,6,"ENABLED0", true, "topRightNumbers");
        menuHandler.createMenuButtonOrSentence(23,7,"ENABLED0", true, "bottomRightNumbers");

        menuHandler.createMenuButtonOrSentence(20,9,"EMBARK", true);

        menuHandler.createMenuButtonOrSentence(7,9,"GO0BACK", true);

        scrollTitle ="CUSTOMLEVEL-";  
    }

    public void makeOptionsMenuButtons(){
        menuHandler.createMenuButtonOrSentence(4,4,"MUSIC0VOLUME", false);

        menuHandler.createMenuButtonOrSentence(22,4,addPercentSignToTheEnd(soundHandler.musicVolumePercent), false, "number");
        menuHandler.createMenuButtonOrSentence(20,4,"<", true, "-music");
        menuHandler.createMenuButtonOrSentence(17,4,"<<", true, "--music");
        menuHandler.createMenuButtonOrSentence(26,4,">", true, "+music");
        menuHandler.createMenuButtonOrSentence(28,4,">>", true, "++music");

        menuHandler.createMenuButtonOrSentence(4,5,"SFX0VOLUME", false);
 
        menuHandler.createMenuButtonOrSentence(22,5,addPercentSignToTheEnd(soundHandler.sfxVolumePercent), false, "number");
        menuHandler.createMenuButtonOrSentence(20,5,"<", true, "-sfx");
        menuHandler.createMenuButtonOrSentence(17,5,"<<", true, "--sfx");
        menuHandler.createMenuButtonOrSentence(26,5,">", true, "+sfx");
        menuHandler.createMenuButtonOrSentence(28,5,">>", true, "++sfx");

        menuHandler.createMenuButtonOrSentence(22,7,addZerosToTheBeginning(PlayerPrefs.GetInt("fpsLimit")), false, "number");
        menuHandler.createMenuButtonOrSentence(4,7,"FPS0LIMIT", false);

        menuHandler.createMenuButtonOrSentence(20,7,"<", true, "-fps");
        menuHandler.createMenuButtonOrSentence(17,7,"<<", true, "--fps");
        menuHandler.createMenuButtonOrSentence(26,7,">", true, "+fps");
        menuHandler.createMenuButtonOrSentence(28,7,">>", true, "++fps");

        menuHandler.createMenuButtonOrSentence(4,2,"FULLSCREEN0(F11)", false);
        menuHandler.createMenuButtonOrSentence(21,2, Screen.fullScreen ? "ENABLED0" : "DISABLED", true, "fullscreen");

        menuHandler.createMenuButtonOrSentence(4,9,"GO0BACK", true);        

        scrollTitle ="OPTIONS-";  
    }

    public void makeCreditsMenuButtons(){
        menuHandler.createMenuButtonOrSentence(1,2,"MADE0BY0KAKADOOKA", false);
        menuHandler.createMenuButtonOrSentence(1,4,"SPECIAL0THANKS0TO0HYPEROUS", false);
        menuHandler.createMenuButtonOrSentence(1,5,"FOR0HELPING0ME0WITH0CODE", false);
        menuHandler.createMenuButtonOrSentence(1,6,"AND0PLAYTESTING", false);        
        menuHandler.createMenuButtonOrSentence(1,8,"GO0BACK", true);
                
        scrollTitle="CREDITS-";
    }
    
    void makeHighscoresMenuButtons(){
        List<string> highscores = saveAndLoad.getHighscoresAsStringList();

        menuHandler.createMenuButtonOrSentence(1,2,"0>", false, "number");
        menuHandler.createMenuButtonOrSentence(1,3,"1>", false);
        menuHandler.createMenuButtonOrSentence(1,4,"2>", false);
        menuHandler.createMenuButtonOrSentence(1,5,"3>", false);
        menuHandler.createMenuButtonOrSentence(1,6,"4>", false);
        menuHandler.createMenuButtonOrSentence(1,7,"5>", false); 
        menuHandler.createMenuButtonOrSentence(1,8,"6>", false);  
        menuHandler.createMenuButtonOrSentence(1,9,"7>", false);

        for(int i = 0; i < 8; i++){
            if(highscores[i] == "NO0SCORE0YET"){menuHandler.createMenuButtonOrSentence(4,2+i,highscores[i], false);}
            else{menuHandler.createMenuButtonOrSentence(4,2+i,highscores[i], false, "number");}
        }

        menuHandler.createMenuButtonOrSentence(28-7,9,"GO0BACK", true);

        scrollTitle="HIGHSCORES-";
    }

    public void makeFirstMenuButtons(){
        if(saveAndLoad.checkIfSaveFilesExist()){
            menuHandler.createMenuButtonOrSentence(4,2,"CONTINUE", true);
        }
        menuHandler.createMenuButtonOrSentence(29-12,3,"CUSTOM0LEVEL", true);
        menuHandler.createMenuButtonOrSentence(4,4,"PLAY0GAME", true);
        menuHandler.createMenuButtonOrSentence(29-11,5,"HOW0TO0PLAY", true);
        menuHandler.createMenuButtonOrSentence(4,6,"HIGHSCORES", true);
        menuHandler.createMenuButtonOrSentence(29-7,7,"OPTIONS", true);
        menuHandler.createMenuButtonOrSentence(4,8,"CREDITS", true); 
        menuHandler.createMenuButtonOrSentence(29-4,9,"EXIT", true);
        menuHandler.createMenuButtonOrSentence(0,10,":)", true, "discord");

        scrollTitle="HEXCROSS-";
    }

    float time = 0f;
    string hexogramTitle = "";
    public string scrollTitle;
    int index = 0;

    void scrollHexogramTitle(){
        time += Time.deltaTime;
        if(time > 0.3f){
            time = 0f; index++; hexogramTitle = "";
            for(int i = 0; i < 33; i++){
                hexogramTitle += scrollTitle[(index+i)%scrollTitle.Length];
            }
            menuHandler.createMenuButtonOrSentence(0,0,hexogramTitle, false);
        }
    }

    void checkIfSmileyAchievementCompleted(){
        steamManager.setAchivement("dc");
    }

    void checkIfMiniMegastructureAchievementsAreCompleted(){
        if(size == 1) steamManager.setAchivement("smol");
        if(size == 200) steamManager.setAchivement("big");
    }


    void Update(){
        if(Input.GetKeyDown(KeyCode.F11) && scrollTitle == "OPTIONS-"){
            menuHandler.createMenuButtonOrSentence(21,2, !Screen.fullScreen ? "ENABLED0" : "DISABLED", true, "fullscreen");
        }
        scrollHexogramTitle();
    }

    public int percentOfBlue = 50, size = 5, newGame = 0, difficulty = 0;
    string stringPercantage, stringSize;

    int convertBoolToInt(bool boolean){
        return boolean ? 1 : 0;
    }

    void setGameOptions(){
        PlayerPrefs.SetInt("percentage", percentOfBlue);
        PlayerPrefs.SetInt("size", size);
        PlayerPrefs.SetInt("newGame", newGame);
        PlayerPrefs.SetInt("leftNumbers", convertBoolToInt(leftNumbers));
        PlayerPrefs.SetInt("topRightNumbers", convertBoolToInt(topRightNumbers));
        PlayerPrefs.SetInt("bottomRightNumbers",convertBoolToInt(bottomRightNumbers)); 
        PlayerPrefs.SetInt("difficulty", difficulty);      
    }

    void OnDisable(){
        if(scrollTitle != "HEXCROSS-"){
            setGameOptions();
        }
        else{
            PlayerPrefs.SetInt("newGame", 0);
        }
    }

    void changePercentageOfSfxVolume(int amount){
        soundHandler.sfxVolumePercent += amount;
        if(soundHandler.sfxVolumePercent>100){soundHandler.sfxVolumePercent=100;}
        else if(soundHandler.sfxVolumePercent<0){soundHandler.sfxVolumePercent=0;}
        PlayerPrefs.SetInt("sfxVolumePercent", soundHandler.sfxVolumePercent);
        soundHandler.changeSfxVolumeAndPlaySample();
    }

    void changePercentageOfMusicVolume(int amount){
        soundHandler.musicVolumePercent += amount;
        if(soundHandler.musicVolumePercent>100){soundHandler.musicVolumePercent=100;}
        else if(soundHandler.musicVolumePercent<0){soundHandler.musicVolumePercent=0;}
        PlayerPrefs.SetInt("musicVolumePercent", soundHandler.musicVolumePercent);
        soundHandler.changeMusicVolumeAndPlaySample();
    }   

    void changeFpsLimit(int amount){
        soundHandler.playButtonClick();
        int fpsLimit = PlayerPrefs.GetInt("fpsLimit");
        fpsLimit += amount;
        if(fpsLimit>200){fpsLimit=200;}
        else if(fpsLimit<10){fpsLimit=10;}
        Application.targetFrameRate = fpsLimit;
        PlayerPrefs.SetInt("fpsLimit", fpsLimit);
    }    

    void changePercentageOfBlue(int amount){
        soundHandler.playButtonClick();
        percentOfBlue += amount;
        if(percentOfBlue>100){percentOfBlue=100;}
        else if(percentOfBlue<0){percentOfBlue=0;}
    }

    string addPercentSignToTheEnd(int percent){
        if(percent < 10){
            return "0"+percent.ToString()+"%";
        }
        else if(percent < 100){
            return percent.ToString()+"%";
        }
        else{
            return percent.ToString();
        }
    }

    string addZerosToTheBeginning(int number){
        if(number < 10){
            return "00"+number.ToString();
        }
        else if(number < 100){
            return "0"+number.ToString();
        }
        else{
            return number.ToString();
        }
    }
    
    int biggestSize = 0;
    void changeSize(int amount){
        soundHandler.playButtonClick();
        size += amount;
        biggestSize = biggestSize < size ? size : biggestSize;

        if(amount > 0){
            if(biggestSize>50){
                menuHandler.createMenuButtonOrSentence(3,4,"GONNA0TAKE0A0LONG0TIME0000000", false);
                if(biggestSize>=100){
                    menuHandler.createMenuButtonOrSentence(3,4,"ARE0YOU0CRAZY0OR0WHAT?0000000", false);
                }}}
        if(size > 100){size=100;}
        else if(size<1){size=1;}
    }

    void setQuickGameVariablesAndPlayGame(int size, int percentOfBlue, int difficulty){
        this.size = size; this.percentOfBlue = percentOfBlue; this.difficulty = difficulty;
        leftNumbers = true; topRightNumbers = true; bottomRightNumbers = true;

        SceneManager.LoadScene("main game");
    }

    bool leftNumbers = true, topRightNumbers = true, bottomRightNumbers = true;
    public bool tutorialUp = false;

    public void doAnActionDependingOnWhatButtonWasPressed(string buttonType){
        switch(scrollTitle){
            case "HEXCROSS-":
                if(!tutorialUp){
                    soundHandler.playButtonClick();
                    if(buttonType != "HOW0TO0PLAY")menuHandler.clearAllButtons();
                    switch(buttonType){
                        case "PLAY0GAME":
                            newGame = 1;
                            makeQuickGameButtons();
                        break;
                        case "CUSTOM0LEVEL":
                            biggestSize = size = 5; percentOfBlue = 50; newGame = 1; difficulty = 0;
                            leftNumbers = true; topRightNumbers = true; bottomRightNumbers = true;
                            makeCustomGameButtons();
                        break;
                        case "HOW0TO0PLAY":
                            //makeFirstMenuButtons();
                            menuAnimation.makeTutorialGoUp();
                        break;
                        case "CONTINUE":
                            newGame = 0;
                            saveAndLoad.setHexgridDataIntoPlayerPrefs();
                            SceneManager.LoadScene("main game");
                        break;
                        case "HIGHSCORES":
                            makeHighscoresMenuButtons();
                        break;
                        case "EXIT":
                            Application.Quit();
                        break;
                        case "CREDITS":
                            makeCreditsMenuButtons();
                        break;
                        case "discord":
                            checkIfSmileyAchievementCompleted();
                            makeFirstMenuButtons();
                            Application.OpenURL("https://discord.gg/7wSzvStkT8");
                        break;
                        case "OPTIONS":
                            makeOptionsMenuButtons();
                        break;
                    }
                }
            break;
            case "PLAYGAME-":
                soundHandler.playButtonClick();
                switch(buttonType){
                    case "BROWN0DWARF":
                        setQuickGameVariablesAndPlayGame(2,50,1);
                    break;
                    case "WHITE0DWARF":
                        setQuickGameVariablesAndPlayGame(4,50,2);
                    break;
                    case "RED0DWARF":
                        setQuickGameVariablesAndPlayGame(6,50,3);
                    break;
                    case "BLUE0DWARF":
                        setQuickGameVariablesAndPlayGame(8,50,4);
                    break;
                    case "RED0GIANT":
                        setQuickGameVariablesAndPlayGame(10,54,5);
                    break;
                    case "BLUE0GIANT":
                        setQuickGameVariablesAndPlayGame(11,57,6);
                    break;
                    case "SUPERGIANT":
                        setQuickGameVariablesAndPlayGame(12,60,7);
                    break;
                    case "HYPERGIANT":
                        setQuickGameVariablesAndPlayGame(13,63,8);
                    break;
                    case "GO0BACK":
                        menuHandler.clearAllButtons();
                        makeFirstMenuButtons();
                    break;
                }
            break;
            case "CUSTOMLEVEL-":
                switch(buttonType){
                    case "-size":
                        changeSize(-1);
                        menuHandler.createMenuButtonOrSentence(7,3,addZerosToTheBeginning(size), false, "number");
                    break;
                    case "--size":
                        changeSize(-10);
                        menuHandler.createMenuButtonOrSentence(7,3,addZerosToTheBeginning(size), false, "number");
                    break;
                    case "+size":
                        changeSize(1);
                        menuHandler.createMenuButtonOrSentence(7,3,addZerosToTheBeginning(size), false, "number");
                    break;
                    case "++size":
                        changeSize(10);
                        menuHandler.createMenuButtonOrSentence(7,3,addZerosToTheBeginning(size), false, "number");
                    break;
                    case "-percent":
                        changePercentageOfBlue(-1);
                        menuHandler.createMenuButtonOrSentence(23,3,addPercentSignToTheEnd(percentOfBlue), false, "number");
                    break;
                    case "--percent":
                        changePercentageOfBlue(-10);
                        menuHandler.createMenuButtonOrSentence(23,3,addPercentSignToTheEnd(percentOfBlue), false, "number");
                    break;
                    case "+percent":
                        changePercentageOfBlue(1);
                        menuHandler.createMenuButtonOrSentence(23,3,addPercentSignToTheEnd(percentOfBlue), false, "number");
                    break;
                    case "++percent":
                        changePercentageOfBlue(10);
                        menuHandler.createMenuButtonOrSentence(23,3,addPercentSignToTheEnd(percentOfBlue), false, "number");
                    break;
                    case "leftNumbers":
                        soundHandler.playButtonClick();
                        menuHandler.createMenuButtonOrSentence(23,5,leftNumbers?"DISABLED":"ENABLED0", true, "leftNumbers");
                        leftNumbers = leftNumbers?false:true;
                    break;
                    case "topRightNumbers":
                        soundHandler.playButtonClick();
                        menuHandler.createMenuButtonOrSentence(23,6,topRightNumbers?"DISABLED":"ENABLED0", true, "topRightNumbers");
                        topRightNumbers = topRightNumbers?false:true;
                    break;
                    case "bottomRightNumbers":
                        soundHandler.playButtonClick();
                        menuHandler.createMenuButtonOrSentence(23,7,bottomRightNumbers?"DISABLED":"ENABLED0", true, "bottomRightNumbers");
                        bottomRightNumbers = bottomRightNumbers?false:true;
                    break;
                    case "EMBARK":
                        checkIfMiniMegastructureAchievementsAreCompleted();
                        soundHandler.playButtonClick();
                        SceneManager.LoadScene("main game");
                    break;
                    case "GO0BACK":
                        soundHandler.playButtonClick();
                        menuHandler.clearAllButtons();
                        makeFirstMenuButtons();
                    break;
                }
            break;
            case "HIGHSCORES-":
                switch(buttonType){
                    case "GO0BACK":
                        soundHandler.playButtonClick();
                        menuHandler.clearAllButtons();
                        makeFirstMenuButtons();
                    break; 
                }     
            break;   
            case "CREDITS-":
                switch(buttonType){
                    case "GO0BACK":
                        soundHandler.playButtonClick();
                        menuHandler.clearAllButtons();
                        makeFirstMenuButtons();
                    break; 
                }
            break;      
            case "OPTIONS-":
                switch(buttonType){
                    case "GO0BACK":
                        soundHandler.playButtonClick();
                        menuHandler.clearAllButtons();
                        makeFirstMenuButtons();
                    break;
                    case "-sfx":
                        changePercentageOfSfxVolume(-1);
                        menuHandler.createMenuButtonOrSentence(22,5,addPercentSignToTheEnd(soundHandler.sfxVolumePercent), false, "number");
                    break;
                    case "--sfx":
                        changePercentageOfSfxVolume(-10);
                        menuHandler.createMenuButtonOrSentence(22,5,addPercentSignToTheEnd(soundHandler.sfxVolumePercent), false, "number");
                    break;
                    case "+sfx":
                        changePercentageOfSfxVolume(1);
                        menuHandler.createMenuButtonOrSentence(22,5,addPercentSignToTheEnd(soundHandler.sfxVolumePercent), false, "number");
                    break;
                    case "++sfx":
                        changePercentageOfSfxVolume(10);
                        menuHandler.createMenuButtonOrSentence(22,5,addPercentSignToTheEnd(soundHandler.sfxVolumePercent), false, "number");
                    break;
                    case "-music":
                        changePercentageOfMusicVolume(-1);
                        menuHandler.createMenuButtonOrSentence(22,4,addPercentSignToTheEnd(soundHandler.musicVolumePercent), false, "number");
                    break;
                    case "--music":
                        changePercentageOfMusicVolume(-10);
                        menuHandler.createMenuButtonOrSentence(22,4,addPercentSignToTheEnd(soundHandler.musicVolumePercent), false, "number");
                    break;
                    case "+music":
                        changePercentageOfMusicVolume(1);
                        menuHandler.createMenuButtonOrSentence(22,4,addPercentSignToTheEnd(soundHandler.musicVolumePercent), false, "number");
                    break;
                    case "++music":
                        changePercentageOfMusicVolume(10);
                        menuHandler.createMenuButtonOrSentence(22,4,addPercentSignToTheEnd(soundHandler.musicVolumePercent), false, "number");
                    break;
                    case "-fps":
                        changeFpsLimit(-1);
                        menuHandler.createMenuButtonOrSentence(22,7,addZerosToTheBeginning(PlayerPrefs.GetInt("fpsLimit")), false, "number");
                    break;
                    case "--fps":
                        changeFpsLimit(-10);
                        menuHandler.createMenuButtonOrSentence(22,7,addZerosToTheBeginning(PlayerPrefs.GetInt("fpsLimit")), false, "number");
                    break;
                    case "+fps":
                        changeFpsLimit(1);
                        menuHandler.createMenuButtonOrSentence(22,7,addZerosToTheBeginning(PlayerPrefs.GetInt("fpsLimit")), false, "number");
                    break;
                    case "++fps":
                        changeFpsLimit(10);
                        menuHandler.createMenuButtonOrSentence(22,7,addZerosToTheBeginning(PlayerPrefs.GetInt("fpsLimit")), false, "number");
                    break;
                    case "fullscreen":
                        soundHandler.playButtonClick();
                        camera.toggleFullscreen();
                        menuHandler.createMenuButtonOrSentence(21,2, !Screen.fullScreen ? "ENABLED0" : "DISABLED", true, "fullscreen");
                    break;

                }
            break;    
        }
    }


}
