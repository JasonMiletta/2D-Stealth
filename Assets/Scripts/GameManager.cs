using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float m_StartDelay = 3f;             // The delay between the start of RoundStarting and RoundPlaying phases.
    public float m_EndDelay = 3f;               // The delay between the end of RoundPlaying and RoundEnding phases..
    public Text m_MessageText;                  // Reference to the overlay Text to display winning text, etc.
    public PlayerCharacter player;              // Reference to the current player
    public string nextLevelSceneName;

    private int m_RoundNumber;                  // Which round the game is currently on.
    private WaitForSeconds m_StartWait;         // Used to have a delay whilst the round starts.
    private WaitForSeconds m_EndWait;           // Used to have a delay whilst the round or game ends
    private bool exitGoalReached;
    
    private void Start()
    {
        // Create the delays so they only have to be made once.
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        // Once the tanks have been created and the camera is using them as targets, start the game.
        StartCoroutine(GameLoop());
    }

    // This is called from start and will run each phase of the game one after another.
    private IEnumerator GameLoop()
    {
        // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
        yield return StartCoroutine(RoundStarting());

        // Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
        yield return StartCoroutine(RoundPlaying());

        // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
        yield return StartCoroutine(RoundEnding());

        // This code is not run until 'RoundEnding' has finished.  At which point, check if a game winner has been found.
        if (isGameOver())
        {
            // If there is a game winner, restart the level.
            if (nextLevelSceneName != null && nextLevelSceneName != "")
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            } else
            {
                SceneManager.LoadScene("Main Menu");
            }
        }
        else if (exitGoalReached)
        {
            if (nextLevelSceneName != null && nextLevelSceneName != "")
            {
                SceneManager.LoadScene(nextLevelSceneName);
            } else
            {
                SceneManager.LoadScene("Main Menu");
            }
        }
        else
        {
            // If there isn't a winner yet, restart this coroutine so the loop continues.
            // Note that this coroutine doesn't yield.  This means that the current version of the GameLoop will end.
            StartCoroutine(GameLoop());
        }
    }


    private IEnumerator RoundStarting()
    {
        // Increment the round number and display text showing the players what round it is.
        m_RoundNumber++;
        m_MessageText.text = SceneManager.GetActiveScene().name;

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return m_StartWait;
    }


    private IEnumerator RoundPlaying()
    {
        // Clear the text from the screen.
        m_MessageText.text = string.Empty;

        // While there is not one tank left...
        while (!isGameOver() && !exitGoalReached)
        {
            // ... return on the next frame.
            yield return null;
        }
    }


    private IEnumerator RoundEnding()
    {
        // Get a message based on the scores and whether or not there is a game winner and display it.
        string message = EndMessage();
        m_MessageText.text = message;

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return m_EndWait;
    }
   
    private bool isGameOver()
    {
        return (!exitGoalReached && (!player || !player.isActiveAndEnabled));
    }
    
    public void setExitGoalReached()
    {
        exitGoalReached = true;
        player.freezePlayer();
    }

    // Returns a string message to display at the end of each round.
    private string EndMessage()
    {
        if (isGameOver())
        {
            return "Game Over!";
        } else if (exitGoalReached)
        {
            return "GOAL!";
        }
        return "";
    }
}
