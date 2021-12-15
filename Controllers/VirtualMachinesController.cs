using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VMApi.Model;

namespace VMApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VirtualMachinesController : ControllerBase
    {
        private static readonly HttpClient Client = new HttpClient();
        private static readonly string Token = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        public VirtualMachinesController()
        {
            Initialize();
        }

        [HttpGet]
        public IEnumerable<VirtualMachine> Get()
        {
            var vms = GetVMs().Result;
            return vms;
        }

        [HttpGet("{status}")]
        public IEnumerable<VirtualMachine> Get(string status)
        {
            var vms = GetVMsByStatus(status).Result;
            return vms;
        }

        private static async Task<IEnumerable<VirtualMachine>> GetVMs()
        {
            var vmsString = await Client.GetStringAsync("https://mock-api-mz3ro.ondigitalocean.app/virtualmachines");
            var vms = JsonSerializer.Deserialize<IEnumerable<VirtualMachine>>(vmsString, Options).Where(x => x != null);
            return VirtualMachine.PrepareVMsData(vms);
        }

        private static async Task<IEnumerable<VirtualMachine>> GetVMsByStatus(string status)
        {
            var vmsString = await Client.GetStringAsync("https://mock-api-mz3ro.ondigitalocean.app/virtualmachines");
            var vms = JsonSerializer.Deserialize<IEnumerable<VirtualMachine>>(vmsString, Options)
                .Where(x => x != null && x.Status == status);
            return VirtualMachine.PrepareVMsData(vms);
        }

        private static void Initialize()
        {
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client.DefaultRequestHeaders.Add("Authorization", Token);
        }
    }
}
