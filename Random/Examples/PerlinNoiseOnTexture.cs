using UnityEngine;

namespace TNNUtils.Random.Examples
{

    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public class PerlinNoiseOnTexture : MonoBehaviour
    {
        public enum PerlinType
        {
            TNNUtilsPerlin1D,
            TNNUtilsPerlin2D,
            UnityPerlin2D,
            TNNUtilsPerlin3D,
            TNNUtilsFBM1D,
            TNNUtilsFBM2D,
            TNNUtilsFBM3D
    }

        [Header("Texture settings")] 
        public PerlinType texturePerlinType = PerlinType.TNNUtilsFBM3D;
        public int textureWidth = 100;
        public int textureHeight = 100;
        
        [Header("Perlin settings")]
        public float perlinScale = 20f;
        public float xOffset = -10f;
        public float yOffset = -10f;
        public float zOffset;

        [Header("FBM settings")] 
        public int octaves = 4;

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

        private void Update()
        {
            _meshRenderer.material.mainTexture = GenerateTexture();
            xOffset += Time.deltaTime;
            yOffset += Time.deltaTime;
            zOffset += Time.deltaTime;
        }

        private Texture2D GenerateTexture()
        {
            var texture = new Texture2D(textureWidth, textureHeight);

            for (var x = 0; x < textureWidth; x++)
            {
                for (var y = 0; y < textureHeight; y++)
                {
                    texture.SetPixel(x, y, GenerateColor(x, y));
                } 
            }
            
            texture.Apply();
            return texture;
        }

        private Color GenerateColor(float x, float y)
        {
            var xCoordinate = x / textureWidth * perlinScale + xOffset;
            var yCoordinate = y / textureHeight * perlinScale + yOffset;

            switch (texturePerlinType)
            {
                case PerlinType.TNNUtilsPerlin1D:
                    return new Color(Perlin.PerlinNoise(xCoordinate), 
                                     Perlin.PerlinNoise(xCoordinate), 
                                     Perlin.PerlinNoise(xCoordinate));
                    
                case PerlinType.TNNUtilsPerlin2D:
                    return new Color(Perlin.PerlinNoise(xCoordinate, yCoordinate), 
                                     Perlin.PerlinNoise(xCoordinate, yCoordinate), 
                                     Perlin.PerlinNoise(xCoordinate, yCoordinate));
                
                case PerlinType.UnityPerlin2D:
                    return new Color(Mathf.PerlinNoise(xCoordinate, yCoordinate),
                                     Mathf.PerlinNoise(xCoordinate, yCoordinate),
                                     Mathf.PerlinNoise(xCoordinate, yCoordinate));
                
                case PerlinType.TNNUtilsPerlin3D:
                    return new Color(Perlin.PerlinNoise(xCoordinate, yCoordinate, zOffset), 
                                     Perlin.PerlinNoise(xCoordinate, yCoordinate, zOffset), 
                                     Perlin.PerlinNoise(xCoordinate, yCoordinate, zOffset));
                
                case PerlinType.TNNUtilsFBM1D:
                    return new Color(Perlin.FractalBrownianMotion(xCoordinate, octaves), 
                                     Perlin.FractalBrownianMotion(xCoordinate, octaves), 
                                     Perlin.FractalBrownianMotion(xCoordinate, octaves));
                
                case PerlinType.TNNUtilsFBM2D:
                    return new Color(Perlin.FractalBrownianMotion(xCoordinate, yCoordinate, octaves), 
                                     Perlin.FractalBrownianMotion(xCoordinate, yCoordinate, octaves), 
                                     Perlin.FractalBrownianMotion(xCoordinate, yCoordinate, octaves));
                
                case PerlinType.TNNUtilsFBM3D:
                    return new Color(Perlin.FractalBrownianMotion(xCoordinate, yCoordinate, zOffset, octaves), 
                                     Perlin.FractalBrownianMotion(xCoordinate, yCoordinate, zOffset, octaves), 
                                     Perlin.FractalBrownianMotion(xCoordinate, yCoordinate, zOffset, octaves));
                
                default:
                    return Color.magenta; //This never happens
            }
        }
    }
}