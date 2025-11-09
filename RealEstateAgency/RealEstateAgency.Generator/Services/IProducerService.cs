using RealEstateAgency.Application.Contracts.Client;
using RealEstateAgency.Application.Contracts.RealEstateObject;
using RealEstateAgency.Application.Contracts.Request;

namespace RealEstateAgency.Generator.Services;

/// <summary>
/// Интерфес службы, занимающейся отправкой сообщений по шине
/// </summary>
public interface IProducerService
{
    /// <summary>
    /// Метод для отправки коллекции DTO клиентов
    /// </summary>
    /// <param name="batch">Коллекция DTO</param>
    public Task SendClientsAsync(IList<ClientCreateUpdateDto> batch);

    /// <summary>
    /// Метод для отправки коллекции DTO объектов недвижимости
    /// </summary>
    /// <param name="batch">Коллекция DTO</param>
    public Task SendRealEstateObjectsAsync(IList<RealEstateObjectCreateUpdateDto> batch);

    /// <summary>
    /// Метод для отправки коллекции DTO заявок
    /// </summary>
    /// <param name="batch">Коллекция DTO</param>
    public Task SendRequestsAsync(IList<RequestCreateUpdateDto> batch);
}