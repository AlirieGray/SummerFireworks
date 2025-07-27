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

    // TODO this should come from mixing level via gameManager
    private int fireworksCreated;
    private int targetsPlayed;

    // score values
    public int perfectScoreValue;
    public int okScoreValue;

    void Start()
    {
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

        rocketDirection = Direction.Center;
        perfectText = perfectGO.GetComponent<DisplayText>();
        okText = okGO.GetComponent<DisplayText>();
        missText = missGO.GetComponent<DisplayText>();
        targetDirection = Direction.Right;
        leftLocation = new Vector3(-5.72f, 1.72f, -.3f);
        centerLocation = new Vector3(0, 1.72f, -.3f);
        rightLocation = new Vector3(5.72f, 1.72f, -.3f);
        ringCenter = rightLocation;
        inTutorial = false;
        StartLevel();
    }

    void StartLevel()
    {
        if (levelManager.GetCurrentCycle() == 0)
        {
            Debug.Log("tutorial!");
            inTutorial = true;
        }
        textHandler.Countdown();
        StartCoroutine(Countdown());
        SetUpFireworks();
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(1.5f);
        currentTarget.SetActive(true);
        currentRing.gameObject.SetActive(true);
    }

    void SetUpFireworks()
    {

    }

    void Launch()
    {
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
        } else
        {
            EndLevel();
        }

    }

    void EndLevel()
    {
        // show little display of everything!
        // then go to resource gathering level
        foreach (GameObject fireworks in allFireworks)
        {

        }

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
        int r = Random.Range(0, 2);
        GameObject ringGO;
        switch (r)
        {
            case 0:
                targetDirection = Direction.Left;
                currentTarget = Instantiate(targetPrefab, leftLocation, Quaternion.identity);
                ringGO = Instantiate(ringPrefab, 
                    new Vector3(leftLocation.x, leftLocation.y, -1f), Quaternion.identity);
                
                ringCenter = leftLocation;
                currentRing = ringGO.GetComponent<RingController>();
                break;
            case 1:
                targetDirection = Direction.Center;
                currentTarget = Instantiate(targetPrefab, centerLocation, Quaternion.identity);
                ringGO = Instantiate(ringPrefab,
                    new Vector3(centerLocation.x, centerLocation.y, -1f), Quaternion.identity);
                ringCenter = centerLocation;
                currentRing = ringGO.GetComponent<RingController>();
                break;
            case 2:
                targetDirection = Direction.Right;
                currentTarget = Instantiate(targetPrefab, rightLocation, Quaternion.identity);
                ringGO = Instantiate(ringPrefab, 
                    new Vector3(rightLocation.x, rightLocation.y, -1f), Quaternion.identity);
                ringCenter = rightLocation;
                currentRing = ringGO.GetComponent<RingController>();
                break;
        }
    }

    void DestroyRingAndTarget()
    {
        Destroy(currentRing.gameObject);
        Destroy(currentTarget.gameObject);
    }

    void SpawnFireworks()
    {
        // move currentFirework index
        // if not at the end then get the next prefab
        // set the color and move the index
        // instantiate
        List<ResourceScriptableObject> resources = gameManager.GetFinishedFireworks()[fireworksIndex];
        List<GameObject> fireworksToSpawn = new List<GameObject>();
        int spawnIndex = 0;

        foreach (ResourceScriptableObject resource in resources) {
            // go through all the resource 
            // if it's a shape, add a new prefab and increment spawnIndex
            // if a color, mix colors and add to prefab
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
                    Debug.Log("got star!");
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
                0.122f, -3.643f, -.5f), Quaternion.Euler(-90f, 0f,0f));
            allFireworks.Add(firework);
        
        }
        audioManager.PlayFireworks();
        
        fireworksIndex++;
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
