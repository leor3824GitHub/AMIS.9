using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMIS.WebApi.Catalog.Domain.ValueObjects;
public enum PurchaseStatus
{
    InProgress,
    PartiallyDelivered,
    FullyDelivered,
    Canceled,
}
