using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Save_And_Load : MonoBehaviour
{
    [Serializable]
    public class MiscData{
        [SerializeField]
        public List<int> highscoreList = new List<int>();
        public List<bool> perfectCompleteList = new List<bool>();

        public MiscData(){
            for(int i = 0; i <= 8; i++){
                highscoreList.Add(-9999999);
            }
            for(int i = 1; i <= 9; i++){
                perfectCompleteList.Add(false);
            }
        }
    }

    [Serializable]
    public class HexProperties{
        [SerializeField]
        public Vector3 position;
        [SerializeField]
        public int spriteNum;
        [SerializeField]
        public bool state, isStateKnown, hexState;
        public void addProperties(bool hexState, Vector3 position, int spriteNum, bool isStateKnown, bool state){ 
            this.hexState = hexState;
            this.position = position;
            this.spriteNum = spriteNum;
            this.state = state;
            this.isStateKnown = isStateKnown;
        }
    }

    [Serializable]
    public class HexPropertiesList{
        [SerializeField]
        public List<HexProperties> hexProperties = new List<HexProperties>();
    }
    [Serializable]
    public class HexPropertiesListOfLists{
        [SerializeField]
        public List<HexPropertiesList> hexPropertiesList = new List<HexPropertiesList>();

    }
    [Serializable]
    public class HexgridData{
        [SerializeField]
        public int hexSize, leftNumbers, topRightNumbers, bottomRightNumbers, difficulty;             
        public HexgridData(int hexSize, int leftNumbers, int topRightNumbers, int bottomRightNumbers, int difficulty){
            this.hexSize = hexSize;
            this.leftNumbers = leftNumbers;
            this.topRightNumbers = topRightNumbers;
            this.bottomRightNumbers = bottomRightNumbers;
            this.difficulty = difficulty;
        }
    }

    [Serializable]
    public class UIStatsInfo{
        [SerializeField]
        public int amountOfGuessedHexes, amountOfWrongGuesses, percentComplete, score;
        [SerializeField]
        public float timeClock;
        public UIStatsInfo(int amountOfGuessedHexes,int amountOfWrongGuesses,int percentComplete, float timeClock, int score){
            this.amountOfGuessedHexes = amountOfGuessedHexes;
            this.amountOfWrongGuesses = amountOfWrongGuesses;
            this.percentComplete = percentComplete;
            this.timeClock = timeClock;
            this.score = score;
        }
    }

    public bool checkIfSaveFilesExist(){
        setSaveDirectory();
        return (Directory.Exists(saveDirectory) && File.Exists(saveDirectory+"/ui.json") && File.Exists(saveDirectory+"/save.json"));
    }

    void createDirectoryIfDoesntExist(){
        setSaveDirectory();
        if (!Directory.Exists(saveDirectory)) {
            DirectoryInfo di = Directory.CreateDirectory(saveDirectory);
        }
    }

    void putObjectIntoJsonAndSaveIt(HexPropertiesListOfLists hexPropertiesListOfLists){
        createDirectoryIfDoesntExist();
        StreamWriter writer = new StreamWriter(saveDirectory+"/save.json");
        writer.WriteLine(JsonUtility.ToJson(hexPropertiesListOfLists));
        writer.Close();
    }
    void putObjectIntoJsonAndSaveIt(UIStatsInfo uIStatsInfo){
        createDirectoryIfDoesntExist();
        StreamWriter writer = new StreamWriter(saveDirectory+"/ui.json");
        writer.WriteLine(JsonUtility.ToJson(uIStatsInfo));
        writer.Close();
    }
    void putObjectIntoJsonAndSaveIt(HexgridData hexgridData){
        createDirectoryIfDoesntExist();
        StreamWriter writer = new StreamWriter(saveDirectory+"/hexgridData.json");
        writer.WriteLine(JsonUtility.ToJson(hexgridData));
        writer.Close();
    }
    void putObjectIntoJsonAndSaveIt(MiscData miscData){
        StreamWriter writer = new StreamWriter(saveDirectory+"/miscData.json");
        writer.WriteLine(JsonUtility.ToJson(miscData));
        writer.Close();
    }

    public int getAmountOfPerfectScoreLevels(){
        checkMiscDataFileExists();

        int amountOfPerfectScoreLevels = 0;
        string jsonText = File.ReadAllText(saveDirectory+"/miscData.json");
        miscData = JsonUtility.FromJson<MiscData>(jsonText);

        for(int i = 0; i < miscData.perfectCompleteList.Count; i++){
            if(miscData.perfectCompleteList[i]){amountOfPerfectScoreLevels++;}
        }
        return amountOfPerfectScoreLevels-1;
    }

    public bool checkIfLevelHadPerfectScore(int difficulty){
        checkMiscDataFileExists();

        string jsonText = File.ReadAllText(saveDirectory+"/miscData.json");
        miscData = JsonUtility.FromJson<MiscData>(jsonText);

        if(miscData.perfectCompleteList[difficulty]){
            return true;
        }
        return false;
    }

    public bool checkIfLevelAlreadyHadPerfectScoreAndSetItIfItDidnt(){
        checkMiscDataFileExists();

        int difficulty = PlayerPrefs.GetInt("difficulty");
        string jsonText = File.ReadAllText(saveDirectory+"/miscData.json");
        miscData = JsonUtility.FromJson<MiscData>(jsonText);

        if(miscData.perfectCompleteList[difficulty]){
            return true;
        }
        if(difficulty == 0){
            return false;
        }
        miscData.perfectCompleteList[difficulty] = true;
        putObjectIntoJsonAndSaveIt(miscData);
        return false;
    }

    void checkMiscDataFileExists(){
        createDirectoryIfDoesntExist();
        if(!File.Exists(saveDirectory+"/miscData.json")){
            miscData = new MiscData();
            putObjectIntoJsonAndSaveIt(miscData);
        }
    }

    MiscData miscData;
    public bool saveHighscoresAndReturnTrueIfNewBest(int score){
        checkMiscDataFileExists();

        string jsonText = File.ReadAllText(saveDirectory+"/miscData.json");
        miscData = JsonUtility.FromJson<MiscData>(jsonText);

        int difficulty = PlayerPrefs.GetInt("difficulty");

        if(difficulty != 0 && (miscData.highscoreList[difficulty] == null || miscData.highscoreList[difficulty] < score)){
            miscData.highscoreList[difficulty] = score;
            putObjectIntoJsonAndSaveIt(miscData);
            return true;
        }
        return false;
    }

    public List<string> getHighscoresAsStringList(){
        checkMiscDataFileExists();

        string jsonText = File.ReadAllText(saveDirectory+"/miscData.json");
        miscData = JsonUtility.FromJson<MiscData>(jsonText);

        List<string> highscoresString = new List<string>();

        for(int i = 1; i < 9; i++){
            if(miscData.highscoreList[i] == -9999999){
                highscoresString.Add("NO0SCORE0YET");
            }
            else{
                highscoresString.Add(miscData.highscoreList[i].ToString());
            }
        }

        return highscoresString;
    }

    public List<List<Hex_Handler>> hexHandlerComponentArray = new List<List<Hex_Handler>>();
    public List<List<bool>> hexStateArray = new List<List<bool>>();

    HexPropertiesListOfLists hexPropertiesListOfLists = new HexPropertiesListOfLists();

    HexPropertiesListOfLists getObjectFromHexProperties(){
        hexHandlerComponentArray = hexgrid.hexHandlerArrayX;
        hexStateArray = hexgrid.hexGridStatesX;
        for(int i = 0; i < hexHandlerComponentArray.Count; i++){

            for(int j = 0; j < hexHandlerComponentArray[i].Count; j++){

                Hex_Handler hexHandler = hexHandlerComponentArray[i][j];
                hexPropertiesListOfLists.hexPropertiesList[i].hexProperties[j].addProperties(hexHandler.hexState, hexHandler.transform.position, hexHandler.spriteNum, hexHandler.isStateKnown, hexStateArray[i][j]);     
            }
        }
        return hexPropertiesListOfLists;
    }

    void setSaveDirectory(){
        if(saveDirectory == null){
            saveDirectory = Application.persistentDataPath;
        }
    }

    UI_Stats_Handler uiStats;
    Hexgrid_Handler hexgrid;
    void Start(){
        uiStats = gameObject.GetComponent<UI_Stats_Handler>();
        hexgrid = gameObject.GetComponent<Hexgrid_Handler>();
        setSaveDirectory();

        if(hexgrid != null){
            for(int i = 0; i < hexgrid.hexHandlerArrayX.Count; i++){
                hexPropertiesListOfLists.hexPropertiesList.Add(new HexPropertiesList());
                for(int j = 0; j < hexgrid.hexHandlerArrayX[i].Count; j++){
                    hexPropertiesListOfLists.hexPropertiesList[i].hexProperties.Add(new HexProperties());
                }
            }
        }
    }
    string saveDirectory;

    public void saveUIStats(){
        UIStatsInfo uiStatsInfo = new UIStatsInfo(uiStats.amountOfGuessedHexes,uiStats.amountOfWrongGuesses,uiStats.percentComplete,uiStats.timeClock, uiStats.score);
        putObjectIntoJsonAndSaveIt(uiStatsInfo);
    }
    
    public UIStatsInfo getUIStats(){
        string jsonText = File.ReadAllText(saveDirectory+"/ui.json");
        return JsonUtility.FromJson<UIStatsInfo>(jsonText);
    }

    public void saveGame(){
        putObjectIntoJsonAndSaveIt(getObjectFromHexProperties());
        saveUIStats();
        saveHexgridData();
    }

    public void saveHexgridData(){
        HexgridData hexgridData = new HexgridData(hexgrid.hexSize, hexgrid.leftNumbers, hexgrid.topRightNumbers, hexgrid.bottomRightNumbers, PlayerPrefs.GetInt("difficulty"));
        putObjectIntoJsonAndSaveIt(hexgridData);
    }

    public void setHexgridDataIntoPlayerPrefs(){

        string jsonText = File.ReadAllText(saveDirectory+"/hexgridData.json");
        HexgridData hexgridData = JsonUtility.FromJson<HexgridData>(jsonText);

        PlayerPrefs.SetInt("difficulty", hexgridData.difficulty);
        PlayerPrefs.SetInt("size", hexgridData.hexSize);
        PlayerPrefs.SetInt("leftNumbers", hexgridData.leftNumbers);
        PlayerPrefs.SetInt("topRightNumbers", hexgridData.topRightNumbers);
        PlayerPrefs.SetInt("bottomRightNumbers", hexgridData.bottomRightNumbers);
    }

    List<List<HexProperties>> translateStupidClassOfClassesIntoBeautiful2DList(HexPropertiesListOfLists hexPropertiesListOfLists){
        List<List<HexProperties>> hexPropertiesArray = new List<List<HexProperties>>();
        for(int i = 0; i < hexPropertiesListOfLists.hexPropertiesList.Count; i++){
            hexPropertiesArray.Add(new List<HexProperties>());
            for(int j = 0; j < hexPropertiesListOfLists.hexPropertiesList[i].hexProperties.Count; j++){
                hexPropertiesArray[i].Add(hexPropertiesListOfLists.hexPropertiesList[i].hexProperties[j]);
            }            
        }
        return hexPropertiesArray;
    }

    public List<List<HexProperties>> getHexProperties(){
        setSaveDirectory();

        HexPropertiesListOfLists hexPropertiesListOfLists = new HexPropertiesListOfLists();
       
        string jsonText = File.ReadAllText(saveDirectory+"/save.json");
        hexPropertiesListOfLists = JsonUtility.FromJson<HexPropertiesListOfLists>(jsonText);

        return translateStupidClassOfClassesIntoBeautiful2DList(hexPropertiesListOfLists);
    }
    
    public void deleteSaveFile(){
        if(File.Exists(saveDirectory+"/save.json")){
            File.Delete(saveDirectory+"/save.json");
        }
    }

}
