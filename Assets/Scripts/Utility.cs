using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace KevinFeng
{
    public class Utility
    {
        public const int sortingOrderDefault = 5000;

        #region CreateWorldSprite
        //Create a gameobject with sprite renderer in designated place
        public static GameObject CreateWorldSprite(string name, Transform parent, Sprite sprite, Vector3 localPosition, Vector3 localScale, int sortingOrder, Color color)
        {
            GameObject gameObject = new GameObject(name, typeof(SpriteRenderer));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            transform.localScale = localScale;
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingOrder = sortingOrder;
            spriteRenderer.color = color;
            return gameObject;
        }

        /// <summary>
        /// Given width and height, generate a blank sprite.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Sprite CreateBlankSprite(int width, int height)
        {
            Sprite sprite;
            Texture2D texture;
            
            texture = new Texture2D(width, width, TextureFormat.ARGB32, false);
            sprite = Sprite.Create(texture, new Rect(Vector2.zero, new Vector2(texture.width, texture.height)), new Vector2(0.5f, 0.5f));
            return sprite;
        }
        #endregion

        #region CreateWorldText
        // Create Text in the World
        public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault)
        {
            if (color == null) color = UnityEngine.Color.white;
            return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
        }

        // Create Text in the World
        public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
        {
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }
        #endregion

        #region GetMouseWorldPosition
        public static Vector3 GetMouseWorldPosition()
        {
            Vector3 vec = GetMouseWorldPositionWithZ();
            vec.z = -Camera.main.transform.position.z;
            return vec;
        }
        public static Vector3 GetMouseWorldPositionWithZ()
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }
        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }
        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }

        #endregion

        #region Color
        /// <summary>
        /// A small tool to convert RGB color to Color;
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Color Color(float r, float g, float b)
        {
            if (r > 255)
                r = 255f;

            if (g > 255)
                g = 255f;

            if (b > 255)
                b = 255f;

            r /= 255f;
            g /= 255f;
            b /= 255f;

            return new Color(r, g, b);
        }

        public static Color Color(string hex)
        {
            float r = Convert.ToInt32(hex.Substring(0, 2), 16);
            float g = Convert.ToInt32(hex.Substring(2, 2), 16);
            float b = Convert.ToInt32(hex.Substring(4, 2), 16);

            return Utility.Color(r, g, b);
        }

        #endregion
    }
}

