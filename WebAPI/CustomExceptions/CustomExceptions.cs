using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebAPI.CustomExceptions
{
    public class CustomExceptions
    {
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

        [Serializable]
        public class ModelStateException : Exception
        {
            public ModelStateException()
            {
            }

            public ModelStateException(string message) : base(message)
            {
            }

            public ModelStateException(string message, Exception inner) : base(message, inner)
            {
            }

            protected ModelStateException(
                SerializationInfo info,
                StreamingContext context) : base(info, context)
            {
            }
        }
    }
}
