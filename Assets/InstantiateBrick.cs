using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Drawing;
[System.Serializable]
public class SerializableKeyValueType
{
    public string key;
    public Mesh value;
}
[System.Serializable]
public class SerializableKeyValueMaterial
{
    public string key;
    public Material value;
}
public class InstantiateBrick : MonoBehaviour
{
    public GameObject brickPrefab;
    public TMP_Dropdown typeDropdown;
    public TMP_Dropdown colorDropdown;
    private BrickType currentBrickType;
    public SerializableKeyValueType[] meshMappings;
    public SerializableKeyValueMaterial[] colorMappings;
    public Material glowMaterial;
    public string t;
    public string c;
    private void Start()
    {
        typeDropdown.onValueChanged.AddListener(UpdateValue);
        colorDropdown.onValueChanged.AddListener(UpdateColor);
    }
    public void UpdateValue(int selectedIndex)
    {
        t = typeDropdown.options[selectedIndex].text;
    }
    public void UpdateColor(int selectedIndex)
    {
        c = colorDropdown.options[selectedIndex].text;
    }
    private Mesh GetMeshFromList(string targetKey)
    {
        foreach (var pair in meshMappings)
        {
            if (pair.key.ToLower() == targetKey.ToLower())
            {
                return pair.value;
            }
        }
        return null;
    }
    private Material GetColorFromList(string targetKey)
    {
        foreach (var pair in colorMappings)
        {
            if (pair.key.ToLower() == targetKey.ToLower())
            {
                return pair.value;
            }
        }
        return null;
    }
    public void CreateBrick()
    {
        currentBrickType = t.ToLower() switch
        {
            "2x2 brick" => BrickType.TwoTwoBrick,
            "2x4 brick" => BrickType.TwoFourBrick,
            "1x2 brick" => BrickType.OneTwoBrick,
            "2x4 plane" => BrickType.TwoFourPlane,
            "2x2 plane" => BrickType.TwoTwoPlane,
            "1x2 plane" => BrickType.OneTwoPlane,
            _ => BrickType.TwoTwoBrick
        };
        GameObject newBrick = Instantiate(brickPrefab);
        LegoHold script = newBrick.GetComponentInChildren<LegoHold>();
        script.settings.brickType = currentBrickType;
        script.settings.modelMesh = GetMeshFromList(t.ToLower());
        var temp = new Material[1];
        temp[0] = GetColorFromList(c.ToLower());
        script.settings.defaultMaterials = temp;
        var temp2 = new Material[2];
        temp2[0] = GetColorFromList(c.ToLower());
        temp2[1] = glowMaterial;
        script.settings.glowMaterials = temp2;
    }
}
