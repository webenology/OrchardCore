using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Security.Services;

namespace OrchardCore.Roles.ViewComponents
{
    public class SelectRolesViewComponent : ViewComponent
    {
        private readonly IRoleService _roleService;

        public SelectRolesViewComponent(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<string> selectedRoles, string htmlName, IEnumerable<string> except = null)
        {
            if (selectedRoles == null)
            {
                selectedRoles = new string[0];
            }

            if (except != null)
            {
                selectedRoles = selectedRoles.Except(except);
            }

            var roleSelections = await BuildRoleSelectionsAsync(selectedRoles);

            var model = new SelectRolesViewModel
            {
                HtmlName = htmlName,
                RoleSelections = roleSelections
            };

            return View(model);
        }

        private async Task<IList<Selection<string>>> BuildRoleSelectionsAsync(IEnumerable<string> selectedRoles)
        {
            var roleNames = await _roleService.GetRoleNamesAsync();
            return roleNames.Select(x => new Selection<string>
            {
                IsSelected = selectedRoles.Contains(x),
                Item = x
            })
            .OrderBy(x => x.Item)
            .ToList();
        }
    }

    public class SelectRolesViewModel
    {
        public string HtmlName { get; set; }
        public IList<Selection<string>> RoleSelections { get; set; }
    }

    public class Selection<T>
    {
        public bool IsSelected { get; set; }
        public T Item { get; set; }
    }
}
