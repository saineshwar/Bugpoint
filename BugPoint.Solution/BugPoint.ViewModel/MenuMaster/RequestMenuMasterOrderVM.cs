namespace BugPoint.ViewModel.MenuMaster
{
    public class RequestMenuMasterOrderVM
    {
        public int[] SelectedOrder { get; set; }
        public int RoleId { get; set; }
    }
    public class RequestMenu
    {
        public int RoleId { get; set; }
        public int MenuCategoryId { get; set; }
    }
}