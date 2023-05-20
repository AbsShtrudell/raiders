using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Raiders.Util;

namespace Raiders
{
    public class RoadsDrawer : MonoBehaviour
    {
        [SerializeField] private Terrain _terrain;
        [SerializeField] Vector2Int _textureSize;
        [SerializeField] int _roadWidth = 3;

        [field: SerializeField] public List<SplineComputer> Roads { get; set; }

        private void Start()
        {
            Draw();
        }

        public void Draw()
        {
            Debug.Log("Draw Terrain Roads");

            _terrain.materialTemplate.SetTexture("_RoadsControl", CreateTexture());
        }

        public void Clear()
        {
            Debug.Log("Clear Terrain Roads");

            _terrain.materialTemplate.SetTexture("_RoadsControl", null);
        }

        private Texture2D CreateTexture()
        {
            var texture = new Texture2D(_textureSize.x, _textureSize.y);

            texture.FillTexture(Color.red);

            texture.Apply();

            foreach (var spline in Roads)
            {
                DrawSpline(texture, spline);
            }

            return texture;
        }

        private void DrawSpline(Texture2D texture, SplineComputer spline)
        {
            for (float i = 0; i < 1; i += 0.005f)
            {
                Vector2 position = TranslateWorldPosition(spline.EvaluatePosition(i));
                texture.DrawCircle(Color.green, (int)position.x, (int)position.y, _roadWidth);
            }

            texture.Apply();
        }

        private Vector2 TranslateWorldPosition(Vector3 position)
        {
            Vector2 step = new Vector2(_textureSize.x / _terrain.terrainData.size.x, _textureSize.y / _terrain.terrainData.size.z);
            Vector3 pos = position - _terrain.transform.position;

            return new Vector2(step.x * pos.x, step.y * pos.z);
        }
    }
}
