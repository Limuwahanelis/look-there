using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogBox : MonoBehaviour
{
    [SerializeField] UIDocument _UIDocument;
    VisualElement _root;

    void Awake()
    {
        _root = _UIDocument.rootVisualElement;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
