using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.MyExceptions
{
    public class SecretLinkException : Exception
    {
        public override string Message 
            => WebMaze.ResourceLocalization.MySystem.SecreteLinkMessage;
    }
}
