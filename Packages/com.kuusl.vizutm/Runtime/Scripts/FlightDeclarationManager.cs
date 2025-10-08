using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/* Unity Argon API
 * FlightDeclarationManager 
 * This class is used to create and manage Flight Declarations obtained from Argon
 */

public class FlightDeclarationManager : MonoBehaviour
{
    string sampleJSON = @"{
  ""links"": {
    ""next"": null,
    ""previous"": null
  },
  ""total"": 5,
  ""page"": 1,
  ""pages"": 1,
  ""page_size"": 10,
  ""results"": [
    {
      ""operational_intent"": {
        ""volumes"": [
          {
            ""volume"": {
              ""outline_polygon"": {},
              ""altitude_lower"": {
                ""value"": 170,
                ""reference"": ""W84"",
                ""units"": ""M""
              },
              ""altitude_upper"": {
                ""value"": 190,
                ""reference"": ""W84"",
                ""units"": ""M""
              },
              ""outline_circle"": null
            },
            ""time_start"": {
              ""format"": ""RFC3339"",
              ""value"": ""2025-08-26T14:00:00+00:00""
            },
            ""time_end"": {
              ""format"": ""RFC3339"",
              ""value"": ""2025-08-26T17:00:00+00:00""
            }
          }
        ],
        ""priority"": 0,
        ""state"": ""Accepted"",
        ""off_nominal_volumes"": []
      },
      ""originating_party"": ""KU USL"",
      ""type_of_operation"": 0,
      ""id"": ""b1a9f5e2-3c4d-4a1f-9e2a-1f3c4d5e6f7a"",
      ""state"": 1,
      ""is_approved"": false,
      ""start_datetime"": ""2025-08-26T14:00:00Z"",
      ""end_datetime"": ""2025-08-26T17:00:00Z"",
      ""flight_declaration_geojson"": {
        ""type"": ""FeatureCollection"",
        ""features"": [
          {
            ""type"": ""Feature"",
            ""properties"": {
              ""time_start"": ""2025-08-26T14:00:00+00:00"",
              ""time_end"": ""2025-08-26T17:00:00+00:00""
            },
            ""geometry"": {
              ""type"": ""Polygon""
            }
          }
        ]
      },
      ""flight_declaration_raw_geojson"": {
        ""type"": ""FeatureCollection"",
        ""features"": [
          {
            ""type"": ""Feature"",
            ""properties"": {
              ""id"": ""0"",
              ""start_datetime"": ""2025-07-16T14:00:00.000Z"",
              ""end_datetime"": ""2025-07-16T17:00:00.000Z"",
              ""max_altitude"": {
                ""meters"": 190,
                ""datum"": ""WGS84""
              },
              ""min_altitude"": {
                ""meters"": 170,
                ""datum"": ""WGS84""
              }
            },
            ""geometry"": {
              ""coordinates"": [
                [-87.63886021384907, 41.88619841785085],
                [-87.63764924307213, 41.88624588027773],
                [-87.63560437107128, 41.887291506663125],
                [-87.62728134910392, 41.88737480813798],
                [-87.62688364732186, 41.88717272766274]
              ],
              ""type"": ""LineString""
            }
          }
        ]
      },
      ""bounds"": ""-87.6388602,41.8861984,-87.6268836,41.8873748"",
      ""approved_by"": null,
      ""submitted_by"": null
    },
    {
      ""operational_intent"": {
        ""volumes"": [
          {
            ""volume"": {
              ""outline_polygon"": {},
              ""altitude_lower"": {
                ""value"": 250,
                ""reference"": ""W84"",
                ""units"": ""M""
              },
              ""altitude_upper"": {
                ""value"": 270,
                ""reference"": ""W84"",
                ""units"": ""M""
              },
              ""outline_circle"": null
            },
            ""time_start"": {
              ""format"": ""RFC3339"",
              ""value"": ""2025-08-26T14:00:00+00:00""
            },
            ""time_end"": {
              ""format"": ""RFC3339"",
              ""value"": ""2025-08-26T17:00:00+00:00""
            }
          }
        ],
        ""priority"": 0,
        ""state"": ""Accepted"",
        ""off_nominal_volumes"": []
      },
      ""originating_party"": ""KU USL"",
      ""type_of_operation"": 0,
      ""id"": ""c2b8e6f3-4d5e-6a2f-8b3c-2d4e5f6a7b8c"",
      ""state"": 1,
      ""is_approved"": false,
      ""start_datetime"": ""2025-08-26T14:00:00Z"",
      ""end_datetime"": ""2025-08-26T17:00:00Z"",
      ""flight_declaration_geojson"": {
        ""type"": ""FeatureCollection"",
        ""features"": [
          {
            ""type"": ""Feature"",
            ""properties"": {
              ""time_start"": ""2025-08-26T14:00:00+00:00"",
              ""time_end"": ""2025-08-26T17:00:00+00:00""
            },
            ""geometry"": {
              ""type"": ""Polygon""
            }
          }
        ]
      },
      ""flight_declaration_raw_geojson"": {
        ""type"": ""FeatureCollection"",
        ""features"": [
          {
            ""type"": ""Feature"",
            ""properties"": {
              ""id"": ""0"",
              ""start_datetime"": ""2025-07-16T14:00:00.000Z"",
              ""end_datetime"": ""2025-07-16T17:00:00.000Z"",
              ""max_altitude"": {
                ""meters"": 270,
                ""datum"": ""WGS84""
              },
              ""min_altitude"": {
                ""meters"": 250,
                ""datum"": ""WGS84""
              }
            },
            ""geometry"": {
              ""coordinates"": [
                [-87.63292661168987, 41.886016587353716],
                [-87.63447816674797, 41.88857136525634],
                [-87.63447527667914, 41.890291499998796],
                [-87.63049470704208, 41.892074282923204]
              ],
              ""type"": ""LineString""
            }
          }
        ]
      },
      ""bounds"": ""-87.6344781,41.8860165,-87.6304947,41.8920742"",
      ""approved_by"": null,
      ""submitted_by"": null
    },
    {
      ""operational_intent"": {
        ""volumes"": [
          {
            ""volume"": {
              ""outline_polygon"": {},
              ""altitude_lower"": {
                ""value"": 270,
                ""reference"": ""W84"",
                ""units"": ""M""
              },
              ""altitude_upper"": {
                ""value"": 300,
                ""reference"": ""W84"",
                ""units"": ""M""
              },
              ""outline_circle"": null
            },
            ""time_start"": {
              ""format"": ""RFC3339"",
              ""value"": ""2025-08-26T14:00:00+00:00""
            },
            ""time_end"": {
              ""format"": ""RFC3339"",
              ""value"": ""2025-08-26T17:00:00+00:00""
            }
          }
        ],
        ""priority"": 0,
        ""state"": ""Accepted"",
        ""off_nominal_volumes"": []
      },
      ""originating_party"": ""KU USL"",
      ""type_of_operation"": 0,
      ""id"": ""d3c7f8a4-5e6f-7b3c-9d4e-3f5a6b7c8d9e"",
      ""state"": 1,
      ""is_approved"": false,
      ""start_datetime"": ""2025-08-26T14:00:00Z"",
      ""end_datetime"": ""2025-08-26T17:00:00Z"",
      ""flight_declaration_geojson"": {
        ""type"": ""FeatureCollection"",
        ""features"": [
          {
            ""type"": ""Feature"",
            ""properties"": {
              ""time_start"": ""2025-08-26T14:00:00+00:00"",
              ""time_end"": ""2025-08-26T17:00:00+00:00""
            },
            ""geometry"": {
              ""type"": ""Polygon""
            }
          }
        ]
      },
      ""flight_declaration_raw_geojson"": {
        ""type"": ""FeatureCollection"",
        ""features"": [
          {
            ""type"": ""Feature"",
            ""properties"": {
              ""id"": ""0"",
              ""start_datetime"": ""2025-07-16T14:00:00.000Z"",
              ""end_datetime"": ""2025-07-16T17:00:00.000Z"",
              ""max_altitude"": {
                ""meters"": 300,
                ""datum"": ""WGS84""
              },
              ""min_altitude"": {
                ""meters"": 270,
                ""datum"": ""WGS84""
              }
            },
            ""geometry"": {
              ""coordinates"": [
                [-87.62080680094427, 41.89038971878098],
                [-87.62411486466416, 41.890946221847955],
                [-87.62432704338511, 41.897168135821644],
                [-87.61924609510805, 41.89729117197618],
                [-87.61954743443401, 41.89379255774487]
              ],
              ""type"": ""LineString""
            }
          }
        ]
      },
      ""bounds"": ""-87.6243270,41.8903897,-87.6192460,41.8972911"",
      ""approved_by"": null,
      ""submitted_by"": null
    },
    {
      ""operational_intent"": {
        ""volumes"": [
          {
            ""volume"": {
              ""outline_polygon"": {},
              ""altitude_lower"": {
                ""value"": 240,
                ""reference"": ""W84"",
                ""units"": ""M""
              },
              ""altitude_upper"": {
                ""value"": 300,
                ""reference"": ""W84"",
                ""units"": ""M""
              },
              ""outline_circle"": null
            },
            ""time_start"": {
              ""format"": ""RFC3339"",
              ""value"": ""2025-08-26T14:00:00+00:00""
            },
            ""time_end"": {
              ""format"": ""RFC3339"",
              ""value"": ""2025-08-26T17:00:00+00:00""
            }
          }
        ],
        ""priority"": 0,
        ""state"": ""Accepted"",
        ""off_nominal_volumes"": []
      },
      ""originating_party"": ""KU USL"",
      ""type_of_operation"": 0,
      ""id"": ""e4d6a9b5-6f7a-8c4d-0e5f-4a6b7c8d9e0f"",
      ""state"": 1,
      ""is_approved"": false,
      ""start_datetime"": ""2025-08-26T14:00:00Z"",
      ""end_datetime"": ""2025-08-26T17:00:00Z"",
      ""flight_declaration_geojson"": {
        ""type"": ""FeatureCollection"",
        ""features"": [
          {
            ""type"": ""Feature"",
            ""properties"": {
              ""time_start"": ""2025-08-26T14:00:00+00:00"",
              ""time_end"": ""2025-08-26T17:00:00+00:00""
            },
            ""geometry"": {
              ""type"": ""Polygon""
            }
          }
        ]
      },
      ""flight_declaration_raw_geojson"": {
        ""type"": ""FeatureCollection"",
        ""features"": [
          {
            ""type"": ""Feature"",
            ""properties"": {
              ""id"": ""0"",
              ""start_datetime"": ""2025-07-16T14:00:00.000Z"",
              ""end_datetime"": ""2025-07-16T17:00:00.000Z"",
              ""max_altitude"": {
                ""meters"": 300,
                ""datum"": ""WGS84""
              },
              ""min_altitude"": {
                ""meters"": 240,
                ""datum"": ""WGS84""
              }
            },
            ""geometry"": {
              ""coordinates"": [
                [-87.61101645177524, 41.89002431126269],
                [-87.61387675470968, 41.883425985699304],
                [-87.62114202237261, 41.883148034597816],
                [-87.6233308404695, 41.877364766889926]
              ],
              ""type"": ""LineString""
            }
          }
        ]
      },
      ""bounds"": ""-87.6233308,41.8773647,-87.6110164,41.8900243"",
      ""approved_by"": null,
      ""submitted_by"": null
    },
    {
      ""operational_intent"": {
        ""volumes"": [
          {
            ""volume"": {
              ""outline_polygon"": {},
              ""altitude_lower"": {
                ""value"": 160,
                ""reference"": ""W84"",
                ""units"": ""M""
              },
              ""altitude_upper"": {
                ""value"": 180,
                ""reference"": ""W84"",
                ""units"": ""M""
              },
              ""outline_circle"": null
            },
            ""time_start"": {
              ""format"": ""RFC3339"",
              ""value"": ""2025-08-26T14:00:00+00:00""
            },
            ""time_end"": {
              ""format"": ""RFC3339"",
              ""value"": ""2025-08-26T17:00:00+00:00""
            }
          }
        ],
        ""priority"": 0,
        ""state"": ""Accepted"",
        ""off_nominal_volumes"": []
      },
      ""originating_party"": ""KU USL"",
      ""type_of_operation"": 0,
      ""id"": ""f5e7b0c6-7a8b-9d5e-1f6a-5b7c8d9e0f1a"",
      ""state"": 1,
      ""is_approved"": false,
      ""start_datetime"": ""2025-08-26T14:00:00Z"",
      ""end_datetime"": ""2025-08-26T17:00:00Z"",
      ""flight_declaration_geojson"": {
        ""type"": ""FeatureCollection"",
        ""features"": [
          {
            ""type"": ""Feature"",
            ""properties"": {
              ""time_start"": ""2025-08-26T14:00:00+00:00"",
              ""time_end"": ""2025-08-26T17:00:00+00:00""
            },
            ""geometry"": {
              ""type"": ""Polygon""
            }
          }
        ]
      },
      ""flight_declaration_raw_geojson"": {
        ""type"": ""FeatureCollection"",
        ""features"": [
          {
            ""type"": ""Feature"",
            ""properties"": {
              ""id"": ""0"",
              ""start_datetime"": ""2025-07-16T14:00:00.000Z"",
              ""end_datetime"": ""2025-07-16T17:00:00.000Z"",
              ""max_altitude"": {
                ""meters"": 180,
                ""datum"": ""WGS84""
              },
              ""min_altitude"": {
                ""meters"": 160,
                ""datum"": ""WGS84""
              }
            },
            ""geometry"": {
              ""coordinates"": [
                [-87.65495515245789, 41.893120322113404],
                [-87.64911838522885, 41.890525380468446],
                [-87.64576612989066, 41.886321108326015],
                [-87.64627927718145, 41.87536293254232]
              ],
              ""type"": ""LineString""
            }
          }
        ]
      },
      ""bounds"": ""-87.6549551,41.8753629,-87.6457661,41.8931203"",
      ""approved_by"": null,
      ""submitted_by"": null
    }
  ]
}";
    
    private Coroutine loopCoroutine;

    List<GameObject> flightDeclarationList = new List<GameObject>();

    Dictionary<Guid, GameObject> flightDeclarationSpawnedList = new Dictionary<Guid, GameObject>();

    public bool test = false;

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
        UpdateAllFlightDeclarations(sampleJSON);
    }

    void LoadAllFlightDeclarations()
    {
        ArgonAPI.GetAllFlightDeclarations(
            onSuccess: response =>
            {
                //Debug.Log("Response: " + response);
                UpdateAllFlightDeclarationsLegacy(response);
            },
            onError: error =>
            {
                //Debug.LogError("Error: " + error);
            });
    }

    private void UpdateAllFlightDeclarationsLegacy(string response)
    {
        JSONNode rootNode = JSON.Parse(response);
        foreach (JSONNode result in rootNode["results"].AsArray)
        {
            GameObject flightDeclarationObject = FlightDeclaration.SpawnFlightDeclaration(result);
            flightDeclarationList.Add(flightDeclarationObject);
        }
    }

    void UpdateAllFlightDeclarations(string response)
    {
        JSONNode rootNode = JSON.Parse(response);
        HashSet<Guid> latestIDs = new HashSet<Guid>();

        //Updating and Adding Elements
        foreach (JSONNode flightDeclaration in rootNode["results"].AsArray)
        {
            latestIDs.Add(Guid.Parse(flightDeclaration["id"]));
            if (flightDeclarationSpawnedList.ContainsKey(Guid.Parse(flightDeclaration["id"])))
            {
                flightDeclarationSpawnedList[Guid.Parse(flightDeclaration["id"])].GetComponent<FlightDeclaration>().FlightDeclarationUpdate(flightDeclaration);
            }
            else
            {
                GameObject flightDeclarationObject = FlightDeclaration.SpawnFlightDeclaration(flightDeclaration);
                flightDeclarationSpawnedList.Add(Guid.Parse(flightDeclaration["id"]), flightDeclarationObject);
            }
        }

        //Removing Elements
        foreach (var id in flightDeclarationSpawnedList.Keys.ToList())
        {
            if (!latestIDs.Contains(id))
            {
                GameObject toRemove = flightDeclarationSpawnedList[id];
                flightDeclarationSpawnedList.Remove(id);
                GameObject.Destroy(toRemove);
            }
        }
    }

    public void StartUpdateLoop()
    {
        if (loopCoroutine == null)
        {
            loopCoroutine = StartCoroutine(FlightDeclarationUpdateLoop());
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

    private IEnumerator FlightDeclarationUpdateLoop()
    {
        bool isDone;
        while (true)
        {
            isDone = false;
            ArgonAPI.GetAllFlightDeclarations(
                onSuccess: response =>
                {
                    Debug.Log("Response: " + response);
                    UpdateAllFlightDeclarations(response);
                    isDone = true;
                },
                onError: error =>
                {
                    Debug.LogError("Error: " + error);
                    isDone = true;
                });

            //Wait until coroutine is done
            yield return new WaitUntil(() => isDone);

            //Optional delay between iterations
            yield return new WaitForSeconds(5f);
        }
    }
}
