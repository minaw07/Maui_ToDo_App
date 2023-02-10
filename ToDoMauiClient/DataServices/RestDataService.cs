using System.Text;
using System.Text.Json;
using ToDoMauiClient.Models;
using Debug = System.Diagnostics.Debug;

namespace ToDoMauiClient.DataServices
{
    public class RestDataService : IRestDataService
    {
        private readonly HttpClient _HttpClient;
        private readonly string _BaseAddress;
        private readonly string _Url;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public RestDataService()
        {
            _HttpClient = new HttpClient();
            _BaseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5272" : "https://localhost:7077";
            _Url = $"{_BaseAddress}/api";

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

        }
        public async Task AddToDoAsync(ToDo todo)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("-----> No internet access");
                return;
            }
            try
            {
                string jsonToDo = JsonSerializer.Serialize<ToDo>(todo,_jsonSerializerOptions);
                StringContent content = new StringContent(jsonToDo, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _HttpClient.PostAsync($"{_Url}/todos", content);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("ToDo created successfully");
                }
                else
                {
                    Debug.WriteLine("-----> Non Http 2xx response");
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Whoops exception :{ex.Message}");
            }
            return;
        }

        public async Task<List<ToDo>> GetAllToDosAsync()
        {
            List<ToDo> todos = new List<ToDo>();
            if(Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("-----> No internet access");
                return todos;
            }
            try
            {
                HttpResponseMessage response = await _HttpClient.GetAsync($"{_Url}/todos");
                if(response.IsSuccessStatusCode)
                {
                    string context = await response.Content.ReadAsStringAsync();
                    todos = JsonSerializer.Deserialize<List<ToDo>>(context, _jsonSerializerOptions);
                }
                else
                {
                    Debug.WriteLine("-----> Non Http 2xx response");
                }
            }catch(Exception ex) 
            {
                Debug.WriteLine($"Whoops exception :{ ex.Message}");
            }
            return todos;
        }

        public async Task RemoveToDoAsync(int id)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("-----> No internet access");
                return;
            }
            try
            {

                HttpResponseMessage response = await _HttpClient.DeleteAsync($"{_Url}/todos/{id}");
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("ToDo Deleted successfully");
                }
                else
                {
                    Debug.WriteLine("-----> ToDo is not deleted successfully");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Whoops exception :{ex.Message}");
            }
            return;
        }

        public async Task UpdateToDoAsync(ToDo todo)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("-----> No internet access");
                return;
            }
            try
            {
                string jsonToDo = JsonSerializer.Serialize<ToDo>(todo, _jsonSerializerOptions);
                StringContent content = new StringContent(jsonToDo, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _HttpClient.PutAsync($"{_Url}/todos/{todo.Id}", content);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("ToDo Updated successfully");
                }
                else
                {
                    Debug.WriteLine("-----> Non Http 2xx response");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Whoops exception :{ex.Message}");
            }
            return;
        }
    }
}
