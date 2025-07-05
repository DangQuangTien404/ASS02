namespace ServiceLayer.DTOs
{
    public class CreateCategoryDto
    {
        public required string CategoryName { get; set; }
        public required string CategoryDesciption { get; set; }
        public short? ParentCategoryId { get; set; }
        public bool? IsActive { get; set; } = true;
    }
}
