using UnityEngine;

public class YellowSign : MonoBehaviour
{
    public string requiredTabletName; // Nom de la tablette correcte pour CE Sign
    private YellowTablet insertedTablet = null;  // La tablette actuellement insérée
    private bool isCorrectlyPlaced = false; // État pour savoir si la tablette est bien placée

    public Material correctMaterial; // Matériau vert pour la bonne réponse
    public Material incorrectMaterial; // Matériau rouge pour la mauvaise réponse

    public GameObject yellowCylinder;  // Objet de validation visuelle de CE sign
    public Transform tabletPlacementPoint; // Point précis où la tablette doit être placée

    // Référence au gestionnaire global de victoire (ajoute-le dans le manager)
    public YellowGameManager gameManager;

    void OnTriggerEnter(Collider other)
    {
        YellowTablet tablet = other.GetComponent<YellowTablet>();  // Vérifie si c'est une tablette
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
            insertedTablet = null;  // Réinitialisation
            ResetValidation();
        }
    }

    // Vérifie si la tablette insérée est correcte
    void CheckTablet()
    {
        if (insertedTablet != null && insertedTablet.GetTabletName() == requiredTabletName)
        {
            Debug.Log("Bonne tablette pour " + gameObject.name + " !");

            // Place SEULEMENT la tablette sur le Sign
            insertedTablet.transform.position = tabletPlacementPoint.position;
            insertedTablet.transform.rotation = tabletPlacementPoint.rotation;

            // Désactive la physique pour qu'elle reste posée
            Rigidbody rb = insertedTablet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            // Change la couleur SEULEMENT de SON cylindre
            if (yellowCylinder != null)
            {
                yellowCylinder.GetComponent<Renderer>().material = correctMaterial;
            }

            // Marque ce signe comme correctement complété
            isCorrectlyPlaced = true;

            // Vérifie si tous les signes sont complétés
            gameManager.CheckVictory();
        }
        else
        {
            Debug.Log("Mauvaise tablette pour " + gameObject.name + " !");

            // Change la couleur SEULEMENT de SON cylindre
            if (yellowCylinder != null)
            {
                yellowCylinder.GetComponent<Renderer>().material = incorrectMaterial;
            }
        }
    }

    // Remet l'affichage par défaut
    void ResetValidation()
    {
        if (yellowCylinder != null)
        {
            yellowCylinder.GetComponent<Renderer>().material = incorrectMaterial;
        }

        // Réinitialiser l'état de la tablette
        isCorrectlyPlaced = false;
    }

    // Retourne l'état du placement de la tablette
    public bool IsCorrectlyPlaced()
    {
        return isCorrectlyPlaced;
    }
}
