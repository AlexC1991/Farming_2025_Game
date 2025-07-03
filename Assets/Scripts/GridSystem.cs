using System;
using System.Collections.Generic;
using UnityEngine;

namespace farming2025
{
    public class GridSystem : MonoBehaviour
    {
        [Header("Grid Settings")] [SerializeField]
        private float gridSize = 1f; // Size of each grid cell

        [SerializeField] private int gridWidth = 10; // Number of cells in X direction
        [SerializeField] private int gridLength = 10; // Number of cells in Z direction

        // Returns the grid position (snapped to grid) from a world position
        public Vector3 GetGridPosition(Vector3 worldPosition)
        {
            float xPosition = Mathf.Round(worldPosition.x / gridSize) * gridSize;
            float zPosition = Mathf.Round(worldPosition.z / gridSize) * gridSize;

            return new Vector3(xPosition, 0f, zPosition);
        }

        // Visualize the grid in the editor
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;

            // Draw lines along X axis
            for (int x = 0; x <= gridWidth; x++)
            {
                Vector3 startPos = transform.position + new Vector3(x * gridSize - (gridWidth * gridSize / 2f), 0,
                    -(gridLength * gridSize / 2f));
                Vector3 endPos = transform.position + new Vector3(x * gridSize - (gridWidth * gridSize / 2f), 0,
                    (gridLength * gridSize / 2f));
                Gizmos.DrawLine(startPos, endPos);
            }

            // Draw lines along Z axis
            for (int z = 0; z <= gridLength; z++)
            {
                Vector3 startPos = transform.position + new Vector3(-(gridWidth * gridSize / 2f), 0,
                    z * gridSize - (gridLength * gridSize / 2f));
                Vector3 endPos = transform.position + new Vector3((gridWidth * gridSize / 2f), 0,
                    z * gridSize - (gridLength * gridSize / 2f));
                Gizmos.DrawLine(startPos, endPos);
            }
        }
    }
}