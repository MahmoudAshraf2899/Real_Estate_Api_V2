namespace Real_Estate_Dtos.DTO
{
    public class DtoGetRolePermissionsList
    {
        public int id { get; set; }
        public int? permissionId { get; set; }
        public int? roleId { get; set; }
        public int? code { get; set; }
        public string permissionTitle { get; set; }
        public string roleTitle { get; set; }
        public bool? isActive { get; set; }
    }
}
