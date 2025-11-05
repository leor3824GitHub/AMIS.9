using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AMIS.Shared.Authorization;
using AMIS.Framework.Infrastructure.Tenant;

namespace TestProject.XUnit.Testing;

public abstract class BaseWebAppFactory : WebApplicationFactory<AMIS.WebApi.Host.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // Disable authorization globally for tests
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAssertion(_ => true)
                    .Build();
            });

            // Configure test tenant context - replace existing registration
            var tenantAccessorDescriptor = services.FirstOrDefault(d => 
                d.ServiceType == typeof(IMultiTenantContextAccessor<FshTenantInfo>));
            if (tenantAccessorDescriptor != null)
            {
                services.Remove(tenantAccessorDescriptor);
            }

            services.AddScoped<IMultiTenantContextAccessor<FshTenantInfo>>(sp =>
            {
                var tenantInfo = new FshTenantInfo(
                    TenantConstants.Root.Id,
                    TenantConstants.Root.Name,
                    string.Empty,
                    TenantConstants.Root.EmailAddress);

                var context = new MultiTenantContext<FshTenantInfo>
                {
                    TenantInfo = tenantInfo
                };

                return new TestTenantAccessor(context);
            });

            // Remove database initializers to avoid database setup during tests
            var initializerDescriptors = services
                .Where(d => d.ServiceType.Name.Contains("IDbInitializer") || 
                           d.ServiceType.Name.Contains("DbInitializer"))
                .ToList();
            foreach (var descriptor in initializerDescriptors)
            {
                services.Remove(descriptor);
            }

            // Remove Hangfire hosted services to avoid DB connections during integration tests
            var hostedServices = services.Where(d => d.ServiceType == typeof(IHostedService)).ToList();
            foreach (var hs in hostedServices)
            {
                var implType = hs.ImplementationType?.FullName
                                 ?? hs.ImplementationInstance?.GetType().FullName
                                 ?? hs.ImplementationFactory?.GetType().FullName;
                if (implType != null && implType.Contains("Hangfire", StringComparison.OrdinalIgnoreCase))
                {
                    services.Remove(hs);
                }
            }

            ConfigureServices(services);
        });
    }

    protected virtual void ConfigureServices(IServiceCollection services) { }

    protected void ReplaceHandler<TRequest, TResponse, TImplementation>(IServiceCollection services)
        where TRequest : IRequest<TResponse>
        where TImplementation : class, IRequestHandler<TRequest, TResponse>
    {
        var handlerDescriptors = services
            .Where(s => s.ServiceType.IsGenericType
                     && s.ServiceType.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)
                     && s.ServiceType.GenericTypeArguments[0] == typeof(TRequest))
            .ToList();
        foreach (var d in handlerDescriptors)
        {
            services.Remove(d);
        }

        services.AddTransient<IRequestHandler<TRequest, TResponse>, TImplementation>();
    }

    private sealed class TestTenantAccessor : IMultiTenantContextAccessor<FshTenantInfo>
    {
        private readonly IMultiTenantContext<FshTenantInfo> _context;

        public IMultiTenantContext<FshTenantInfo> MultiTenantContext => _context;

        IMultiTenantContext IMultiTenantContextAccessor.MultiTenantContext => _context;

        public TestTenantAccessor(IMultiTenantContext<FshTenantInfo> context)
        {
            _context = context;
        }
    }
}
