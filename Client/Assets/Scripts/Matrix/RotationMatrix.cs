using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filterartifact
{
    //-----------------------------------------------------------------------------------------
    public enum ChooseNumCount
    {
        Eight = 8,
        Night = 9,
        Ten = 10,
        EleVen = 11,
        Twelve = 12,
        Thirteen = 13,
        Fourteen = 14,
        Fifteen = 15,
        SixTeen = 16,
    }
    //-----------------------------------------------------------------------------------------
    public enum Num
    {
        A = 1,
        B = 2,
        C = 3,
        D = 4,
        E = 5,
        F = 6,
        G = 7,
        H = 8,
        I = 9,
        J = 10,
        K = 11,
        L = 12,
        M = 13,
        N = 14,
        O = 15,
        P = 16,
    }
    //-----------------------------------------------------------------------------------------
    public static class RotationMatrix
    {
        public static Dictionary<int, List<Num>> _eSFRoMatrix;
        public static Dictionary<int, List<Num>> eSFRoMatrix
        {
            get
            {
                if (_eSFRoMatrix == null)
                {
                    _eSFRoMatrix = SetESFRoMatrix();
                }

                return _eSFRoMatrix;
            }
        }
        public static Dictionary<int, List<Num>> _nSFRoMatrix;
        public static Dictionary<int, List<Num>> nSFRoMatrix
        {
            get
            {
                if (_nSFRoMatrix == null)
                {
                    _nSFRoMatrix = SetNSFRoMatrix();
                }
                return _nSFRoMatrix;
            }
        }
        public static Dictionary<int, List<Num>> _tSFRoMatrix;
        public static Dictionary<int, List<Num>> tSFRoMatrix
        {
            get
            {
                if (_tSFRoMatrix == null)
                {
                    _tSFRoMatrix = SetTSFRoMatrix();
                }
                return _tSFRoMatrix;
            }
        }
        public static Dictionary<int, List<Num>> _elSFRoMatrix;
        public static Dictionary<int, List<Num>> elSFRoMatrix
        {
            get
            {
                if (_elSFRoMatrix == null)
                {
                    _elSFRoMatrix = SetElSFRoMatrix();
                }
                return _elSFRoMatrix;
            }
        }

        public static Dictionary<int, List<Num>> _twSFRoMatrix;
        public static Dictionary<int, List<Num>> twSFRoMatrix
        {
            get
            {
                if (_twSFRoMatrix == null)
                {
                    _twSFRoMatrix = SetTwSFRoMatrix();
                }

                return _twSFRoMatrix;

            }
        }
        /// <summary>
        /// 中六保五旋转矩阵缩水结果
        /// </summary>
        /// <param name="numberCount"></param>
        /// <param name="choNum"></param>
        /// <returns></returns>
        public static List<List<byte>> GetRotationMatrixResult(int numberCount, List<byte> choNum)
        {

            var listBytes = new List<List<byte>>();

            switch ((ChooseNumCount)numberCount)
            {
                case ChooseNumCount.Eight://8
                    listBytes = GetResult(elSFRoMatrix, choNum);
                    break;
                case ChooseNumCount.Night://9
                    listBytes = GetResult(nSFRoMatrix, choNum);
                    break;
                case ChooseNumCount.Ten://10
                    listBytes = GetResult(tSFRoMatrix, choNum);
                    break;
                case ChooseNumCount.EleVen://11
                    listBytes = GetResult(elSFRoMatrix, choNum);
                    break;
                case ChooseNumCount.Twelve:
                    listBytes = GetResult(twSFRoMatrix, choNum);
                    break;
                case ChooseNumCount.Thirteen:
                    break;
                case ChooseNumCount.Fourteen:
                    break;
                case ChooseNumCount.Fifteen:
                    break;
                case ChooseNumCount.SixTeen:
                    break;
                default:
                    break;
            }
            return listBytes;
        }
        //-----------------------------------------------------------------------------------------
        public static List<List<byte>> GetResult(Dictionary<int, List<Num>> dict, List<byte> choNum)
        {
            var result = new List<List<byte>>();
            var dictEnumerator = dict.GetEnumerator();
            while (dictEnumerator.MoveNext())
            {
                var list = dictEnumerator.Current.Value;
                var listByte = new List<byte>();
                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = 0; j < choNum.Count; j++)
                    {
                        if ((int)list[i] - 1 == j)
                        {
                            listByte.Add(choNum[j]);
                        }
                    }
                }
                result.Add(listByte);
            }
            return result;
        }
        //-----------------------------------------------------------------------------------------
        //旋转矩阵08-6-5共四注
        public static Dictionary<int, List<Num>> SetESFRoMatrix()
        {
            // A B C E G H
            // A B D F G H
            // A C D E F H
            // B C D E F G
            var list1 = new List<Num>() { Num.A, Num.B, Num.C, Num.E, Num.G, Num.H };
            var list2 = new List<Num>() { Num.A, Num.B, Num.D, Num.F, Num.G, Num.H };
            var list3 = new List<Num>() { Num.A, Num.C, Num.D, Num.E, Num.F, Num.H };
            var list4 = new List<Num>() { Num.B, Num.C, Num.D, Num.E, Num.F, Num.G };
            var dict = new Dictionary<int, List<Num>>();
            dict.Add(1, list1);
            dict.Add(2, list2);
            dict.Add(3, list3);
            dict.Add(4, list4);
            return dict;
        }
        //-----------------------------------------------------------------------------------------
        //旋转矩阵09-6-5共七注
        public static Dictionary<int, List<Num>> SetNSFRoMatrix()
        {
            //A,B,C,D,F,H
            //A,B,C,E,G,I
            //A,B,D,F,G,H
            //A,C,D,E,G,I
            //A,C,E,F,G,I
            //B,C,D,F,G,H
            //B,D,E,F,H,I
            var list1 = new List<Num>() { Num.A, Num.B, Num.C, Num.D, Num.F, Num.H };
            var list2 = new List<Num>() { Num.A, Num.B, Num.C, Num.E, Num.G, Num.I };
            var list3 = new List<Num>() { Num.A, Num.B, Num.D, Num.F, Num.G, Num.H };
            var list4 = new List<Num>() { Num.A, Num.C, Num.D, Num.E, Num.G, Num.I };
            var list5 = new List<Num>() { Num.A, Num.C, Num.E, Num.F, Num.G, Num.I };
            var list6 = new List<Num>() { Num.B, Num.C, Num.D, Num.F, Num.G, Num.H };
            var list7 = new List<Num>() { Num.B, Num.D, Num.E, Num.F, Num.H, Num.I };
            var dict = new Dictionary<int, List<Num>>();
            dict.Add(1, list1);
            dict.Add(2, list2);
            dict.Add(3, list3);
            dict.Add(4, list4);
            dict.Add(5, list5);
            dict.Add(6, list6);
            dict.Add(7, list7);
            return dict;
        }
        //-----------------------------------------------------------------------------------------
        //旋转矩阵10-6-5共十五注
        public static Dictionary<int, List<Num>> SetTSFRoMatrix()
        {
            //A,B,C,D,H,I
            //A,B,D,E,G,I
            //A,B,D,E,I,J
            //A,B,D,G,I,J
            //A,C,D,E,F,I
            //A,C,D,F,G,H
            //A,C,D,H,I,J
            //A,C,F,G,H,I
            //A,D,E,F,H,I
            //A,D,E,G,I,J
            //B,C,E,F,G,J
            //B,C,E,F,H,J
            //B,C,E,G,H,J
            //B,E,F,G,H,J
            //C,D,F,G,H,I
            var list1 = new List<Num>() { Num.A, Num.B, Num.C, Num.D, Num.H, Num.I };
            var list2 = new List<Num>() { Num.A, Num.B, Num.D, Num.E, Num.G, Num.I };
            var list3 = new List<Num>() { Num.A, Num.B, Num.D, Num.E, Num.I, Num.J };
            var list4 = new List<Num>() { Num.A, Num.B, Num.D, Num.G, Num.I, Num.J };
            var list5 = new List<Num>() { Num.A, Num.C, Num.D, Num.E, Num.F, Num.I };
            var list6 = new List<Num>() { Num.A, Num.C, Num.D, Num.F, Num.G, Num.H };
            var list7 = new List<Num>() { Num.A, Num.C, Num.D, Num.H, Num.I, Num.J };
            var list8 = new List<Num>() { Num.A, Num.C, Num.F, Num.G, Num.H, Num.I };
            var list9 = new List<Num>() { Num.A, Num.D, Num.E, Num.F, Num.H, Num.I };
            var list10 = new List<Num>() { Num.A, Num.D, Num.E, Num.G, Num.I, Num.J };
            var list11 = new List<Num>() { Num.B, Num.C, Num.E, Num.F, Num.G, Num.J };
            var list12 = new List<Num>() { Num.B, Num.C, Num.E, Num.F, Num.H, Num.J };
            var list13 = new List<Num>() { Num.B, Num.C, Num.E, Num.G, Num.H, Num.J };
            var list14 = new List<Num>() { Num.B, Num.E, Num.F, Num.G, Num.H, Num.J };
            var list15 = new List<Num>() { Num.C, Num.D, Num.F, Num.G, Num.H, Num.I };
            var dict = new Dictionary<int, List<Num>>();
            dict.Add(1, list1);
            dict.Add(2, list2);
            dict.Add(3, list3);
            dict.Add(4, list4);
            dict.Add(5, list5);
            dict.Add(6, list6);
            dict.Add(7, list7);
            dict.Add(8, list8);
            dict.Add(9, list9);
            dict.Add(10, list10);
            dict.Add(11, list11);
            dict.Add(12, list12);
            dict.Add(13, list13);
            dict.Add(14, list14);
            dict.Add(15, list15);
            return dict;
        }
        //-----------------------------------------------------------------------------------------
        //旋转矩阵11-6-5共二十六注
        public static Dictionary<int, List<Num>> SetElSFRoMatrix()
        {
            var list1 = new List<Num>() { Num.A, Num.B, Num.C, Num.D, Num.H, Num.K };
            var list2 = new List<Num>() { Num.A, Num.B, Num.C, Num.E, Num.F, Num.K };
            var list3 = new List<Num>() { Num.A, Num.B, Num.C, Num.G, Num.I, Num.J };
            var list4 = new List<Num>() { Num.A, Num.B, Num.D, Num.E, Num.I, Num.J };
            var list5 = new List<Num>() { Num.A, Num.B, Num.D, Num.F, Num.G, Num.K };
            var list6 = new List<Num>() { Num.A, Num.B, Num.E, Num.G, Num.H, Num.K };
            var list7 = new List<Num>() { Num.A, Num.B, Num.F, Num.H, Num.I, Num.J };
            var list8 = new List<Num>() { Num.A, Num.C, Num.D, Num.E, Num.G, Num.J };
            var list9 = new List<Num>() { Num.A, Num.C, Num.D, Num.F, Num.I, Num.K };
            var list10 = new List<Num>() { Num.A, Num.C, Num.E, Num.H, Num.I, Num.K };
            var list11 = new List<Num>() { Num.A, Num.C, Num.F, Num.G, Num.H, Num.J };
            var list12 = new List<Num>() { Num.A, Num.D, Num.E, Num.F, Num.H, Num.J };
            var list13 = new List<Num>() { Num.A, Num.D, Num.G, Num.H, Num.I, Num.K };
            var list14 = new List<Num>() { Num.A, Num.E, Num.F, Num.G, Num.I, Num.K };
            var list15 = new List<Num>() { Num.A, Num.B, Num.C, Num.D, Num.H, Num.I };
            var list16 = new List<Num>() { Num.B, Num.C, Num.D, Num.E, Num.G, Num.I };
            var list17 = new List<Num>() { Num.B, Num.C, Num.D, Num.F, Num.J, Num.K };
            var list18 = new List<Num>() { Num.B, Num.C, Num.E, Num.H, Num.J, Num.K };
            var list19 = new List<Num>() { Num.B, Num.D, Num.E, Num.F, Num.H, Num.I };
            var list20 = new List<Num>() { Num.B, Num.D, Num.G, Num.H, Num.J, Num.K };
            var list21 = new List<Num>() { Num.B, Num.E, Num.F, Num.G, Num.J, Num.K };
            var list22 = new List<Num>() { Num.C, Num.D, Num.E, Num.F, Num.G, Num.H };
            var list23 = new List<Num>() { Num.C, Num.D, Num.H, Num.I, Num.J, Num.K };
            var list24 = new List<Num>() { Num.C, Num.E, Num.F, Num.I, Num.J, Num.K };
            var list25 = new List<Num>() { Num.D, Num.F, Num.G, Num.I, Num.J, Num.K };
            var list26 = new List<Num>() { Num.E, Num.G, Num.H, Num.I, Num.J, Num.K };
            var dict = new Dictionary<int, List<Num>>();
            dict.Add(1, list1);
            dict.Add(2, list2);
            dict.Add(3, list3);
            dict.Add(4, list4);
            dict.Add(5, list5);
            dict.Add(6, list6);
            dict.Add(7, list7);
            dict.Add(8, list8);
            dict.Add(9, list9);
            dict.Add(10, list10);
            dict.Add(11, list11);
            dict.Add(12, list12);
            dict.Add(13, list13);
            dict.Add(14, list14);
            dict.Add(15, list15);
            dict.Add(16, list16);
            dict.Add(17, list17);
            dict.Add(18, list18);
            dict.Add(19, list19);
            dict.Add(20, list20);
            dict.Add(21, list21);
            dict.Add(22, list22);
            dict.Add(23, list23);
            dict.Add(24, list24);
            dict.Add(25, list25);
            dict.Add(26, list26);
            return dict;
        }
        //-----------------------------------------------------------------------------------------
        //旋转矩阵12-6-5共四十二注
        public static Dictionary<int, List<Num>> SetTwSFRoMatrix()
        {
            var list1 = new List<Num>() { Num.A, Num.B, Num.C, Num.D, Num.G, Num.I };
            var list2 = new List<Num>() { Num.A, Num.B, Num.C, Num.E, Num.H, Num.I };
            var list3 = new List<Num>() { Num.A, Num.B, Num.C, Num.I, Num.J, Num.K };
            var list4 = new List<Num>() { Num.A, Num.B, Num.D, Num.E, Num.F, Num.K };
            var list5 = new List<Num>() { Num.A, Num.B, Num.D, Num.F, Num.H, Num.J };
            var list6 = new List<Num>() { Num.A, Num.B, Num.D, Num.H, Num.K, Num.L };
            var list7 = new List<Num>() { Num.A, Num.B, Num.E, Num.F, Num.G, Num.J };
            var list8 = new List<Num>() { Num.A, Num.B, Num.E, Num.G, Num.K, Num.L };
            var list9 = new List<Num>() { Num.A, Num.B, Num.G, Num.H, Num.J, Num.L };
            var list10 = new List<Num>() { Num.A, Num.C, Num.D, Num.E, Num.G, Num.H };
            var list11 = new List<Num>() { Num.A, Num.C, Num.D, Num.F, Num.G, Num.L };
            var list12 = new List<Num>() { Num.A, Num.C, Num.D, Num.G, Num.J, Num.K };
            var list13 = new List<Num>() { Num.A, Num.C, Num.E, Num.F, Num.H, Num.L };
            var list14 = new List<Num>() { Num.A, Num.C, Num.E, Num.H, Num.J, Num.K };
            var list15 = new List<Num>() { Num.A, Num.C, Num.F, Num.H, Num.I, Num.L };
            var list16 = new List<Num>() { Num.A, Num.C, Num.F, Num.J, Num.K, Num.L };
            var list17 = new List<Num>() { Num.A, Num.D, Num.E, Num.I, Num.J, Num.L };
            var list18 = new List<Num>() { Num.A, Num.D, Num.F, Num.H, Num.I, Num.K };
            var list19 = new List<Num>() { Num.A, Num.E, Num.F, Num.G, Num.I, Num.K };
            var list20 = new List<Num>() { Num.A, Num.F, Num.G, Num.H, Num.I, Num.J };
            var list21 = new List<Num>() { Num.A, Num.G, Num.H, Num.I, Num.K, Num.L };
            var list22 = new List<Num>() { Num.B, Num.C, Num.D, Num.E, Num.F, Num.J };
            var list23 = new List<Num>() { Num.B, Num.C, Num.D, Num.E, Num.K, Num.L };
            var list24 = new List<Num>() { Num.B, Num.C, Num.D, Num.H, Num.J, Num.L };
            var list25 = new List<Num>() { Num.B, Num.C, Num.E, Num.G, Num.J, Num.L };
            var list26 = new List<Num>() { Num.B, Num.C, Num.F, Num.G, Num.H, Num.K };
            var list27 = new List<Num>() { Num.B, Num.D, Num.E, Num.G, Num.H, Num.I };
            var list28 = new List<Num>() { Num.B, Num.D, Num.F, Num.G, Num.I, Num.L };
            var list29 = new List<Num>() { Num.B, Num.D, Num.G, Num.I, Num.J, Num.K };
            var list30 = new List<Num>() { Num.B, Num.E, Num.F, Num.H, Num.I, Num.L };
            var list31 = new List<Num>() { Num.B, Num.E, Num.H, Num.I, Num.J, Num.K };
            var list32 = new List<Num>() { Num.B, Num.F, Num.I, Num.J, Num.K, Num.L };
            var list33 = new List<Num>() { Num.C, Num.D, Num.E, Num.F, Num.I, Num.K };
            var list34 = new List<Num>() { Num.C, Num.D, Num.F, Num.H, Num.I, Num.J };
            var list35 = new List<Num>() { Num.C, Num.D, Num.H, Num.I, Num.K, Num.L };
            var list36 = new List<Num>() { Num.C, Num.E, Num.F, Num.G, Num.I, Num.J };
            var list37 = new List<Num>() { Num.C, Num.E, Num.G, Num.I, Num.K, Num.L };
            var list38 = new List<Num>() { Num.C, Num.G, Num.H, Num.I, Num.J, Num.L };
            var list39 = new List<Num>() { Num.D, Num.E, Num.F, Num.G, Num.H, Num.L };
            var list40 = new List<Num>() { Num.D, Num.E, Num.G, Num.H, Num.J, Num.K };
            var list41 = new List<Num>() { Num.D, Num.F, Num.G, Num.J, Num.K, Num.L };
            var list42 = new List<Num>() { Num.E, Num.F, Num.H, Num.J, Num.K, Num.L };
            var dict = new Dictionary<int, List<Num>>();
            dict.Add(1, list1);
            dict.Add(2, list2);
            dict.Add(3, list3);
            dict.Add(4, list4);
            dict.Add(5, list5);
            dict.Add(6, list6);
            dict.Add(7, list7);
            dict.Add(8, list8);
            dict.Add(9, list9);
            dict.Add(10, list10);
            dict.Add(11, list11);
            dict.Add(12, list12);
            dict.Add(13, list13);
            dict.Add(14, list14);
            dict.Add(15, list15);
            dict.Add(16, list16);
            dict.Add(17, list17);
            dict.Add(18, list18);
            dict.Add(19, list19);
            dict.Add(20, list20);
            dict.Add(21, list21);
            dict.Add(22, list22);
            dict.Add(23, list23);
            dict.Add(24, list24);
            dict.Add(25, list25);
            dict.Add(26, list26);
            dict.Add(27, list27);
            dict.Add(28, list28);
            dict.Add(29, list29);
            dict.Add(30, list30);
            dict.Add(31, list31);
            dict.Add(32, list32);
            dict.Add(33, list33);
            dict.Add(34, list34);
            dict.Add(35, list35);
            dict.Add(36, list36);
            dict.Add(37, list37);
            dict.Add(38, list38);
            dict.Add(39, list39);
            dict.Add(40, list40);
            dict.Add(41, list41);
            dict.Add(42, list42);
            return dict;
        }
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------


    }
}
