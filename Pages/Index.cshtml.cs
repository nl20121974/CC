using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CC.Data;

namespace CC.Pages
{
    public class IndexModel : PageModel
    {
        private readonly CC.Data.DataContext _context;

        public IndexModel(CC.Data.DataContext context)
        {
            _context = context;
        }

        public IList<UserGroup> UserGroup { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.UserGroups != null)
            {
                UserGroup = await _context.UserGroups
                .Include(u => u.Group)
                .Include(u => u.User).ToListAsync();
            }
        }
    }
}
