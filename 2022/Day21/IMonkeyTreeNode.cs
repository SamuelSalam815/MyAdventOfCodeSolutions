using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day21;

internal interface IMonkeyTreeNode
{
    public string Name { get; }
    public long GetValue();

    public long InferValue();
}
