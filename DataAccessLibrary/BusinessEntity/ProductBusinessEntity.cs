using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using GsmsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DataAccessLibrary.BusinessEntity
{
    public class ProductBusinessEntity
    {
        private IUnitOfWork work;
        public ProductBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            IEnumerable<Product> products = await work.Products.GetAllAsync();
            products = from product in products
                       where product.IsDeleted == false
                       select product;
            return products;
        }

        public async Task<Product> GetProductAsync(string id)
        {
            Product product = await work.Products.GetAsync(id);
            if (product != null && product.IsDeleted == true)
            {
                return null;
            }
            return product;
        }

        public async Task<IEnumerable<Product>> GetProductsByMasterProductAsync(string masterProductId)
        {
            IEnumerable<Product> products = await work.Products.GetAllAsync();
            products = from product in products
                       where !string.IsNullOrEmpty(product.MasterProductId) && product.MasterProductId.Equals(masterProductId) && product.IsDeleted == false
                       select product;
            return products;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryId)
        {
            IEnumerable<Product> products = await work.Products.GetAllAsync();
            products = from product in products
                       where product.CategoryId.Equals(categoryId) && product.IsDeleted == false
                       select product;
            return products;
        }

        public async Task<Product> AddProductAsync(Product newProduct)
        {
            await CheckProduct(newProduct);
            newProduct.Id = GsmsUtils.CreateGuiId();
            newProduct.IsDeleted = false;
            if (newProduct.ProductDetails.Any())
            {

                foreach (ProductDetail productDetail in newProduct.ProductDetails)
                {
                    if (productDetail.Price == null || productDetail.Price < 0)
                    {
                        throw new Exception("Product Price must be a positive decimal number!!");
                    }
                    if (productDetail.StoredQuantity == null || productDetail.StoredQuantity < 0)
                    {
                        throw new Exception("Stored Quantity must be a positive integer!!");
                    }
                }
            }
            await work.Products.AddAsync(newProduct);
            work.Save();
            return newProduct;
        }

        public async Task<Product> UpdateProductAsync(Product updatedProduct)
        {
            Product product = await work.Products.GetAsync(updatedProduct.Id);
            if (product == null || product.IsDeleted == true)
            {
                throw new Exception("Product is not existed!!");
            }
            await CheckProduct(updatedProduct);
            product.Name = updatedProduct.Name;
            product.AtomicPrice = updatedProduct.AtomicPrice;
            product.MasterProductId = updatedProduct.MasterProductId;
            product.CategoryId = updatedProduct.CategoryId;
            product.IsDeleted = updatedProduct.IsDeleted;
            if (updatedProduct.ProductDetails.Any())
            {
                foreach (ProductDetail productDetail in updatedProduct.ProductDetails)
                {
                    if (productDetail.Price == null || productDetail.Price < 0)
                    {
                        throw new Exception("Product Price must be a positive decimal number!!");
                    }
                    if (productDetail.StoredQuantity == null || productDetail.StoredQuantity < 0)
                    {
                        throw new Exception("Stored Quantity must be a positive integer!!");
                    }
                }
                product.ProductDetails = updatedProduct.ProductDetails;
            }
            work.Products.Update(product);
            work.Save();
            return product;
        }

        private async Task CheckProduct(Product product)
        {
            Category category = await work.Categories.GetAsync(product.CategoryId);
            if (category == null)
            {
                throw new Exception("Category is not existed!!");
            }
            if (!string.IsNullOrEmpty(product.MasterProductId))
            {
                Product masterProduct = await work.Products.GetAsync(product.MasterProductId);
                if (masterProduct == null)
                {
                    throw new Exception("Master Product is not existed!!");
                }
            }
            if (product.AtomicPrice == null || product.AtomicPrice < 0)
            {
                throw new Exception("Product Atomic Price must be a positive decimal number!!");
            }
        }

        public async Task DeleteProductAsync(string id)
        {
            Product product = await work.Products.GetAsync(id);
            if (product == null)
            {
                throw new Exception("Product is not existed!!");
            }
            //work.Products.Delete(product);
            product.IsDeleted = true;
            work.Products.Update(product);
            work.Save();
        }
    }
}
