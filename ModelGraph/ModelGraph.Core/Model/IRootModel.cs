using System;
using System.Collections.Generic;
using System.Text;

namespace ModelGraph.Core
{
    public interface IRootModel : IModel
    {
        IList<IModel> ChildModels { get; }
    }
}
