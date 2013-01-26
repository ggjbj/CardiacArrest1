using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardiacArrest
{
    class MapRow
    {
        public List<MapCell> Columns = new List<MapCell>();
    }

    class TileMap
    {
        Random randomTiles = new Random();

        public List<MapRow> Rows = new List<MapRow>();
        public int MapWidth = 200;
        public int MapHeight = 150;


        public Dictionary<string, int> Tiles = new Dictionary<string, int>();
        

        public TileMap()
        {
            int counter = 0;
            Tiles.Add("Empty", counter++);
            Tiles.Add("Wall (Generic)", counter++);
            Tiles.Add("Cracked Ladder", counter++);
            Tiles.Add("Left Wall Ladder", counter++);
            Tiles.Add("Wall Ladder", counter++);
            Tiles.Add("Trap Door", counter++);
            Tiles.Add("Left Stairs", counter++);
            Tiles.Add("Right Stairs", counter++);
            Tiles.Add("Right Wall Ladder", counter++);
            Tiles.Add("Seweage Man Hole", counter++);
            Tiles.Add("Right Wall", counter++);
            Tiles.Add("Left Wall", counter++);
            Tiles.Add("Cracked Floor", counter++);
            Tiles.Add("Floor", counter++);
            Tiles.Add("Ceiling", counter++);
            Tiles.Add("Brick Wall", counter++);
            Tiles.Add("Transparent Front Ladder", counter++);
            Tiles.Add("Lower Left Door", counter++);
            Tiles.Add("Lower Right Door", counter++);
            Tiles.Add("Upper Left Door", counter++);
            Tiles.Add("Uppder Right Door", counter++);
            Tiles.Add("Inside Wall", counter++);
            Tiles.Add("Window", counter++);

            
            for (int y = 0; y < MapHeight; y++)
            {
                MapRow thisRow = new MapRow();
                for (int x = 0; x < MapWidth; x++)
                {
                    thisRow.Columns.Add(new MapCell(0)); //Default TileID is 0
                }
                Rows.Add(thisRow);
            }

            /*for (int i = 0; i < Rows.Count; i++)
            {
                for (int j = 0; j < Rows[i].Columns.Count; j++)
                {
                    Rows[i].Columns[j].TileID = randomTiles.Next(0, 21);
                }
            }*/

            #region Level Calls

            ProvingGrounds();

            #endregion
        }

        public void ProvingGrounds()
        {
            drawFloor(14, 0, MapWidth);
            //BuildingGenerator(4, 3, 4, 14);
            addSewageCover(14, 12);
            addTrapDoor(14, 17);
            LadderLevellerLeft(5, 25, 10, 3);
            LadderLevellerRight(5, 45, 9, 3);
            fillFloor(15, 0, MapWidth, MapHeight);
            fillSewage(16,8,20,20);
        }

        public void BuildingGenerator(int buildingTopRow, int buildingFirstColumn, int buildingWidth, int buildingBottomFloor)
        {
            /*The difference between buildingTopRow and buildingBottomFlorr is the height of the building
             * Make sure to keep positive and above 1 as the doors are 2 tiles high
             */
            int buildingHeight = buildingBottomFloor - buildingTopRow;

            for (int j = buildingTopRow; j < buildingHeight + buildingTopRow; j++)
            {
                for (int i = buildingFirstColumn; i < buildingWidth + buildingFirstColumn; i++)
                {
                    Rows[j].Columns[i].TileID = 15;
                }
            }

            //Adds double doors in middle of building
            int leftDoorPlacement = (buildingFirstColumn + (buildingWidth / 2)) - 1;
            int rightDoorPlacement = leftDoorPlacement + 1;
            int topOfDoorRow = buildingBottomFloor - 2;
            int bottomOfDoorRow = buildingBottomFloor - 1;

            Rows[topOfDoorRow].Columns[leftDoorPlacement].TileID = 19;
            Rows[topOfDoorRow].Columns[rightDoorPlacement].TileID = 20;
            Rows[bottomOfDoorRow].Columns[leftDoorPlacement].TileID = 17;
            Rows[bottomOfDoorRow].Columns[rightDoorPlacement].TileID = 18;
        }

        public void LadderLevellerLeft(int topRow, int firstColumn, int width, int levelDepth)
        {
            int floorLength = (firstColumn + 1) + width;

            for(int k = 0; k < levelDepth; k++)
            {
                for (int j = firstColumn; j < floorLength; j++)
                {
                    Rows[topRow].Columns[j].TileID = 13;
                }

                for (int i = 0; i < 3; i++)
                {
                    Rows[topRow + i].Columns[firstColumn].TileID = 8;
                }
                topRow = topRow + 3;
            }
        }

        public void LadderLevellerRight(int topRow, int firstColumn, int width, int levelDepth)
        {
            int floorLength = (firstColumn - 1) - width;

            for (int k = 0; k < levelDepth; k++)
            {
                for (int j = firstColumn; j > floorLength; j--)
                {
                    Rows[topRow].Columns[j].TileID = 13;
                }

                for (int i = 0; i < 3; i++)
                {
                    Rows[topRow + i].Columns[firstColumn].TileID = 3;
                }
                topRow = topRow + 3;
            }
        }

        public void drawFloor(int row, int firstColumn, int width)
        {
            for (int i = firstColumn; i < firstColumn + width; i++)
            {
                Rows[row].Columns[i].TileID = 13;
            }
        }

        public void addTrapDoor(int row, int firstColumn)
        {
            Rows[row].Columns[firstColumn].TileID = 5;
        }

        public void addSewageCover(int row, int firstColumn)
        {
            Rows[row].Columns[firstColumn].TileID = 9;
        }

        public void fillFloor(int row, int firstColumn, int finalColumn, int bottomRow)
        {
            for (int i = row; i < bottomRow; i++)
            {
                for(int j = firstColumn; j < finalColumn; j++)
                {
                    Rows[i].Columns[j].TileID = 1;
                }
            }
        }

        public void fillSewage(int row, int firstColumn, int finalColumn, int bottomRow)
        {
            for (int i = row; i < bottomRow; i++)
            {
                for (int j = firstColumn; j < finalColumn; j++)
                {
                    Rows[i].Columns[j].TileID = 21;
                }
            }
        }

        
    }
}
