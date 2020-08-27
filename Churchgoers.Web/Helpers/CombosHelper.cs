using Churchgoers.Common.Entities;
using Churchgoers.Web.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Churchgoers.Web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetComboChurches(int districtId)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            District district = _context.Districts
                .Include(d => d.Churches)
                .FirstOrDefault(d => d.Id == districtId);
            if (district != null)
            {
                list = district.Churches.Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = $"{t.Id}"
                })
                    .OrderBy(t => t.Text)
                    .ToList();
            }

            list.Insert(0, new SelectListItem
            {
                Text = "[Select a church...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboDistricts(int fieldId)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            Field field = _context.Fields
                .Include(f => f.Districts)
                .FirstOrDefault(f => f.Id == fieldId);
            if (field != null)
            {
                list = field.Districts.Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = $"{t.Id}"
                })
                    .OrderBy(t => t.Text)
                    .ToList();
            }

            list.Insert(0, new SelectListItem
            {
                Text = "[Select a district...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboFields()
        {
            List<SelectListItem> list = _context.Fields.Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = $"{t.Id}"
            })
                .OrderBy(t => t.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select a field...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboProfessions()
        {
            List<SelectListItem> list = _context.Professions.Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = $"{t.Id}"
            })
                .OrderBy(t => t.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select a profession...]",
                Value = "0"
            });

            return list;
        }
    }
}
