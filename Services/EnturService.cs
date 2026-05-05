using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TransitInsight.Models;

namespace TransitInsight.Services;

public class EnturService
{
    private readonly HttpClient _httpClient;

    public EnturService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("ET-Client-Name", "reza-transitinsight");
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<List<Departure>> GetDeparturesAsync(int stopPlaceId, string enturId)
    {
        const string url = "https://api.entur.io/journey-planner/v3/graphql";

        var query = """
        query($id: String!) {
          stopPlace(id: $id) {
            estimatedCalls(timeRange: 72100, numberOfDepartures: 12) {
              aimedDepartureTime
              expectedDepartureTime
              destinationDisplay {
                frontText
              }
              serviceJourney {
                line {
                  publicCode
                  transportMode
                }
              }
            }
          }
        }
        """;

        var requestBody = new
        {
            query,
            variables = new { id = enturId }
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        var responseText = await response.Content.ReadAsStringAsync();

        using var document = JsonDocument.Parse(responseText);

        var departures = new List<Departure>();

        var stopPlace = document.RootElement
            .GetProperty("data")
            .GetProperty("stopPlace");

        if (stopPlace.ValueKind == JsonValueKind.Null)
        {
            return departures;
        }

        var calls = stopPlace.GetProperty("estimatedCalls");

        foreach (var call in calls.EnumerateArray())
        {
            var aimed = call.GetProperty("aimedDepartureTime").GetDateTime();
            var expected = call.GetProperty("expectedDepartureTime").GetDateTime();

            var delayMinutes = (int)Math.Max(0, (expected - aimed).TotalMinutes);

            var line = call
                .GetProperty("serviceJourney")
                .GetProperty("line");

            var publicCode = line.GetProperty("publicCode").GetString() ?? "Unknown";
            var transportMode = line.GetProperty("transportMode").GetString() ?? "Unknown";

            var destination = call
                .GetProperty("destinationDisplay")
                .GetProperty("frontText")
                .GetString() ?? "Unknown";

            departures.Add(new Departure
            {
                StopPlaceId = stopPlaceId,
                LineName = publicCode,
                Destination = destination,
                AimedDepartureTime = aimed,
                ExpectedDepartureTime = expected,
                DelayMinutes = delayMinutes,
                TransportMode = transportMode
            });
        }

        return departures;
    }

    public async Task<List<EnturStopSearchResult>> SearchStopPlacesAsync(string searchText)
    {
        var url =
            $"https://api.entur.io/geocoder/v1/autocomplete?text={Uri.EscapeDataString(searchText)}&lang=no&size=10";

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("ET-Client-Name", "reza-transitinsight");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        using var document = JsonDocument.Parse(json);

        var results = new List<EnturStopSearchResult>();

        var features = document.RootElement.GetProperty("features");

        foreach (var feature in features.EnumerateArray())
        {
            var properties = feature.GetProperty("properties");

            var id = properties.TryGetProperty("id", out var idValue)
                ? idValue.GetString()
                : null;

            var name = properties.TryGetProperty("name", out var nameValue)
                ? nameValue.GetString()
                : null;

            var locality = properties.TryGetProperty("locality", out var localityValue)
                ? localityValue.GetString()
                : null;

            if (string.IsNullOrWhiteSpace(id) || !id.StartsWith("NSR:StopPlace"))
            {
                continue;
            }

            double? longitude = null;
            double? latitude = null;

            if (feature.TryGetProperty("geometry", out var geometry) &&
                geometry.TryGetProperty("coordinates", out var coordinates) &&
                coordinates.GetArrayLength() >= 2)
            {
                longitude = coordinates[0].GetDouble();
                latitude = coordinates[1].GetDouble();
            }

            results.Add(new EnturStopSearchResult
            {
                EnturId = id,
                Name = name ?? "Unknown stop",
                Locality = locality,
                Latitude = latitude,
                Longitude = longitude
            });
        }

        return results;
    }
    public async Task<List<EnturStopSearchResult>> GetNearbyStopsAsync(double latitude, double longitude)
{
    var url =
        $"https://api.entur.io/geocoder/v1/reverse?point.lat={latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}&point.lon={longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}&boundary.circle.radius=1.5&size=15&lang=no";

    using var request = new HttpRequestMessage(HttpMethod.Get, url);
    request.Headers.Add("ET-Client-Name", "reza-transitinsight");

    var response = await _httpClient.SendAsync(request);
    response.EnsureSuccessStatusCode();

    var json = await response.Content.ReadAsStringAsync();

    using var document = JsonDocument.Parse(json);

    var results = new List<EnturStopSearchResult>();

    if (!document.RootElement.TryGetProperty("features", out var features))
    {
        return results;
    }

    foreach (var feature in features.EnumerateArray())
    {
        var properties = feature.GetProperty("properties");

        var id = properties.TryGetProperty("id", out var idValue)
            ? idValue.GetString()
            : null;

        var name = properties.TryGetProperty("name", out var nameValue)
            ? nameValue.GetString()
            : null;

        var locality = properties.TryGetProperty("locality", out var localityValue)
            ? localityValue.GetString()
            : null;

        if (string.IsNullOrWhiteSpace(id) || !id.StartsWith("NSR:StopPlace"))
        {
            continue;
        }

        double? resultLongitude = null;
        double? resultLatitude = null;

        if (feature.TryGetProperty("geometry", out var geometry) &&
            geometry.TryGetProperty("coordinates", out var coordinates) &&
            coordinates.GetArrayLength() >= 2)
        {
            resultLongitude = coordinates[0].GetDouble();
            resultLatitude = coordinates[1].GetDouble();
        }

        results.Add(new EnturStopSearchResult
        {
            EnturId = id,
            Name = name ?? "Unknown stop",
            Locality = locality,
            Latitude = resultLatitude,
            Longitude = resultLongitude
        });
    }

    return results
        .GroupBy(s => s.EnturId)
        .Select(g => g.First())
        .ToList();
}
}