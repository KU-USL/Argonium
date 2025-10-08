using CesiumForUnity;
using NavigationToolkit.Model;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RID : MonoBehaviour
{
    public string id; //RID/Flight ID

    public LlaPosition mostRecentPosition;

    //NOT IMPLEMENTED
    public List<LlaPosition> recentPathPositions;

    public GameObject droneModel;

    public static GameObject cesiumGeoreference;

    public override string ToString()
    {
        return $"Drone\nID: {id}\nPosition: {mostRecentPosition}";
    }


    public static GameObject SpawnRID(JSONNode flight)
    {
        GameObject obj = new GameObject();
        RID rID = obj.AddComponent<RID>();
        if (cesiumGeoreference == null)
        {
            cesiumGeoreference = GameObject.Find("CesiumGeoreference");
        }
        rID.RIDUpdate(flight);
        rID.RIDVisualizer(); //Generate the visuals for the RID object

        obj.name = "RID "+rID.id;

        return obj;
    }

    /// <summary>
    /// Parse JSON to update RID details and update world coordinates
    /// </summary>
    public void RIDUpdate(JSONNode flight)
    {
        RIDParser(flight); //Parse Provided RID values
        WorldPlacementHandler(); //Place the object into the world. 
    }

    void RIDParser(JSONNode flight)
    {
        id = flight["id"];
        mostRecentPosition = new LlaPosition(flight["most_recent_position"]["lat"], flight["most_recent_position"]["lng"], flight["most_recent_position"]["alt"]);
    }

    void WorldPlacementHandler()
    {
        PlaceRIDCesium();
    }

    void PlaceRIDCesium()
    {
        //Parent to Cesium Georeference
        if (cesiumGeoreference != null)
        {
            transform.parent = cesiumGeoreference.transform;
        }
        //Add Globe Anchor
        CesiumGlobeAnchor cesiumGlobeAnchor = this.gameObject.AddComponent<CesiumGlobeAnchor>();
        cesiumGlobeAnchor.longitudeLatitudeHeight = new double3(mostRecentPosition.Longitude, mostRecentPosition.Latitude, mostRecentPosition.Altitude);
    }

    void RIDVisualizer()
    {
        //FindDroneModel();
        StartCoroutine(LoadDroneModel());
    }

    void FindDroneModel()
    {
        throw new NotImplementedException();
    }

    IEnumerator LoadDroneModel(string prefabName = "Drone")
    {
        ResourceRequest request = Resources.LoadAsync<GameObject>("DroneModels/"+prefabName);
        yield return request;

        GameObject prefab = request.asset as GameObject;
        if(prefab != null)
        {
            droneModel = Instantiate(prefab, this.transform);
            droneModel.transform.localPosition = Vector3.zero;
            droneModel.layer = LayerMask.NameToLayer("Drone");
        }
        else
        {
            Debug.LogError("Failed to load prefab asynchronously.");
        }
    }
}
