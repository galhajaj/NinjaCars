using System;

namespace AssemblyCSharp
{
    public class Shape4 : Shape
    {
        public Shape4()
        {
        }

        public override Tuple<int,int>[][] GetShape()
        {
            /*
                    *
                   ***
                    *
            */
            return new Tuple<int,int>[][]
            {
                new Tuple<int,int>[]{ new Tuple<int,int>(1, 0), new Tuple<int,int>(0, 1), new Tuple<int,int>(1, 1), new Tuple<int,int>(2, 1), new Tuple<int,int>(1, 2) },
            };
        }
    }
}

