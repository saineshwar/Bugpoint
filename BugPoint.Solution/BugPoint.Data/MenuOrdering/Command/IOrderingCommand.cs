using System.Collections.Generic;
using BugPoint.ViewModel.MenuMaster;

namespace BugPoint.Data.MenuOrdering.Command
{
    public interface IOrderingCommand
    {
        void UpdateMenuCategoryOrder(List<MenuCategoryStoringOrder> menuCategorylist);
        void UpdateMenuOrder(List<MenuStoringOrder> menuStoringOrder);
    }
}