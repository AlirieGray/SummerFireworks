using UnityEngine;
[CreateAssetMenu(menuName = "Fireworks/Create New Resource", fileName = "New Resource", order = 0)]
public class ResourceScriptableObject : ScriptableObject
{
    public Sprite overrideSprite;
    public Sprite groundUpSprite;
    public Color color;

    public GameObject shapePrefab;
    public Shape shape;

    public enum Shape
    {
        None,
        Circle,
        Heart,
        Star,
        Starburst,
        Triangle
    }
}
