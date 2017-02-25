using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseController_FileBuilder
{
    public class Coordinate
    {
        public int OrderId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public static List<Coordinate> GetCoordinates(string fileName)
        {
            List<Coordinate> coordinates = new List<Coordinate>();

            int count = 0;
            string line = string.Empty;
            char[] comma = { ',' };
            string[] valueList = null;
            int OrderId,
                X,
                Y;
            int indexOrderId = -1,
                indexX = -1,
                indexY = -1;

            using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var sr = new StreamReader(fileStream))
                {
                    while (!sr.EndOfStream)
                    {
                        line = sr.ReadLine();
                        valueList = line.Split(comma);
                        if (count++ == 0)
                        {
                            int index = 0;
                            foreach (string v in valueList)
                            {
                                string value = "";
                                if (!string.IsNullOrEmpty(v)) value = v.Trim().ToLowerInvariant();
                                switch (value)
                                {
                                    case "order":
                                        indexOrderId = index;
                                        break;
                                    case "x":
                                        indexX = index;
                                        break;
                                    case "y":
                                        indexY = index;
                                        break;
                                }
                                index++;

                                if (indexOrderId >= 0 && indexX >= 0 && indexY >= 0)
                                    break;
                            }
                        }
                        else
                        {
                            if (!(indexOrderId >= 0 && indexX >= 0 && indexY >= 0))
                            {
                                throw new Exception("Not all expected columns are present!");
                            }

                            if (!int.TryParse(valueList[indexOrderId], out OrderId))
                                throw new Exception("Order is not a valid integer!");
                            if (!int.TryParse(valueList[indexX], out X))
                                throw new Exception("X is not a valid integer!");
                            if (!int.TryParse(valueList[indexY], out Y))
                                throw new Exception("Y is not a valid integer!");

                            coordinates.Add(new Coordinate() { OrderId = OrderId, X = X, Y = Y });
                        }

                    }
                }
            }

            return coordinates;
        }

    }

}
