using UnityEngine;

public interface IBuildable
{
    GameObject BuildablePrefab { get; }
    
    void Build();
}
