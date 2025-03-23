﻿using System.ComponentModel;
using FluentValidation;
using AMIS.Shared.Authorization;

namespace AMIS.Framework.Core.Identity.Tokens.Features.Generate;
public record TokenGenerationCommand(
    [property: DefaultValue(TenantConstants.Root.EmailAddress)] string Email,
    [property: DefaultValue(TenantConstants.DefaultPassword)] string Password);

public class GenerateTokenValidator : AbstractValidator<TokenGenerationCommand>
{
    public GenerateTokenValidator()
    {
        RuleFor(p => p.Email).Cascade(CascadeMode.Stop).NotEmpty().EmailAddress();

        RuleFor(p => p.Password).Cascade(CascadeMode.Stop).NotEmpty();
    }
}
