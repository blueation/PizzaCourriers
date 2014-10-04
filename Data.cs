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
        int amountofdata = 101;
        int amountofsetspersize = 5;
        public DataSet[] dataset;
        //Random random;
        
        public Data()
        {
            dataset = new DataSet[amountofsetspersize];
            //random = new Random();
            for (int tel = 0; tel < 5; tel++)
            {            
                dataset[tel] = new DataSet(amountofdata);
                dataset[tel] = FillSets(dataset[tel]);
            }
            //Console.ReadLine(); 
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
                    x = Program.random.Next(minx, maxx);
                    y = Program.random.Next(miny, maxy);
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
