using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using GreenCartFarmers.Models;
using Newtonsoft.Json;

public class AuthController : Controller
{
    private readonly string baseUrl = "https://localhost:7234/api/Auth";

    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginModel model)
    {
        using (var client = new HttpClient())
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{baseUrl}/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject(responseString);
                string token = result.token;
                string role = result.role;

                Session["JWT"] = token;
                Session["Role"] = role;

                if (role == "Farmer")
                    return RedirectToAction("FarmerDashboard", "Dashboard");
                else
                    return RedirectToAction("BuyerDashboard", "Dashboard");
            }

            ViewBag.Error = "Invalid credentials";
            return View(model);
        }
    }

    public ActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Register(RegisterModel model)
    {
        using (var client = new HttpClient())
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{baseUrl}/register", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }

            ViewBag.Error = "Registration failed";
            return View(model);
        }
    }
    public ActionResult Logout()
    {
        Session.Clear();  // Clear all session data like JWT token, role, etc.
        Session.Abandon();

        return RedirectToAction("Index", "Home");
    }

}
