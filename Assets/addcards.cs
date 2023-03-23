using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addcards : MonoBehaviour
{
    //creating a puzzlefield varibale
    [SerializeField]
    private Transform puzzleField;

    //creating the card buttons variable
    [SerializeField]
    private GameObject button;

    //runs a single method named awake
    void Awake(){

        //runs a for loop that starts at 0 and increments by 1 every loop but stops at the 16th loop
        for(int i = 0; i < 16; i++){

            //instantiates the button as a game object
            GameObject btn = Instantiate(button);

            //names the button as the i value which gets incremented
            btn.name = "" + i;

            //sets the parent to the puzzlefield
            btn.transform.SetParent(puzzleField, false);
        }
    }
}
