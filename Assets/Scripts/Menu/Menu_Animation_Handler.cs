using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Animation_Handler : MonoBehaviour
{
    Animator animator;
    Menu_Handler menuHandler;
    bool enableBoxCollidersTrigger = false;
    Sound_Handler soundHandler;
    Menu_Button_Handler menuButtonHandler;

    void Start(){
        animator = gameObject.GetComponent<Animator>();
        menuHandler = GameObject.Find("!SCRIPT HOLDER!").GetComponent<Menu_Handler>();
        menuButtonHandler = GameObject.Find("!SCRIPT HOLDER!").GetComponent<Menu_Button_Handler>();
        soundHandler = GameObject.Find("!SCRIPT HOLDER!").GetComponent<Sound_Handler>();
    }

    public void makeTutorialGoUp(){
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("stayDown")){
            animator.SetTrigger("goUp");
            soundHandler.playSweep();

            menuHandler.layerMask = 0;
        }
    }

    public void makeTutorialGoDown(){
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("stayUp")){
            animator.SetTrigger("goDown");
            soundHandler.playSweepReverese();

            menuHandler.layerMask = 1;
        }
    }

    void Update(){
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("stayDown")){
            menuButtonHandler.tutorialUp = false;
        }
        if(Input.GetMouseButtonDown(0)){
            menuButtonHandler.tutorialUp = true;
            makeTutorialGoDown();
        }
    }

    void OnMouseDown(){

    }

}
