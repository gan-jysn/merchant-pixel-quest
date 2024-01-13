using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObjects/SignData")]
public class SignSO : ScriptableObject {
    public int signID;
    [TextArea] public string signDescription;
}
