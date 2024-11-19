using UnityEngine;

public class PlanetDataLoader : MonoBehaviour
{
    public static PlanetList LoadPlanetData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("planets");
        return JsonUtility.FromJson<PlanetList>(jsonFile.text);
    }
}
