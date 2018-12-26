using UnityEngine;
using System.Collections.Generic;

public class SpriteColorChanger : MonoBehaviour
{

    public Texture2D[] mapsToChange;

    private List<Maps> maps;

    public List<ColorToColor> colorsList;

    [ContextMenu("Setup Textures")]
    public void SetupTextures()
    {
        maps = new List<Maps>();
        colorsList = new List<ColorToColor>();

        foreach (Texture2D t in mapsToChange)
        {
            Maps _m = new Maps(t);
            _m = MapSpriteColors(_m);
            maps.Add(_m);
        }
    }

    [ContextMenu("Color Mapping")]
    public Maps MapSpriteColors(Maps _m)
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

                    foreach (ColorToColor c in colorsList)
                    {
                        if (c.originalColor.Equals(pixelColor)) {
                            map.SetPixel(x, y, c.replacementColor);
                        }
                    }
                }
            }

            map.Apply();
        }
    }

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