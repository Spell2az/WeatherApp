using api.Models;
using FluentResults;

namespace api.Services;

public interface ICoordinateTranslationService
{
    Task<Result<Coordinates>> GetCoordinatesFromLocation(string location);
}
