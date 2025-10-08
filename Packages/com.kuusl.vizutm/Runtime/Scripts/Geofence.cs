using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Mathematics;
using SimpleJSON;
using NavigationToolkit.Model;
using NavigationToolkit.Model.LocalTangentPlane;
using NavigationToolkit.Converters;
using CesiumForUnity;
using System.Linq;

/* Unity Argon API
 * Geofence
 * A class for the geofence gameObject
 */

public enum GeoJSONGeometryType
{
    Point,
    MultiPoint,
    LineString,
    MultiLineString,
    Polygon,
    MultiPolygon,
    GeometryCollection
}

public class Geofence : MonoBehaviour
{
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public MeshCollider meshCollider;

    public Guid id; //Geofences should have a uuid associated with them

    //Parsed Geofence values
    public string type;
    public string featureType;
    public GeoJSONGeometryType geometryType;
    public List<(double lat, double lon)> coordinates = new List<(double lat, double lon)>();
    public float upperLimit;
    public float lowerLimit;

    //Polygon Raw values
    public List<Vector2> polygon = new List<Vector2>();
    public float height;

    public static GameObject cesiumGeoreference;

    public override string ToString()
    {
        return $"Geofence\nGUID: {id}\nUpper Limit: {upperLimit}m\nLower Limit: {lowerLimit}m";
    }

    public static GameObject SpawnGeofence(JSONNode rawGeoFence, string uuid)
    {
        GameObject obj = new GameObject("Geofence");
        Geofence geofence = obj.AddComponent<Geofence>();

        geofence.meshFilter = obj.AddComponent<MeshFilter>();
        geofence.meshRenderer = obj.AddComponent<MeshRenderer>();
        geofence.meshCollider = obj.AddComponent<MeshCollider>();

        geofence.GeoJSONParser(rawGeoFence);
        geofence.GeofenceCoordinateBasicConvertor();

        Mesh tempMesh = geofence.GeofenceMeshGenerator(geofence.polygon, geofence.height);
        geofence.meshFilter.mesh = tempMesh;
        geofence.meshCollider.sharedMesh = tempMesh;
        geofence.meshCollider.convex = false; //Set to true if needed
        obj.layer = LayerMask.NameToLayer("Geofence");

        geofence.AssignMaterial();

        if (Guid.TryParse(uuid, out geofence.id))
        {
            Debug.Log("Valid UUID found and assigned: "+geofence.id);
        }
        else
        {
            Debug.LogError("Invalid UUID provided: "+uuid);
        }

        if (cesiumGeoreference == null)
        {
            cesiumGeoreference = GameObject.Find("CesiumGeoreference");
        }
        geofence.WorldPlacementHandler();

        obj.name = "Geofence "+geofence.id.ToString();

        return obj;
    }

    //Parses the provided rawGeofence
    void GeoJSONParser(JSONNode rawGeoFence)
    {
        type = rawGeoFence["type"];
        foreach (JSONNode feature in rawGeoFence["features"].AsArray)
        {
            featureType = feature["type"];
            System.Enum.TryParse(feature["geometry"]["type"], ignoreCase: true, out geometryType); 
            coordinates.Clear();//Necessary. There will be prior data if this function is called by Update Loops
            foreach (JSONNode coordinate in feature["geometry"]["coordinates"][0].AsArray)
            {
                Debug.Log("Latitude: "+ coordinate[1].AsDouble);
                Debug.Log("Longitude: "+ coordinate[0].AsDouble);
                //Notice order of coordinates have been flipped
                //GeoJSON (longitude, latitude)
                //Our convention (latitude, longitude)
                coordinates.Add((coordinate[1].AsDouble, coordinate[0].AsDouble)); 
            }
            upperLimit = feature["properties"]["upper_limit"].AsFloat;
            lowerLimit = feature["properties"]["lower_limit"].AsFloat;
        }
    }

    public void GeofenceUpdate(JSONNode rawGeoFence)
    {
        List<(double lat, double lon)> coordinatesOriginal = new List<(double lat, double lon)>(coordinates);
        float upperLimitOriginal = upperLimit;
        float lowerLimitOrignal = lowerLimit;
        GeoJSONParser(rawGeoFence);
        //Check if the new data is same as the old, if so, skip regeneration.
        if (upperLimit==upperLimitOriginal&&lowerLimit==lowerLimitOrignal&&coordinates.SequenceEqual(coordinatesOriginal))
            return;

        Debug.Log("upperlimit: "+(upperLimit == upperLimitOriginal));
        Debug.Log("LowerLimit: "+(lowerLimit == lowerLimitOrignal));
        Debug.Log("Coordinates: "+(coordinates.SequenceEqual(coordinatesOriginal)));

        Debug.Log("Updating Geofence Mesh");
        GeofenceCoordinateBasicConvertor();

        Mesh tempMesh = GeofenceMeshGenerator(polygon, height);
        meshFilter.mesh = null;
        meshFilter.mesh = tempMesh;
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = tempMesh;
        meshCollider.convex = false; //Set to true if needed

        Debug.Log("Vertex count: " + meshFilter.mesh.vertexCount);
        //AssignMaterial();
        WorldPlacementHandler();
    }

    //A function that will parse and output the result of the GeoJSON
    void GeoJSONParserDebug(JSONNode rawGeoFence)
    {
        Debug.Log("Type: " + rawGeoFence["type"]);
        foreach (JSONNode feature in rawGeoFence["features"].AsArray)
        {
            Debug.Log("Feature Type: " + feature["type"]);
            Debug.Log("Geometry Type: " + feature["geometry"]["type"]);
            foreach (JSONNode coordinate in feature["geometry"]["coordinates"][0].AsArray)
            {
                Debug.Log("Coordinate: " + coordinate[0] + ", " + coordinate[1]);
            }
            Debug.Log("Upper Limit: " + feature["properties"]["upper_limit"]);
            Debug.Log("Lower Limit: " + feature["properties"]["lower_limit"]);
        }
    }

    //Convert GPS values of the geofence into raw values for Unity Mesh
    //Assumes the mesh will be a prism. Its a naive yet simple interpretation. 
    void GeofenceCoordinateBasicConvertor()
    {
        int lastIndex = coordinates.Count - 1;
        LlaPosition targetPosition;
        LlaPosition originPosition = new LlaPosition(coordinates[lastIndex].lat, coordinates[lastIndex].lon, 0);
        LtpPosition convertedPosition;

        PositionConverter positionConverter = new PositionConverter();

        for (int i = 0; i < lastIndex; i++)
        {
            targetPosition = new LlaPosition(coordinates[i].lat, coordinates[i].lon, 0);

            convertedPosition = positionConverter.LlaToLtp(targetPosition, originPosition);
            //In Unity, East = +X, Up = +Y, North = +Z. With ENU, East = +X, Up = +Z, North = +Y. 
            polygon.Add(new Vector2((float)convertedPosition.East, (float)convertedPosition.North));
            Debug.Log("Coordinate "+i+": "+polygon[i]);
        }

        height = upperLimit - lowerLimit;
    }

    //[WIP]
    //Convert GPS values of the geofence into raw values for Unity Mesh
    //Assumes the mesh will be an inverted frustum/truncated pyramid instead of a prism
    void GeofenceCoordinateAccurateConvertor()
    {

        height = upperLimit - lowerLimit;
    }

    //This function determines which GPS to Unity transform backend to use
    //It then calls the backend specific function to handle placement
    void WorldPlacementHandler()
    {
        PlaceGeofenceCesium();
    }

    //A function to handle geofence placement using Cesium. 
    void PlaceGeofenceCesium()
    {
        //Parent to Cesium Georeference
        if (cesiumGeoreference != null)
        {
            transform.parent = cesiumGeoreference.transform;
        }
        CesiumGlobeAnchor cesiumGlobeAnchor = this.gameObject.GetComponent<CesiumGlobeAnchor>();
        //Add Globe Anchor
        if (cesiumGlobeAnchor==null)
        {
            cesiumGlobeAnchor = this.gameObject.AddComponent<CesiumGlobeAnchor>();
        }
        
        cesiumGlobeAnchor.longitudeLatitudeHeight = new double3(coordinates[0].lon, coordinates[0].lat, lowerLimit);

    }

    //[WIP] Function for assigning the correct material to the geofence 
    void AssignMaterial()
    {
        //this.meshRenderer.material = new Material(Shader.Find("Standard"));
        this.meshRenderer.material = Resources.Load<Material>("Materials/TransparentCyan");
    }

    //Creates the geofence mesh from the provided geoJSON.
    Mesh GeofenceMeshGenerator(List<Vector2> poly, float height)
    {
        // Ensure counter-clockwise winding
        if (SignedPolygonArea(poly) < 0)
            poly.Reverse();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        int count = poly.Count;

        // Bottom face
        for (int i = 0; i < count; i++)
            vertices.Add(new Vector3(poly[i].x, 0, poly[i].y));

        // Top face
        for (int i = 0; i < count; i++)
            vertices.Add(new Vector3(poly[i].x, height, poly[i].y));

        // Bottom face triangles
        for (int i = 1; i < count - 1; i++)
        {
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i + 1);
        }

        // Top face triangles (reverse winding)
        for (int i = 1; i < count - 1; i++)
        {
            triangles.Add(count);
            triangles.Add(count + i + 1);
            triangles.Add(count + i);
        }

        // Side faces
        for (int i = 0; i < count; i++)
        {
            int next = (i + 1) % count;

            int bl = i;
            int br = next;
            int tl = i + count;
            int tr = next + count;

            triangles.Add(bl);
            triangles.Add(tl);
            triangles.Add(tr);

            triangles.Add(bl);
            triangles.Add(tr);
            triangles.Add(br);
        }

        // UVs
        for (int i = 0; i < vertices.Count; i++)
            uvs.Add(new Vector2(vertices[i].x, vertices[i].z));

        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
        return mesh;
    }

    // Helper method to compute signed area
    // Shoelace formula (Specifically the triangle formula variant)
    float SignedPolygonArea(List<Vector2> poly)
    {
        float area = 0;
        for (int i = 0; i < poly.Count; i++)
        {
            Vector2 current = poly[i];
            Vector2 next = poly[(i + 1) % poly.Count];
            area += (current.x * next.y) - (next.x * current.y);
        }
        return area * 0.5f;
    }
}