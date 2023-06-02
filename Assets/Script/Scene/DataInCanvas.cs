using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DataInCanvas : MonoBehaviour
{
    public TMP_InputField name_Inputfield;
    public string playerName;

    private void Update()
    {
        playerName = name_Inputfield.text;
    }
}
