global using DrVideoLibrary.Api;
global using DrVideoLibrary.Api.Extensions;
global using DrVideoLibrary.Api.Helpers;
global using DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.GetRelatives;
global using DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.SeachMovies;
global using DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.SearchMoveDetail;
global using DrVideoLibrary.Entities.Models;
global using DrVideoLibrary.Entities.ValueObjects;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Azure.Functions.Extensions.DependencyInjection;
global using Microsoft.Azure.WebJobs;
global using Microsoft.Azure.WebJobs.Extensions.Http;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using System;
global using System.Collections.Generic;
global using System.IO;
global using System.Text;
global using System.Text.Json;
global using System.Threading.Tasks;
global using DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.AddMovie;
global using DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.GetAll;
