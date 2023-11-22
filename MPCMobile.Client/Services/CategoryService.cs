using MPCMobile.Client.Interfaces;
using MPCMobile.Client.Models;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MPCMobile.Client.Services
{
    public class CategoryService : ICategorySerice<CategoryModel>
    {
        public CategoryService() { }

        public async Task<List<CategoryModel>> GetAllAsync()
        {
            var list = new List<CategoryModel>();

            // dummy data
            for (int i = 0; i < 5; i++)
                list.Add(new CategoryModel()
                {
                    Id = Guid.NewGuid(),
                    CategoryName = $"Category {(i + 1)}"
                });

            await Task.CompletedTask;

            return list;
        }
    }
}
