using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class EllipseMesh : MonoBehaviour
{
    public int segments = 100;  // Segmenttien määrä ellipsissä
    public float a = 1f;        // Ellipsin leveys (X-akselilla)
    public float b = 1f;        // Ellipsin korkeus (Y-akselilla)
    public float planeHeight = 1.25f; // Haluttu Y-koordinaatti Plane-objektin keskelle
    public float scale = 1.23f; // Skaalauskerroin

    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = CreateEllipseMesh(a * scale, b * scale, segments, planeHeight);
    }

    Mesh CreateEllipseMesh(float a, float b, int segments, float yPosition)
    {
        Mesh mesh = new Mesh();

        // Listat vertekseille ja indekseille
        Vector3[] vertices = new Vector3[segments + 1];
        int[] triangles = new int[segments * 3];

        // Keskipiste
        vertices[0] = new Vector3(0, yPosition, 0);

        // Luodaan verteksit ja indeksit
        float angleStep = 2 * Mathf.PI / segments;
        for (int i = 0; i < segments; i++)
        {
            float angle = i * angleStep;
            float x = Mathf.Cos(angle) * a;
            float y = Mathf.Sin(angle) * b;

            // Siirretään ellipsi Plane-objektin keskelle
            vertices[i + 1] = new Vector3(x, y + yPosition, 0);

            // Kolmiot (indeksit): liitetään jokainen segmentti keskipisteeseen
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = (i + 1) % segments + 1;
        }

        // Asetetaan verteksit ja kolmiot meshille
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Lasketaan normaalit ja UV:t
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        
        return mesh;
    }
}
