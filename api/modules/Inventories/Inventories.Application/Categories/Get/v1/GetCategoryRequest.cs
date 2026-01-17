using MediatR;

namespace AMIS.WebApi.Inventories.Application.Categories.Get.v1;
public class GetCategoryRequest : IRequest<CategoryResponse>
{
    public Guid Id { get; set; }
    public GetCategoryRequest(Guid id) => Id = id;
}

