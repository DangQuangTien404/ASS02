namespace ServiceLayer.DTOs
{
    public class UpdateCategoryDto
    {
        public short CategoryId { get; set; }
        public required string CategoryName { get; set; }
        public required string CategoryDesciption { get; set; }
        public short? ParentCategoryId { get; set; }
        public bool? IsActive { get; set; }
    }
}
