using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Gamecontroller : MonoBehaviour
{

    //creates the card backing variable to populate it
    [SerializeField]
    private Sprite cardBack;

    //creates the array of cards
    public Sprite[] crads;

    //creates a dynamic list called gamepieces filled with sprites
    public List<Sprite> gamePieces = new List<Sprite>();

    //creates a dynamic list called btns which is filled with buttons
    public List<Button> btns = new List<Button>();

    //creates first and second guess variables that begin false as they are not initialized to true
    private bool firstGuess;
    private bool secondGuess;

    //creates different int variables to adjust loops, indexs and guesses
    private int countGuesses;
    private int countCorrectGuesses;
    private int overallGameGuesses;
    private int firstGuessIndex;
    private int secondGuessIndex;

    //creates variables for the name of the image that the user firstly and secondly guesses
    private string firstGuessName;
    private string secondGuessName;

    //performs the awake function which loads all cards from the card fronts folder and stores them in their respective card objects
    void Awake(){
        crads = Resources.LoadAll<Sprite>("CardSprites/cardFronts");
    }

    //on start of the page the following methods are ran
    void Start(){
        GetButtons();
        AddAListener();
        AddGamePieces();

        //randomizes the pieces using the game pieces variable
        RandomizeGame(gamePieces);

        //the overall game guess is updated as the count of game pieces divided by 2 (8)
        overallGameGuesses = gamePieces.Count / 2;
    }

    //this method runs through the card backs and populates them against the objects found
    void GetButtons(){
        //searchs for game objects with the tag PuzzleButton
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");

        //runs through a for loop for the length of the objects array
        for(int i = 0; i < objects.Length; i++){

            //adds the array of buttons to the list of btns
            btns.Add(objects[i].GetComponent<Button>());

            //populate the btns objects with the images of the card backs
            btns[i].image.sprite = cardBack;
        }
    }

    //adds the game pieces to the gamepieces list
    void AddGamePieces(){

        //creates a loop variable of the size of the btns list
        int loop = btns.Count;

        //creates an index handler that starts at 0
        int indexHandler = 0;

        //creates a for loop that will not run longer than the size of the btns list
        for(int i = 0; i < loop; i++){

            //if the index handler is equal to the original number of images presented then set index handler to 0
            if(indexHandler == loop / 2){
                indexHandler = 0;
            }

            //add the card at the indexhandler variable to the gamepieces list 
            gamePieces.Add(crads[indexHandler]);

            //increment the index handler by 1
            indexHandler ++;
        }
    }

    //creating my own listener method
    void AddAListener(){

        //for each button in the btns list
        foreach(Button btn in btns){

            //add the on click listener and set it to the choose a puzzle method
            btn.onClick.AddListener(() => ChooseAPuzzle());
        }
    }

    //creates a choose a puzzle method
    public void ChooseAPuzzle(){

        //stores the neame of the selected card in the cardName variable (card number)
        string cardName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        
        //Helped in testing as I was able to see in the console log which card name was being selected
        Debug.Log("Button is being clicked! The Button is " + cardName);

        //if the first guess is chosen
        if(!firstGuess){

            //set the variable to true
            firstGuess = true;

            //parse the name of the button (as it is a number) to an int to store in the firstguess in the variable
            firstGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            
            //store the name of the first guess into the firstguessname variable
            firstGuessName = gamePieces[firstGuessIndex].name;

            //set the sprite of the first guessed card to the image stored in that given gamepiece
            btns[firstGuessIndex].image.sprite = gamePieces[firstGuessIndex];
        }

        //if the second guess is chosen
        else if(!secondGuess){

            //set the second guess to true
            secondGuess = true;

            //parse the name of the button (as it is a number) to an int to store in the secondguess in the variable
            secondGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            
            //store the name of the second guess into the secondguessname variable
            secondGuessName = gamePieces[secondGuessIndex].name;
            
            //set the sprite of the second guessed card to the image stored in that given gamepiece
            btns[secondGuessIndex].image.sprite = gamePieces[secondGuessIndex];

            //increment the count guesses
            countGuesses++;

            //run the DoPiecesMatch IEnumerator to see if the pieces are identical
            StartCoroutine(DoPiecesMatch());
        }
    }

    //creates a special IEnumerator method that checks if the pieces chosen match
    IEnumerator DoPiecesMatch(){

        //waits a specific amount of time
        yield return new WaitForSeconds(1f);

        //if the names of both guesses are the same
        if(firstGuessName == secondGuessName){

            //wait timer which is half of the original time
            yield return new WaitForSeconds(.5f);

            //btns wont be able to be clicked after the user guesses correct
            btns[firstGuessIndex].interactable = false;
            btns[secondGuessIndex].interactable = false;

            //btns wont be able to see btns after the user guesses correct
            btns[firstGuessIndex].image.color = new Color(0,0,0,0);
            btns[secondGuessIndex].image.color = new Color(0,0,0,0);

            //check if game is completed
            GameComplete();
        }

        //if the names are not the same of both the guesses
        else{

            //wait for half the starting time
            yield return new WaitForSeconds(.5f);

            //turns the cards back over (so you cant see what they are)
            btns[firstGuessIndex].image.sprite = cardBack;
            btns[secondGuessIndex].image.sprite = cardBack;
        }

        //waits again for half the starting time
        yield return new WaitForSeconds(.5f);

        //sets the first and second guess variables back to false so the user can guess again
        firstGuess = false;
        secondGuess = false;

    }

    //check if the game is complete
    void GameComplete(){

        //increment the correct guesses variable
        countCorrectGuesses++;

        //if the correct guesses matches the overall gamne guess variable then the user wins
        if(countCorrectGuesses == overallGameGuesses){

            //A winning message is displayed in the console log
            Debug.Log("You did it! You won!");
        }
    }

    //randomize the game board passing through the given list
    void RandomizeGame(List<Sprite> list){

        //runs a for loop that does not end until the end of the list size and increments each loop
        for(int i = 0; i <list.Count; i++){

            //create a temporary value of a Sprite in the array using the i variable in the for loop
            Sprite tempvar = list[i];

            //creates a random number from the range, from i to the size of the list and stores it in randomIndex
            int randomIndex = Random.Range(i, list.Count);

            //the list at spot i is instead the randomized index
            list[i] = list[randomIndex];

            //the random index of the list is the temporary value
            list[randomIndex] = tempvar;
        }

    }
}
