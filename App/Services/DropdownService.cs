using App.Common;
using App.Interfaces;
using App.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static App.Models.DropdownModel;

namespace App.Services
{
    public class DropdownService : IDropdownService
    {
        private readonly IDapperService _dapper;
        private List<SelectListItem> listItems;
        public DropdownService ( IDapperService dapper )
        {
            _dapper = dapper;
        }
        #region Commom
        List<PageSizeDdl> IDropdownService.GetPageSizeDdl ( )
        {
            return _dapper.GetAll<PageSizeDdl> ( CommonSp.getAllPageSizeDdl, null, commandType: CommandType.StoredProcedure );
        }

        List<ProductType> IDropdownService.GetProductTypes ( )
        {
            return _dapper.GetAll<ProductType> ( CommonSp.getProductType, null, commandType: CommandType.StoredProcedure );
        }

        List<Unit> IDropdownService.GetUnits ( )
        {
            return _dapper.GetAll<Unit> ( CommonSp.getUnit, null, commandType: CommandType.StoredProcedure );
        }
        #endregion

    }
}