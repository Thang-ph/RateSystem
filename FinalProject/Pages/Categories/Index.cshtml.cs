using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FinalProject.Data;
using FinalProject.Models;
using Newtonsoft.Json;

namespace FinalProject.Pages.Categories
{
    public class IndexModel : PageModel
    {

        public IndexModel()
        {
        }

        public IList<Category> Category { get;set; } = default!;

        string url = "https://localhost:7229/api/Category";
        public async Task OnGetAsync()
        {
            using (HttpClient client = new HttpClient()) { 
            using(HttpResponseMessage res=await client.GetAsync(url))
                {
                    using (HttpContent content = res.Content)
                    {
                        string result=content.ReadAsStringAsync().Result;
                        Category = JsonConvert.DeserializeObject<List<Category>>(result);
                    }

                }
            }
        }
    }
}
