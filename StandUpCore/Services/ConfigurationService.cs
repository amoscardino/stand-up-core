using Blazor.Extensions.Storage;
using StandUpCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace StandUpCore.Services
{
    public class ConfigurationService
    {
        private const string FILE_NAME = "appsettings.json";

        private HttpClient _http;
        private Dictionary<string, string> _values;

        public ConfigurationService(HttpClient http)
        {
            _http = http;
        }

        public async Task<string> GetValueAsync(string key)
        {
            if (_values == null)
                await GetValuesAsync();

            return _values[key];
        }

        private async Task GetValuesAsync()
        {
            _values = await _http.GetJsonAsync<Dictionary<string, string>>(FILE_NAME);

            if (_values == null)
                throw new Exception("Missing settings data");
        }
    }
}
