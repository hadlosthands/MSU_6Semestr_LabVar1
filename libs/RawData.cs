using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libs
{
    public delegate double FRaw(double x);
    public class RawData
    {
        public double Left { get; set; }
        public double Right { get; set; }
        public int NumOfNodes { get; set; }
        public bool UniformityCheck { get; set; }
        public FRaw Fraw { get; set; }
        public double[] GridNodes { get; set; }
        public double[] ArrayValues { get; set; }


        public RawData(double left, double right, int num, bool b, FRaw func)
        {
            Left = left;
            Right = right;
            NumOfNodes = num;
            UniformityCheck = b;
            Fraw = func;
            GridNodes = new double[num];
            ArrayValues = new double[num];

            GridNodes[0] = left;
            GridNodes[num - 1] = right;

            if (b)
            {
                for (int i = 1; i < NumOfNodes - 1; ++i)
                {
                    GridNodes[i] = Left + i * (Right - Left) / NumOfNodes;
                }
            } 
            else
            {
                Random R = new();
                for (int i = 1; i < NumOfNodes - 1; ++i)
                {
                    GridNodes[i] = Left + R.NextDouble() * (Right - Left);
                }
                Array.Sort(GridNodes);
            }

            for (int i = 0; i < NumOfNodes; ++i)
            {
                ArrayValues[i] = func(GridNodes[i]);
            }
        }

        public RawData(string filename)
        {
            try
            {
                Load(filename, out RawData rawData);
                Left = rawData.Left;
                Right = rawData.Right;
                NumOfNodes = rawData.NumOfNodes;
                UniformityCheck = rawData.UniformityCheck;
                Fraw = rawData.Fraw;
                GridNodes = rawData.GridNodes;
                ArrayValues = rawData.ArrayValues;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Save(string filename)
        {
            FileStream? fs = null;
            StreamWriter? writer = null;
            try
            {
                fs = File.Create(filename);
                writer = new StreamWriter(fs);
                writer.WriteLine(Left.ToString());
                writer.WriteLine(Right.ToString());
                writer.WriteLine(NumOfNodes.ToString());
                writer.WriteLine(UniformityCheck.ToString());

                for (int i = 0; i < NumOfNodes; ++i)
                {
                    writer.WriteLine(GridNodes[i].ToString());
                }
                for (int i = 0; i < NumOfNodes; ++i)
                {
                    writer.WriteLine(ArrayValues[i].ToString());
                }
                return;
            }
            catch (Exception x)
            {
                Console.WriteLine($"ERROR SAVING FILE ! ! !: {x}");
                throw;
            }
            finally
            {
                writer?.Dispose();
                fs?.Close();
            }
        }
        public static void Load(string filename, out RawData rawData)
        {
            FileStream? fs = null;
            StreamReader? reader = null;
            try
            {
                double left, right;
                int node_cnt;
                bool is_uniform;

                fs = File.OpenRead(filename);
                reader = new StreamReader(fs);
                left = double.Parse(reader.ReadLine());
                right = double.Parse(reader.ReadLine());
                node_cnt = Convert.ToInt32(reader.ReadLine());
                is_uniform = Convert.ToBoolean(reader.ReadLine());

                rawData = new RawData(left, right, node_cnt, is_uniform, CreationFunctions.LinearFunction);
                for (int i = 0; i < node_cnt; ++i)
                {
                    rawData.GridNodes[i] = double.Parse(reader.ReadLine());
                }
                for (int i = 0; i < node_cnt; ++i)
                {
                    rawData.ArrayValues[i] = double.Parse(reader.ReadLine());
                }
                return;
            }
            catch (Exception x)
            {
                Console.WriteLine($"ERROR LOADING FILE ! ! !: {x}");
                throw;
            }
            finally
            {
                reader?.Dispose();
                fs?.Close();
            }
        }
        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < NumOfNodes; i++)
            {
                result += $"Coordinate: {GridNodes[i]}\tValue: {ArrayValues[i]}";
            }
            return result;
        }
    }
}
