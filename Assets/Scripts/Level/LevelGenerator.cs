using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{  
    public void generate(LevelRandom levelRandom, List<GameObject> objects)
    {
        List<Cell> getNeighbors(Cell[][] field, Cell cell)
        {
            List<Cell> list = new List<Cell>();

            if (cell.x > 0 && !field[cell.x - 1][cell.z].isVisited) list.Add(field[cell.x - 1][cell.z]);
            if (cell.z > 0 && !field[cell.x][cell.z - 1].isVisited) list.Add(field[cell.x][cell.z - 1]);

            if (cell.x < Constants.LENGTH - 1 && !field[cell.x + 1][cell.z].isVisited) list.Add(field[cell.x + 1][cell.z]);
            if (cell.z < Constants.LENGTH - 1 && !field[cell.x][cell.z + 1].isVisited) list.Add(field[cell.x][cell.z + 1]);

            return list;
        }

        void removeWall(Cell first, Cell second)
        {
            if (first.x == second.x - 1)
            {
                second.up = false;
            }
            else if (first.x == second.x + 1)
            {
                first.up = false;
            }
            else if (first.z == second.z - 1)
            {
                second.left = false;
            }
            else if (first.z == second.z + 1)
            {
                first.left = false;
            }
        }

        List<Cell> randomPoints(Cell[][] field, int count)
        {
            (int, int)[,] arr = new (int, int)[Constants.LENGTH, Constants.LENGTH];

            for (int i = 0; i < Constants.LENGTH; ++i)
            {
                for (int j = 0; j < Constants.LENGTH; ++j)
                {
                    arr[i, j].Item1 = i;
                    arr[i, j].Item2 = j;
                }
            }

            for (int i = 0; i < Constants.LENGTH; ++i)
            {
                for (int j = 0; j < Constants.LENGTH; ++j)
                {
                    int other_i = levelRandom.Next(0, Constants.LENGTH);
                    int other_j = levelRandom.Next(0, Constants.LENGTH);

                    (int, int) temp = arr[i, j];
                    arr[i, j] = arr[other_i, other_j];
                    arr[other_i, other_j] = temp;
                }
            }

            List<Cell> reference_points = new List<Cell>();

            foreach ((int, int) it in arr)
            {
                reference_points.Add(field[it.Item1][it.Item2]);
            }

            reference_points.RemoveRange(count, Constants.LENGTH * Constants.LENGTH - count);

            return reference_points;
        }

        void createPath(Cell[][] field, Cell current, List<Cell> reference_points)
        {
            current.isVisited = true;

            List<Cell> neighborsList = getNeighbors(field, current);

            while (neighborsList.Count > 0)
            {
                Cell next = null;

                foreach (Cell cell in neighborsList)
                {
                    if (reference_points.Contains(cell))
                    {
                        next = cell;
                    }
                }

                if (next == null)
                {
                    next = neighborsList[levelRandom.Next(0, neighborsList.Count)];
                }

                removeWall(current, next);
                createPath(field, next, reference_points);

                neighborsList = getNeighbors(field, current);
            }
        }

        void createWall(Vector3 position, Vector3 scale)
        {
            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.transform.position = position;
            wall.transform.localScale = scale;
            objects.Add(wall);
        }

        void buildWalls(Cell[][] field)
        {
            for (int x = 0; x < Constants.LENGTH; x++)
            {
                for (int z = 0; z < Constants.LENGTH; z++)
                {
                    Cell cell = field[x][z];

                    if (cell.up && x > 0)
                    {
                        createWall(
                            new Vector3(x - Constants.HALF_CELL, Constants.HALF_HEIGHT, z),
                            new Vector3(Constants.WALL_THICKNESS, Constants.WALL_HEIGHT, Constants.WALL_LENGTH)
                        );
                    }

                    if (cell.left && z > 0)
                    {
                        createWall(
                            new Vector3(x, Constants.HALF_HEIGHT, z - Constants.HALF_CELL),
                            new Vector3(Constants.WALL_LENGTH, Constants.WALL_HEIGHT, Constants.WALL_THICKNESS)
                        );
                    }

                    if (x == 0)
                    {
                        createWall(
                            new Vector3(-Constants.HALF_CELL, Constants.HALF_HEIGHT, z),
                            new Vector3(Constants.WALL_THICKNESS, Constants.WALL_HEIGHT, Constants.WALL_LENGTH)
                        );
                    }
                    else if (x == Constants.LENGTH - 1)
                    {
                        createWall(
                            new Vector3(Constants.LENGTH - Constants.HALF_CELL, Constants.HALF_HEIGHT, z),
                            new Vector3(Constants.WALL_THICKNESS, Constants.WALL_HEIGHT, Constants.WALL_LENGTH)
                        );
                    }
                    if (z == 0)
                    {
                        createWall(
                            new Vector3(x, Constants.HALF_HEIGHT, -Constants.HALF_CELL),
                            new Vector3(Constants.WALL_LENGTH, Constants.WALL_HEIGHT, Constants.WALL_THICKNESS)
                        );
                    }
                    else if (z == Constants.LENGTH - 1)
                    {
                        createWall(
                            new Vector3(x, Constants.HALF_HEIGHT, Constants.LENGTH - Constants.HALF_CELL),
                            new Vector3(Constants.WALL_LENGTH, Constants.WALL_HEIGHT, Constants.WALL_THICKNESS)
                        );
                    }
                }
            }
        }

        foreach (var obj in objects)
        {
            if (Application.isPlaying)
            {
                Destroy(obj);
            }
            else
            {
                DestroyImmediate(obj);
            }
        }
        objects.Clear();

        Cell[][] field = new Cell[Constants.LENGTH][];
        for (int i = 0; i < Constants.LENGTH; ++i)
        {
            field[i] = new Cell[Constants.LENGTH];

            for (int j = 0; j < Constants.LENGTH; ++j)
            {
                field[i][j] = new Cell(false, i, j);
            }
        }

        Cell start = field[0][0];

        createPath(field, start, randomPoints(field, Constants.REFERENCE_POINTS_COUNT));

        foreach (Cell cell in randomPoints(field, Constants.INTERSECTIONS_COUNT))
        {
            if (cell.x == 0 || cell.x == Constants.LENGTH - 1 || cell.z == 0 || cell.z == Constants.LENGTH - 1)
            {
                continue;
            }

            if ((!cell.up || !cell.down))
            {
                if ((cell.z & cell.x & 1) == 1)
                {
                    cell.left = false;
                }
                else
                {
                    cell.right = false;
                }
            }
            else if ((!cell.left || !cell.right))
            {
                if ((cell.z & cell.x & 1) == 1)
                {
                    cell.up = false;
                }
                else
                {
                    cell.down = false;
                }
            }
        }

        buildWalls(field);
    }
}