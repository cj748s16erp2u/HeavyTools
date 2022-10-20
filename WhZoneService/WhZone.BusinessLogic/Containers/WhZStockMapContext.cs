using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Containers.Interfaces;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Enums;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Containers;

public class WhZStockMapContext : IWhZStockMapContext
{
    private readonly List<IWhZStockMapData> movementList = new();

    public WhZStockMapContext()
    {
    }

    public IEnumerable<IWhZStockMapData> MovementList => this.movementList;

    public IWhZStockMapData AddMovement(IWhZStockMapData movementData)
    {
        if (this.disposedValue)
        {
#pragma warning disable S112 // General exceptions should never be thrown
            throw new Exception("This context is disposed");
#pragma warning restore S112 // General exceptions should never be thrown
        }

        if (!this.movementList.Contains(movementData))
        {
            this.movementList.Add(movementData);
        }

        return movementData;
    }

    public bool ProbeRemoveMovement(IWhZStockMapData movementData)
    {
        if (this.disposedValue)
        {
#pragma warning disable S112 // General exceptions should never be thrown
            throw new Exception("This context is disposed");
#pragma warning restore S112 // General exceptions should never be thrown
        }

        if (this.movementList.Contains(movementData) && movementData.Movement == WhZStockMovement.AddReceving)
        {
            var pos = this.movementList.IndexOf(movementData);
            var furtherMovementQty = this.movementList
                .Skip(pos + 1)
                .Sum(m => m.Movement == WhZStockMovement.AddReceving || m.Movement == WhZStockMovement.RemoveReserved ? m.Qty : -m.Qty);
            if (furtherMovementQty < 0)
            {
                return false;
            }
        }

        return true;
    }

    public bool TryRemoveMovement(IWhZStockMapData movementData)
    {
        if (this.disposedValue)
        {
#pragma warning disable S112 // General exceptions should never be thrown
            throw new Exception("This context is disposed");
#pragma warning restore S112 // General exceptions should never be thrown
        }

        if (this.movementList.Contains(movementData))
        {
            if (movementData.Movement == WhZStockMovement.AddReceving)
            {
                var pos = this.movementList.IndexOf(movementData);
                var furtherMovementQty = this.movementList
                    .Skip(pos + 1)
                    .Sum(m => m.Movement == WhZStockMovement.AddReceving || m.Movement == WhZStockMovement.RemoveReserved ? m.Qty : -m.Qty);
                if (furtherMovementQty < 0)
                {
                    return false;
                }
            }

            this.movementList.Remove(movementData);
        }

        return true;
    }

    public void Clear()
    {
        this.movementList.Clear();
    }

    #region IDisposable members

    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.movementList.Clear();
            }

            this.disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    #endregion
}
