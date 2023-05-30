using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DataInCanvas : MonoBehaviour
{
    public TMP_InputField name_Inputfield;
    public string name;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        name = name_Inputfield.text;
    }
}
