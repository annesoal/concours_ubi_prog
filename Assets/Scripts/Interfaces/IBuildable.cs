using Grid;
using UnityEngine;

public interface IBuildable
{
    void Build(Vector3 positionToBuild);

    /// <summary>
    /// All IBuildable Objects must have a reference to their BuildableObjectSO.
    /// </summary>
    BuildableObjectSO GetBuildableObjectSO();
}
