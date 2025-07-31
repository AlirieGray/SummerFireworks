using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.ParticleSystem;

public class LaunchRandomFirework : MonoBehaviour
{
    public GameObject baseFireworks;
    public GameObject circleFireworks;
    public GameObject heartFireworks;
    public GameObject starFireworks;
    public GameObject shapeless;

    public static LaunchRandomFirework randomFireworkDisplay;
    public bool stop = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        randomFireworkDisplay = this;
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        while (true)
        {
            if(!stop)
                SpawnFireworks();
            yield return new WaitForSecondsRealtime(Random.Range(0.13f, 0.66f));
        }
        
    }

    List<ResourceScriptableObject> CreateFirework()
    {
        List<ResourceScriptableObject> resources = new List<ResourceScriptableObject>();

        //pick 5 non-blunder resources

        while(resources.Count < 5)
        {
            var roll = Random.Range(0, GameManager.manager.resources.Count);
            ResourceScriptableObject res = GameManager.manager.resources[roll];

            while (res.name == "Blunder")
            {
                roll = Random.Range(0, GameManager.manager.resources.Count);
                res = GameManager.manager.resources[roll];
            }

            resources.Add(res);

            if(Random.Range(0f, 1f) < 0.01f * resources.Count)
            {
                break;
            }
        }

        return resources;
    }

    void SpawnFireworks()
    {
        

        List<ResourceScriptableObject> resources = CreateFirework();
        GameObject currentFirework = Instantiate(baseFireworks, null);
        var main = currentFirework.GetComponent<ParticleSystem>().main;
            main.startLifetime = Random.Range(0.15f, 0.5f);
        //currentFirework.transform.localScale = Vector3.one * 0.2f;
        List<GameObject> allShapes = new List<GameObject>();

        

        GameObject currentBurst = null;

        if (resources[0].shape == ResourceScriptableObject.Shape.None)
        {
            currentBurst = Instantiate(shapeless, currentFirework.transform);
            currentFirework.GetComponent<ParticleSystem>().subEmitters.AddSubEmitter(currentBurst.GetComponent<ParticleSystem>(), ParticleSystemSubEmitterType.Death, ParticleSystemSubEmitterProperties.InheritColor);
            allShapes.Add(currentFirework);
        }
        Color[] colors = { Color.cyan, Color.magenta, Color.yellow, 
            GameManager.manager.MixColor(Color.cyan, Color.magenta),
            GameManager.manager.MixColor(Color.cyan, Color.yellow),
            GameManager.manager.MixColor(Color.magenta, Color.yellow)
        };

        ResourceScriptableObject.Shape currentShape = ResourceScriptableObject.Shape.None;
        Color currentBlend = Color.white;

        foreach (ResourceScriptableObject resource in resources)
        {
            if (resource.shape != ResourceScriptableObject.Shape.None)
            {
                currentBlend = Color.white;
                currentShape = resource.shape;
                currentBurst = Instantiate(resource.shapePrefab);
                allShapes.Add(currentBurst);
            }
            else
            {
                if(currentBlend == Color.white)
                {
                    currentBlend = resource.color;
                }
                var m = currentBurst.GetComponent<ParticleSystem>().main;
                m.startColor = colors[Random.Range(0, colors.Length)]; ;
            }
            /*switch (resource.shape)
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
            }*/
        }

        // go through all fireworksToSpawn and fan out the locations, instantiate all
        
        if(allShapes.Count > 0)
        {
            var em =currentFirework.transform.GetChild(0).GetComponent<ParticleSystem>().emission;
            var emptyBurst = new ParticleSystem.Burst();
            emptyBurst.probability = 0;
            em.SetBurst(0, emptyBurst);
        }
        currentFirework.transform.position = new Vector3(Random.Range(-5f, 5f), -3.643f, -.5f);
        //currentFirework.transform.localScale = Vector3.one * 0.2f;
        foreach (GameObject burst in allShapes)
        {
            burst.transform.SetParent(currentFirework.transform);
            //burst.transform.localScale = Vector3.one;
            if(burst.GetComponent<CustomShapedFirework>()!=null)
                burst.GetComponent<CustomShapedFirework>().stopEmitting = true;
            //
            //
                

            /*GameObject fireworksClone = Instantiate(firework, new Vector3(
                0f, -3.643f, -.5f), Quaternion.Euler(-90f, 0f, 0f));*/
        }
        //audioManager.PlayFireworks();
    }
}
