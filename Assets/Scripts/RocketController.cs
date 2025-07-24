using UnityEngine;

public class RocketController : MonoBehaviour
{
    public enum Direction
    {
        Center, Left, Right 
    };
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
    public GameObject fireworks;
    private Vector3 ringCenter;
    public Direction rocketDirection;
    private Direction targetDirection;
    private Vector3 leftLocation;
    private Vector3 rightLocation;
    private Vector3 centerLocation;

    void Start()
    {
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
        leftLocation = new Vector3(-0.84f, 0.27f, 0);
        centerLocation = new Vector3(0, 0.27f, 0f);
        rightLocation = new Vector3(0.84f, 0.27f, 0f);
        ringCenter = rightLocation;
    }

    void Launch()
    {
        if (ringInPerfectZone && rocketDirection == targetDirection)
        {
            perfectText.DisplayWithShrink();
            // create dazzling particle effect
            SpawnFireworks();
        }
        else if (ringInOkZone && rocketDirection == targetDirection) {
            okText.DisplayWithShrink();
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

    void SpawnNewRingAndTarget()
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
                ringGO = Instantiate(ringPrefab, leftLocation, Quaternion.identity);
                ringCenter = leftLocation;
                currentRing = ringGO.GetComponent<RingController>();
                break;
            case 1:
                targetDirection = Direction.Center;
                currentTarget = Instantiate(targetPrefab, centerLocation, Quaternion.identity);
                ringGO = Instantiate(ringPrefab, centerLocation, Quaternion.identity);
                ringCenter = centerLocation;
                currentRing = ringGO.GetComponent<RingController>();
                break;
            case 2:
                targetDirection = Direction.Right;
                currentTarget = Instantiate(targetPrefab, rightLocation, Quaternion.identity);
                ringGO = Instantiate(ringPrefab, rightLocation, Quaternion.identity);
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
        GameObject fireworksClone = Instantiate(fireworks, ringCenter, Quaternion.identity);
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
