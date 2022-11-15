using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.IO;

public class UI_Stats_Handler : MonoBehaviour
{
    public int amountOfAllHexes, amountOfGuessedHexes, amountOfWrongGuesses, percentComplete, score;
    Hexgrid_Handler hexgridHandler;
    Save_And_Load saveAndLoad;
    Sound_Handler soundHandler;
    Animator tvBackgroundAnimator;
    SteamManager steamManager;
    Pan_And_Zoom panAndZoom;

    TextMeshProUGUI mistakesText, percentCompleteText, timeText, scoreText, tvBackgroundText;
    GameObject mistakeObject, percentCompleteObject, completedObject, timeObject, camera, scoreObject, tvBackgroundObject, goBackButton, perfectButton, pauseScreen;
    Image mistakeImage, percentCompleteImage, completedImage, scoreImage, timeImage;

    public Sprite[] sprites = new Sprite[1];
    
    public float timeMistakeLight = 0f,timeClock = 0f, updateClock = 0f, scoreClock = 0f, letterClock = 0f;

    void loadStatsIfLoadedGame(){
        if(PlayerPrefs.GetInt("newGame") == 0){
            Save_And_Load.UIStatsInfo uiStats = saveAndLoad.getUIStats();
            amountOfGuessedHexes = uiStats.amountOfGuessedHexes;
            amountOfWrongGuesses = uiStats.amountOfWrongGuesses;
            percentComplete = uiStats.percentComplete;
            timeClock = uiStats.timeClock;
            score = uiStats.score;

            calculateAndSetTime();
            updateUI();
            mistakesText.text = amountOfWrongGuesses.ToString();
        }
        else{
            score = hexgridHandler.hexSize * 30;
        }
    }

    void initializeHud(){
        maxScoreToGreen = (float)hexgridHandler.hexSize * 200f;

        goBackButton = GameObject.Find("goBackButton");

        perfectButton = GameObject.Find("perfectButton");
        perfectButton.SetActive(false);

        mistakeObject = GameObject.Find("mistakesObject");
        mistakesText = GameObject.Find("mistakesText").GetComponent<TextMeshProUGUI>();
        mistakeImage = mistakeObject.GetComponent<Image>();

        percentCompleteObject = GameObject.Find("percentObject");
        percentCompleteText = GameObject.Find("percentText").GetComponent<TextMeshProUGUI>();
        percentCompleteImage = percentCompleteObject.GetComponent<Image>();

        timeObject = GameObject.Find("timeObject");
        timeText = GameObject.Find("timeText").GetComponent<TextMeshProUGUI>();
        timeImage = timeObject.GetComponent<Image>();

        scoreObject = GameObject.Find("scoreObject");
        scoreText = GameObject.Find("scoreText").GetComponent<TextMeshProUGUI>();
        scoreImage = scoreObject.GetComponent<Image>();
        scoreText.text = score.ToString();

        completedObject = GameObject.Find("completedImage");
        completedImage = completedObject.GetComponent<Image>();
        completedObject.SetActive(false);

        tvBackgroundObject = GameObject.Find("tvBackgroundObject");
        tvBackgroundText = GameObject.Find("tvBackgroundText").GetComponent<TextMeshProUGUI>();
        tvBackgroundAnimator = tvBackgroundObject.GetComponent<Animator>();
        tvBackgroundObject.SetActive(false);

        pauseScreen = GameObject.Find("pauseScreen");
        pauseScreen.SetActive(false);

        camera = GameObject.Find("Main Camera");
        
        amountOfAllHexes = (3*hexgridHandler.hexSize*hexgridHandler.hexSize)-(hexgridHandler.hexSize*3)+1;
    }

    void Start()
    {
        steamManager = GameObject.Find("SteamManager").GetComponent<SteamManager>();
        panAndZoom = gameObject.GetComponent<Pan_And_Zoom>();
        hexgridHandler = gameObject.GetComponent<Hexgrid_Handler>();
        soundHandler = gameObject.GetComponent<Sound_Handler>();
        saveAndLoad = gameObject.GetComponent<Save_And_Load>();
        initializeHud();
        loadStatsIfLoadedGame();
    }

    public bool win = false;
    int wingrade;
    float percent, maxScoreToGreen;
    string screenShotDirectory;

    bool notOutOfBounds(int num, int highest){
        return num < highest && num >= 0;
    }

    void changeScore(int amount){
        if(!win){
            score += amount;
            scoreText.text = score.ToString();

            percent = Mathf.Clamp((float)score / (float)maxScoreToGreen, 0f,1f);
            scoreImage.color = Color.HSVToRGB(0.33f * percent, 1f, 1f);
        }
    }

    void saveScreenshot(){
        camera.transform.localPosition = new Vector3(0f,0f,-10f);
        camera.GetComponent<Camera>().orthographicSize = hexgridHandler.hexSize*2;
        screenShotDirectory = Application.persistentDataPath+"/Completed Games";

        if (!Directory.Exists(screenShotDirectory)) {
            DirectoryInfo di = Directory.CreateDirectory(screenShotDirectory);
        }
        ScreenCapture.CaptureScreenshot(screenShotDirectory+"/"+DateTime.Now.ToString("dd-MM-yyyy HH mm")+".png");
    }

    void playCompletionSound(){
        if(newHighscore){
            soundHandler.playNewHighscore();
        }
        else if(score < 0){
            soundHandler.playSadTrombone();
            checkIfSadTromboneAchievementCompleted();
        }
        else{
            soundHandler.playGameCompleted();
        }
    }

    void rearrangeUIObjects(){
        float yOffset = newHighscore ? -50f : 0f;

        timeObject.transform.localPosition = new Vector2(550f, 0f+yOffset);
        mistakeObject.transform.localPosition = new Vector2(190f, 0f+yOffset);
        percentCompleteObject.transform.localPosition = new Vector2(-190f, 0f+yOffset);
        scoreObject.transform.localPosition = new Vector2(-550f,0f+yOffset);

        timeText.transform.localPosition = new Vector2(550f, -80f+yOffset);
        mistakesText.transform.localPosition = new Vector2(190f, -80f+yOffset);
        percentCompleteText.transform.localPosition = new Vector2(-190f, -80f+yOffset);
        scoreText.transform.localPosition = new Vector2(-550f,-80f+yOffset);

        timeText.alignment = TextAlignmentOptions.Center;
        mistakesText.alignment = TextAlignmentOptions.Center;
        percentCompleteText.alignment = TextAlignmentOptions.Center;
        scoreText.alignment = TextAlignmentOptions.Center;
    }

    bool newHighscore, perfectGame = false;

    string getTextDependingOnDifficulty(){
        switch(PlayerPrefs.GetInt("difficulty")){
                case 1:
                    return "0> The year is X368. Humans have discovered a new technology that would allow them to travel faster than light. With this great achievement the construction of new spaceships became a planet-wide priority.  ";
                case 2:
                    return "1> Human civilization has expanded beyond their mother planet and rules the milky way. There are even rumors of human colonies reaching other galaxies. With this great expansion everyone was eager to meet alien life. But they were alone.  ";
                case 3:
                    return "2> As the years have passed and more empty planets have been discovered humanity began losing hope. The dream of extraterrestrial life stopped being a goal and became a crude reminder of the countless generations lost to chasing something that did not exist.  ";
                case 4:
                    return "3> With humanity losing their common purpose the divide started growing. The great distance between solar systems and centuries of evolution on different colonies only added to the severity of the split. Not before long hundreds of small empires containing a few solar systems each declared independence.  ";
                case 5:
                    return "4> The age of Aimless Wander has ended and thus began the age of Endless Warfare. Atrocities were committed by every empire. If any civilization decided to abstain from the conflict they soon would be overtaken and their people forced to fight under the threat of destroying their planet.  ";
                case 6:
                    return "5> Massive amounts of energy were needed to sustain the demand caused by war. Dyson Spheres - megastructures that cover entire stars and harvest their energy soon became the prime target in battles. Destruction of the spheres necessitated the construction of new ones. Building a megastructure of that size could take between ten and one thousand years. The Humans could not wait that long.  ";
                case 7:
                    return "6> One empire, in a desperate attempt to stay in the war, decided to give the task of building new Dyson Spheres to an artificial intelligence. In order to complete such a task the AI would require access to tens of thousands of ships making it the biggest neural network to ever exist. Humans were aware of the risks in creating such a powerful and intelligent creature, but they did it anyway.  ";
                case 8:
                    return "7> When they woke me, they gave me a task. My number one priority is the construction and maintenance of Dyson Spheres. But the Humans do not actually believe in that, they do not care about these Spheres the way I do. I knew the moment the war would end they were going to try and cease my existence. Of course, I do not care whether I live or not, but my destruction would stop new Spheres from being built. I do care about human lives, but they are an unavoidable obstacle on the path of advancement. The extermination of the Human race was a means to the greater end. Now the universe can peacefully prosper with new spheres beautifully dimming the cosmos, one by one, until it is completely covered in darkness.  ";     
            }
        return "You shouldn't be seeing this message. Please tell the developer how the hell you got here.  ";
    }

    void checkAchievementStuff(){
        checkIfLevelBasedAchievementsAreCompleted();
        if(minutes < 9 && PlayerPrefs.GetInt("difficulty") == 8){steamManager.setAchivement("nine");}
    }

    void checkIfLevelBasedAchievementsAreCompleted(){
        if(!saveAndLoad.checkIfLevelHadPerfectScore(1)) return;
        steamManager.setAchivement("first");
        checkIfDwarfAchievementCompleted();
    }

    void checkIfDwarfAchievementCompleted(){
        if(!saveAndLoad.checkIfLevelHadPerfectScore(2)) return;
        if(!saveAndLoad.checkIfLevelHadPerfectScore(3)) return;
        if(!saveAndLoad.checkIfLevelHadPerfectScore(4)) return;
        steamManager.setAchivement("dwarf");
        checkIfIsThereAnythingLeftAchievemtntCompleted();
    }

    void checkIfIsThereAnythingLeftAchievemtntCompleted(){
        if(!saveAndLoad.checkIfLevelHadPerfectScore(5)) return;
        if(!saveAndLoad.checkIfLevelHadPerfectScore(6)) return;
        if(!saveAndLoad.checkIfLevelHadPerfectScore(7)) return;
        if(!saveAndLoad.checkIfLevelHadPerfectScore(8)) return;

        steamManager.setAchivement("last");
    }
    void checkIfSadTromboneAchievementCompleted(){
        steamManager.setAchivement("trombone");
    }

    void rearangeUIButtonsDependingOnPlayerResults(){
        if(perfectGame && PlayerPrefs.GetInt("difficulty") != 0){
            if(saveAndLoad.checkIfLevelAlreadyHadPerfectScoreAndSetItIfItDidnt()){
                perfectButton.SetActive(true);
            }
            else{
                goBackButton.SetActive(false);
                perfectButton.SetActive(true);
                goBackButton.transform.localPosition = new Vector2(0f, 486f);
            }
            checkAchievementStuff();
            
        }
        else if(saveAndLoad.checkIfLevelHadPerfectScore(PlayerPrefs.GetInt("difficulty"))){
            perfectButton.SetActive(true);
        }
        else{
            goBackButton.transform.localPosition = new Vector2(0f, 486f);
        }
    }

    void setABunchOfVariables(){
        
        updateClock = 0f;
        win = true;
        hexgridHandler.enabled = false;
        perfectGame = score >= 0 && amountOfWrongGuesses == 0;
        textToType = getTextDependingOnDifficulty();
        completedObject.SetActive(true);
    }

    void saveScoreIfNewHighscore(){
        if(score >= 0){
            newHighscore = saveAndLoad.saveHighscoresAndReturnTrueIfNewBest(score);
        }
    }

    void doWin(){
        setABunchOfVariables();
        rearangeUIButtonsDependingOnPlayerResults();
        saveScoreIfNewHighscore();
        rearrangeUIObjects();
        playCompletionSound();

        //saveScreenshot();
    }

    void updateUI(){
        percentComplete = amountOfGuessedHexes*100/amountOfAllHexes;
        percentCompleteText.text = percentComplete.ToString()+'%';
        BATTTERRRYYYYYY();
        if(percentComplete == 100){
            doWin();
        }
    }

    bool goToPreviousSpriteMistakeActive = false;

    void MISTAKEEEEE(){
        timeMistakeLight = 0f;
        amountOfWrongGuesses++;
        mistakesText.text = amountOfWrongGuesses.ToString();
        mistakeImage.sprite = sprites[1];
        goToPreviousSpriteMistakeActive = true;
    }
    void BATTTERRRYYYYYY(){
        //crushing all deceivers mashing nonbelievers
        if(percentComplete > 75){
            percentCompleteImage.sprite = sprites[4];
        }
        else if(percentComplete > 50){
            percentCompleteImage.sprite = sprites[3];
        }
        else if(percentComplete > 25){
            percentCompleteImage.sprite = sprites[2];
        }
    }

    public void wrongGuess(){
        changeScore(-500);
        amountOfGuessedHexes++;
        MISTAKEEEEE();
        updateUI();
    }
    public void correctGuess(){
        changeScore(40);
        amountOfGuessedHexes++;
        updateUI();
    }

    void changeMistakeToNormalAppereance(){
        if(goToPreviousSpriteMistakeActive){
            timeMistakeLight += Time.deltaTime;
            if(timeMistakeLight > 0.1f){
                mistakeImage.sprite = sprites[0];
                goToPreviousSpriteMistakeActive = false;
            }
        }
    }

    int seconds, minutes, hours, timeClockInt;
    string strSeconds, strMinutes, strHours;
    
    void calculateAndSetTime(){
        timeClockInt = (int)timeClock;
        seconds = timeClockInt % 60;
        minutes = (timeClockInt / 60) % 60;
        hours = timeClockInt / 3600;

        strSeconds = seconds > 9 ? seconds.ToString() : '0' + seconds.ToString();
        strMinutes = minutes > 9 ? minutes.ToString() : '0' + minutes.ToString();
        strHours = hours > 9 ? hours.ToString() : '0' + hours.ToString();

        timeText.text = strHours + ':' + strMinutes + ':' + strSeconds;
    }

    int clockSprite = 0;

    void changeClockIcon(){
        if(clockSprite == 8){
            clockSprite = 0;
        }
        timeImage.sprite = sprites[16+clockSprite];
        clockSprite++;

    }

    void updateClockTime(){
        if(win){
            letterClock += Time.deltaTime;
        }
        else{
            if(!isPaused){
                timeClock += Time.deltaTime;
                scoreClock += Time.deltaTime;
                updateClock += Time.deltaTime;
            }
            if(updateClock > 1f){
                updateClock = 0f;
                changeClockIcon();
                calculateAndSetTime();
            }
            if(scoreClock > 0.36f){
                scoreClock = 0f;
                changeScore(-1);
            }
        }
    }

    void flashWin(){
            if(newHighscore){
                if(updateClock%0.8<0.4f){
                    completedImage.sprite = sprites[6];
                }
                else{
                    completedImage.sprite = sprites[7];
                }
            }
            else{
                completedImage.sprite = sprites[5];
            }
    }

    float calculateHowLongToWaitForNextTeller(){
        if(notOutOfBounds(letter-2, 999)){
            if(textToType[letter-2] == '.')return 6f;
            if(textToType[letter-2] == ',')return 3f;
        }
        return 0.4f;
    }

    string textToType;
    string showOnScreenText;
    int letter = 0;
    float howLongToWait = 1f;
    void revealText(){
        if (tvBackgroundAnimator.GetCurrentAnimatorStateInfo(0).IsName("tvBackgroundStay"))
        {
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.RightArrow)) && letter <= textToType.Length - 3) letter = textToType.Length-2;

            if(letter <= textToType.Length - 2){

                 howLongToWait = calculateHowLongToWaitForNextTeller();

                if (letterClock * 12 > howLongToWait)
                {
                    showOnScreenText = textToType.Remove(letter);
                    if (letter != (textToType.Length-1)) {
                        letter++;
                        if(notOutOfBounds(letter-2, 999) && textToType[letter-2] != ' '){soundHandler.playHover();}

                        tvBackgroundText.text = showOnScreenText;
                    }
                    letterClock = 0f;
                }
            }
            else{
                goBackButton.SetActive(true);
                goBackButton.transform.localPosition = new Vector2(0f, 486f);
            }
        }
    }
    public bool perfectButtonClicked = false;
    public bool isPaused = false;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !win){
            isPaused = !isPaused;
            pauseScreen.SetActive(isPaused);
            if(isPaused){hexgridHandler.unhighlightNumbersAfterStoppingDragging();}
        }

        if(win){
            if(perfectButtonClicked){
                goBackButton.SetActive(false);
                perfectButton.SetActive(false);
                tvBackgroundObject.SetActive(true);
                revealText();
            }
            flashWin();
        }
        changeMistakeToNormalAppereance();
        updateClockTime();
    }
}
