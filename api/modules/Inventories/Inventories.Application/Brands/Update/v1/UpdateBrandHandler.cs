using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.Brands.Update.v1;
public sealed class UpdateBrandHandler(
    ILogger<UpdateBrandHandler> logger,
    [FromKeyedServices("inventories:brands")] IRepository<Brand> repository)
    : IRequestHandler<UpdateBrandCommand, UpdateBrandResponse>
{
    public async Task<UpdateBrandResponse> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var brand = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = brand ?? throw new BrandNotFoundException(request.Id);
        var updatedBrand = brand.Update(request.Name, request.Description);
        await repository.UpdateAsync(updatedBrand, cancellationToken);
        logger.LogInformation("Brand with id : {BrandId} updated.", brand.Id);
        return new UpdateBrandResponse(brand.Id);
    }
}

