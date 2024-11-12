using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FinalProject.Models;
using Newtonsoft.Json;

namespace FinalProject.Pages.Products
{
    public class IndexModel : PageModel
    {

        public IndexModel()
        {
        }
        string url = "https://localhost:7229/api/Product";
        public IList<Product> Product { get;set; } = default!;

        public async Task OnGetAsync()
        {
            using (HttpClient client = new HttpClient()) {
                using (HttpResponseMessage res = await client.GetAsync(url))
                {
                    using (HttpContent content = res.Content)
                    {
                        string result = content.ReadAsStringAsync().Result;
                        Product = JsonConvert.DeserializeObject<List<Product>>(result);
                    }
                }
            }
                }
    }
}
