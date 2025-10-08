using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/* Unity Argon API
 * GeofenceManager
 * This class is used to create and manage geofences obtained from Argon
 */

public class GeofenceManager : MonoBehaviour
{
    string sampleJSON = @"{
    ""links"": {
        ""next"": null,
        ""previous"": null
    },
    ""total"": 7,
    ""page"": 1,
    ""pages"": 1,
    ""page_size"": 10,
    ""results"": [
        {
            ""id"": ""3bdadd34-2970-49df-b0bc-344483d05fe6"",
            ""altitude_ref"": ""WGS84"",
            ""raw_geo_fence"": {
                ""type"": ""FeatureCollection"",
                ""features"": [
                    {
                        ""type"": ""Feature"",
                        ""properties"": {
                            ""upper_limit"": 300,
                            ""lower_limit"": 100
                        },
                        ""geometry"": {
                            ""type"": ""Polygon"",
                            ""coordinates"": [
                                [
                                    [
                                        -87.63233721907162,
                                        41.88445453379035
                                    ],
                                    [
                                        -87.63230639627557,
                                        41.88325436897293
                                    ],
                                    [
                                        -87.63096531566055,
                                        41.883267174283645
                                    ],
                                    [
                                        -87.63097639028935,
                                        41.88444417641702
                                    ],
                                    [
                                        -87.63233721907162,
                                        41.88445453379035
                                    ]
                                ]
                            ]
                        }
                    }
                ]
            },
            ""geozone"": {},
            ""upper_limit"": ""300.00"",
            ""lower_limit"": ""100.00"",
            ""name"": ""Standard Geofence"",
            ""bounds"": ""-87.6323372,41.8832544,-87.6309653,41.8844545"",
            ""status"": 0,
            ""message"": null,
            ""is_test_dataset"": false,
            ""start_datetime"": ""2025-08-28T12:11:19.407905Z"",
            ""end_datetime"": ""2025-08-28T13:11:19.408031Z"",
            ""created_at"": ""2025-08-28T12:11:19.428896Z"",
            ""updated_at"": ""2025-08-28T12:11:19.428913Z""
        },
        {
            ""id"": ""fcfd1a8f-94ac-474f-959e-126de6b879f5"",
            ""altitude_ref"": ""WGS84"",
            ""raw_geo_fence"": {
                ""type"": ""FeatureCollection"",
                ""features"": [
                    {
                        ""type"": ""Feature"",
                        ""properties"": {
                            ""upper_limit"": 350,
                            ""lower_limit"": 100
                        },
                        ""geometry"": {
                            ""type"": ""Polygon"",
                            ""coordinates"": [
                                [
                                    [
                                        -87.63072854477586,
                                        41.87818672846481
                                    ],
                                    [
                                        -87.63070492939627,
                                        41.877178014669994
                                    ],
                                    [
                                        -87.63020390987629,
                                        41.877172504076526
                                    ],
                                    [
                                        -87.62866626909094,
                                        41.878151890329406
                                    ],
                                    [
                                        -87.62869326624384,
                                        41.87937585468353
                                    ],
                                    [
                                        -87.62933661413851,
                                        41.87938942880052
                                    ],
                                    [
                                        -87.63047323426125,
                                        41.8787090270221
                                    ],
                                    [
                                        -87.63072854477586,
                                        41.87818672846481
                                    ]
                                ]
                            ]
                        }
                    }
                ]
            },
            ""geozone"": {},
            ""upper_limit"": ""350.00"",
            ""lower_limit"": ""100.00"",
            ""name"": ""Standard Geofence"",
            ""bounds"": ""-87.6307285,41.8771725,-87.6286663,41.8793894"",
            ""status"": 0,
            ""message"": null,
            ""is_test_dataset"": false,
            ""start_datetime"": ""2025-08-28T12:11:43.939185Z"",
            ""end_datetime"": ""2025-08-28T13:11:43.939297Z"",
            ""created_at"": ""2025-08-28T12:11:43.958125Z"",
            ""updated_at"": ""2025-08-28T12:11:43.958138Z""
        },
        {
            ""id"": ""dd7c30a4-d7ba-4b5f-aa1b-d27272ca9ec7"",
            ""altitude_ref"": ""WGS84"",
            ""raw_geo_fence"": {
                ""type"": ""FeatureCollection"",
                ""features"": [
                    {
                        ""type"": ""Feature"",
                        ""properties"": {
                            ""upper_limit"": 200,
                            ""lower_limit"": 100
                        },
                        ""geometry"": {
                            ""type"": ""Polygon"",
                            ""coordinates"": [
                                [
                                    [
                                        -87.61791237817447,
                                        41.86180400490974
                                    ],
                                    [
                                        -87.61732043354667,
                                        41.86110844783738
                                    ],
                                    [
                                        -87.6157353756097,
                                        41.86124642215202
                                    ],
                                    [
                                        -87.61562001172798,
                                        41.86230260857863
                                    ],
                                    [
                                        -87.61593923020081,
                                        41.86350794015581
                                    ],
                                    [
                                        -87.61726634689488,
                                        41.863533230857996
                                    ],
                                    [
                                        -87.61779698098157,
                                        41.86321066534444
                                    ],
                                    [
                                        -87.61799714314165,
                                        41.862725454725904
                                    ],
                                    [
                                        -87.61791237817447,
                                        41.86180400490974
                                    ]
                                ]
                            ]
                        }
                    }
                ]
            },
            ""geozone"": {},
            ""upper_limit"": ""200.00"",
            ""lower_limit"": ""100.00"",
            ""name"": ""Standard Geofence"",
            ""bounds"": ""-87.6179971,41.8611084,-87.6156200,41.8635332"",
            ""status"": 0,
            ""message"": null,
            ""is_test_dataset"": false,
            ""start_datetime"": ""2025-08-28T12:12:14.504376Z"",
            ""end_datetime"": ""2025-08-28T13:12:14.504448Z"",
            ""created_at"": ""2025-08-28T12:12:14.517618Z"",
            ""updated_at"": ""2025-08-28T12:12:14.517630Z""
        },
        {
            ""id"": ""b6b516f4-79e9-4167-bd2a-a774ab92e5ec"",
            ""altitude_ref"": ""WGS84"",
            ""raw_geo_fence"": {
                ""type"": ""FeatureCollection"",
                ""features"": [
                    {
                        ""type"": ""Feature"",
                        ""properties"": {
                            ""upper_limit"": 500,
                            ""lower_limit"": 100
                        },
                        ""geometry"": {
                            ""type"": ""Polygon"",
                            ""coordinates"": [
                                [
                                    [
                                        -87.76274282573641,
                                        41.793402216704465
                                    ],
                                    [
                                        -87.76248161155374,
                                        41.7773659886152
                                    ],
                                    [
                                        -87.73973929639588,
                                        41.77778306288951
                                    ],
                                    [
                                        -87.7402708718615,
                                        41.79382937597185
                                    ],
                                    [
                                        -87.76274282573641,
                                        41.793402216704465
                                    ]
                                ]
                            ]
                        }
                    }
                ]
            },
            ""geozone"": {},
            ""upper_limit"": ""500.00"",
            ""lower_limit"": ""100.00"",
            ""name"": ""Standard Geofence"",
            ""bounds"": ""-87.7627428,41.7773660,-87.7397393,41.7938294"",
            ""status"": 0,
            ""message"": null,
            ""is_test_dataset"": false,
            ""start_datetime"": ""2025-08-28T12:12:33.055437Z"",
            ""end_datetime"": ""2025-08-28T13:12:33.055565Z"",
            ""created_at"": ""2025-08-28T12:12:33.074418Z"",
            ""updated_at"": ""2025-08-28T12:12:33.074437Z""
        },
        {
            ""id"": ""9861ca9a-0c80-468e-a6bd-873b91171cfa"",
            ""altitude_ref"": ""WGS84"",
            ""raw_geo_fence"": {
                ""type"": ""FeatureCollection"",
                ""features"": [
                    {
                        ""type"": ""Feature"",
                        ""properties"": {
                            ""upper_limit"": 500,
                            ""lower_limit"": 150
                        },
                        ""geometry"": {
                            ""type"": ""Polygon"",
                            ""coordinates"": [
                                [
                                    [
                                        -87.93864079932032,
                                        41.995774387452656
                                    ],
                                    [
                                        -87.9380231335094,
                                        41.95555138773835
                                    ],
                                    [
                                        -87.91390892360361,
                                        41.95269957031266
                                    ],
                                    [
                                        -87.89621847104428,
                                        41.95063862446162
                                    ],
                                    [
                                        -87.87098579436349,
                                        41.96909477268929
                                    ],
                                    [
                                        -87.8824093978928,
                                        41.99697817226976
                                    ],
                                    [
                                        -87.9060654326418,
                                        42.008886037154895
                                    ],
                                    [
                                        -87.92584363997304,
                                        42.0083599104733
                                    ],
                                    [
                                        -87.93864079932032,
                                        41.995774387452656
                                    ]
                                ]
                            ]
                        }
                    }
                ]
            },
            ""geozone"": {},
            ""upper_limit"": ""500.00"",
            ""lower_limit"": ""150.00"",
            ""name"": ""Standard Geofence"",
            ""bounds"": ""-87.9386408,41.9506386,-87.8709858,42.0088860"",
            ""status"": 0,
            ""message"": null,
            ""is_test_dataset"": false,
            ""start_datetime"": ""2025-08-28T12:13:08.702384Z"",
            ""end_datetime"": ""2025-08-28T13:13:08.702464Z"",
            ""created_at"": ""2025-08-28T12:13:08.720573Z"",
            ""updated_at"": ""2025-08-28T12:13:08.720595Z""
        },
        {
            ""id"": ""b9ceafcd-a27a-439b-acf6-662c1a8a1848"",
            ""altitude_ref"": ""WGS84"",
            ""raw_geo_fence"": {
                ""type"": ""FeatureCollection"",
                ""features"": [
                    {
                        ""type"": ""Feature"",
                        ""properties"": {
                            ""upper_limit"": 250,
                            ""lower_limit"": 130
                        },
                        ""geometry"": {
                            ""type"": ""Polygon"",
                            ""coordinates"": [
                                [
                                    [
                                        -87.68039320406132,
                                        41.86826721765473
                                    ],
                                    [
                                        -87.68163222390706,
                                        41.8682808728521
                                    ],
                                    [
                                        -87.68360916801656,
                                        41.86655086710783
                                    ],
                                    [
                                        -87.683519438765,
                                        41.864883235185374
                                    ],
                                    [
                                        -87.67865166495322,
                                        41.86495080860618
                                    ],
                                    [
                                        -87.67865810605973,
                                        41.866614331826185
                                    ],
                                    [
                                        -87.68039320406132,
                                        41.86826721765473
                                    ]
                                ]
                            ]
                        }
                    }
                ]
            },
            ""geozone"": {},
            ""upper_limit"": ""250.00"",
            ""lower_limit"": ""130.00"",
            ""name"": ""Standard Geofence"",
            ""bounds"": ""-87.6836092,41.8648832,-87.6786517,41.8682809"",
            ""status"": 0,
            ""message"": null,
            ""is_test_dataset"": false,
            ""start_datetime"": ""2025-08-28T12:13:41.541805Z"",
            ""end_datetime"": ""2025-08-28T13:13:41.541913Z"",
            ""created_at"": ""2025-08-28T12:13:41.558244Z"",
            ""updated_at"": ""2025-08-28T12:13:41.558260Z""
        },
        {
            ""id"": ""f1d62a63-e165-48bb-889a-afa2b833ab9b"",
            ""altitude_ref"": ""WGS84"",
            ""raw_geo_fence"": {
                ""type"": ""FeatureCollection"",
                ""features"": [
                    {
                        ""type"": ""Feature"",
                        ""properties"": {
                            ""upper_limit"": 200,
                            ""lower_limit"": 150
                        },
                        ""geometry"": {
                            ""type"": ""Polygon"",
                            ""coordinates"": [
                                [
                                    [
                                        -87.66620528342399,
                                        41.883440008265865
                                    ],
                                    [
                                        -87.66482840875707,
                                        41.88315513245652
                                    ],
                                    [
                                        -87.66416007295122,
                                        41.88401322406486
                                    ],
                                    [
                                        -87.66495429091653,
                                        41.884880624004836
                                    ],
                                    [
                                        -87.66624909072387,
                                        41.884415792405036
                                    ],
                                    [
                                        -87.66620528342399,
                                        41.883440008265865
                                    ]
                                ]
                            ]
                        }
                    }
                ]
            },
            ""geozone"": {},
            ""upper_limit"": ""200.00"",
            ""lower_limit"": ""150.00"",
            ""name"": ""Standard Geofence"",
            ""bounds"": ""-87.6662491,41.8831551,-87.6641601,41.8848806"",
            ""status"": 0,
            ""message"": null,
            ""is_test_dataset"": false,
            ""start_datetime"": ""2025-08-28T12:14:03.065949Z"",
            ""end_datetime"": ""2025-08-28T13:14:03.066058Z"",
            ""created_at"": ""2025-08-28T12:14:03.086386Z"",
            ""updated_at"": ""2025-08-28T12:14:03.086405Z""
        }
    ]
}";

    private Coroutine loopCoroutine;

    List<GameObject> geofenceList = new List<GameObject>();

    Dictionary<Guid, GameObject> geofenceSpawnedList = new Dictionary<Guid, GameObject>();

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
        UpdateAllGeofences(sampleJSON);
    }

    //This function obtains all geofences from Argon and spawns them in Unity
    void LoadAllGeofences()
    {
        ArgonAPI.GetAllGeofences(
            onSuccess: response =>
            {
                //Debug.Log("Response: " + response);
                UpdateAllGeofencesLegacy(response);
            },
            onError: error =>
            {
                //Debug.LogError("Error: " + error);
            });
    }

    void UpdateAllGeofencesLegacy(string response)
    {
        JSONNode rootNode = JSON.Parse(response);
        foreach (JSONNode result in rootNode["results"].AsArray)
        {
            GameObject geofenceObject = Geofence.SpawnGeofence(result["raw_geo_fence"], result["id"]);
            geofenceList.Add(geofenceObject);
        }
    }

    void UpdateAllGeofences(string response)
    {
        JSONNode rootNode = JSON.Parse(response);
        HashSet<Guid> latestIDs = new HashSet<Guid>();

        //Updating and Adding Elements
        foreach (JSONNode geofence in rootNode["results"].AsArray)
        {
            latestIDs.Add(Guid.Parse(geofence["id"]));
            if (geofenceSpawnedList.ContainsKey(Guid.Parse(geofence["id"])))
            {
                geofenceSpawnedList[Guid.Parse(geofence["id"])].GetComponent<Geofence>().GeofenceUpdate(geofence["raw_geo_fence"]);
            }
            else
            {
                GameObject geofenceObject = Geofence.SpawnGeofence(geofence["raw_geo_fence"], geofence["id"]);
                geofenceSpawnedList.Add(Guid.Parse(geofence["id"]), geofenceObject);
            }
        }

        //Removing Elements
        foreach (var id in geofenceSpawnedList.Keys.ToList())
        {
            if (!latestIDs.Contains(id))
            {
                GameObject toRemove = geofenceSpawnedList[id];
                geofenceSpawnedList.Remove(id);
                GameObject.Destroy(toRemove);
            }
        }
    }

    public void StartUpdateLoop()
    {
        if (loopCoroutine == null)
        {
            loopCoroutine = StartCoroutine(GeofenceUpdateLoop());
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

    private IEnumerator GeofenceUpdateLoop()
    {
        bool isDone;
        while (true)
        {
            isDone = false;
            ArgonAPI.GetAllGeofences(
                onSuccess: response =>
                {
                    Debug.Log("Response: " + response);
                    UpdateAllGeofences(response);
                    isDone = true;
                },
                onError: error =>
                {
                    Debug.LogError("Error: " + error);
                    isDone = true;
                }
                );

            //Wait until coroutine is done
            yield return new WaitUntil(() => isDone);

            //Optional delay between iterations 
            yield return new WaitForSeconds(5f);
        }
    }
}
