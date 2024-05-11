global using System.Security.Claims;
global using System.ComponentModel.DataAnnotations;
global using System.Net.Http.Headers;
global using System.Text;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.ModelBinding;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;

global using ErrorOr;
global using MongoDB.Driver;
global using Swashbuckle.AspNetCore.Filters;
global using Serilog;
global using Mapster;

global using Database;
global using Database.Repositories;
global using Domain.Data;
global using Domain.Services;
global using Domain.Interfaces;

global using WebAPI;
global using WebAPI.Contracts.Cat;
global using WebAPI.Contracts.User;
global using WebAPI.Contracts.CartItem;
global using WebAPI.Contracts.Order;
global using WebAPI.Contracts.Payment;

global using TokensPair = (string AccessToken, string RefreshToken);