﻿namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.GetRelatives;
public interface IGetRelativesController
{
    Task<IEnumerable<RelativeMovie>> GetRelatives(GetRelativesDto query);
}
