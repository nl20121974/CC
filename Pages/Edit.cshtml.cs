using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CC.Data;

namespace CC.Pages
{
    public class EditModel : PageModel
    {
        private readonly CC.Data.DataContext _context;

        public EditModel(CC.Data.DataContext context)
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

            var usergroup =  await _context.UserGroups.FirstOrDefaultAsync(m => m.Id == id);
            if (usergroup == null)
            {
                return NotFound();
            }
            UserGroup = usergroup;
           ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Code");
           ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(UserGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserGroupExists(UserGroup.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool UserGroupExists(int id)
        {
          return (_context.UserGroups?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
