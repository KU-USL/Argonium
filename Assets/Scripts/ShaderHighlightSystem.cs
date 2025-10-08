using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public struct HighlightEffect
{
    public string effectName;
    public Color targetColor;
    public AnimationCurve curve;
    public bool loop;
    public int speedMultiplier;
}

public class ShaderHighlightSystem : MonoBehaviour
{
    public List<HighlightEffect> effects = new List<HighlightEffect>();
    public Renderer objectRenderer;
    public int materialIndex = 0;
    public bool affectAllMaterials = false; // New boolean to control all materials
    private List<Material> objectMaterials = new List<Material>();
    private Coroutine currentCoroutine;
    private List<Color> originalColors = new List<Color>();
    private string currentEffectName;
    private bool isNonLoopingEffectRunning;

    private bool isInitialized = false;

    void Start()
    {
        if (objectRenderer == null)
        {
            objectRenderer = GetComponent<Renderer>();
        }
        if (objectRenderer != null)
        {
            if (affectAllMaterials)
            {
                foreach (var mat in objectRenderer.materials)
                {
                    objectMaterials.Add(mat);
                    originalColors.Add(mat.color);
                }
            }
            else if (materialIndex >= 0 && materialIndex < objectRenderer.materials.Length)
            {
                objectMaterials.Add(objectRenderer.materials[materialIndex]);
                originalColors.Add(objectRenderer.materials[materialIndex].color);
            }
            else
            {
                Debug.LogError("Material index is out of range.");
            }
            isInitialized = true;
        }
        else
        {
            isInitialized = false;
        }
    }

    public void StartEffect(string effectName)
    {
        //if (!isInitialized)
        //{
        //    Debug.Log("Intialization Not Complete");
        //    return;
        //}
        HighlightEffect effect = effects.Find(e => e.effectName == effectName);
        if (effect.effectName != null)
        {
            if (currentCoroutine != null && currentEffectName == effectName)
            {
                // If the same effect is already running, do nothing
                return;
            }

            if (currentCoroutine != null && !isNonLoopingEffectRunning)
            {
                StopCoroutine(currentCoroutine);
                ResetToDefaultState();
            }

            currentEffectName = effectName;
            currentCoroutine = StartCoroutine(InterpolateColor(effect));
        }
    }

    public void StopEffect()
    {
        //if (!isInitialized)
        //{
        //    Debug.Log("Intialization Not Complete");
        //    return;
        //}
        if (currentCoroutine != null && !isNonLoopingEffectRunning)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
            currentEffectName = null;
            ResetToDefaultState();
        }
    }

    public void ResetToDefaultState()
    {
        //if (!isInitialized)
        //{
        //    Debug.Log("Intialization not complete");
        //    return;
        //}
        for (int i = 0; i < objectMaterials.Count; i++)
        {
            objectMaterials[i].color = originalColors[i];
        }
    }

    private IEnumerator InterpolateColor(HighlightEffect effect)
    {
        while (!isInitialized)
            yield return null;
        float time = 0f;
        List<Color> initialColors = new List<Color>();

        foreach (var mat in objectMaterials)
        {
            initialColors.Add(mat.color);
        }

        Debug.Log(effect.effectName);

        float curveEndTime = effect.curve.keys[effect.curve.length - 1].time;

        isNonLoopingEffectRunning = !effect.loop;

        while (true)
        {
            float t = effect.curve.Evaluate(time);

            for (int i = 0; i < objectMaterials.Count; i++)
            {
                objectMaterials[i].color = Color.Lerp(initialColors[i], effect.targetColor, t);
            }

            time += Time.deltaTime * effect.speedMultiplier;

            if (time >= curveEndTime)
            {
                if (effect.loop)
                {
                    time = 0f;
                    for (int i = 0; i < objectMaterials.Count; i++)
                    {
                        initialColors[i] = objectMaterials[i].color; // Reset initial color to current color
                    }
                }
                else
                {
                    break;
                }
            }

            yield return null;
        }

        if (!effect.loop)
        {
            for (int i = 0; i < objectMaterials.Count; i++)
            {
                objectMaterials[i].color = effect.targetColor;
            }
            isNonLoopingEffectRunning = false;
        }

        currentCoroutine = null;
        currentEffectName = null;
    }
}