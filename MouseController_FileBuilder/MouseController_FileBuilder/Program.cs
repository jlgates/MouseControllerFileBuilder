using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MouseController_FileBuilder
{
    class Program
    {
        static void Main(string[] args)
        {

            string inputFile = string.Empty;
            string outputFile = string.Empty;

            foreach(string arg in args)
            {
                if(arg.Split(new char[] { '=' })[0] == "/input")
                {
                    inputFile = arg.Split(new char[] { '=' })[1];
                }
                else if (arg.Split(new char[] { '=' })[0] == "/output")
                {
                    outputFile = arg.Split(new char[] { '=' })[1];
                }
            }

            List<Coordinate> coordinates = Coordinate.GetCoordinates(inputFile);

            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true };
            using (XmlWriter writer = XmlWriter.Create(outputFile, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("ArrayOfMouseEvents", "http://schemas.datacontract.org/2004/07/MouseController");
                writer.WriteAttributeString("xmlns", "i", null, "http://www.w3.org/2001/XMLSchema-instance");

                coordinates.ForEach(c => WriteMouseClick(writer, c));
                
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
            }
            //Console.ReadLine();
        }

        static void WriteMouseClick(XmlWriter writer, int order, int x, int y)
        {
            WriteMouseEvent(writer, "WM_LBUTTONDOWN", order.ToString(), x.ToString(), y.ToString(), "450000");
            WriteMouseEvent(writer, "WM_LBUTTONUP", string.Empty, x.ToString(), y.ToString(), "110000");
        }

        static void WriteMouseClick(XmlWriter writer, Coordinate coordinate)
        {
            WriteMouseClick(writer, coordinate.OrderId, coordinate.X, coordinate.Y);
        }

        static void WriteMouseEvent(XmlWriter writer, string message, string tag, string x, string y, string offset)
        {
            writer.WriteStartElement("MouseEvents");
            writer.WriteElementString("MouseMsg", message);
            writer.WriteElementString("Tag", tag);


            writer.WriteStartElement("pos");
            writer.WriteAttributeString("xmlns", "a", null, "http://schemas.datacontract.org/2004/07/System.Drawing");
            writer.WriteElementString("a", "x", null, x);
            writer.WriteElementString("a", "y", null, y);
            writer.WriteEndElement();
            writer.WriteElementString("tOffset", offset.ToString());
            writer.WriteEndElement();
        }
    }
}
