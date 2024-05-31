﻿global using Application.Abstractions;
global using Application.User.Commands.Validators;
global using Domain.Aggregates;
global using Domain.Entities;
global using Domain.Ports;
global using FluentValidation;
global using MediatR;
global using Microsoft.Extensions.DependencyInjection;
global using Shared.Utils;
global using System.Reflection;
global using UserEntity = Domain.Entities.User;