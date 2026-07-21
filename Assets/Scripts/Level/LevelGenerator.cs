using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    const int LENGTH = 21;
    const int REFERENCE_POINTS_COUNT = 17;
    const int INTERSECTIONS_COUNT = 123;

    private const float CELL_SIZE = 1f;
    private const float WALL_THICKNESS = 0.2f;
    private const float WALL_HEIGHT = 2f;
    private const float WALL_LENGTH = CELL_SIZE + WALL_THICKNESS;
    private const float HALF_CELL = CELL_SIZE / 2f;
    private const float HALF_HEIGHT = WALL_HEIGHT / 2f;

    System.Random random = new System.Random();
    List<GameObject> objects = new List<GameObject>();

    private class Cell
    {
        public bool isVisited;
        public int x;
        public int z;
        public bool up;
        public bool down;
        public bool left;
        public bool right;

        public Cell(bool isVisited, int x, int z)
        {
            this.isVisited = isVisited;
            this.x = x;
            this.z = z;
            up = down = left = right = true;
        }
    }

    public void generate()
    {
        List<Cell> getNeighbors(Cell[][] field, Cell cell)
        {
            List<Cell> list = new List<Cell>();

            if (cell.x > 0 && !field[cell.x - 1][cell.z].isVisited) list.Add(field[cell.x - 1][cell.z]);
            if (cell.z > 0 && !field[cell.x][cell.z - 1].isVisited) list.Add(field[cell.x][cell.z - 1]);

            if (cell.x < LENGTH - 1 && !field[cell.x + 1][cell.z].isVisited) list.Add(field[cell.x + 1][cell.z]);
            if (cell.z < LENGTH - 1 && !field[cell.x][cell.z + 1].isVisited) list.Add(field[cell.x][cell.z + 1]);

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
            (int, int)[,] arr = new (int, int)[LENGTH, LENGTH];

            for (int i = 0; i < LENGTH; ++i)
            {
                for (int j = 0; j < LENGTH; ++j)
                {
                    arr[i, j].Item1 = i;
                    arr[i, j].Item2 = j;
                }
            }

            for (int i = 0; i < LENGTH; ++i)
            {
                for (int j = 0; j < LENGTH; ++j)
                {
                    int other_i = random.Next(0, LENGTH);
                    int other_j = random.Next(0, LENGTH);

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

            reference_points.RemoveRange(count, LENGTH * LENGTH - count);

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
                    next = neighborsList[random.Next(0, neighborsList.Count)];
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
            for (int x = 0; x < LENGTH; x++)
            {
                for (int z = 0; z < LENGTH; z++)
                {
                    Cell cell = field[x][z];

                    if (cell.up && x > 0)
                    {
                        createWall(
                            new Vector3(x - HALF_CELL, HALF_HEIGHT, z),
                            new Vector3(WALL_THICKNESS, WALL_HEIGHT, WALL_LENGTH)
                        );
                    }

                    if (cell.left && z > 0)
                    {
                        createWall(
                            new Vector3(x, HALF_HEIGHT, z - HALF_CELL),
                            new Vector3(WALL_LENGTH, WALL_HEIGHT, WALL_THICKNESS)
                        );
                    }

                    if (x == 0)
                    {
                        createWall(
                            new Vector3(-HALF_CELL, HALF_HEIGHT, z),
                            new Vector3(WALL_THICKNESS, WALL_HEIGHT, WALL_LENGTH)
                        );
                    }
                    else if (x == LENGTH - 1)
                    {
                        createWall(
                            new Vector3(LENGTH - HALF_CELL, HALF_HEIGHT, z),
                            new Vector3(WALL_THICKNESS, WALL_HEIGHT, WALL_LENGTH)
                        );
                    }
                    if (z == 0)
                    {
                        createWall(
                            new Vector3(x, HALF_HEIGHT, -HALF_CELL),
                            new Vector3(WALL_LENGTH, WALL_HEIGHT, WALL_THICKNESS)
                        );
                    }
                    else if (z == LENGTH - 1)
                    {
                        createWall(
                            new Vector3(x, HALF_HEIGHT, LENGTH - HALF_CELL),
                            new Vector3(WALL_LENGTH, WALL_HEIGHT, WALL_THICKNESS)
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

        Cell[][] field = new Cell[LENGTH][];
        for (int i = 0; i < LENGTH; ++i)
        {
            field[i] = new Cell[LENGTH];

            for (int j = 0; j < LENGTH; ++j)
            {
                field[i][j] = new Cell(false, i, j);
            }
        }

        Cell start = field[0][0];

        createPath(field, start, randomPoints(field, REFERENCE_POINTS_COUNT));

        foreach (Cell cell in randomPoints(field, INTERSECTIONS_COUNT))
        {
            if (cell.x == 0 || cell.x == LENGTH - 1 || cell.z == 0 || cell.z == LENGTH - 1)
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