﻿using MediatR;

namespace AMIS.WebApi.Todo.Features.Delete.v1;
public sealed record DeleteTodoCommand(
    Guid Id) : IRequest;



