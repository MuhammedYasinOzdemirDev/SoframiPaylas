using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SoframiPaylas.Application.DTOs.Event;
using SoframiPaylas.WebUI.Models;
using SoframiPaylas.WebUI.Services.Interfaces;

namespace SoframiPaylas.WebUI.Services
{
    public class EventApiService : IEventApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public EventApiService(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _httpClient = httpClientFactory.CreateClient("API");
            _mapper = mapper;
        }

        public async Task<List<EventViewModel>> GetAllEventsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("Event/events");
                response.EnsureSuccessStatusCode(); // HTTP yanıtını kontrol et

                // API'den IEnumerable olarak gelen veriyi List'e çevir
                IEnumerable<EventDto> eventDtoEnumerable = await response.Content.ReadFromJsonAsync<IEnumerable<EventDto>>();
                List<EventDto> eventDtoList = eventDtoEnumerable.ToList();

                Console.WriteLine(eventDtoList.Count);
                return _mapper.Map<List<EventViewModel>>(eventDtoList);
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException($"An error occurred when retrieving events: {ex.Message}", ex);
            }
        }

    }
}