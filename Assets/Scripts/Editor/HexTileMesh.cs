using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Scripts.Editor
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class HexTileMesh : MonoBehaviour
    {
        [SerializeField]
        private float h = 1, r = 1;
        Mesh mesh;
        Vector3[] vertices;
        int[] triangles;

        void Awake()
        {
            mesh = GetComponent<MeshFilter>().mesh;
            mesh.name = "HexTile";
            setMeshData();
            createProceduralMesh();
        }

        void setMeshData()
        {
            vertices = new Vector3[] {
            new Vector3(0, h, 0),
            new Vector3(r, h, 0),
            new Vector3(r/2f , h, -Mathf.Sqrt(r * 3) / 2.0f),
            new Vector3(-r/2f , h, -Mathf.Sqrt(r * 3) / 2.0f),
            new Vector3(-r, h, 0),
            new Vector3(-r/2f , h, Mathf.Sqrt(r * 3) / 2.0f),
            new Vector3(r/2f , h, Mathf.Sqrt(r * 3) / 2.0f),
            new Vector3(0, 0, 0),
            new Vector3(r, 0, 0),
            new Vector3(r/2f , 0, -Mathf.Sqrt(r * 3) / 2.0f),
            new Vector3(-r/2f , 0, -Mathf.Sqrt(r * 3) / 2.0f),
            new Vector3(-r, 0, 0),
            new Vector3(-r/2f , 0, Mathf.Sqrt(r * 3) / 2.0f),
            new Vector3(r/2f , 0, Mathf.Sqrt(r * 3) / 2.0f),
            };

            triangles = new int[] {
                0, 1, 2,
                0, 2, 3,
                0, 3, 4,
                0, 4, 5,
                0, 5, 6,
                0, 6, 1,

                1, 7 + 1, 2, 7 + 1, 7 + 2, 2,
                2, 7 + 2, 3, 7 + 2, 7 + 3, 3,
                3, 7 + 3, 4, 7 + 3, 7 + 4, 4,
                4, 7 + 4, 5, 7 + 4, 7 + 5, 5,
                5, 7 + 5, 6, 7 + 5, 7 + 6, 6,
                6, 7 + 6, 1, 7 + 6, 7 + 1, 1,

                7, 7 + 1, 7 + 6,
                7, 7 + 6, 7 + 5,
                7, 7 + 5, 7 + 4,
                7, 7 + 4, 7 + 3,
                7, 7 + 3, 7 + 2,
                7, 7 + 2, 7 + 1,
            };
        }

        void createProceduralMesh()
        {
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            gameObject.AddComponent<MeshCollider>();
        }

#if UNITY_EDITOR
        [ContextMenu("SaveMesh")]
        void saveMesh()
        {
            string path = "Assets/Resources/Meshs/MyMesh.asset";
            AssetDatabase.CreateAsset(mesh, AssetDatabase.GenerateUniqueAssetPath(path));
            AssetDatabase.SaveAssets();
        }
#endif
    }
}
