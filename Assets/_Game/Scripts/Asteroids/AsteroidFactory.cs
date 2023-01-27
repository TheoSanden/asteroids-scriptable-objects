using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asteroids;
public class AsteroidFactory : MonoBehaviour
{
    int resolution;
    [SerializeField] AsteroidSettings[] settings;
    [SerializeField] Sprite[] sprites;
    [SerializeField] AnimationCurve sizeDistribution;


    [ContextMenu("Load Sprites")]
    private void LoadAsteroids()
    {
        Texture2D[] textures = Resources.LoadAll<Texture2D>("AstroidGenerator/Sprites/");
        Debug.Log(textures.Length);
        List<Sprite> sortedSprites = new List<Sprite>();
        List<AsteroidSettings> settings = new List<AsteroidSettings>();
        foreach (Texture2D texture in textures)
        {
            settings.Add(new AsteroidSettings(texture));
        }
        settings.Sort((a, b) => a.SizePercentage.CompareTo(b.SizePercentage));
        foreach (AsteroidSettings s in settings)
        {
            sortedSprites.Add(s.Sprite);
        }
        this.sprites = sortedSprites.ToArray();
        this.settings = settings.ToArray();
    }
    public AsteroidSettings GetRandomAsteroid()
    {
        float value = Random.Range(0.0f, 1.0f);
        value = sizeDistribution.Evaluate(value);
        int index = Mathf.RoundToInt(value * (settings.Length - 1));
        return settings[index];
    }
    public AsteroidSettings GetAsteroid(float size)
    {
        float value = size;
        value = sizeDistribution.Evaluate(value);
        int index = Mathf.RoundToInt(value * (settings.Length - 1));
        return settings[index];
    }
}
