using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CC.Data;

namespace CC.Pages
{
    public class CreateModel : PageModel
    {
        private readonly CC.Data.DataContext _context;

        public CreateModel(CC.Data.DataContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Code");
        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public UserGroup UserGroup { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.UserGroups == null || UserGroup == null)
            {
                return Page();
            }

            _context.UserGroups.Add(UserGroup);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
