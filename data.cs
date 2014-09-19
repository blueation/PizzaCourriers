using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PizzaCourriers
{
    class Data
    {
        int maxx = 20;
        int minx = 0;
        int maxy = 218;
        int miny = 0;
        int amountofdatainsmall = 10;
        int amountofdatainmedium = 100;
        int amountofdatainlarge = 1000;
        int amountofsetspersize = 5;
        public DataSet[] small;
        public DataSet[] medium;
        public DataSet[] large;
        Random random;
        
        public Data()
        {
            small = new DataSet[amountofsetspersize];
            medium = new DataSet[amountofsetspersize];
            large = new DataSet[amountofsetspersize];
            random = new Random();
            for (int tel = 0; tel < 5; tel++)
            {
                small[tel] = new DataSet(amountofdatainsmall);                
                medium[tel] = new DataSet(amountofdatainmedium);
                large[tel] = new DataSet(amountofdatainlarge);
                small[tel] = FillSets(small[tel]);
                medium[tel] = FillSets(medium[tel]);
                large[tel] = FillSets(large[tel]);
            }
            Console.ReadLine(); 
        }

        public DataSet FillSets(DataSet set)
        {
            bool[,] grid = new bool[20, 218]; 
            //int x = random.Next(minx, maxx + 1);
            //int y = random.Next(miny, maxy + 1);
            int x = 10;
            int y = 109;
            set.data[0] = "Restaurant: (" + x + "," + y + ")";
            grid[x, y] = true;
            for (int tel = 1; tel < set.data.Length; tel++)
            {
                while (true)
                {
                    x = random.Next(minx, maxx);
                    y = random.Next(miny, maxy);
                    if (!grid[x, y])
                    {
                        grid[x, y] = true;
                        set.data[tel] = "(" + x + "," + y + ")";
                        break;
                    }
                }
            }
            return set;
        }
    }

    class DataSet
    {
        public string[] data;
        public DataSet(int length)
        {
            data = new string[length];
        }
    }   
}
