using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BindingModels
{
    public interface IDirty
    {
        bool IsDirty { get; }
    }   // eo interface IDirty
}
