using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex_Light : MonoBehaviour
{

    SpriteRenderer spriteRenderer;
    public Sprite[] sprites = new Sprite[1];

    void Start(){
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void enableHoverLight(){
        spriteRenderer.sprite = sprites[1];
    }

    void disableHoverLight(){
        spriteRenderer.sprite = sprites[0];
    }

}
