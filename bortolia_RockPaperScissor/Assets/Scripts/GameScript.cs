using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour
{
    [Header("Set in Inspector")]

    public Text YouScoreGT;
    public Text ComputerScoreGT;
    public Text GamesPlayedGT;
    //Add text after win/lose of each round
    public Text ResultText;
    public Text DescriptionText;
    //Images
    public Sprite RockImage, PaperImage, ScissorImage;
    public Image YouImage, ComputerImage;

    private string state = "none";
    private int randomNum;
    private string playerChoice = "";
    private string computerChoice = "";
    private int gamesPlayed = 0;
    private int youScore = 0;
    private int computerScore = 0;
    private readonly string[] computerArray = {"rock", "paper", "scissor"};
    private Color YouImageColor;
    private Color ComputerImageColor;

    // Start is called before the first frame update
    void Start()
    {
        //reference to scores text in-game objects
        GameObject YouScoreGO = GameObject.Find("YouScore");
        GameObject ComputerScoreGO = GameObject.Find("ComputerScore");
        GameObject GamesPlayedGO = GameObject.Find("GamesPlayed");

        YouImageColor = YouImage.GetComponent<Image>().color;
        YouImageColor.a = 0f; // Color for player default (white) and transparent
        YouImage.GetComponent<Image>().color = YouImageColor;

        ComputerImageColor = ComputerImage.GetComponent<Image>().color;
        ComputerImageColor = new Color32(255, 85, 66, 0); // Color for computer is red and transparent
        ComputerImage.GetComponent<Image>().color = ComputerImageColor;
        ComputerImage.rectTransform.rotation = Quaternion.Euler(0f, 180f, 0f); //Rotate Computer image

        // Gets the Text component of the GameObject
        YouScoreGT = YouScoreGO.GetComponent<Text>();
        ComputerScoreGT = ComputerScoreGO.GetComponent<Text>();
        GamesPlayedGT = GamesPlayedGO.GetComponent<Text>();
        ResultText.text = "";
        DescriptionText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(state == "none") {

            if(gamesPlayed == 10) {
                //After 10 games are played, finds all buttons and disables them, then restarts game after 6 seconds
                GameObject[] allButtons = GameObject.FindGameObjectsWithTag("UserButton");
                foreach(GameObject obj in allButtons) {
                    obj.GetComponent<Button>().interactable = false;
                }
                if (youScore > computerScore) {
                    ResultText.text = "You won the game!";
                    DescriptionText.text = "GAME OVER";
                    Invoke("EndGame", 6f);
                }
                else if(youScore < computerScore) {
                    ResultText.text = "You lost the game.";
                    DescriptionText.text = "GAME OVER";
                    Invoke("EndGame", 6f);
                }
                else {
                    ResultText.text = "You and the computer tied the game";
                    DescriptionText.text = "GAME OVER";
                    Invoke("EndGame", 6f);
                }
            }

        }

        else if(state == "chose") {

            DisplayChoices(playerChoice, computerChoice);

            // Change the state in Update() to commence evaluation of the current turn
            state = "evaluate";
        }

        else if(state == "evaluate") {
            if(playerChoice != computerChoice) {

                if((playerChoice == "rock" && computerChoice == "scissor") ||
                (playerChoice == "scissor" && computerChoice == "paper") ||
                (playerChoice == "paper" && computerChoice == "rock")){

                    ResultText.text = "YOU WIN";
                    DescriptionText.text = char.ToUpper(playerChoice[0]) + playerChoice.Substring(1) + " beats " + char.ToUpper(computerChoice[0]) + computerChoice.Substring(1);

                    youScore += 1;
                    YouScoreGT.text = "Your Score: " + youScore.ToString();
                }
                else {
                    ResultText.text = "YOU LOSE";
                    DescriptionText.text = char.ToUpper(playerChoice[0]) + playerChoice.Substring(1) + " loses to " + char.ToUpper(computerChoice[0]) + computerChoice.Substring(1);

                    computerScore += 1;
                    ComputerScoreGT.text = "Computer Score: " + computerScore.ToString();
                }
            }
            else {
                ResultText.text = "TIE GAME";
                DescriptionText.text = "";
            }

            gamesPlayed++;
            GamesPlayedGT.text = "Games Played: " + gamesPlayed.ToString();
            state = "none";
        }

    }

    public void ButtonChoice(string choice)
    {
        // The onClick from each buttom passes rock, paper or scissor to the choice variable
        playerChoice = choice;
        // A random number is generated between 0 and 2 for computerArray
        randomNum = Random.Range(0, 3);
        computerChoice = computerArray[randomNum];

        // Changes state to access the chose state in Update()
        state = "chose";
    }

    public void DisplayChoices(string player, string computer)
    {   
        switch (playerChoice) {
            case "rock":
                YouImage.GetComponent<Image>().sprite = RockImage;
                YouImage.rectTransform.sizeDelta = new Vector2(325.5f, 286.85f);
                break;
            case "paper":
                YouImage.GetComponent<Image>().sprite = PaperImage;
                YouImage.rectTransform.sizeDelta = new Vector2(340.5f, 300.81f);
                break;
            case "scissor":
                YouImage.GetComponent<Image>().sprite = ScissorImage;
                YouImage.rectTransform.sizeDelta = new Vector2(340.5f, 241.77f);
                break;
            default:
                break;
        }

        switch (computerChoice) {
            case "rock":
                ComputerImage.GetComponent<Image>().sprite = RockImage;
                ComputerImage.rectTransform.sizeDelta = new Vector2(325.5f, 286.85f);
                break;
            case "paper":
                ComputerImage.GetComponent<Image>().sprite = PaperImage;
                ComputerImage.rectTransform.sizeDelta = new Vector2(340.5f, 300.81f);
                break;
            case "scissor":
                ComputerImage.GetComponent<Image>().sprite = ScissorImage;
                ComputerImage.rectTransform.sizeDelta = new Vector2(340.5f, 241.77f);
                break;
            default:
                break;
        }

        // Changing the transparency (a) component to be visible
        YouImageColor.a = 1f;
        YouImage.GetComponent<Image>().color = YouImageColor;
        ComputerImageColor.a = 1f;
        ComputerImage.GetComponent<Image>().color = ComputerImageColor;
    }

    public void EndGame()
    {
        SceneManager.LoadScene("_Scene_0");
    }

}
