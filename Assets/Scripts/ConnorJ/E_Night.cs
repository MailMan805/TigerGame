/// <summary>
/// Contains data and variables for a specific night level.
/// </summary>
public struct Night
{
    /// <summary>
    /// Inital number of bodies within the level.
    /// </summary>
    public int BodyCount;
    /// <summary>
    /// Inital fog density level within the level.
    /// </summary>
    public FogDensity StartingFogDensity;

    /// <summary>
    /// Contains data and variables for a specific night level.
    /// </summary>
    public Night(int BodyCount, FogDensity StartingFogDensity)
    {
        this.BodyCount = BodyCount;
        this.StartingFogDensity = StartingFogDensity;
    }
}