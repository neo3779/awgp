using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWGP
{
    public class Parameter<T>
    {
        public Parameter( )
        {
            this.data = default(T);
        }

        public Parameter(T value)
        {
            this.data = value;
        }

        // dchapman: User-Defined conversion operator from Parameter<T> to T - 04/11/2011
        public static implicit operator T(Parameter<T> rhs)
        {
            return rhs.data;
        }

        public T Data
        {
            get { return this.data; }
            set { this.data = value; }
        }

        // dchapman: User-Defined conversion operator from T to Parameter<T> - 04/11/2011
        //public static explicit operator Parameter<T>(T rhs)
        //{
        //    return new Parameter<T>(rhs);
        //}

        private T data;
    }
}
