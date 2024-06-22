namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces;
public interface INotificationService
{
    Task SendNotificationAsync(string message, string link);
}
