
using UnityEngine;
using System.Collections.Generic;
public enum BrickType { TwoFourPlane, TwoTwoPlane, OneTwoPlane, TwoFourBrick, TwoTwoBrick, OneTwoBrick };
[System.Serializable]
public class BrickSettings
{
    public BrickType brickType;
    public Mesh modelMesh;
    public Material[] defaultMaterials;
    public Material[] glowMaterials;

}
public class LegoHold : MonoBehaviour
{
    public BrickSettings settings;
    private Material[] newMaterial;
    private Material[] glowMaterial;
    private MeshRenderer meshRenderer;
    public Color previousColor;
    private MeshFilter meshFilter;
    private BoxCollider boxCollider;

    [Header("Magnetize")]
    public float connectCheckRange = 0.06f;
    private Rigidbody rb;
    private bool isMagnetized = false;
    private Vector3 targetGridPoint;
    public bool isGrabbed;
    public bool touch = false;
    //TODO UI
    private Dictionary<BrickType, (Vector3 size, Vector3 center)> colliderData = new Dictionary<BrickType, (Vector3, Vector3)>
    {
        { BrickType.TwoFourPlane, (new Vector3(1.58f, 0.34f, 3.180001f), new Vector3(0, 0.1601651f, 0)) },
        { BrickType.TwoTwoPlane, (new Vector3(1.58f, 0.34f, 1.58f), new Vector3(0, 0.1601651f, 0)) },
        { BrickType.OneTwoPlane, (new Vector3(1.58f, 0.34f, 0.7899999f), new Vector3(0, 0.1601651f, 0)) },
        { BrickType.TwoFourBrick, (new Vector3(1.58f, 0.9669788f, 3.180001f), new Vector3(0, 0.4786873f, 0)) },
        { BrickType.TwoTwoBrick, (new Vector3(1.58f, 0.9669788f, 1.58f), new Vector3(0, 0.4791666f, 0))},
        { BrickType.OneTwoBrick, (new Vector3(1.58f, 0.9669788f, 0.7899999f), new Vector3(0, 0.476216f, 0)) }
    };
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        boxCollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();

        gameObject.tag = "Lego";
        InitializeBrick();
    }

    void Update()
    {
        touch = false;
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, connectCheckRange);
        foreach (var col in nearbyColliders)
        {
            if (col.CompareTag("Lego") && col.gameObject != this.gameObject)
            {
                if (col.gameObject.name == "Ground")
                {
                    touch = true;
                    break;
                }
                else if (col.GetComponent<LegoHold>().isMagnetized)
                {
                    touch = true;
                    break;
                }
            }
        }
        if (isGrabbed && touch)
        {
            glowMaterial[1].SetColor("_Color", Color.green);
            meshRenderer.materials = glowMaterial;
            return;
        }
        else if (isGrabbed && !touch)
        {
            glowMaterial[1].SetColor("_Color", previousColor);
            meshRenderer.materials = glowMaterial;
            return;
        }
    }
    private void InitializeBrick()
    {
        meshFilter.mesh = settings.modelMesh;

        newMaterial = settings.defaultMaterials;
        glowMaterial = settings.glowMaterials;

        meshRenderer.materials = newMaterial;

        boxCollider.size = colliderData[settings.brickType].size;
        boxCollider.center = colliderData[settings.brickType].center;
    }

    void MagnetizeToGrid()
    {
        isMagnetized = true;
        transform.position = targetGridPoint;
        transform.rotation = GridManager.GetNearestDirection(transform.rotation);
        rb.isKinematic = true;
        rb.useGravity = false;
    }


    public void OnGrab()
    {
        isGrabbed = true;
        isMagnetized = false;
        rb.isKinematic = false;
        rb.useGravity = true;
    }
    public void EndGrab()
    {
        isGrabbed = false;
        rb.isKinematic = false;
        rb.useGravity = true;
        if (isMagnetized) return;
        Vector3 currentGridPoint = GridManager.GetNearestGridPoint(transform.position);
        float distanceToGrid = Vector3.Distance(transform.position, currentGridPoint);
        targetGridPoint = currentGridPoint;
        if (touch)
        {
            MagnetizeToGrid();
        }
    }



    public void Hover()
    {
        if (touch)
        {
            glowMaterial[1].SetColor("_Color", Color.green);
        }
        else
        {
            try
            {
                glowMaterial[1].SetColor("_Color", previousColor);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        meshRenderer.materials = glowMaterial;
    }
    public void ExitHover()
    {
        meshRenderer.materials = newMaterial;
    }

}
