using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameFOV : MonoBehaviour
{
    Mesh mesh;
    public Vector3[] polygonPoints;
    public int[] polygonTriangles;

    //polygon properties
    public int polygonSides;
    public float polygonRadius;
    public float centerRadius;
    public float fov = 70;

    void Start()
    {
        mesh = new Mesh();
        this.GetComponent<MeshFilter>().mesh = mesh;
    }

    void Update()
    {
        DrawFilled(polygonSides, polygonRadius);
    }

    void DrawFilled(int sides, float radius)
    {
        polygonPoints = GetCircumferencePoints(sides, radius).ToArray();
        polygonTriangles = DrawFilledTriangles(polygonPoints);
        mesh.Clear();
        mesh.vertices = polygonPoints;
        mesh.triangles = polygonTriangles;
    }    

    List<Vector3> GetCircumferencePoints(int sides, float radius)
    {
        List<Vector3> points = new List<Vector3>();
        float circumferenceProgressPerStep = (float)1 / sides;
        float TAU = 2 * Mathf.PI;
        float radianProgressPerStep = circumferenceProgressPerStep * TAU;

        //Add the center
        points.Add(new Vector3(0, 0, 0));

        for (int i = 0; i < sides; i++)
        {
            float currentRadian = radianProgressPerStep * i;

            points.Add(new Vector3(Mathf.Cos(currentRadian) * radius, Mathf.Sin(currentRadian) * radius, 0));
        }
        return points;
    }

    int[] DrawFilledTriangles(Vector3[] points)
    {
        Debug.Log("FILLED "+points.Length);
        int triangleAmount = points.Length - 2;
        List<int> newTriangles = new List<int>();
        for (int i = 0; i < triangleAmount; i++)
        {
            newTriangles.Add(0);
            newTriangles.Add(i + 2);
            newTriangles.Add(i + 1);
        }
        newTriangles.Add(0);       
        newTriangles.Add(1);
        newTriangles.Add(points.Length - 1);
        return newTriangles.ToArray();
    }
}
