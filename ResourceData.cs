using System;
using UnityEngine;

[Serializable]
public class ResourceData
{
    public string UniqueId;
    public int maxAmount;
    public int currentAmount;
}

[Serializable]
public class ResourceState
{
    [SerializeField]
    ResourceData[] data = new ResourceData[0];

    public ResourceData[] GetResourceDatas()
    {
        return (ResourceData[])data.Clone();
    }

    public void SetResourceData(ResourceData[] value)
    {
        data = (ResourceData[])value.Clone();
    }
}