﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model.Interfaces;

public interface IBusinessEntity : IEntity
{
    string Addusrid { get; set; }
    DateTime Adddate { get; set; }
}
