﻿using libs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace WPF_UI
{
    public class ViewData: IDataErrorInfo
    {
        public double Left { get; set; }
        public double Right { get; set; }
        public int NumOfNodes { get; set; }
        public bool UniformityCheck { get; set; }
        public FRawEnum Func { get; set; }
        public double LeftDer { get; set; }
        public double RightDer { get; set; }
        public int SplineNodes { get; set; }
        public RawData? RawData { get; set; }
        public SplineData? SplineData { get; set; }

        public ViewData()
        {
            Left = 0;
            Right = 10;
            NumOfNodes = 2;
            SplineNodes = 2;
            UniformityCheck = true;
            RawData = new RawData(0, 1, 2, true, CreationFunctions.LinearFunction);
            SplineData = null;
        }

        public void Save(string filename)
        {
            try
            {
                RawData.Save(filename);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void LoadFromControls()
        {
            try
            {
                FRaw fRaw = GetFuncFromEnum(Func);
                RawData = new RawData(Left, Right, NumOfNodes, UniformityCheck, fRaw);
            }
            catch (Exception)
            {
                throw new Exception("Неправильный формат ввода!"); ;
            }
        }

        public void LoadFromFile(string filename)
        {
            try
            {
                RawData = new RawData(filename);
                Left = RawData.Left;
                Right = RawData.Right;
                NumOfNodes = RawData.NumOfNodes;
                UniformityCheck = RawData.UniformityCheck;
                Func = GetEnumFromFunc(RawData.Fraw);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int ComputeSpline()
        {
            try
            {
                SplineData = new SplineData(RawData, LeftDer, RightDer, SplineNodes);
                return SplineData.CreateSpline();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static FRaw GetFuncFromEnum(FRawEnum fRawEnum)
        {
            FRaw? fRaw = fRawEnum switch
            {
                FRawEnum.LinearFunction => CreationFunctions.LinearFunction,
                FRawEnum.CubicPolynomial => CreationFunctions.CubicPolynomial,
                FRawEnum.PseudoRandomGenerator => CreationFunctions.PseudoRandomGenerator,
                _ => CreationFunctions.LinearFunction
            };
            return fRaw;
        }

        public static FRawEnum GetEnumFromFunc(FRaw fRaw)
        {
            string frawName = fRaw.Method.Name;
            FRawEnum fRawEnum = frawName switch
            {
                "LinearFunction" => FRawEnum.LinearFunction,
                "CubicPolynomial" => FRawEnum.CubicPolynomial,
                "PseudoRandomGenerator" => FRawEnum.PseudoRandomGenerator,
                _ => FRawEnum.LinearFunction
            };
            return fRawEnum;
        }

        public string Error
        {
            get { return "Введены некорректные данные"; }
        }

        public string this[string columnName]
        {
            get
            {
                string result = String.Empty;
                switch (columnName)
                {
                    case "Left":
                        if (Left > Right)
                            result = "Левая граница должна быть меньше правой";
                        break;
                    case "Right":
                        if (Right < Right)
                            result = "Правая граница должна быть больше левой";
                        break;
                    case "NumOfNodes":
                        if (NumOfNodes < 2)
                            result = "Должно быть хотя бы 2 точки у интерполируемой функции!";
                        break;
                    case "SplineNodes":
                        if (SplineNodes < 2)
                            result = "Интерполяция должна быть проведена хотя бы в 2 точках!";
                        break;
                }
                return result;
            }
        }
    }
}
