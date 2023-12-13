global using System.Security.Claims;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.ModelBinding;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;

global using ErrorOr;
global using Swashbuckle.AspNetCore.Filters;

global using Database;
global using Database.Repositories;
global using Domain.Data;
global using Domain.Services;
global using Domain.Interfaces;

global using WebAPI.Contracts.Cat;
global using WebAPI.Contracts.User;
global using WebAPI.Contracts.CartItem;
global using WebAPI.Contracts.Order;