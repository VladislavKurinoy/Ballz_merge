using System.Collections.Generic;
using UnityEngine;

public class Utilities
{
    public static List<int> GetRandomIndexes(int inListCount, int outListCount)
    {
        List<int> indexesList = new List<int>();
        
        if (inListCount < outListCount)
        {
            Debug.LogError("The list does not contain enough elements to remove!!!");
            return indexesList;
        }
        
        for (int i = 0; i < inListCount; i++)
        {
            indexesList.Add(i);
        }
        
        int numberOfElementsToRemove = inListCount - outListCount;
        
        HashSet<int> indexesToRemove = new HashSet<int>();
        
        while (indexesToRemove.Count < numberOfElementsToRemove)
        {
            int randomIndex = Random.Range(0, indexesList.Count);
            indexesToRemove.Add(randomIndex);
        }
        
        List<int> sortedIndexes = new List<int>(indexesToRemove);
        sortedIndexes.Sort((a, b) => b.CompareTo(a));
        
        foreach (int index in sortedIndexes)
        {
            indexesList.RemoveAt(index);
        }

        return indexesList;
    }
    
    public static Cell GetNeighborCell(Cell[,] array, Vector2 position, Vector2 direction)
    {
        int row = (int)position.x;
        int col = (int)position.y;

        int neighborRow = row + (int)direction.x;
        int neighborCol = col + (int)direction.y;

        int rows = array.GetLength(0);
        int cols = array.GetLength(1);

        if (neighborRow > 0 && neighborRow < rows && neighborCol >= 0 && neighborCol < cols)
        {
            return array[neighborRow, neighborCol];
        }

        return null;
    }
}
