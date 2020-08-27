using UnityEngine;

public class PrefabRenderer : Singleton<PrefabRenderer>
{
    public GameObject RenderPrefabInParent<T>(Transform parent, Transform prefab, string newObjectName, bool setActive, bool reRenderOnce = false)
    {
        if (prefab != null)
        {
            bool isDestroy = false;
            // Check if this prefab exists in the parent object, 
            // if so and [reRenderOnce] is [true], then destory it and render again.
            T targetPrefab = parent.GetComponentInChildren<T>(true);
            if (targetPrefab != null && reRenderOnce)
            {
                Destroy(targetPrefab as Transform);
                isDestroy = true;
            }
            if (targetPrefab == null || isDestroy)
            {
                Transform newObject = Instantiate(prefab, parent.position, parent.rotation);
                newObject.SetParent(parent);
                newObject.localPosition = prefab.localPosition;
                newObject.name = newObjectName;
                newObject.gameObject.SetActive(setActive);

                return newObject.gameObject;
            }
        }

        return null;
    }

    public GameObject RenderPrefab<T>(Transform prefab, string newObjectName, bool setActive)
    {
        if (prefab != null)
        {
            Transform newObject = Instantiate(prefab);
            newObject.localPosition = prefab.localPosition;
            newObject.name = newObjectName;
            newObject.gameObject.SetActive(setActive);

            return newObject.gameObject;
        }

        return null;
    }
}
