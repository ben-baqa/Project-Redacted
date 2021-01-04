using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionTest : MonoBehaviour
{
    public FileStorageHandler toConnect;

    private PersonalDeviceHandler personal;
    void Start()
    {
        personal = gameObject.GetComponent<PersonalDeviceHandler>();
        personal.ConnectToStorage(toConnect);
    }
}
