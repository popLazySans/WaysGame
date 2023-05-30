using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class NamePlayerTogether : MonoBehaviour
{
    //public Name;
    private void Start()
    {
        this.gameObject.GetComponent<Player>();
    }
    private void Update()
    {
        Vector3 playerPos = this.transform.position;
    }
}
