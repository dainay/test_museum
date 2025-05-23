using UnityEngine;
using System.Collections;

public class wr_FloorTriggerRiser : MonoBehaviour
{
    [System.Serializable]
    public class SphereWallPair
    {
        public wr_SphereController sphereController;
        public wr_FillWallController fillWallController;
    }

    [Header("Associations Sphère-Mur")]
    [SerializeField] private SphereWallPair[] sphereWallPairs;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(CheckWallsWithDelay());
        }
    }

    IEnumerator CheckWallsWithDelay()
    {
        yield return new WaitForSeconds(0.1f);
        
        foreach(var pair in sphereWallPairs)
        {
            
            if(pair.sphereController == null || pair.fillWallController == null)
            {
                Debug.LogError("Paire incomplète !");
                continue;
            }

            bool isSphereActive = pair.sphereController.IsSphereActive();
            
            if(!isSphereActive)
            {
                pair.fillWallController.Activate();
            }
        }
    }

    void OnDrawGizmos()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        if(collider != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(transform.position + collider.center, collider.size);
        }
    }
}