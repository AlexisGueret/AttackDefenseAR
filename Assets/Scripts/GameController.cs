using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using TMPro;

public enum GameState
{
    Initializing,
    NotPlaying,
    Playing,
    InitializingMaze,
    PlayingMaze,
    GameEnded

}

public class GameController : MonoBehaviour
{
    [SerializeField]
    private bool AR_MODE;

    [SerializeField]
    GameObject attackerPrefab, ballPrefab, defenderPrefab, ARButton;

    [SerializeField]
    private Text enemyScoreText, playerScoreText;   

    [SerializeField]
    Slider attackerEnergySlider, defenderEnergySlider;

    [SerializeField]
    GameObject endPanel, pointAnimation;

    [SerializeField]
    TMP_Text endText;

    [SerializeField]
    private Material enemyMat, playerMat, enemyColor, playerColor;

    [SerializeField]
    GameData gameData;

    [SerializeField]
    Button startButton;

    [SerializeField]
    MeshRenderer[] attackerSide, defenderSide;

    [SerializeField]
    Image energyFillColorAttacker, energyFillColorDefender;

    private TimeHandler timeHandler;
    private GameObject fieldObject;
    private int gameNumber = 0;
    private GameState gameState;
    private float attackerEnergy = 0, defenderEnergy = 0, playerScore, enemyScore;
    private bool isFieldPlaced = false;
    private BallSpawnZone spawnZone;
    private NavMeshSurface surface;
    private static int GAME_DURATION = 140, MAZE_DURATION = 50, NUMBER_OF_GAMES = 4;
    private Camera mainCam;
    private TapToPlaceGameField tapHandlerAR;
    private LayerMask zoneLayer;


    // Start is called before the first frame update
    private void Awake()
    {
        surface = FindObjectOfType<NavMeshSurface>();
        fieldObject = GameObject.FindGameObjectWithTag("Field");
        mainCam = Camera.main;
        spawnZone = FindObjectOfType<BallSpawnZone>();
        tapHandlerAR = FindObjectOfType<TapToPlaceGameField>();
        timeHandler = FindObjectOfType<TimeHandler>();
        zoneLayer = LayerMask.GetMask("ZoneLayer");
        SetGameColors();

        if (AR_MODE)
        {
            this.startButton.gameObject.SetActive(false);
            Screen.orientation = ScreenOrientation.AutoRotation;
        }
        else
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
#if PLATFORM_ANDROID
            ARButton.SetActive(true);
#endif
            surface.BuildNavMesh();
        }
    }


    public GameState GetGameState()
    {
        return this.gameState;
    }


    /// <summary>
    /// Start the maze mini-game
    /// First we generate the maze object, then creation of the attacker and finally we start the timer
    /// </summary>
    public void StartMaze()
    {
        this.SetGameColors();

        this.gameState = GameState.InitializingMaze;
        FindObjectOfType<MazeSpawner>().GenerateMaze();
        var spawnPosition = GameObject.FindGameObjectWithTag("MazeSpawnPosition").transform.position;
        GameObject.Instantiate(attackerPrefab, spawnPosition, Quaternion.identity);
        this.gameState = GameState.PlayingMaze;
        if(timeHandler == null)
            timeHandler = FindObjectOfType<TimeHandler>();
        timeHandler.StartTime(MAZE_DURATION);
    }

    public void RotateField(float rotationValue)
    {
        if(this.gameState != GameState.Playing)
            fieldObject.transform.Rotate(0,rotationValue,0,Space.World);
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetARMode()
    {
        SceneManager.LoadScene("ARGame");
    }


    public void SetNormalMode()
    {
        SceneManager.LoadScene("NormalGame");
    }


    /// <summary>
    /// Function called by the Input Manager when a touch is recognized
    /// In AR mode, we first want to put the football field on a plane.
    /// After that, both modes will wait for the user to press the start button.
    /// Once that button is pressed, the game can start and the players will be able to create attackers and defenders through raycasts.
    /// </summary>
    public void HandleTouch(Vector2 touchPosition)
    {
        if (AR_MODE && !isFieldPlaced)
        {
            isFieldPlaced = tapHandlerAR.InitializeField(touchPosition);
            if (isFieldPlaced)
                startButton.gameObject.SetActive(true);
            return;
        }

        if (this.gameState !=GameState.Playing)
            return;

        RaycastHit hit;
        Ray ray = mainCam.ScreenPointToRay(touchPosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity,zoneLayer))
        {
            if (hit.transform.tag == "AttackerField")
            {
                    if (attackerEnergy >= gameData.attackerEnergyCost)
                    {
                        Instantiate(attackerPrefab, hit.point, Quaternion.identity).transform.SetParent(fieldObject.transform);
                        attackerEnergy -= gameData.attackerEnergyCost;
                        UpdateEnergyDisplay();
                    }
                    else
                    {
                        //print message attacker doesn't have enough energy
                    }
                }
                else
                {
                if(hit.transform.tag == "DefenderField")
                {
                    if (defenderEnergy >= gameData.defenderEnergyCost)
                    {
                        Instantiate(defenderPrefab, hit.point, Quaternion.identity).transform.SetParent(fieldObject.transform);
                        defenderEnergy -= gameData.defenderEnergyCost;
                        UpdateEnergyDisplay();
                    }
                    else
                    {
                        //print message defender doesn't have enough energy
                    }
                }
            }
        }
    }


    /// <summary>
    /// Called by the start Button
    /// </summary>
    public void StartGame()
    {
        if(this.gameState !=GameState.Playing)
        {
            timeHandler.StartTime(GAME_DURATION);
            this.gameState = GameState.Playing;
            StartCoroutine(AttackerEnergy());
            StartCoroutine(DefenderEnergy());
            startButton.gameObject.SetActive(false);
            
            SpawnBall();
        }       
    }

   
    private void SpawnBall()
    {
        var spawnPosition = spawnZone.GetSpawnPosition();
        if(AR_MODE)
            spawnPosition.y = fieldObject.transform.position.y+0.05f;
        else
            spawnPosition.y = fieldObject.transform.position.y + 0.5f;
        GameObject.Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
    }

    /// <summary>
    /// Coroutine that handles the attacker's energy
    /// </summary>
    IEnumerator AttackerEnergy()
    {
        while(this.gameState == GameState.Playing)
        {
            yield return new WaitForSeconds(1);
            if (attackerEnergy + gameData.attackerEnergyRegeneration <= gameData.energyBar && this.gameState == GameState.Playing)
            {
                attackerEnergy += gameData.attackerEnergyRegeneration;
                UpdateEnergyDisplay();
            }
                
        }        
    }

    /// <summary>
    /// Coroutine that handles the defender's energy
    /// </summary>
    IEnumerator DefenderEnergy()
    {
        while (this.gameState == GameState.Playing)
        {
            yield return new WaitForSeconds(1);
            if (defenderEnergy + gameData.defenderEnergyRegeneration <= gameData.energyBar && this.gameState == GameState.Playing)
            {
                defenderEnergy += gameData.defenderEnergyRegeneration;
                UpdateEnergyDisplay();
            }
        }
    }

    public bool GetIsARMode()
    {
        return this.AR_MODE;
    }


    /// <summary>
    /// Fills the energy bars
    /// </summary>
    private void UpdateEnergyDisplay()
    {
        attackerEnergySlider.value = attackerEnergy;
        defenderEnergySlider.value = defenderEnergy;
    }



    #region Point scoring methods
    public void AttackScorePoint()
    {
        timeHandler.StopTime();

        //if gameNumber%2 == 0 => the player is the attacker else the player is the defender
        if (gameNumber % 2 == 0)
            playerScore++;
        else
            enemyScore++;

        PlayPointAnimation("ATTACK WINS !");
        UpdateScoreDisplay();
        ResetGame();
    }

    /// <summary>
    /// Called by the TimeHandler when no time is left
    /// If the player was in the maze mini-game, he has lost, otherwise the game is a Draw.
    /// </summary>
    public void EndTime()
    {
        if(this.gameState == GameState.PlayingMaze)
        {
            MazeVictory(false);
        }
        else
        {
            PlayPointAnimation("DRAW");
            ResetGame();
        }
        
    }

    public void MazeVictory(bool victory)
    {
        timeHandler.StopTime();
        if (victory)
        {
            playerScore++;
            PlayerWins();
        }           
        else
        {
            enemyScore++;
            EnemyWins();
        }
        this.gameState = GameState.GameEnded;
    }

    public void DefenseWin()
    {
        timeHandler.StopTime();

        if (gameNumber % 2 == 0)
            enemyScore++;
        else
            playerScore++;
        PlayPointAnimation("DEFENSE WINS !");
        UpdateScoreDisplay();
        ResetGame();
    }

    #endregion


    private void PlayPointAnimation(string s)
    {
        pointAnimation.GetComponent<TMP_Text>().text = s;
        pointAnimation.SetActive(false);
        pointAnimation.SetActive(true);
    }
    private void UpdateScoreDisplay()
    {
        playerScoreText.text = playerScore.ToString();
        enemyScoreText.text = enemyScore.ToString();
    }

    private void ResetGame()
    {
        this.gameState = GameState.NotPlaying;
        foreach (AttackerBehaviour attacker in GameObject.FindObjectsOfType<AttackerBehaviour>())
        {
            Destroy(attacker.gameObject);
        }
        foreach (DefenderBehaviour defender in GameObject.FindObjectsOfType<DefenderBehaviour>())
        {
            Destroy(defender.gameObject);
        }
        var ball = FindObjectOfType<BallBehaviour>();
        if (ball != null)
            Destroy(ball.gameObject);
        attackerEnergy = 0;
        defenderEnergy = 0;
        UpdateEnergyDisplay();
        if(CheckScore())
        {
            gameNumber++;
            SetGameColors();
            startButton.gameObject.SetActive(true);
            Debug.Log("Score :  " + playerScore + ": " + enemyScore);
        }
        
    }

    private bool CheckScore()
    {
        if (gameNumber >= NUMBER_OF_GAMES - 1 && playerScore == enemyScore)
        {
            gameNumber++;
            StartMaze();
            return false;
        }
        else
        {
            if (playerScore == 3)
            {
                PlayerWins();
                return false;
            }

            if (enemyScore == 3)
            {
                EnemyWins();
                return false;
            }
        }
        return true;
    }

   
        
    

    private void PlayerWins()
    {
        endPanel.SetActive(true);
        endText.text = "YOU WIN !";
    }

    private void EnemyWins()
    {
        endPanel.SetActive(true);
        endText.text = "YOU LOST !";
    }

    private void SetGameColors()
    {
        Material attackerMaterial, defenderMaterial, defenderCharacterMat, attackerCharacterMat;
        if(gameNumber%2==0)
        {

            //Player attacks
            attackerMaterial = playerColor;
            attackerCharacterMat = playerMat;

            defenderMaterial = enemyColor;
            defenderCharacterMat = enemyMat;

        }
        else
        {
            //Enemy ATTACKS
            attackerMaterial = enemyColor;
            attackerCharacterMat = enemyMat;

            defenderMaterial = playerColor;
            defenderCharacterMat = playerMat;
        }

        // Player ATTACKS
        

        foreach (MeshRenderer meshR in attackerSide)
        {
            meshR.material = attackerMaterial;
        }

        foreach (MeshRenderer meshR in defenderSide)
        {
            meshR.material = defenderMaterial;
        }

        attackerPrefab.GetComponent<AttackerBehaviour>().SetColor(attackerCharacterMat);
        defenderPrefab.GetComponent<DefenderBehaviour>().SetColor(defenderCharacterMat);

        energyFillColorAttacker.color = attackerMaterial.color;

        energyFillColorDefender.color = defenderMaterial.color;

    }
}
