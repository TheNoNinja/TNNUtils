using System;
using UnityEngine;


namespace TNNUtils.Random.Examples
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public class PerlinNoiseOnTexture : MonoBehaviour
    {
        public int width = 256;
        public int height = 256;
        public float scale = 20;

        private MeshRenderer _meshRenderer;
        
        public void Awake()
        {
            //Just some code to generate a quad on runtime to display the perlin noise. You can ignore this if you already have a quad or want to use the perlin noise for something else
            var meshFilter = gameObject.GetComponent<MeshFilter>();
            meshFilter.mesh = new Mesh
            {
                vertices = new Vector3[]
                {
                    new (0, 0, 0),
                    new (1, 0, 0),
                    new (0, 1, 0),
                    new (1, 1, 0)
                },
                triangles = new []
                {
                    0, 2, 1,
                    2, 3, 1
                },
                normals = new []
                {
                    -Vector3.forward,
                    -Vector3.forward,
                    -Vector3.forward,
                    -Vector3.forward
                },
                uv = new Vector2[]
                {
                    new (0, 0),
                    new (1, 0),
                    new (0, 1),
                    new (1, 1)
                }
            };
            _meshRenderer = gameObject.GetComponent<MeshRenderer>();
            
        }

        private void LateUpdate()
        {
            //Generate a new texture every physics update
            _meshRenderer.material.mainTexture = GenerateTexture();
        }

        private Texture2D GenerateTexture()
        {
            var texture = new Texture2D(width, height);

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    texture.SetPixel(x, y, GenerateColor(x, y));
                } 
            }
            
            texture.Apply();
            return texture;
        }

        private Color GenerateColor(float x, float y)
        {
            var xCoordinate = x / width * scale;
            var yCoordinate = y / height * scale;
            return new Color(Perlin.PerlinNoise(xCoordinate, yCoordinate), 
                             Perlin.PerlinNoise(xCoordinate, yCoordinate), 
                             Perlin.PerlinNoise(xCoordinate, yCoordinate));
        }
    }
}