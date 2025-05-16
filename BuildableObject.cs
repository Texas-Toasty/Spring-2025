using UnityEngine;

[CreateAssetMenu(fileName = "BuildableObject", menuName = "Building/Buildable Object")]
public class BuildableObject : ScriptableObject
{
    public GameObject prefab;
    public int woodCost;
    public int stoneCost;
}
