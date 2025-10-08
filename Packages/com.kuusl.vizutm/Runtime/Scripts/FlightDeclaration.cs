using CesiumForUnity;
using NavigationToolkit.Converters;
using NavigationToolkit.Model.LocalTangentPlane;
using NavigationToolkit.Model;
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Globalization;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using System.Linq;


public class FlightDeclaration : MonoBehaviour
{

    public static bool enableCylinderBuffer = false;

    //GeoJSON
    public string type;
    public string featureType;
    public GeoJSONGeometryType geometryType;
    public List<(double lat, double lon)> coordinates = new List<(double lat, double lon)>();
    public List<(double lat, double lon)> bounds = new List<(double lat, double lon)>();

    public float minAltitude;
    public float maxAltitude;

    public DateTime startDateTime;
    public DateTime endDateTime;

    public bool isApproved;

    public string originatingParty;
    public int typeOfOperation; //Replace with Enum when you learn more
    public int state; //Replace with Enum when you learn more
    public string approvedBy; //Email
    public string submittedBy; //Email

    public Guid id; //Flight declarations should have a uuid associated with them

    public static GameObject cesiumGeoreference;

    public GameObject outerSpline;
    private SplineContainer outerSplineContainer;
    private MeshRenderer outerMeshRenderer;

    public GameObject innerSpline;
    private SplineContainer innerSplineContainer;
    private MeshRenderer innerMeshRenderer;

    public List<Vector2> splinePoints = new List<Vector2>();

    public List<List<Vector2>> polygons = new List<List<Vector2>>();

    private LineRenderer lineRenderer;

    public override string ToString()
    {
        return $"Flight Declaration\nGUID: {id}\nMax Altitude: {maxAltitude}m\nMin Altitude: {minAltitude}m\nStart Date: {startDateTime}\nEnd Date: {endDateTime}\nOriginating Party: {originatingParty}\nApproval Status: {isApproved}\nApproved By: {approvedBy}\nSubmitted By: {submittedBy}";
    }

    public static GameObject SpawnFlightDeclaration(JSONNode flightDeclarationJSON)
    {
        GameObject obj = new GameObject("FlightDeclaration");
        obj.layer = LayerMask.NameToLayer("FlightDeclaration");

        //Assign components to gameObjects
        FlightDeclaration flightDeclaration = obj.AddComponent<FlightDeclaration>();

        //Call Flight declaration functions to parse and initialize Flight declaration
        flightDeclaration.Parser(flightDeclarationJSON);
        flightDeclaration.CoordinateBasicConvertor();

        flightDeclaration.FlightDeclarationVisualizer();

        flightDeclaration.AssignMaterial();

        if (cesiumGeoreference == null)
        {
            cesiumGeoreference = GameObject.Find("CesiumGeoreference");
        }

        flightDeclaration.WorldPlacementHandler();

        obj.name = "Flight Declaration "+flightDeclaration.id.ToString();

        return obj;
    }

    //Parses and initializes the flight declaration
    void Parser(JSONNode flightDeclarationJSON)
    {
        originatingParty = flightDeclarationJSON["originating_party"];
        typeOfOperation = flightDeclarationJSON["type_of_operation"].AsInt;

        if (Guid.TryParse(flightDeclarationJSON["id"], out id))
        {
            Debug.Log("Valid UUID found and assigned: " + id);
        }
        else
        {
            Debug.LogError("Invalid UUID provided: " + flightDeclarationJSON["id"]);
        }

        state = flightDeclarationJSON["state"].AsInt;
        isApproved = flightDeclarationJSON["is_approved"].AsBool;

        //startDateTime
        if(DateTime.TryParse(
            flightDeclarationJSON["start_datetime"],
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
            out startDateTime
            ))
        {
            Debug.Log("Parsed startDate: "+startDateTime.ToString("o"));
        }
        else
        {
            Debug.LogError("Unable to parse start DateTime: " + flightDeclarationJSON["start_datetime"]);
        }

        //endDatetime
        if (DateTime.TryParse(
            flightDeclarationJSON["start_datetime"],
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
            out endDateTime
            ))
        {
            Debug.Log("Parsed startDate: " + endDateTime.ToString("o"));
        }
        else
        {
            Debug.LogError("Unable to parse start DateTime: " + flightDeclarationJSON["start_datetime"]);
        }

        bounds = ParseLongLatString(flightDeclarationJSON["bounds"]);
        approvedBy = flightDeclarationJSON["approved_by"];
        submittedBy = flightDeclarationJSON["submitted_by"];

        type = flightDeclarationJSON["flight_declaration_raw_geojson"]["type"];
        foreach (JSONNode feature in flightDeclarationJSON["flight_declaration_raw_geojson"]["features"].AsArray)
        {
            featureType = feature["type"];
            System.Enum.TryParse(feature["geometry"]["type"], ignoreCase: true, out geometryType);
            coordinates.Clear(); //Necessary. There will be prior data if this function is called by Update Loops
            foreach (JSONNode coordinate in feature["geometry"]["coordinates"].AsArray)
            {
                Debug.Log("Latitude: " + coordinate[1].AsDouble);
                Debug.Log("Longitude: " + coordinate[0].AsDouble);
                //Notice that order of coordinates have been flipped
                //GeoJSON (longitude, latitude)
                //Our convention (latitude, longitude)
                coordinates.Add((coordinate[1].AsDouble, coordinate[0].AsDouble));
            }
            maxAltitude = feature["properties"]["max_altitude"]["meters"].AsFloat;
            minAltitude = feature["properties"]["min_altitude"]["meters"].AsFloat;
        }
    }

    public void FlightDeclarationUpdate(JSONNode flightDeclarationJSON)
    {
        List<(double lat, double lon)> coordinatesOriginal = new List<(double lat, double lon)>(coordinates);
        float minAltitudeOriginal = minAltitude;
        float maxAltitudeOriginal = maxAltitude;

        Parser(flightDeclarationJSON);
        //Check if the new data is same as the old, if so, skip regeneration.
        if (minAltitude == minAltitudeOriginal && maxAltitude == maxAltitudeOriginal && coordinates.SequenceEqual(coordinatesOriginal))
            return;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        CoordinateBasicConvertor();

        FlightDeclarationVisualizer();

        AssignMaterial();
            
        WorldPlacementHandler();
    }

    public static List<(double lat, double lon)> ParseLongLatString(string input)
    {
        List<(double lat, double lon)> latLongList = new List<(double lat, double lon)>();
        string[] tokens = input.Split(',');

        if (tokens.Length % 2 != 0)
        {
            Debug.LogError("Invalid input string. Must contain an even number of values.");
            return latLongList;
        }

        for (int i = 0; i < tokens.Length; i += 2)
        {
            if (double.TryParse(tokens[i], out double lon) && double.TryParse(tokens[i + 1], out double lat))
            {
                latLongList.Add((lat, lon));
            }
            else
            {
                Debug.LogWarning($"Invalid double values at index {i} and {i + 1}");
            }
        }

        return latLongList;
    }

    //Maybe I should turn this into a common function that all classes can access but I can't because the ordering of the coordinates is different
    void CoordinateBasicConvertor()
    {
        LlaPosition targetPosition;
        LlaPosition originPosition = new LlaPosition(coordinates[0].lat, coordinates[0].lon, 0);
        LtpPosition convertedPosition;

        PositionConverter positionConverter = new PositionConverter();

        for (int i = 0; i < coordinates.Count; i++)
        {
            targetPosition = new LlaPosition(coordinates[i].lat, coordinates[i].lon, 0);

            convertedPosition = positionConverter.LlaToLtp(targetPosition, originPosition);
            //In Unity, East = +X, Up = +Y, North = +Z. With ENU, East = +X, Up = +Z, North = +Y. 
            splinePoints.Add(new Vector2((float)convertedPosition.East, (float)convertedPosition.North));
            Debug.Log("Coordinate " + i + ": " + splinePoints[i]);
        }
    }

    void CoordinateAccurateConvertor()
    {
        throw new NotImplementedException(); 
    }

    //This function determines which GPS to Unity transform backend to use
    //It then calls the backend specific function to handle placement
    void WorldPlacementHandler()
    {
        PlaceFlightDeclarationCesium();
    }

    //A function to handle geofence placement using Cesium. 
    void PlaceFlightDeclarationCesium()
    {
        //Parent to Cesium Georeference
        if (cesiumGeoreference != null)
        {
            transform.parent = cesiumGeoreference.transform;
        }
        //Add Globe Anchor
        CesiumGlobeAnchor cesiumGlobeAnchor = this.gameObject.AddComponent<CesiumGlobeAnchor>();
        cesiumGlobeAnchor.longitudeLatitudeHeight = new double3(coordinates[0].lon, coordinates[0].lat, (maxAltitude+minAltitude)/2);
    }

    //[WIP] Function for assigning the correct material to the geofence
    //Need to be modified so that it is more flexible in case the required Resources were not packaged. 
    void AssignMaterial()
    {
        //this.meshRenderer.material = new Material(Shader.Find("Standard"));
        //this.meshRenderer.material = Resources.Load<Material>("Materials/MRTK_Standard_TransparentEmerald");
        outerMeshRenderer.material = Resources.Load<Material>("Materials/TransparentGreen");
        innerMeshRenderer.material = Resources.Load<Material>("Materials/Black");
    }

    private void FlightDeclarationVisualizer()
    {
        //Create the outer spline object with component
        outerSpline = new GameObject("Outer Spline");
        outerSpline.transform.parent = this.transform;
        outerSplineContainer = outerSpline.AddComponent<SplineContainer>();

        //Create the inner spline object with component
        innerSpline = new GameObject("Inner Spline");
        innerSpline.transform.parent = this.transform;
        innerSplineContainer = innerSpline.AddComponent<SplineContainer>();

        //Creating the spline from coordinates
        var spline = new Spline();

        for (int i = 0; i < splinePoints.Count; i++)
        {
            Vector3 position = new Vector3(splinePoints[i].x, 0f, splinePoints[i].y); // Y as Z for 2D on XZ plane
            BezierKnot knot = new BezierKnot(position);
            spline.Add(knot);
        }

        //Assigning the spline
        outerSplineContainer.Spline = spline;
        innerSplineContainer.Spline = spline;

        //Generating the Outer Spline Mesh
        var extrudeOuter = outerSpline.AddComponent<SplineExtrude>();
        extrudeOuter.Container = outerSplineContainer;
        var meshFilterOuter = outerSpline.GetComponent<MeshFilter>();
        meshFilterOuter.mesh = new Mesh { name = "OuterSplineMesh" };

        extrudeOuter.Radius = 50f; //Set the buffer size here. Ideally, the buffer sizes are obtained from flight declaration. 
        extrudeOuter.Rebuild();

        outerMeshRenderer = outerSpline.GetComponent<MeshRenderer>();
        var meshColliderOuter = outerSpline.AddComponent<MeshCollider>();
        meshColliderOuter.sharedMesh = meshFilterOuter.sharedMesh;
        meshColliderOuter.convex = false;
        outerSpline.layer = LayerMask.NameToLayer("FlightDeclaration");

        if (!enableCylinderBuffer)
        {
            outerSpline.SetActive(false);
        }

        //Generating the Inner Spline Mesh
        var extrudeInner = innerSpline.AddComponent<SplineExtrude>();
        extrudeInner.Container = innerSplineContainer;
        var meshFilterInner = innerSpline.GetComponent<MeshFilter>();
        meshFilterInner.mesh = new Mesh { name = "InnerSplineMesh" };

        extrudeInner.Radius = 1f; //Set the buffer size here. Ideally, the buffer sizes are obtained from flight declaration. 
        extrudeInner.Rebuild();

        innerMeshRenderer = innerSpline.GetComponent<MeshRenderer>();
        var meshColliderInner = innerSpline.AddComponent<MeshCollider>();
        meshColliderInner.sharedMesh = meshFilterInner.sharedMesh;
        meshColliderInner.convex = false;
        innerSpline.layer = LayerMask.NameToLayer("FlightDeclaration");

        CakeVisualizer();
    }

    private void CakeVisualizer()
    {
        float lengthBuffer = 10f; //10 meters
        float widthBuffer = 50f; //50 meters
        float height = maxAltitude - minAltitude;

        //Iterate over line segments to generate Polygons
        for (int i = 0; i< splinePoints.Count-1; i++)
        {
            Vector2 pointA = splinePoints[i];
            Vector2 pointB = splinePoints[i+1];

            Vector2 direction = (pointB - pointA).normalized;

            // Step 2: Perpendicular vector (rotated 90 degrees)
            Vector2 perpendicular = new Vector2(-direction.y, direction.x);

            // Step 3: Extend the line at both ends
            Vector2 extendedA = pointA - direction * lengthBuffer;
            Vector2 extendedB = pointB + direction * lengthBuffer;

            // Step 4: Compute rectangle corners
            Vector2 corner1 = extendedA + perpendicular * widthBuffer;
            Vector2 corner2 = extendedA - perpendicular * widthBuffer;
            Vector2 corner3 = extendedB - perpendicular * widthBuffer;
            Vector2 corner4 = extendedB + perpendicular * widthBuffer;

            List<Vector2> polygon = new List<Vector2> {corner1, corner2, corner3, corner4};

            polygons.Add(polygon);
        }

        int count = 0;

        //Generate the GameObject and mesh for each polygon
        foreach (var polygon in polygons)
        {
            GameObject obj = new GameObject("Buffer_" + count);
            obj.transform.parent = this.transform;
            obj.transform.localPosition += new Vector3(0, -(height/2), 0);
            MeshFilter meshfilter = obj.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
            var meshCollider = obj.AddComponent<MeshCollider>();

            var tempMesh = MeshGenerator(polygon, height);
            meshfilter.mesh = tempMesh;
            meshCollider.sharedMesh = tempMesh;
            meshCollider.convex = false;
            obj.layer = LayerMask.NameToLayer("FlightDeclaration");
            if (enableCylinderBuffer)
            {
                obj.SetActive(false);
            }

            meshRenderer.material = Resources.Load<Material>("Materials/TransparentPurple");
            count++;
        }
    }

    //Not Implemented. 
    private void PolygonGenerator()
    {
        float lengthBuffer = 10f; //10 meters
        float widthBuffer = 50f; //50 meters

        Vector2 pointA = splinePoints[0];
        Vector2 pointB = splinePoints[1];

        Vector2 direction = (pointB - pointA).normalized;

        // Step 2: Perpendicular vector (rotated 90 degrees)
        Vector2 perpendicular = new Vector2(-direction.y, direction.x);

        // Step 3: Extend the line at both ends
        Vector2 extendedA = pointA - direction * lengthBuffer;
        Vector2 extendedB = pointB + direction * lengthBuffer;

        // Step 4: Compute rectangle corners
        Vector2 corner1 = extendedA + perpendicular * widthBuffer;
        Vector2 corner2 = extendedA - perpendicular * widthBuffer;
        Vector2 corner3 = extendedB - perpendicular * widthBuffer;
        Vector2 corner4 = extendedB + perpendicular * widthBuffer;
    }

    //Creates the geofence mesh from the provided geoJSON.
    Mesh MeshGenerator(List<Vector2> poly, float height)
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
