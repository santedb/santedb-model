using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanteDB.Core.Model.Attributes
{
    /// <summary>
    /// Indicates that a particular property has no case
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NoCaseAttribute : Attribute
    {
    }
}
