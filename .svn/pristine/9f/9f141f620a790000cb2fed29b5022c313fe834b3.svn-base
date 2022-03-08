using System;
using System.Collections.Generic;
using System.Text;
using Elect.Core.StringUtils;

namespace TIGE.Core.Constants
{
    public static class CodeGenerator
    {
        public static string GetCode ()
        {
            var prefix = StringHelper.Generate(2, true, false, false, false);
            var last = StringHelper.Generate(2, false, false, true, false);
            return prefix + last;
        }
    }
}
