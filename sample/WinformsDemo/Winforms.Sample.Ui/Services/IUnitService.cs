using System;
using System.Linq;
using Winforms.Sample.Domain;

namespace Winforms.Sample.Ui.Services
{
    public interface IUnitService
    {
        bool CreateUnit(Unit unit);
    }
}
