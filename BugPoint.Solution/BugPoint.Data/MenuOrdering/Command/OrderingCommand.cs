using BugPoint.ViewModel.MenuMaster;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BugPoint.Data.MenuOrdering.Command
{
    public class OrderingCommand : IOrderingCommand
    {
        private readonly IConfiguration _configuration;
        public OrderingCommand(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void UpdateMenuCategoryOrder(List<MenuCategoryStoringOrder> menuCategorylist)
        {
            using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            using var transaction = connection.BeginTransaction();
            try
            {
               
                foreach (var menuCategory in menuCategorylist)
                {
                    var param = new DynamicParameters();
                    param.Add("@MenuCategoryId", menuCategory.MenuCategoryId);
                    param.Add("@RoleId", menuCategory.RoleId);
                    param.Add("@SortingOrder", menuCategory.SortingOrder);
                    connection.Execute("Usp_UpdateMenuCategoryOrder", param, transaction, 0, CommandType.StoredProcedure);
                }
                transaction.Commit();
            }
            catch (System.Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public void UpdateMenuOrder(List<MenuStoringOrder> menuStoringOrder)
        {
            using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            using var transaction = connection.BeginTransaction();
            try
            {
              
                foreach (var menu in menuStoringOrder)
                {
                    var param = new DynamicParameters();
                    param.Add("@MenuId", menu.MenuId);
                    param.Add("@RoleId", menu.RoleId);
                    param.Add("@SortingOrder", menu.SortingOrder);
                    connection.Execute("Usp_UpdateMenuOrder", param, transaction, 0, CommandType.StoredProcedure);
                }
                transaction.Commit();
            }
            catch (System.Exception)
            {
                transaction.Rollback();
                throw;
            }
        }


    }
}