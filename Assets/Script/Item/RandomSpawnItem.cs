using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using QFSW.QC;
public class RandomSpawnItem : NetworkBehaviour
{
    public List<GameObject> objectList = new List<GameObject>();
    List<GameObject> spawnList = new List<GameObject>();
    public TurnBaseStateMachine turnBaseManager;
    public int range;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    [Command]
    public void RandomSpawn()
    {
        int rangeX;
        int rangeZ;
        int type;
        if(isSpawned() == true)
        {
            while (true)
            {
                rangeX = randomPosition(range);
                rangeZ = randomPosition(range);
                if (GameObject.FindGameObjectsWithTag("Player") != null)
                {
                    bool isSamePosition = CheckObjectPositionWithPlayer(rangeX, rangeZ)||CheckObjectPositionWithItem(rangeX,rangeZ);
                    if (isSamePosition == true)
                    {

                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }

            }
            type = randomPosition(objectList.Count);
            GameObject spawnPrefab = Instantiate(objectList[type]);
            spawnPrefab.transform.position = new Vector3(rangeX*10, 2, -rangeZ*10);
            spawnList.Add(spawnPrefab);
            spawnPrefab.GetComponent<DisappearObject>().randomSpawnItem = this;
            spawnPrefab.GetComponent<NetworkObject>().Spawn(true);
        }
    }
    public bool CheckObjectPositionWithPlayer(int posX, int posZ)
    {
        bool isSamePosition = false;
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Player").Length; i++)
        {
            if ((posX == GameObject.FindGameObjectsWithTag("Player")[i].transform.position.x && posZ == GameObject.FindGameObjectsWithTag("Player")[i].transform.position.z))
            {
                isSamePosition = true;
            }
        }
        return isSamePosition;
    }
    public bool CheckObjectPositionWithItem(int posX, int posZ)
    {
        bool isSamePosition = false;
        if (GameObject.FindGameObjectsWithTag("Item") != null)
        {
            for (int i = 0; i < GameObject.FindGameObjectsWithTag("Item").Length; i++)
            {
                if ((posX == GameObject.FindGameObjectsWithTag("Item")[i].transform.position.x && posZ == GameObject.FindGameObjectsWithTag("Item")[i].transform.position.z))
                {
                    isSamePosition = true;
                }
            }
        }
        return isSamePosition;
    }
    public int randomPosition(int range)
    {
        int randomNumber = Random.Range(0, range);
        Debug.Log("Your : Object" + randomNumber);
        return randomNumber;
    }
    public bool isSpawned()
    {
        int spawned = 1;
        spawned = Random.Range(0, 4);
        if (spawned == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    [ServerRpc]
    public void DestroyObjectServerRpc(ulong networkObjectId)
    {
        GameObject toDestroy = findObjectfromNetworkId(networkObjectId);
        if (toDestroy == null) return;
        toDestroy.GetComponent<NetworkObject>().Despawn();
        spawnList.Remove(toDestroy);
        toDestroy.SetActive(false);
    }
    public GameObject findObjectfromNetworkId(ulong networkObjectId)
    {
        foreach (GameObject gameObject in spawnList)
        {
            ulong objId = gameObject.GetComponent<NetworkObject>().NetworkObjectId;
            if (objId == networkObjectId)
            {
                return gameObject;
            }
        }
        return null;
    }
}
