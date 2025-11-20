namespace AMIS.Shared.Authorization;

public static class FshActions
{
    public const string View = nameof(View);
    public const string Search = nameof(Search);
    public const string Create = nameof(Create);
    public const string Update = nameof(Update);
    public const string Delete = nameof(Delete);
    public const string Export = nameof(Export);
    public const string Generate = nameof(Generate);
    public const string Clean = nameof(Clean);
    public const string UpgradeSubscription = nameof(UpgradeSubscription);
    // Workflow / Domain-Specific Actions
    public const string Complete = nameof(Complete);
    public const string Post = nameof(Post);
    public const string Link = nameof(Link);
    public const string Cancel = nameof(Cancel);
    public const string Assign = nameof(Assign);
    public const string Accept = nameof(Accept);
    public const string StatusUpdate = nameof(StatusUpdate);
}
