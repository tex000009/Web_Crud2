using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Web_Crud2.Models;

namespace Web_Crud2.Controllers
{
    public class CrudMvcController : Controller
    {
        // GET: CrudMvc
        HttpClient client = new HttpClient();
        public ActionResult Index()
        {
            List<Employee> emp_list = new List<Employee>();
            client.BaseAddress = new Uri("https://localhost:44397/api/CrudApi");
            var response = client.GetAsync("CrudApi");
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<List<Employee>>();
                display.Wait();
                emp_list = display.Result;
            }
            if (emp_list == null || emp_list.Count == 0)
            {
                ViewBag.Message = "No data found.";
            }
            return View(emp_list);
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Employee emp)
        {
            client.BaseAddress = new Uri("https://localhost:44397/api/CrudApi");
            var response = client.PostAsJsonAsync<Employee>("CrudApi", emp);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View("Create");
        }

        public ActionResult Edit(int id)
        {
            Employee emp = null;
            client.BaseAddress = new Uri("https://localhost:44397/api/CrudApi");
            var response = client.GetAsync("CrudApi?id=" + id.ToString());
            response.Wait();

            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<List<Employee>>();
                display.Wait();
                emp = display.Result.FirstOrDefault(e => e.id == id);
            }
            return View(emp);
        }

        [HttpPost]
        public ActionResult Edit(Employee e)
        {
            client.BaseAddress = new Uri("https://localhost:44397/api/CrudApi");
            var response = client.PutAsJsonAsync<Employee>("CrudApi", e);
            response.Wait();

            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View("Edit");
        }

        public ActionResult Delete(int id)
        {
            client.BaseAddress = new Uri("https://localhost:44397/api/CrudApi");
            var response = client.GetAsync("CrudApi/" + id);
            response.Wait();

            var result = response.Result;
            if (result.IsSuccessStatusCode)
            {
                var jsonContent = result.Content.ReadAsStringAsync().Result;
                var e = JsonConvert.DeserializeObject<List<Employee>>(jsonContent);
                return View(e[0]);
            }
            return View("Index");
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            client.BaseAddress = new Uri("https://localhost:44397/api/CrudApi");
            var response = client.DeleteAsync("CrudApi/" + id);
            response.Wait();

            var result = response.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("DeleteConfirmed");
            }
            return View("Index");
        }
    }
}
