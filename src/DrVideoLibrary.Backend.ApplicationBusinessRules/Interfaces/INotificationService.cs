namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces;
public interface INotificationService
{
    Task SendNotificationAsync(SendNotificationType type, string message, string link, ILogger logger, bool isNew);
}
