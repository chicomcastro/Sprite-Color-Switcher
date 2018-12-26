using UnityEngine;
using System.Collections.Generic;

public class SpriteColorChanger : MonoBehaviour
{
    // An array of all sprites you wanna change
    public Texture2D[] mapsToChange;

    private List<Maps> maps;

    // A list of all color present in yours sprites
    // Here you make link between old and new colors
    public List<ColorToColor> colorsList;

    // An initial method to setup things after getting reference to sprites you wanna change
    [ContextMenu("Setup Textures")]
    public void SetupTextures()
    {
        // Clearing lists
        maps = new List<Maps>();
        colorsList = new List<ColorToColor>();

        // Creating Maps (our main object) from input sprites with its colors mapped
        foreach (Texture2D t in mapsToChange)
        {
            // Make a new Maps object
            Maps _m = new Maps(t);

            // Map its texture color from input sprite
            _m = MapSpriteColors(_m);

            // Add it on Maps list
            maps.Add(_m);
        }
    }

    // Method to map color on textures2D from sprites
    private Maps MapSpriteColors(Maps _m)
    {
        // Get texture from Maps _m
        Texture2D map = _m.texture;

        // Get all pixels from this texture
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                // Get the color from each pixel
                Color pixelColor = map.GetPixel(x, y);

                // See if it isn't transparent
                if (pixelColor.a != 0)
                {
                    // Add it to Maps _m list of colors
                    if (!_m.colors.Contains(pixelColor))
                        _m.colors.Add(pixelColor);

                    // Search our colorsList to found duplicated colors
                    if (!HasColorsListThisColor(pixelColor))
                        colorsList.Add(new ColorToColor(pixelColor));
                }
            }
        }

        return _m;
    }

    // Method for applying changes from selected colors
    [ContextMenu("Switch Colors")]
    public void SwitchColors()
    {
        foreach (Maps _m in maps)
        {
            // Get texture from Maps _m
            Texture2D map = _m.texture;

            // Search in all pixels from this texture
            for (int x = 0; x < map.width; x++)
            {
                for (int y = 0; y < map.height; y++)
                {
                    // Get the color from each pixel
                    Color pixelColor = map.GetPixel(x, y);

                    // Find the replacement color for it
                    foreach (ColorToColor c in colorsList)
                    {
                        if (c.originalColor.Equals(pixelColor)) {
                            // Apply the new color in top of older one
                            map.SetPixel(x, y, c.replacementColor);
                        }
                    }
                }
            }

            map.Apply();
        }
    }

    // Method to search if one color is on our main list
    private bool HasColorsListThisColor(Color _color)
    {
        foreach (ColorToColor c in colorsList)
        {
            if (c.originalColor.Equals(_color))
            {
                return true;
            }
        }

        return false;
    }
}

[System.Serializable]
public class Maps
{
    public Texture2D texture;
    public List<Color> colors;

    public Maps(Texture2D _texture)
    {
        texture = _texture;
        colors = new List<Color>();
    }
}

[System.Serializable]
public class ColorToColor
{
    public Color originalColor;
    public Color replacementColor;

    public ColorToColor(Color _color)
    {
        originalColor = _color;
        replacementColor = _color;
    }
}
