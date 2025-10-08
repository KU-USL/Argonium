using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RIDManager : MonoBehaviour
{
    string sampleJSON = @"{
  ""flights"": [
    {
      ""id"": ""12345678"",
      ""most_recent_position"": {
        ""lat"": ""24.44958"",
        ""lng"": ""54.39554"",
        ""alt"": ""30.0""
      },
      ""recent_paths"": [
        {
          ""positions"": [
            {
              ""lat"": ""<double>"",
              ""lng"": ""<double>"",
              ""alt"": ""<double>""
            },
            {
              ""lat"": ""<double>"",
              ""lng"": ""<double>"",
              ""alt"": ""<double>""
            }
          ]
        },
        {
          ""positions"": [
            {
              ""lat"": ""<double>"",
              ""lng"": ""<double>"",
              ""alt"": ""<double>""
            },
            {
              ""lat"": ""<double>"",
              ""lng"": ""<double>"",
              ""alt"": ""<double>""
            }
          ]
        }
      ]
    },
    {
      ""id"": ""<string>"",
      ""most_recent_position"": {
        ""lat"": ""<double>"",
        ""lng"": ""<double>"",
        ""alt"": ""<double>""
      },
      ""recent_paths"": [
        {
          ""positions"": [
            {
              ""lat"": ""<double>"",
              ""lng"": ""<double>"",
              ""alt"": ""<double>""
            },
            {
              ""lat"": ""<double>"",
              ""lng"": ""<double>"",
              ""alt"": ""<double>""
            }
          ]
        },
        {
          ""positions"": [
            {
              ""lat"": ""<double>"",
              ""lng"": ""<double>"",
              ""alt"": ""<double>""
            },
            {
              ""lat"": ""<double>"",
              ""lng"": ""<double>"",
              ""alt"": ""<double>""
            }
          ]
        }
      ]
    }
  ]
}";

    List<GameObject> rIDList = new List<GameObject>();

    Dictionary<string, GameObject> rIDSpawnedList = new Dictionary<string, GameObject>();

    private Coroutine loopCoroutine;

    public bool test= false;

    private void OnEnable()
    {
        AccessTokenManager.AccessTokenObtained += StartUpdateLoop;
    }

    private void OnDisable()
    {
        AccessTokenManager.AccessTokenObtained -= StartUpdateLoop;
    }

    public void SpawnCachedData()
    {
        UpdateAllRIDDetails(sampleJSON);
    }

    void LoadAllRIDDetails()
    {
        ArgonAPI.GetRIDDataForGivenViewBoundingBox(
            onSuccess: response =>
            {
                //Debug.Log("Response: " + response);
                UpdateAllRIDDetails(response);
            },
            onError: error =>
            {
                //Debug.LogError("Error: " + error);
            });
    }

    void UpdateAllRIDDetails(string response)
    {
        JSONNode rootNode = JSONNode.Parse(response);
        HashSet<string> latestIDs = new HashSet<string>();
        foreach (JSONNode flight in rootNode["flights"].AsArray)
        {
            latestIDs.Add(flight["id"]);
            if (rIDSpawnedList.ContainsKey(flight["id"]))
            {
                //Update the object if it already exists
                rIDSpawnedList[flight["id"]].GetComponent<RID>().RIDUpdate(flight);
            }
            else
            {
                //Spawn a new object if it doesn't already exist
                GameObject rID = RID.SpawnRID(flight);
                rIDSpawnedList.Add(flight["id"], rID);
            }
        }

        //Removing elements
        foreach (var id in rIDSpawnedList.Keys.ToList())
        {
            if (!latestIDs.Contains(id))
            {
                GameObject toRemove = rIDSpawnedList[id];
                rIDSpawnedList.Remove(id);
                GameObject.Destroy(toRemove);
            }
        }
    }

    void UpdateAllRIDDetailsLegacy(string response)
    {
        JSONNode rootNode = JSON.Parse(response);
        foreach (JSONNode flight in rootNode["flights"].AsArray)
        {
            if (RIDExists(flight["id"]))
            {
                FindRID(flight["id"]).RIDUpdate(flight);
            }
            else
            {
                GameObject rID = RID.SpawnRID(flight);
                rIDList.Add(rID);
            }

        }
    }

    public bool RIDExists(string id)
    {
        foreach (var go in rIDList)
        {
            var comp = go.GetComponent<RID>();
            if (comp != null && comp.id == id)
            {
                return true;
            }
        }
        return false;
    }


    public RID FindRID(string id)
    {
        foreach (var go in rIDList)
        {
            var comp = go.GetComponent<RID>();
            if (comp != null && comp.id == id)
            {
                return comp; // Stop after the first match (optional)
            }
        }
        return null;
    }


    public void StartUpdateLoop()
    {
        if (loopCoroutine == null)
        {
            loopCoroutine = StartCoroutine(RIDUpdateLoop());
        }
    }
    
    public void StopUpdateLoop()
    {
        if (loopCoroutine != null)
        {
            StopCoroutine(loopCoroutine);
            loopCoroutine = null;
        } 
    }

    private IEnumerator RIDUpdateLoop()
    {
        bool isDone;
        while (true)
        {
            isDone = false;
            
            //Query Parameters don't matter as it constrains nothing
            //But Argon refuses to accept a query without a specified view. 
            ArgonAPI.GetRIDDataForGivenViewBoundingBox(
                onSuccess: response =>
                {
                    Debug.Log("Success: " + response);
                    UpdateAllRIDDetails(response);
                    isDone = true;
                },
                onError: error =>
                {
                    Debug.Log(error);
                    isDone = true;
                },
                queryParameters: ArgonAPI.QueryParser(
                    view: "46.97471588813772,7.473307389701852,46.975508163062706,7.473955394898998"
                    )
                );

            //Wait until coroutine is done
            yield return new WaitUntil(() => isDone);

            //Optional delay between iterations 
            yield return new WaitForSeconds(1f);
        }
    }
}
