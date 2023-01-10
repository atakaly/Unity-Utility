using System;
using UnityEngine;

public static class ResourceManager
{
    public static Action OnDataChange;

    public const string PLAYER_PREF_KEY = "ResourcesData";
    private static ResourceData[] resources;
    private static ResourceState resourceState;

    static ResourceManager()
    {
        Load();
    }

    public static ResourceData[] GetResources()
    {
        return resources;
    }

    public static ResourceData GetResource(string resourceUniqueId)
    {
        foreach (var resource in resources)
        {
            if (resource.UniqueId == resourceUniqueId)
                return resource;
        }
        return null;
    }

    public static ResourceData AddResource(ResourceSO resourceScriptableObject, int amount = 0)
    {
        var arrayLength = resources.Length;

        if(arrayLength == 0)
        {
            resources = new ResourceData[1];
            resources[0] = new ResourceData() { currentAmount = amount, maxAmount = resourceScriptableObject.initialMaxAmount, UniqueId = resourceScriptableObject.uniqueId };
            
            return resources[0];
        }

        if (GetResource(resourceScriptableObject.uniqueId) != null)
            return null;

        var currentResources = resources;

        resources = new ResourceData[arrayLength + 1];

        for (int i = 0; i < resources.Length - 1; i++)
        {
            resources[i] = currentResources[i];
        }

        resources[arrayLength] = new ResourceData() { currentAmount = amount, maxAmount = resourceScriptableObject.initialMaxAmount, UniqueId = resourceScriptableObject.uniqueId };

        OnDataChange?.Invoke();

        Save();

        return resources[arrayLength];
    }

    public static void SetResourceAmount(ResourceSO resourceScriptableObject, int amount)
    {
        var resource = GetResource(resourceScriptableObject.uniqueId);

        if (resource == null)
            AddResource(resourceScriptableObject, amount);

        if (resource.currentAmount > resource.maxAmount)
            resource.currentAmount = resource.maxAmount;

        if (resource.currentAmount < 0)
            resource.currentAmount = 0;

        resource.currentAmount = amount;

        OnDataChange?.Invoke();

        Save();
    }

    public static void IncreaseResourceAmount(ResourceSO resourceScriptableObject, int amount)
    {
        var resource = GetResource(resourceScriptableObject.uniqueId);

        if(resource == null)
            AddResource(resourceScriptableObject, amount);

        if (resource.currentAmount > resource.maxAmount && resourceScriptableObject.IsLimited)
            return;

        resource.currentAmount += amount;

        if (resource.currentAmount > resource.maxAmount && resourceScriptableObject.IsLimited)
            resource.currentAmount = resource.maxAmount;

        OnDataChange?.Invoke();

        Save();
    }

    public static void DecreaseResourceAmount(ResourceSO resourceScriptableObject, int amount)
    {
        var resource = GetResource(resourceScriptableObject.uniqueId);

        if (resource.currentAmount <= 0)
            return;

        resource.currentAmount -= amount;

        if (resource.currentAmount < 0)
            resource.currentAmount = 0;

        Save();

        OnDataChange?.Invoke();
    }

    public static void Load()
    {
        if(resourceState == null)
            resourceState = new ResourceState();

        if(!PlayerPrefs.HasKey(PLAYER_PREF_KEY))
            resourceState = JsonUtility.FromJson<ResourceState>(GameData.Instance.InitialResources);
        else
            resourceState = JsonUtility.FromJson<ResourceState>(PlayerPrefs.GetString(PLAYER_PREF_KEY));

        resources = resourceState.GetResourceDatas();
    }

    public static void Save()
    {
        if (resourceState == null)
            resourceState = new ResourceState();

        resourceState.SetResourceData(resources);

        PlayerPrefs.SetString(PLAYER_PREF_KEY, JsonUtility.ToJson(resourceState));
    }
}