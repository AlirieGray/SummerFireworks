using UnityEngine;
[CreateAssetMenu(menuName = "Fireworks/Create New Resource", fileName = "New Resource", order = 0)]
public class ResourceScriptableObject : ScriptableObject
{
    public Sprite overrideSprite;
    public Color color;
}
