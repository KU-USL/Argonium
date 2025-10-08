using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A class for handling the raycast system in the sample application.
/// While the behavior is fundamentally simple, its very complicated to work with from all the branching possibilities it needs to accomodate.
/// Code refactor is required to reduce technical debt of maintaining this class. 
/// </summary>
public class RayCastSystem : MonoBehaviour
{
    public bool hoverEnabled = false;

    public bool objectSelected = false;

    public InputActionReference test;
    public InputActionReference select;
    public PlayerInput playerInput;
    public ShaderHighlightSystem baseShaderHighlight;

    public static event Action<GameObject> OnObjectHit;
    GameObject currentHover = null;
    GameObject previousHover = null;
    GameObject currentSelection = null;
    GameObject previousSelection = null;

    int layerMask;

    Ray hoverRay;

    public void RecalculateMask(int mask)
    {
        layerMask ^= (1 << mask);
    }

    private void OnEnable()
    {
        test.action.started += Fire;
        select.action.started += SelectObject;
        UIManager.LayerMaskChange += RecalculateMask;
    }

    private void OnDisable()
    {
        test.action.started -= Fire;
        select.action.started -= SelectObject;
        UIManager.LayerMaskChange -= RecalculateMask;
    }

    private void Fire(InputAction.CallbackContext obj)
    {
        Debug.Log("Button Pressed!");
    }

    private void SelectObject(InputAction.CallbackContext obj)
    {
        Ray ray;
        if (playerInput.currentControlScheme == "KeyboardAndMouse")
        {
            Debug.Log("KM Selection Event");
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            HandleSelect(ray);
        }
        else
        {
            Debug.Log("Gamepad Selection Event");
            //Default for other modes
            ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            HandleSelect(ray);
        }
    }


    void HandleSelect(Ray ray)
    {
        //Perform the Raycast
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            //An object has been found from the raycast
            //If its FlightDeclaration, then it needs different logic to obtain the relevant GameObject
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("FlightDeclaration"))
            {
                currentSelection = hit.collider.gameObject.transform.parent.gameObject;
            }
            else
            {
                currentSelection = hit.collider.gameObject;
            }

            //If we have already hovered on this object and nothing has changed, then there is nothing to do
            if (currentSelection == previousSelection)
                return;
            Debug.Log("Selected Object");
            //If its a new object
            if (hit.collider.gameObject.GetComponent<ShaderHighlightSystem>())
            {
                //Make sure the previous Object Stops its effect. 
                if (previousSelection != null)
                {
                    if (previousSelection.layer == LayerMask.NameToLayer("FlightDeclaration"))
                    {
                        foreach (Transform child in previousSelection.transform)
                        {
                            child.GetComponent<ShaderHighlightSystem>().StopEffect();
                        }
                    }
                    else
                    {
                        previousSelection.GetComponent<ShaderHighlightSystem>().StopEffect();
                    }
                }
                //Start the effect on the current object
                if (currentSelection.layer == LayerMask.NameToLayer("FlightDeclaration"))
                {
                    foreach (Transform child in currentSelection.transform)
                    {
                        if(child.gameObject.activeInHierarchy)
                            child.GetComponent<ShaderHighlightSystem>().StartEffect("Select");
                    }
                }
                else
                {
                    currentSelection.GetComponent<ShaderHighlightSystem>().StartEffect("Select");
                }
                OnObjectHit?.Invoke(hit.collider.gameObject);
                //Set previous object as current object for the next pass
                previousSelection = currentSelection;
                objectSelected = true; 
                Debug.Log("Hover: " + GetHierarchyPath(hit.transform));
            }
            else
            {
                //Make sure the previous Object Stops its effect. 
                if (previousSelection != null)
                {
                    if (previousSelection.layer == LayerMask.NameToLayer("FlightDeclaration"))
                    {
                        foreach (Transform child in previousSelection.transform)
                        {
                            if (child.gameObject.activeInHierarchy)
                                child.GetComponent<ShaderHighlightSystem>().StopEffect();
                        }
                    }
                    else
                    {
                        previousSelection.GetComponent<ShaderHighlightSystem>().StopEffect();
                    }
                }
                currentSelection = null;
                previousSelection = null;
                objectSelected = false;
            }
        }
        else
        {
            //If no object was found, then we need to reset previous and current Hover.
            //Make sure the previous Object Stops its effect. 
            if (previousSelection != null)
            {
                if (previousSelection.layer == LayerMask.NameToLayer("FlightDeclaration"))
                {
                    foreach (Transform child in previousSelection.transform)
                    {
                        if (child.gameObject.activeInHierarchy)
                            child.GetComponent<ShaderHighlightSystem>().StopEffect();
                    }
                }
                else
                {
                    previousSelection.GetComponent<ShaderHighlightSystem>().StopEffect();
                }
            }
            previousSelection = null;
            currentSelection = null;
            objectSelected = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Drone", "Geofence", "FlightDeclaration");
    }

    // Update is called once per frame
    void Update()
    {
        //if (!hoverEnabled)
        //    return;

        if (playerInput.currentControlScheme == "KeyboardAndMouse")
        {
            //Debug.Log("KM Selection Event");
            hoverRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            HandleHover(hoverRay);
        }
        else
        {
            //Debug.Log("Gamepad Selection Event");
            //Default for other modes
            hoverRay = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            HandleHover(hoverRay);
        }
    }

    void HandleHover(Ray ray)
    {
        //Perform the Raycast
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            //An object has been found from the raycast
            //If its FlightDeclaration, then it needs different logic to obtain the relevant GameObject
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("FlightDeclaration"))
            { 
                currentHover = hit.collider.gameObject.transform.parent.gameObject;
            }
            else
            {
                currentHover = hit.collider.gameObject;
            }

            //If we have already hovered on this object and nothing has changed, then there is nothing to do
            if (currentHover == previousHover)
                return;

            //Make sure the object has not already been selected
            if (currentHover == previousSelection && previousSelection != null)
                return;

            //Debug.Log("Point of Interest");
            //If its a new object
            if (hit.collider.gameObject.GetComponent<ShaderHighlightSystem>())
            {
                //Make sure the previous Object Stops its effect. 
                if (previousHover != null)
                {
                    if (previousHover != previousSelection)
                    {
                        if (previousHover.layer == LayerMask.NameToLayer("FlightDeclaration"))
                        {
                            foreach (Transform child in previousHover.transform)
                            {
                                if (child.gameObject.activeInHierarchy)
                                    child.GetComponent<ShaderHighlightSystem>().StopEffect();
                            }
                        }
                        else
                        {
                            previousHover.GetComponent<ShaderHighlightSystem>().StopEffect();
                        }
                    }
                }
                //Start the effect on the current object
                if (currentHover.layer == LayerMask.NameToLayer("FlightDeclaration"))
                {
                    foreach (Transform child in currentHover.transform)
                    {
                        if (child.gameObject.activeInHierarchy)
                            child.GetComponent<ShaderHighlightSystem>().StartEffect("Hover");
                    }
                }
                else
                {
                    currentHover.GetComponent<ShaderHighlightSystem>().StartEffect("Hover");
                }
                //Set previous object as current object for the next pass
                previousHover = currentHover;
                Debug.Log("Hover: " + GetHierarchyPath(hit.transform));
            }
            else
            {
                //Make sure the previous Object Stops its effect. 
                if (previousHover != null)
                {
                    if (previousHover != previousSelection)
                    {
                        if (previousHover.layer == LayerMask.NameToLayer("FlightDeclaration"))
                        {
                            foreach (Transform child in previousHover.transform)
                            {
                                if (child.gameObject.activeInHierarchy)
                                    child.GetComponent<ShaderHighlightSystem>().StopEffect();
                            }
                        }
                        else
                        {
                            previousHover.GetComponent<ShaderHighlightSystem>().StopEffect();
                        }
                    }
                }
                if (currentHover.layer == LayerMask.NameToLayer("FlightDeclaration"))
                {
                    foreach (Transform child in currentHover.transform)
                    {
                        var shs = child.gameObject.AddComponent<ShaderHighlightSystem>();
                        shs.effects = new List<HighlightEffect>(baseShaderHighlight.effects);
                        Color baseColor = child.GetComponent<Renderer>().material.color;
                        HighlightEffect temp = shs.effects[1];
                        temp.targetColor = Color.Lerp(baseColor, Color.black, 0.30f);
                        temp.targetColor.a = baseColor.a;
                        shs.effects[1] = temp;
                        if (child.gameObject.activeInHierarchy)
                            shs.StartEffect("Hover");
                    }
                }
                else
                {
                    var shs = currentHover.AddComponent<ShaderHighlightSystem>();
                    shs.effects = new List<HighlightEffect>(baseShaderHighlight.effects);
                    Color baseColor = currentHover.GetComponent<Renderer>().material.color;
                    HighlightEffect temp = shs.effects[1];
                    temp.targetColor = Color.Lerp(baseColor, Color.black, 0.30f);
                    temp.targetColor.a = baseColor.a;
                    shs.effects[1] = temp;
                    shs.StartEffect("Hover");
                }
                previousHover = currentHover;
                Debug.Log("Added Component and hover: "+ GetHierarchyPath(hit.transform));
            }
        }
        else
        {
            //If no object was found, then we need to reset previous and current Hover.
            //Make sure the previous Object Stops its effect. 
            if (previousHover != null)
            {
                if (previousHover != previousSelection)
                {
                    if (previousHover.layer == LayerMask.NameToLayer("FlightDeclaration"))
                    {
                        foreach (Transform child in previousHover.transform)
                        {
                            if (child.gameObject.activeInHierarchy)
                                child.GetComponent<ShaderHighlightSystem>().StopEffect();
                        }
                    }
                    else
                    {
                        previousHover.GetComponent<ShaderHighlightSystem>().StopEffect();
                    }
                }
            }
            previousHover = null;
            currentHover = null;
        }
    }

    string GetHierarchyPath(Transform transform)
    {
        string path = transform.name;
        while (transform.parent != null)
        {
            transform = transform.parent;
            path = transform.name + "/" + path;
        }
        return path;
    }
}
