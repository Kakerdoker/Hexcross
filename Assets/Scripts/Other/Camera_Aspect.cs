using UnityEngine;

public class Camera_Aspect : MonoBehaviour
{
    void Awake()
    {
        float width = Screen.currentResolution.width;
        float height = Screen.currentResolution.height;
        Camera.main.aspect = sixteenByNine;
        if(PlayerPrefs.GetInt("checkScreenFirstTime") != 1){
            PlayerPrefs.SetInt("checkScreenFirstTime", 1);

            Screen.fullScreen = true;
            Screen.SetResolution((int)(width/2f), (int)(height/2f), false);
            Screen.fullScreen = false;
        }
        resolutionAspect = (float)Screen.currentResolution.width/(float)Screen.currentResolution.height;
    }
    float resolutionAspect;
    float sixteenByNine = 16f/9f;

    public void toggleFullscreen(){
        float width = Screen.currentResolution.width;
            float height = Screen.currentResolution.height;

            if(Screen.fullScreen){
                Screen.SetResolution((int)(width/2f), (int)(height/2f), false);
                Screen.fullScreen = false;
            }
            else{
                if(resolutionAspect < sixteenByNine){
                    Screen.SetResolution((int)width, (int)(height/sixteenByNine), true);
                }
                else if(resolutionAspect > sixteenByNine){
                    Screen.SetResolution((int)(width*sixteenByNine), (int)height, true);
                }
                else{
                    Screen.SetResolution((int)width, (int)height, true);
                }
                
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            }
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.F11))
        {
            toggleFullscreen();
        }
    }
}
