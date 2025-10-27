using System.Collections.ObjectModel;

namespace AMIS.Shared.Authorization;

public static class FshRoles
{
    public const string Admin = nameof(Admin);
    public const string Basic = nameof(Basic);

    public static IReadOnlyList<string> DefaultRoles { get; } = new ReadOnlyCollection<string>(new[]
    {
        Admin,
        Basic
    });

    // public static bool IsDefault(string roleName) => DefaultRoles.Any(r => r == roleName);
    
    /// <summary>
    /// Checks if the given role name is one of the default roles.
    /// </summary>
    /// <param name="roleName">The name of the role to check.</param>
    /// <returns>True if the roleName is a default role, false otherwise.</returns>
    public static bool IsDefault(string roleName)
    {
        // The DefaultRoles collection is assumed to be defined elsewhere in the scope,
        // for example, as a static field or property:
        // private static readonly string[] DefaultRoles = { "Admin", "User", "Guest" };
        // The Any() LINQ extension method checks if any element in the collection
        // satisfies the condition specified by the lambda expression.
        // In this case, it checks if any role 'r' in DefaultRoles is equal to roleName.
        return DefaultRoles.Any(r => r == roleName);
    }
}
