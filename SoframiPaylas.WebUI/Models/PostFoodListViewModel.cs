namespace SoframiPaylas.WebUI.Models
{
    public class PostFoodListViewModel
    {
        public string PostId { get; set; }
        public string PostName { get; set; }
        public List<FoodViewModel> FoodList { get; set; }
    }
}