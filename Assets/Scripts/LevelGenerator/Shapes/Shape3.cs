﻿using System;

namespace AssemblyCSharp
{
    public class Shape3 : Shape
    {
        public Shape3()
        {
        }

        public override Tuple<int,int>[][] GetShape()
        {
            /*
                   *
                   ***
            */
            return new Tuple<int,int>[][]
            {
                new Tuple<int,int>[]{ new Tuple<int,int>(0, 0), new Tuple<int,int>(0, 1), new Tuple<int,int>(0, 2), new Tuple<int,int>(1, 0) },
                new Tuple<int,int>[]{ new Tuple<int,int>(0, 0), new Tuple<int,int>(1, 0), new Tuple<int,int>(2, 0), new Tuple<int,int>(0, 1) },
                new Tuple<int,int>[]{ new Tuple<int,int>(0, 1), new Tuple<int,int>(1, 1), new Tuple<int,int>(2, 1), new Tuple<int,int>(1, 0) },
            };
        }
    }
}

