using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

/* Unity Argon API
 * ArgonAPI
 * A class for making API calls to Argon
 * TO DO: 
 * 1. Expand and add more Argon Endpoints
 * 2. Modify function names for better consistency with Argon Endpoints.
 * 3. Complete the Query Param Parser and test
 */

public static class ArgonAPI
{
    public static void GetAllGeofences(Action<string> onSuccess, Action<string> onError = null, string queryParameters ="")
    {
        CoroutineUtility.Instance.RunCoroutine(GetAllGeofencesCoroutine(onSuccess, onError, queryParameters));
    }

    private static IEnumerator GetAllGeofencesCoroutine(Action<string> onSuccess, Action<string> onError, string queryParameters="")
    {
        if (AccessTokenManager.AccessToken == null) 
        {
            Debug.Log("No valid access token received from Access Token Manager");
            yield break;
        }
        string url = APIEnv.argonURL+"/geo_fence_ops/geo_fence"+queryParameters;

        UnityWebRequest request = UnityWebRequest.Get(url);

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer "+ AccessTokenManager.AccessToken);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Response: " + request.downloadHandler.text);
            onSuccess?.Invoke(request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Error: " + request.error);
            onError?.Invoke(request.error);
        }
    }

    public static void GetAllFlightDeclarations(Action<string> onSuccess, Action<string> onError = null, string queryParameters = "")
    {
        CoroutineUtility.Instance.RunCoroutine(GetAllFlightDeclarationsCoroutine(onSuccess, onError, queryParameters));
    }

    private static IEnumerator GetAllFlightDeclarationsCoroutine(Action<string> onSuccess, Action<string> onError, string queryParameters = "")
    {
        if (AccessTokenManager.AccessToken == null)
        {
            Debug.Log("No valid access token received from Access Token Manager");
            yield break;
        }
        string url = APIEnv.argonURL + "/flight_declaration_ops/flight_declaration"+queryParameters;

        UnityWebRequest request = UnityWebRequest.Get(url);

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + AccessTokenManager.AccessToken);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Response: " + request.downloadHandler.text);
            onSuccess?.Invoke(request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Error: " + request.error);
            onError?.Invoke(request.error);
        }
    }

    public static void GetRIDDataForGivenViewBoundingBox(Action<string> onSuccess, Action<string> onError = null, string queryParameters = "")
    {
        CoroutineUtility.Instance.RunCoroutine(GetRIDDataForGivenViewBoundingBoxCoroutine(onSuccess, onError, queryParameters));
    }

    private static IEnumerator GetRIDDataForGivenViewBoundingBoxCoroutine(Action<string> onSuccess, Action<string> onError, string queryParameters = "")
    {
        if (AccessTokenManager.AccessToken == null)
        {
            Debug.Log("No valid access token received from Access Token Manager");
            yield break;
        }

        string url = APIEnv.argonURL + "/rid/display_data"+queryParameters;

        UnityWebRequest request = UnityWebRequest.Get(url);

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + AccessTokenManager.AccessToken);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Response: " + request.downloadHandler.text);
            onSuccess?.Invoke(request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Error: " + request.error);
            onError?.Invoke(request.error);
        }
    }


    //Function to Parse Query Parameters to add to request URL
    public static string QueryParser(string view = "", DateTime? startDate =  null, DateTime? endDate = null)
    {
        StringBuilder query = new StringBuilder();

        void AddParam(string key, string value)
        {
            if (query.Length > 0)
                query.Append("&");
            query.Append(Uri.EscapeDataString(key));
            query.Append("=");
            query.Append(Uri.EscapeDataString(value));
        }

        if (!string.IsNullOrEmpty(view))
            AddParam("view", view);

        if (startDate.HasValue)
            AddParam("start_date", startDate.Value.ToString("yyyy-MM-dd"));

        if (endDate.HasValue)
            AddParam("end_date", endDate.Value.ToString("yyyy-MM-dd"));

        return query.Length > 0 ? "?" + query.ToString() : string.Empty;
    }
}
