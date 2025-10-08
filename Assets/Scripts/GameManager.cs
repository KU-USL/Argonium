using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool enableCesiumOSM = false;
    public static bool enableCacheMode = true;

    public GameObject CesiumOSM;

    public FlightDeclarationManager FlightDeclarationManager;
    public GeofenceManager GeofenceManager;
    public AccessTokenManager AccessTokenManager;

    public void OnEnable()
    {
        if(CesiumOSM != null)
        {
            CesiumOSM.SetActive(enableCesiumOSM);
        }

        if (enableCacheMode)
        {
            FlightDeclarationManager.SpawnCachedData();
            GeofenceManager.SpawnCachedData();
        }
        else
        {
            AccessTokenManager.enabled = true;
        }
    }
}
