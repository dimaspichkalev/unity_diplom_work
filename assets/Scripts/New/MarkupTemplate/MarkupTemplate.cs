using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class MarkupTemplate : ScriptableObject
{
    public string Name;
    public List<MarkupElement> roadsList;
}


[Serializable]
public class MarkupElement : ScriptableObject
{

}