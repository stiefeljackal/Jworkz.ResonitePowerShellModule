using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JworkzResonitePowerShellModule.Test;

internal class Example
{
    public event EventHandler<CarEventArgs>? Fire;

    public void A()
    {
    }

    public void Print()
    {
        var invo = Fire.GetInvocationList();
        var t = invo[0].GetType();
    }
}

internal class Car
{
    public void SendFire(object? sender, CarEventArgs car)
    {
        Console.Write(car?.Count.ToString());
    }
}

internal class CarEventArgs : EventArgs
{
    public int Count { get; set; }
}