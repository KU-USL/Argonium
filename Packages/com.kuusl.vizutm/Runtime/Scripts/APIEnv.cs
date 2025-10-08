using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Unity Argon API
 * APIEnv 
 * A static class containing variables and endpoints for the making Argon API calls.
 * You will need to obtain credentials and URLs from your Deployment of the Argon stack and fill them here. 
 * TO DO: Export the endpoint collection from Postman and add endpoints
 */

public static class APIEnv
{
    //URLs
    public static string passportURL = "";
    public static string argonURL = "";

    //Client Credentials
    public static string clientIdPKCE = "";
    public static string clientSecretPKCE = "";
    public static string clientIdCC = "";
    public static string clientSecretCC = "";
    public static string clientIdRO = "";
    public static string clientSecretRO = "";

    //User Credentials
    public const string username = "";
    public const string password = "";

    public const string scope = "argonserver.read argonserver.write";

    //API Endpoints (Export and copy it in)
}
