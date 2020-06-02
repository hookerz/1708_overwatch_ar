using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Hook.HXF
{
    public static class AssetLoader
    {
        public static void LoadImage(string assetPath, Image image)
        {
            // getting path to image
            var path = string.Format("{0}/{1}", Application.streamingAssetsPath, assetPath);
        
            // loading image data
            var imageData = File.ReadAllBytes(path);
        
            // creating texture from image data
            var texture = new Texture2D(2,2);
            texture.LoadImage(imageData);
        
            // creating/applying sprite to Image component
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            image.sprite = sprite;

            // updating aspect ratio based on height
            var imageTransform = image.GetComponent<RectTransform>();
            var currentAspectRatio = (float)texture.width / texture.height;
            var newWidth = imageTransform.rect.height * currentAspectRatio;
            imageTransform.sizeDelta = new Vector2(newWidth, imageTransform.rect.height);
        }
    }
}