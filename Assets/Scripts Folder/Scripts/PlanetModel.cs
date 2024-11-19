using System;
using System.Collections.Generic;

[Serializable]
public class PlanetData
{
    public string name;
    public string description;
    public float distance_from_sun;
}

[Serializable]
public class PlanetList
{
    public List<PlanetData> planets;
}
