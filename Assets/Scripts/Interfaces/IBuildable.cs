using UnityEngine;

public interface IBuildable
{
    void Build();

    /// <summary>
    /// All IBuildable Objects must have a reference to their BuildableObjectSO.
    /// </summary>
    BuildableObjectSO GetBuildableObjectSO();
}
