﻿using SportsStore.Domain.Abstract;
using System.Linq;
using SportsStore.Domain.Entities;
using System.Data.Entity;
using System;

namespace SportsStore.Domain.Concrete {
    public class EFProductRepository : IProductRepository {
        private EFDbContext context = new EFDbContext();

        public IQueryable<Product> Products {
            get {
                return context.Products;
            }
        }

        public void SaveProduct(Product product) {
            if (product.ProductID == 0) {
                context.Products.Add(product);
            } else {
                context.Entry(product).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteProduct(Product product) {
            context.Products.Remove(product);
            context.SaveChanges(); 
        }

    }
}