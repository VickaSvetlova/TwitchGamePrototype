using System;
using UnityEngine;
using UnityEngine.UI;

public class UIName : MonoBehaviour
{
    [SerializeField] private Text label;
    private Transform _transform;

    private string name;

    private void Start()
    {
        label = GetComponent<Text>();
    }

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
        if (TransformFolowObject != null)
        {
            Vector3 transformLabel = Camera.main.WorldToScreenPoint(TransformFolowObject.transform.position);
            transform.position = transformLabel;
        }
        else
        {
            DestroyObj();
        }
    }

    public void DestroyObj()
    {
        Destroy(gameObject);
    }
}