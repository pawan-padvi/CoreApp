using App.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static App.Models.DropdownModel;

namespace App.Interfaces
{
    public interface IDropdownService
    {
        public List<PageSizeDdl> GetPageSizeDdl();
        public List<Unit> GetUnits ( );
        public List<ProductType> GetProductTypes ( );
    }
}
