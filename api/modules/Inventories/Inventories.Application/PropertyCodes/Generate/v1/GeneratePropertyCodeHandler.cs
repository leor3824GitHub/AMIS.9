using AMIS.WebApi.Inventories.Application.PropertyCodes.Generate.v1;
using AMIS.WebApi.Inventories.Application.Acceptances.Services;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.PropertyCodes.Generate.v1;

public sealed class GeneratePropertyCodeHandler(
    IAssetPropertyCodeGenerator codeGenerator) : IRequestHandler<GeneratePropertyCodeCommand, GeneratePropertyCodeResponse>
{
    private readonly IAssetPropertyCodeGenerator _codeGenerator = codeGenerator ?? throw new ArgumentNullException(nameof(codeGenerator));

    public async Task<GeneratePropertyCodeResponse> Handle(GeneratePropertyCodeCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var coaRequest = new CoaPropertyCodeRequest(
            AcquisitionDate: request.AcquisitionDate,
            Classification: request.Classification,
            OfficeCode: request.OfficeCode,
            ClassCode: request.ClassCode,
            CategoryCode: request.CategoryCode,
            ItemCode: request.ItemCode,
            SequenceSuffix: request.SequenceSuffix);

        // The CoaPropertyCodeGenerator will generate the full code, but we only need to store/return the sequence
        var propertyCode = await _codeGenerator.GenerateAsync(coaRequest, cancellationToken).ConfigureAwait(false);

        // Parse the response to extract sequence
        // Format: {Year}-{Agency}-{Office}-{Class}-{Category}-{Item}-{Sequence.Decimal}
        var parts = propertyCode.Split('-');
        var sequence = 0;
        if (parts.Length > 6)
        {
            var sequencePart = parts[6].Split('.')[0];
            int.TryParse(sequencePart, out sequence);
        }
        
        return new GeneratePropertyCodeResponse(
            Classification: request.ClassCode ?? "00",
            Category: request.CategoryCode ?? "00",
            Sequence: sequence);
    }
}
