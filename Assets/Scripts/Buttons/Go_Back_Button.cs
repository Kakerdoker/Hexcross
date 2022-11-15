using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Go_Back_Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Image buttonImage;
    Sound_Handler soundHandler;
    Save_And_Load saveAndLoad;

    public Sprite[] sprites = new Sprite[1];
    void Start()
    {
        buttonImage = gameObject.GetComponent<Image>();
        saveAndLoad = GameObject.Find("!SCRIPT HOLDER!").GetComponent<Save_And_Load>();

        gameObject.GetComponent<Button>().onClick.AddListener(Click);
        
        soundHandler = GameObject.Find("!SCRIPT HOLDER!").GetComponent<Sound_Handler>();
    }

    public void OnPointerExit(PointerEventData eventData){
        buttonImage.sprite = sprites[0];
        gameObject.transform.localScale = new Vector2(1f,1f);
    }

    public void OnPointerEnter(PointerEventData eventData){
        buttonImage.sprite = sprites[1];
        gameObject.transform.localScale = new Vector2(1.2f,1.2f);
        soundHandler.playHover();
    }

    void Click(){
        if(GameObject.Find("!SCRIPT HOLDER!").GetComponent<UI_Stats_Handler>().win){
            saveAndLoad.deleteSaveFile();
        }
        else{
            saveAndLoad.saveGame();
        }
        SceneManager.LoadScene("main menu");
    }

    void Update(){

    }

}
