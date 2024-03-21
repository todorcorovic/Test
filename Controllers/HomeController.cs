using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Test.Models;
using System.Web.Helpers;
using Newtonsoft.Json.Converters;

namespace Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly string apiUrl = "https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code=vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==";

        public async Task<ActionResult> Index()
        {
            List<Worklog> workLogs = await GetWorklogsFromAPI();

            if (workLogs == null)
            {
                return View();
            }

            List<Employee> employees = CalculateTotalTimeWorked(workLogs);
            employees = employees.OrderByDescending(e => e.TotalTimeWorked).ToList();

            return View(employees);
        }

        private async Task<List<Worklog>> GetWorklogsFromAPI()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        List<Worklog> workLogs = JsonConvert.DeserializeObject<List<Worklog>>(json);
                        return workLogs;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.Message;
                return null;
            }
        }

        private List<Employee> CalculateTotalTimeWorked(List<Worklog> workLogs)
        {
            var groupedEntries = workLogs.GroupBy(wl => wl.EmployeeName);

            List<Employee> employees = new List<Employee>();

            foreach (var group in groupedEntries)
            {
                Employee employee = new Employee();
                employee.Name = group.Key;
                employee.Worklogs = group.ToList();
                employee.TotalTimeWorked = group.Sum(wl => (wl.EndTimeUtc - wl.StarTimeUtc).TotalHours);
                employee.TotalTimeWorked = Math.Round(employee.TotalTimeWorked, 2);
                employees.Add(employee);
            }

            return employees;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}