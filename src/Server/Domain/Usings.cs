global using System.Security.Claims;
global using System.Security.Cryptography;
global using System.IdentityModel.Tokens.Jwt;
global using System.Text.RegularExpressions;

global using Microsoft.IdentityModel.Tokens;
global using Microsoft.Extensions.Configuration;

global using ErrorOr;

global using Domain.Data;
global using Domain.Interfaces;

global using TokensPair = (string AccessToken, string RefreshToken);