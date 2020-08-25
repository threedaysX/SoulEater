using UnityEngine;

public class ResourcesManager : Singleton<ResourcesManager>
{
    // Start is called before the first frame update
    void Start()
    {
        Resources.UnloadUnusedAssets();
    }
}
