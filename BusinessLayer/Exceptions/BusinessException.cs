using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Exceptions
{
    public class BusinessException :Exception
    {
        /// <summary>
        /// İş kuralı ihlallerinde fırlatılır. Mesajı doğrudan kullanıcıya gösterilebilir.
        /// </summary>
        public BusinessException(string mssg) : base(mssg)  { }
    }
}
