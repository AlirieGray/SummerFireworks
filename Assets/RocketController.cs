using TMPro;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    public bool ringInPerfectZone;
    public bool ringInOkZone;
    public GameObject perfectGO;
    public GameObject okGO;
    void Start()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Rocket.Enable();
        inputActions.Rocket.Launch.performed += context =>
        {
            Launch();
        };
    }

    void Update()
    {
        
    }

    void Launch()
    {
        if (ringInPerfectZone)
        {
            perfectGO.SetActive(true);
            // create dazzling particle effect
        }
        else if (ringInOkZone) {
            // create nice particle effect
        } else
        {
            // create mediocre particle effect
        }
    }

    void SpawnNewShrinkingRing()
    {

    }
}
