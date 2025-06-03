using UnityEngine;

public class YellowSign : MonoBehaviour
{
    public string requiredTabletName;
    private YellowTablet insertedTablet = null;
    private bool isCorrectlyPlaced = false;

    public Material defaultMaterial;
    public Material correctMaterial;
    public Material incorrectMaterial;

    public GameObject yellowCylinder;
    public Transform tabletPlacementPoint;

    public YellowGameManager gameManager;

    void OnTriggerEnter(Collider other)
    {
        YellowTablet tablet = other.GetComponent<YellowTablet>();
        if (tablet != null)
        {
            insertedTablet = tablet;
            CheckTablet();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (insertedTablet != null && other.gameObject == insertedTablet.gameObject)
        {
            Debug.Log("Tablette retirée.");
            insertedTablet = null;
            ResetValidation();
        }
    }

    void CheckTablet()
    {
        if (insertedTablet != null && insertedTablet.GetTabletName() == requiredTabletName)
        {
            Debug.Log("Bonne tablette pour " + gameObject.name + " !");

            insertedTablet.transform.position = tabletPlacementPoint.position;
            insertedTablet.transform.rotation = tabletPlacementPoint.rotation;

            Rigidbody rb = insertedTablet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            if (yellowCylinder != null)
            {
                yellowCylinder.GetComponent<Renderer>().material = correctMaterial;
            }

            isCorrectlyPlaced = true;

            gameManager.CheckVictory();
        }
        else
        {
            Debug.Log("Mauvaise tablette pour " + gameObject.name + " !");
            if (yellowCylinder != null)
            {
                yellowCylinder.GetComponent<Renderer>().material = incorrectMaterial;
            }

            isCorrectlyPlaced = false;
        }
    }

    void ResetValidation()
    {
        if (yellowCylinder != null)
        {
            yellowCylinder.GetComponent<Renderer>().material = defaultMaterial;
        }

        isCorrectlyPlaced = false;
    }

    public bool IsCorrectlyPlaced()
    {
        return isCorrectlyPlaced;
    }
}
