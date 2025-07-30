using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class RocketController : MonoBehaviour
{
    public enum Direction
    {
        Center, Left, Right 
    };
    public TextHandler textHandler;
    private InputSystem_Actions inputActions;
    private bool ringInPerfectZone;
    private bool ringInOkZone;
    public GameObject perfectGO;
    public GameObject okGO;
    public GameObject missGO;
    private DisplayText perfectText;
    private DisplayText okText;
    private DisplayText missText;
    public RingController currentRing;
    public GameObject currentTarget;
    public GameObject targetPrefab;
    public GameObject ringPrefab;
    public GameObject baseFireworks;
    public GameObject circleFireworks;
    public GameObject heartFireworks;
    public GameObject starFireworks;
    private Vector3 ringCenter;
    public Direction rocketDirection;
    private Direction targetDirection;
    private Vector3 leftLocation;
    private Vector3 rightLocation;
    private Vector3 centerLocation;
    private AudioManager audioManager;
    private GameManager gameManager;
    private LevelManager levelManager;
    private bool inTutorial;
    private int fireworksIndex;
    private List<GameObject> allFireworks;
    private TutorialHandler tutorial;
    private bool checkForDrag;
    private Vector3 startPos;

    // TODO this should come from mixing level via gameManager
    private int fireworksCreated;
    private int targetsPlayed;

    // score values
    public int perfectScoreValue;
    public int okScoreValue;

    void Start()
    {
        checkForDrag = false;
        fireworksIndex = 0;
        allFireworks = new List<GameObject>();
        audioManager = FindFirstObjectByType<AudioManager>();
        gameManager = FindFirstObjectByType<GameManager>();
        levelManager = FindFirstObjectByType<LevelManager>();
        inputActions = new InputSystem_Actions();
        inputActions.Rocket.Enable();
        inputActions.Rocket.Launch.performed += context =>
        {
            Launch();
        };
        inputActions.Rocket.RotateRight.performed += context =>
        {
            HandleRotateRight();
        };
        inputActions.Rocket.RotateLeft.performed += context =>
        {
            HandleRotateLeft();
        };
        inputActions.Rocket.DragMouse.started += context =>
        {
            checkForDrag = true;
            startPos = GameManager.manager.CursorWorldPosition();
        };
        inputActions.Rocket.DragMouse.canceled += context =>
        {
            Debug.Log("cancel???");
            checkForDrag = false;
            Launch();
        };

        rocketDirection = Direction.Center;
        perfectText = perfectGO.GetComponent<DisplayText>();
        okText = okGO.GetComponent<DisplayText>();
        missText = missGO.GetComponent<DisplayText>();
        targetDirection = Direction.Center;
        leftLocation = new Vector3(-5.72f, 0.05f, -.3f);
        centerLocation = new Vector3(0, 0.05f, -.3f);
        rightLocation = new Vector3(5.72f, 0.05f, -.3f);
        ringCenter = rightLocation;
        inTutorial = false;
        if (levelManager.GetCurrentCycle() == 0 && !gameManager.playedFireworksTutorial)
        {
            inTutorial = true;
            tutorial = FindFirstObjectByType<TutorialHandler>();
            if (tutorial != null)
            {
                gameManager.playedFireworksTutorial = true;
                tutorial.StartTutorial();
            }
            else
            {
                StartLevel();
            }
        } else
        {
            StartLevel();
        }
    }

    private void Update()
    {
        if (checkForDrag) {
            
            var xDiff = GameManager.manager.CursorWorldPosition().x - startPos.x;
            if (xDiff >= .5f)
            {
                HandleRotateRight();
            } else if (xDiff <= -.5)
            {
                HandleRotateLeft();
            } else
            {
                HandleRotateCenter();
            }
        }       
    }



    public void StartLevel()
    {
        textHandler.Countdown();
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(1.5f);
        currentTarget.SetActive(true);
        currentRing.gameObject.SetActive(true);
        inTutorial = false;
    }

    void Launch()
    {
        if (inTutorial)
        {
            return;
        }
        textHandler.ResetAll();
        if (fireworksIndex < gameManager.GetFinishedFireworks().Count)
        {
            if (ringInPerfectZone && rocketDirection == targetDirection)
            {
                perfectText.DisplayWithShrink();
                gameManager.IncreaseScore(perfectScoreValue);
                textHandler.UpdateScore(gameManager.GetScore());

                // create dazzling particle effect
                SpawnFireworks();
            }
            else if (ringInOkZone && rocketDirection == targetDirection) {
                okText.DisplayWithShrink();
                gameManager.IncreaseScore(okScoreValue);
                textHandler.UpdateScore(gameManager.GetScore());

                // create nice particle effect
                SpawnFireworks();
            } else
            {
                missText.DisplayWithShake();
                // create mediocre particle effect
            }
            ringInOkZone = false;
            ringInPerfectZone = false;

            SpawnNewRingAndTarget();
        } 

    }

    IEnumerator EndLevel()
    {
        float delay = 0.3f;
        yield return new WaitForSeconds(0.5f);
        foreach (GameObject fireworks in allFireworks)
        {
            Instantiate(fireworks, new Vector3(0f, -4.5f, -.5f), Quaternion.Euler(0f, 0f, 0f));
            yield return new WaitForSeconds(delay);
        }

        yield return new WaitForSeconds(2.5f);
        levelManager.LoadNextLevel();
    }

    void HandleRotateRight()
    {
        switch (rocketDirection)
        {
            case Direction.Left:
                gameObject.transform.rotation = Quaternion.Euler(0, 0, gameObject.transform.rotation.eulerAngles.z - 30);
                rocketDirection = Direction.Center;
                break;
            case Direction.Center:
                gameObject.transform.rotation = Quaternion.Euler(0, 0, gameObject.transform.rotation.eulerAngles.z - 30);
                rocketDirection = Direction.Right;
                break;
            case Direction.Right:
                break;
        }
    }

    void HandleRotateCenter()
    {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            rocketDirection = Direction.Center;
    }

    void HandleRotateLeft()
    {
        switch (rocketDirection)
        {
            case Direction.Center:
                gameObject.transform.rotation = Quaternion.Euler(0, 0, gameObject.transform.rotation.eulerAngles.z + 30);
                rocketDirection = Direction.Left;
                break;
            case Direction.Right:
                gameObject.transform.rotation = Quaternion.Euler(0, 0, gameObject.transform.rotation.eulerAngles.z + 30);
                rocketDirection = Direction.Center;
                break;
            case Direction.Left:
                break;
        }
    }

    public void SpawnNewRingAndTarget()
    {
        // set new center of ring to instantiate the fireworks location
        DestroyRingAndTarget();
        if (fireworksIndex >= gameManager.GetFinishedFireworks().Count)
        {
            return;
        }
        int r = Random.Range(0, 2);
        GameObject ringGO;
        Vector3 locationToSpawn;
        switch (r)
        {
            case 0:
                if (targetDirection == Direction.Left)
                {
                    targetDirection = Direction.Center;
                    locationToSpawn = centerLocation;
                } else
                {
                    targetDirection = Direction.Left;
                    locationToSpawn = leftLocation;
                }
                currentTarget = Instantiate(targetPrefab, locationToSpawn, Quaternion.identity);
                ringGO = Instantiate(ringPrefab, 
                    new Vector3(locationToSpawn.x, locationToSpawn.y, -.4f), Quaternion.identity);
                
                ringCenter = locationToSpawn;
                currentRing = ringGO.GetComponent<RingController>();
                break;
            case 1:
                if (targetDirection == Direction.Center)
                {
                    targetDirection = Direction.Left;
                    locationToSpawn = leftLocation;
                } else
                {
                    targetDirection = Direction.Center;
                    locationToSpawn = centerLocation;
                }
                    currentTarget = Instantiate(targetPrefab, locationToSpawn, Quaternion.identity);
                ringGO = Instantiate(ringPrefab,
                    new Vector3(locationToSpawn.x, locationToSpawn.y, -.4f), Quaternion.identity);
                ringCenter = locationToSpawn;
                currentRing = ringGO.GetComponent<RingController>();
                break;
            case 2:
                if (targetDirection == Direction.Right)
                {
                    targetDirection = Direction.Left;
                    locationToSpawn = leftLocation;
                } else
                {
                    targetDirection = Direction.Right;
                    locationToSpawn = rightLocation;
                }
                currentTarget = Instantiate(targetPrefab, locationToSpawn, Quaternion.identity);
                ringGO = Instantiate(ringPrefab, 
                    new Vector3(locationToSpawn.x, locationToSpawn.y, -.4f), Quaternion.identity);
                ringCenter = locationToSpawn;
                currentRing = ringGO.GetComponent<RingController>();
                break;
        }
    }

    void DestroyRingAndTarget()
    {
        Destroy(currentRing.gameObject);
        Destroy(currentTarget.gameObject);

        if (fireworksIndex >= gameManager.GetFinishedFireworks().Count)
        {
            StartCoroutine(EndLevel());
        }
    }

    void SpawnFireworks()
    {
        List<ResourceScriptableObject> resources = gameManager.GetFinishedFireworks()[fireworksIndex];
        fireworksIndex++;
        List<GameObject> fireworksToSpawn = new List<GameObject>();
        int spawnIndex = 0;

        foreach (ResourceScriptableObject resource in resources) {
            switch (resource.shape)
            {
                case ResourceScriptableObject.Shape.None:
                    // set color
                    // TODO: MIX COLORS
                    if (fireworksToSpawn.Count == 0)
                    {
                        fireworksToSpawn.Add(baseFireworks);
                        //fireworksToSpawn[spawnIndex].GetComponent<Firework>().SetColor(resource.color);
                    }
                    fireworksToSpawn[spawnIndex].GetComponent<Firework>().SetColor(resource.color);
                    break;
                case ResourceScriptableObject.Shape.Starburst:
                    if (fireworksToSpawn.Count > 0)
                    {
                        spawnIndex++;
                    }
                    fireworksToSpawn.Add(baseFireworks);
                    break;
                case ResourceScriptableObject.Shape.Circle:
                    if (fireworksToSpawn.Count > 0)
                    {
                        spawnIndex++;
                    }
                    fireworksToSpawn.Add(circleFireworks);
                    break;
                case ResourceScriptableObject.Shape.Star:
                    if (fireworksToSpawn.Count > 0)
                    {
                        spawnIndex++;
                    }
                    fireworksToSpawn.Add(starFireworks);
                    break;
                case ResourceScriptableObject.Shape.Heart:
                    if (fireworksToSpawn.Count > 0)
                    {
                        spawnIndex++;
                    }
                    fireworksToSpawn.Add(heartFireworks);
                    break;
                default:
                    break;
            }
        }

        // go through all fireworksToSpawn and fan out the locations, instantiate all
        foreach (GameObject firework in fireworksToSpawn) {
            GameObject fireworksClone = Instantiate(firework, new Vector3(
                0f, -4.5f, -.5f), Quaternion.Euler(0f, 0f,0f));
            allFireworks.Add(firework);
        
        }
        audioManager.PlayFireworks();
    }

    public void SetPerfectZone(bool inPerfectZone)
    {
        ringInPerfectZone = inPerfectZone;
    }

    public void SetOkZone(bool inOkZone)
    {
        ringInOkZone = inOkZone;
    }
}
