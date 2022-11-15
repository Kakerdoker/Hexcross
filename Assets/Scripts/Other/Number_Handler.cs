using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Number_Handler : MonoBehaviour
{

    bool greyedOut = false;
    TextMeshProUGUI tmPro;

    void Start(){
        tmPro = gameObject.GetComponent<TextMeshProUGUI>();
    }

    void OnMouseDown(){
        if(greyedOut){
            tmPro.color = new Color32(255, 255, 255, 255);
            greyedOut = false;
        }
        else{
            tmPro.color = new Color32(255, 255, 255, 63); 
            greyedOut = true;
        }
    }
}
