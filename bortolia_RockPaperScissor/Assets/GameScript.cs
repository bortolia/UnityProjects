using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    [Header("Set in Inspector")]

    public Text YouScoreGT;
    public Text ComputerScoreGT;
    public Text GamesPlayedGT;
    //prolly gonna delete these
    //public int YouScore = 0;
    //public int ComputerScore = 0;
    //public int GamesPlayed = 0;

    private string state = "free";
    private int randomNum;
    private string playerChoice = "";
    private int gamesPlayed = 0;
    private int youScore = 0;
    private int computerScore = 0;
    private readonly string[] computerChoice = {"rock", "paper", "scissor"};

    // Start is called before the first frame update
    void Start()
    {
        //reference to scores text in-game objects
        GameObject YouScoreGO = GameObject.Find("YouScore");
        GameObject ComputerScoreGO = GameObject.Find("ComputerScore");
        GameObject GamesPlayedGO = GameObject.Find("GamesPlayed");

        // Gets the Text component of the GameObject
        YouScoreGT = YouScoreGO.GetComponent<Text>();
        ComputerScoreGT = ComputerScoreGO.GetComponent<Text>();
        GamesPlayedGT = GamesPlayedGO.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == "chosen") {

            youScore += 1;
            computerScore += 1;
            gamesPlayed += 1;

            GamesPlayedGT.text = "Games Played: " + gamesPlayed;
            YouScoreGT.text = "Your Score: " + youScore;
            ComputerScoreGT.text = "Computer Score: " + computerScore;

            print("The computer chose: " + computerChoice[randomNum]);

            state = "free";
            //randomNum = Random.Range(0, 3);
        }
    }

    public void ButtonChoice(string choice)
    {
        state = "chosen";
        //randomNum = Random.Range(0, 3);

        if (choice == "rock") {
            playerChoice = "rock";
            randomNum = Random.Range(0, 3);
        }
        else if (choice == "paper") {
            playerChoice = "paper";
            randomNum = Random.Range(0, 3);
        }
        else if (choice == "scissor") {
            playerChoice = "scissor";
            randomNum = Random.Range(0, 3);
        }
    }

}
