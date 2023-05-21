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
    public class DeleteModel : PageModel
    {
        private readonly CC.Data.DataContext _context;

        public DeleteModel(CC.Data.DataContext context)
        {
            _context = context;
        }

        [BindProperty]
      public UserGroup UserGroup { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.UserGroups == null)
            {
                return NotFound();
            }

            var usergroup = await _context.UserGroups.FirstOrDefaultAsync(m => m.Id == id);

            if (usergroup == null)
            {
                return NotFound();
            }
            else 
            {
                UserGroup = usergroup;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.UserGroups == null)
            {
                return NotFound();
            }
            var usergroup = await _context.UserGroups.FindAsync(id);

            if (usergroup != null)
            {
                UserGroup = usergroup;
                _context.UserGroups.Remove(UserGroup);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
