using TMPro;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    private bool ringInPerfectZone;
    private bool ringInOkZone;
    public GameObject perfectGO;
    public GameObject okGO;
    private TextMeshPro perfectText;
    private TextMeshPro okText;
    public RingController currentRing;
    void Start()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Rocket.Enable();
        inputActions.Rocket.Launch.performed += context =>
        {
            Launch();
        };
        okText = okGO.GetComponent<TextMeshPro>();
        perfectText = perfectGO.GetComponent<TextMeshPro>();
    }

    void Update()
    {
        
    }

    void Launch()
    {
        Debug.Log("Launching!");
        Debug.Log(ringInOkZone);
        Debug.Log(ringInPerfectZone);   
        if (ringInPerfectZone)
        {
            Debug.Log("perfeft?");
            perfectText.color = new Color(255, 255, 255, 1);
            // create dazzling particle effect
        }
        else if (ringInOkZone) {
            // create nice particle effect
            okText.color = new Color(255, 255, 255, 1);
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
