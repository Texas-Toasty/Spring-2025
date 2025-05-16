using UnityEngine;

public class BiomeTrigger : MonoBehaviour
{
    public string biomeName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<BiomeMusicManager>().ChangeBiome(biomeName);
        }
    }
}
