using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Perfect_Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Image buttonImage;
    Sound_Handler soundHandler;
    UI_Stats_Handler uiStats;

    public Sprite[] sprites = new Sprite[1];
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(Click);
        soundHandler = GameObject.Find("!SCRIPT HOLDER!").GetComponent<Sound_Handler>();
        uiStats = GameObject.Find("!SCRIPT HOLDER!").GetComponent<UI_Stats_Handler>();
    }

    public void OnPointerExit(PointerEventData eventData){
        gameObject.transform.localScale = new Vector2(1f,1f);
    }

    public void OnPointerEnter(PointerEventData eventData){
        gameObject.transform.localScale = new Vector2(1.2f,1.2f);
        soundHandler.playHover();
    }

    void Click(){
        soundHandler.playSweep();
        uiStats.perfectButtonClicked = true;
        gameObject.SetActive(false);
    }

}
