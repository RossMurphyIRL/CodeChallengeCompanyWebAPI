using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Services.CustomExceptions
{
    public class CompanyExceptions
    {
        [Serializable]
        public class ExistingIsinException : Exception
        {
            public ExistingIsinException()
            {
            }

            public ExistingIsinException(string message) : base(message)
            {
            }

            public ExistingIsinException(string message, Exception inner) : base(message, inner)
            {
            }

            protected ExistingIsinException(
                SerializationInfo info,
                StreamingContext context) : base(info, context)
            {
            }
        }

        [Serializable]
        public class IsinException : Exception
        {
            public IsinException()
            {
            }

            public IsinException(string message) : base(message)
            {
            }

            public IsinException(string message, Exception inner) : base(message, inner)
            {
            }

            protected IsinException(
                SerializationInfo info,
                StreamingContext context) : base(info, context)
            {
            }
        }
    }
}
