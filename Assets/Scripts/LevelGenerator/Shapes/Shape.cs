using System;

namespace AssemblyCSharp
{
    public class Tuple<T1, T2>
    {
        public T1 First { get; set; }
        public T2 Second { get; set; }
        internal Tuple(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }

        public Tuple(Tuple<T1,T2> other)
        {
            First = other.First;
            Second = other.Second;
        }
    }


    public abstract class Shape
    {
        public Shape()
        {
        }

        public abstract Tuple<int,int>[][] GetShape();
    }
}

