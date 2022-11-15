using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background_Handler : MonoBehaviour
{
    Save_And_Load saveAndLoad;
    public Sprite[] sprites = new Sprite[1];
    int spriteNum;
    void Start()
    {
        spriteNum = GameObject.Find("!SCRIPT HOLDER!").GetComponent<Save_And_Load>().getAmountOfPerfectScoreLevels();
        if(spriteNum != -1){
            GameObject.Find("background").GetComponent<Image>().sprite = sprites[spriteNum];
            GameObject.Find("background2").GetComponent<Image>().sprite = sprites[spriteNum];
        }
    }
}
