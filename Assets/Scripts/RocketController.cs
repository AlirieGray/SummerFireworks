using TMPro;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    private bool ringInPerfectZone;
    private bool ringInOkZone;
    public GameObject perfectGO;
    public GameObject okGO;
    private TextMeshProUGUI perfectText;
    private TextMeshProUGUI okText;
    public RingController currentRing;
    public GameObject fireworks;
    private Vector3 ringCenter;

    void Start()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Rocket.Enable();
        inputActions.Rocket.Launch.performed += context =>
        {
            Launch();
        };
        okText = okGO.GetComponent<TextMeshProUGUI>();
        perfectText = perfectGO.GetComponent<TextMeshProUGUI>();
        ringCenter = new Vector3(0.515f, 0.27f, -1);
    }

    void Update()
    {
        
    }

    void Launch()
    {
        if (ringInPerfectZone)
        {
            perfectText.color = new Color(255, 255, 255, 1);
            // create dazzling particle effect
            SpawnFireworks();
        }
        else if (ringInOkZone) {
            okText.color = new Color(255, 255, 255, 1);
            // create nice particle effect
            SpawnFireworks();
        } else
        {
            // create mediocre particle effect
        }

        // delete old shrinking ring
        Destroy(currentRing.gameObject);
        SpawnNewShrinkingRing();
    }

    void SpawnNewShrinkingRing()
    {
        // set new center of ring to instantiate the fireworks location
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
