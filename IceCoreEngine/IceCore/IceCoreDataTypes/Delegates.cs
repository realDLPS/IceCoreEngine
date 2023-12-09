using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCoreEngine
{
    public delegate void InputDelegate(float value);
    public delegate void FinishedLineTrace(List<LinetraceResponse> hits);
    public delegate void WidgetClick();
}
