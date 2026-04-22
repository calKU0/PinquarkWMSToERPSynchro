using Microsoft.Extensions.Options;
using PinquarkWMSToERPSynchro.Contracts.Clients;
using PinquarkWMSToERPSynchro.Contracts.DTOs;
using PinquarkWMSToERPSynchro.Contracts.DTOs.Requests;
using PinquarkWMSToERPSynchro.Contracts.DTOs.Responses;
using PinquarkWMSToERPSynchro.Contracts.Settings;
using System.Net.Http.Json;
using System.Text.Json;

namespace PinquarkWMSToERPSynchro.Infrastructure.Clients
{
    public class PinquarkApiClient : IPinquarkApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly PinquarkApiSettings _settings;
        private readonly JsonSerializerOptions _jsonOptions;

        public PinquarkApiClient(HttpClient httpClient, IOptions<PinquarkApiSettings> options)
        {
            _httpClient = httpClient;
            _settings = options.Value;

            _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
            _httpClient.DefaultRequestHeaders.Add("token-mer", _settings.ApiKey);

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }
        public async Task<List<GetZonesResponse>> GetZonesAsync(string endpoint, CancellationToken cancellationToken = default)
        {
            var requestBody = new
            {
                warehouse = "HURT"
            };

            return await PostDataAsync<List<GetZonesResponse>>(endpoint, requestBody, cancellationToken);
        }
        public async Task<List<GetLocationsResponse>> GetLocationsAsync(string endpoint, GetLocationsRequest request, CancellationToken cancellationToken = default)
        {
            return await PostDataAsync<List<GetLocationsResponse>>(endpoint, request, cancellationToken);
        }

        public async Task<List<GetWarehouseStockResponse>> GetWarehouseStockAsync(string endpoint, CancellationToken cancellationToken = default)
        {
            return await PostDataAsync<List<GetWarehouseStockResponse>>(endpoint, cancellationToken);
        }


        public async Task<List<GetDocsResponse>> GetDocsAsync(string endpoint, CancellationToken cancellationToken = default)
        {
            return await PostDataAsync<List<GetDocsResponse>>(endpoint, cancellationToken);
        }

        public async Task<List<GetTasksResponse>> GetTasksAsync(string endpoint, GetTasksRequest request, CancellationToken cancellationToken = default)
        {
            return await PostDataAsync<List<GetTasksResponse>>(endpoint, request, cancellationToken);
        }

        public async Task<List<GetLuResourcesResponse>> GetLuResourcesAsync(string endpoint, CancellationToken cancellationToken = default)
        {
            return await PostDataAsync<List<GetLuResourcesResponse>>(endpoint, cancellationToken);
        }

        public async Task<List<GetOperationsResponse>> GetOperationsAsync(string endpoint, GetOperationsRequest request, CancellationToken cancellationToken = default)
        {
            return await PostDataAsync<List<GetOperationsResponse>>(endpoint, request, cancellationToken);
        }

        public async Task<List<GetCustomOperationResponse>> GetCustomOperationsAsync(string endpoint, CancellationToken cancellationToken = default)
        {
            return await PostDataAsync<List<GetCustomOperationResponse>>(endpoint, cancellationToken);
        }

        public async Task<List<GetCustomOperationResponse>> GetCustomOperationsAsync(string endpoint, GetCustomOperetionsRequest request, CancellationToken cancellationToken = default)
        {
            return await PostDataAsync<List<GetCustomOperationResponse>>(endpoint, request, cancellationToken);
        }

        private async Task<T> PostDataAsync<T>(string endpoint, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, new { }, cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>(_jsonOptions, cancellationToken)
                ?? throw new InvalidOperationException("Failed to deserialize response");
        }

        private async Task<T> PostDataAsync<T>(string endpoint, object requestBody, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, requestBody, cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>(_jsonOptions, cancellationToken)
                ?? throw new InvalidOperationException("Failed to deserialize response");
        }
    }
}
