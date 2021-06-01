using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UIName : MonoBehaviour
{
    private Transform _transform;
    private string name;
    private Text label => GetComponent<Text>();

    public string Name
    {
        get => name;
        set
        {
            name = value;
            label.text = value;
        }
    }

    public Transform TransformFolowObject
    {
        get => _transform;
        set => _transform = value;
    }
    
    void Update()
    {
        if (TransformFolowObject == null) return;
        Vector3 transformLabel = Camera.main.WorldToScreenPoint(TransformFolowObject.transform.position);
        transform.position = transformLabel;
    }

    public void DestroyObj()
    {
        Destroy(gameObject);
    }
}