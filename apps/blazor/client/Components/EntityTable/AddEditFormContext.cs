namespace AMIS.Blazor.Client.Components.EntityTable;

public sealed class AddEditFormContext<TRequest>
{
    public AddEditFormContext(TRequest requestModel, bool isCreate)
    {
        RequestModel = requestModel;
        IsCreate = isCreate;
    }

    public TRequest RequestModel { get; }

    public bool IsCreate { get; }
}
