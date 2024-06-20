using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[CreateAssetMenu(menuName = "Dialog")]
public class Dialog : ScriptableObject
{
    public string Text => _text;
    [SerializeField,TextArea] string _text;
}