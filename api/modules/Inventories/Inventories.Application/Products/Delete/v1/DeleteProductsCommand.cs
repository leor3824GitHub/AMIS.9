using MediatR;
using System.Collections.Generic;

namespace AMIS.WebApi.Inventories.Application.Products.Delete.v1
{
    public record DeleteProductsCommand(IEnumerable<Guid> ProductIds) : IRequest;
}

