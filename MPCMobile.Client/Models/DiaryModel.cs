using System;
using System.Collections.Generic;

namespace MPCMobile.Client.Models
{
    public class DiaryModel
    {
        public Guid DiaryId { get; set; }
        public List<ImageModel> Images { get; set; }
        public string Comments { get; set; }
        public DateTime Date { get; set; }
        public AreaModel SelectedArea { get; set; }
        public CategoryModel SelectedCategory { get; set; }
        public List<string> Tags { get; set; }
        public EventModel SelectedEvent { get; set; }
    }
}
