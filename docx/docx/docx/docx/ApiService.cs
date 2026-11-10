using RaceDay.MVC.Models;
using System.Text;
using System.Text.Json;

namespace RaceDay.MVC.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public ApiService(HttpClient http)
        {
            _http = http;
            _http.BaseAddress = new Uri("https://localhost:7002/");
        }

        // ---- USERS ----
        public async Task<List<Users>> GetUsers()
        {
            return new List<Users>();
        }

        public async Task<bool> CreateUser(Users user)
        {
            return await Register(user);
        }

        // ---- EVENTS ----
        public async Task<List<events>> GetEvents()
        {
            var res = await _http.GetAsync("api/Event");
            if (!res.IsSuccessStatusCode) return new List<events>();
            var json = await res.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json) || !json.TrimStart().StartsWith("["))
                return new List<events>();
            return JsonSerializer.Deserialize<List<events>>(json, _options) ?? new List<events>();
        }

        public async Task<events> GetEvent(int id)
        {
            try
            {
                var response = await _http.GetAsync($"api/Event/{id}");
                if (!response.IsSuccessStatusCode) return null;
                var json = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(json) || !json.TrimStart().StartsWith("{")) return null;
                return JsonSerializer.Deserialize<events>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> CreateEvent(events ev)
        {
            int userId = ev.userID != 0 ? ev.userID : 1;
            string eventDateStr = ev.eventDate ?? "";

            var url = $"api/Event/addEvent?userID={userId}" +
                      $"&name={Uri.EscapeDataString(ev.name ?? "")}" +
                      $"&description={Uri.EscapeDataString(ev.description ?? "")}" +
                      $"&eventDate={Uri.EscapeDataString(eventDateStr)}" +
                      $"&location={Uri.EscapeDataString(ev.location ?? "")}" +
                      $"&eventType={Uri.EscapeDataString(ev.eventType ?? "")}";

            var res = await _http.PostAsync(url, null);
            return res.IsSuccessStatusCode;
        }

        // ---- LOGIN ----
        public async Task<Users?> Login(string username, string password)
        {
            try
            {
                var url = $"api/User/loginUser/{Uri.EscapeDataString(username)}/{Uri.EscapeDataString(password)}";
                var res = await _http.GetAsync(url);
                if (!res.IsSuccessStatusCode) return null;
                var text = await res.Content.ReadAsStringAsync();
                if (!text.Contains("Login successefull")) return null;

                var user = new Users { email = username, password = password };
                try
                {
                    var idPart = text.Split("UserID:")[1].Split("Name:")[0].Trim();
                    user.userID = int.Parse(idPart);
                    var namePart = text.Split("Name:")[1].Split("Role:")[0].Trim();
                    user.fullName = namePart;
                    var rolePart = text.Split("Role:")[1].Trim();
                    user.role = rolePart;
                }
                catch { }
                return user;
            }
            catch
            {
                return null;
            }
        }

        // ---- REGISTER ----
        public async Task<bool> Register(Users user)
        {
            try
            {
                var url = $"api/User?fullName={Uri.EscapeDataString(user.fullName ?? "")}" +
                          $"&email={Uri.EscapeDataString(user.email ?? "")}" +
                          $"&password={Uri.EscapeDataString(user.password ?? "")}" +
                          $"&role={Uri.EscapeDataString(user.role ?? "participant")}";

                var response = await _http.PostAsync(url, null);
                if (!response.IsSuccessStatusCode) return false;
                var text = await response.Content.ReadAsStringAsync();
                return text.Contains("User Added");
            }
            catch
            {
                return false;
            }
        }

        // ---- EDIT ----
        public async Task<bool> UpdateEvent(int id, events ev)
        {
            int userId = ev.userID != 0 ? ev.userID : 1;
            string eventDateStr = ev.eventDate ?? "";

            var url = $"api/Event/updateEvent/{id}?userID={userId}" +
                      $"&name={Uri.EscapeDataString(ev.name ?? "")}" +
                      $"&description={Uri.EscapeDataString(ev.description ?? "")}" +
                      $"&eventDate={Uri.EscapeDataString(eventDateStr)}" +
                      $"&location={Uri.EscapeDataString(ev.location ?? "")}" +
                      $"&eventType={Uri.EscapeDataString(ev.eventType ?? "")}";

            var res = await _http.PutAsync(url, null);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteEvent(int id)
        {
            var res = await _http.DeleteAsync($"api/Event/deleteEvent/{id}");
            return res.IsSuccessStatusCode;
        }

        // ---- PARTICIPANT ----
        private static List<Enrolment> _enrolments = new();
        private static List<Result> _results = new()
        {
            new Result { EventId = 1, UserId = 2, EventName = "Demo Race", FinishTime = "01:12:30", Position = 3 }
        };

        public Task<List<events>> GetAllEventsAsync() => GetEvents().ContinueWith(t => t.Result);
        public Task<events> GetEventByIdAsync(int id) => GetEvent(id);
        public Task<Users> GetUserByIdAsync(int id) => Task.FromResult(new Users { userID = id });

        public Task<bool> EnterEventAsync(int eventId, int userId)
        {
            if (_enrolments.Any(e => e.EventId == eventId && e.UserId == userId))
                return Task.FromResult(false);

            var ev = GetEvent(eventId).Result;
            _enrolments.Add(new Enrolment
            {
                EventId = eventId,
                UserId = userId,
                EventName = ev?.name ?? $"Event {eventId}",
                EventDate = ev?.eventDate ?? "",
                Location = ev?.location ?? ""
            });
            return Task.FromResult(true);
        }

        public async Task<List<events>> GetMyEventsAsync(int userId)
        {
            var all = await GetEvents();
            var myIds = _enrolments.Where(e => e.UserId == userId).Select(e => e.EventId).ToList();
            return all.Where(e => myIds.Contains(e.eventID)).ToList();
        }

        public Task<List<Result>> GetMyResultsAsync(int userId)
        {
            return Task.FromResult(_results.Where(r => r.UserId == userId).ToList());
        }

        public Task<bool> AddResultAsync(Result r)
        {
            _results.Add(r);
            return Task.FromResult(true);
        }

        public Task<List<Enrolment>> GetEnrolmentsForEventAsync(int eventId)
        {
            return Task.FromResult(_enrolments.Where(e => e.EventId == eventId).ToList());
        }
    }
}
