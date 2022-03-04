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
    public class ProductDetailBusinessEntity
    {
        private IUnitOfWork work; 
        public ProductDetailBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }

        public async Task<IEnumerable<ProductDetail>> GetProductDetailsAsync(
            SortType? sortByPrice,
            SortType? sortByStoredQuantity,
            SortType? sortByManufacturingDate,
            SortType? sortByExpiringDate,
            int page,
            int pageSize)
        {
            IEnumerable<ProductDetail> productDetails = await work.ProductDetails.GetAllAsync();
            
            if (sortByPrice.HasValue)
            {
                productDetails = GsmsUtils.Sort(productDetails, d => d.Price, sortByPrice.Value);
            } else if (sortByStoredQuantity.HasValue)
            {
                productDetails = GsmsUtils.Sort(productDetails, d => d.StoredQuantity, sortByStoredQuantity.Value);
            } else if (sortByManufacturingDate.HasValue)
            {
                productDetails = GsmsUtils.Sort(productDetails, d => d.ManufacturingDate, sortByManufacturingDate.Value);
            } else if (sortByExpiringDate.HasValue)
            {
                productDetails = GsmsUtils.Sort(productDetails, d => d.ExpiringDate, sortByExpiringDate.Value);
            }

            productDetails = GsmsUtils.Paging(productDetails, page, pageSize);

            return productDetails;
        }

        public async Task<ProductDetail> GetProductDetailAsync(string id)
        {
            ProductDetail productDetail = await work.ProductDetails.GetAsync(id);
            if (productDetail != null)
            {
                Product product = await work.Products.GetAsync(productDetail.ProductId);
                if (product != null && product.IsDeleted == true)
                {
                    return null;
                }
            }
            return productDetail;
        }

        public async Task<IEnumerable<ProductDetail>> GetProductDetailsAsync(string productId)
        {
            IEnumerable<ProductDetail> productDetails = await work.ProductDetails.GetAllAsync();
            productDetails = from productDetail in productDetails
                             where productDetail.ProductId.Equals(productId)
                             select productDetail;
            return productDetails;
        }

        public async Task<IEnumerable<ProductDetail>> GetProductDetailsAsync(Status status)
        {
            IEnumerable<ProductDetail> productDetails = await work.ProductDetails.GetAllAsync();
            productDetails = from productDetail in productDetails
                             where productDetail.Status == status
                             select productDetail;
            return productDetails;
        }

        public async Task<ProductDetail> AddProductDetailAsync(ProductDetail newProductDetail)
        {
            await CheckProductDetail (newProductDetail);
            newProductDetail.Id = GsmsUtils.CreateGuiId();
            newProductDetail.Product = null;
            await work.ProductDetails.AddAsync(newProductDetail);
            await work.Save();
            return newProductDetail;
        }

        public async Task<ProductDetail> UpdateProductDetailAsync(ProductDetail updatedProductDetail)
        {
            ProductDetail productDetail = await work.ProductDetails.GetAsync(updatedProductDetail.Id);
            if (productDetail == null)
            {
                throw new Exception("Product Detail is not existed!!");
            }
            await CheckProductDetail (updatedProductDetail);
            productDetail.ProductId = updatedProductDetail.ProductId;
            productDetail.Price = updatedProductDetail.Price;
            productDetail.Status = updatedProductDetail.Status;
            productDetail.StoredQuantity = updatedProductDetail.StoredQuantity;
            productDetail.Product = null;
            work.ProductDetails.Update(productDetail);
            await work.Save();
            return productDetail;
        }
        private async Task CheckProductDetail(ProductDetail productDetail)
        {
            Product product = await work.Products.GetAsync(productDetail.ProductId);
            if (product == null || product.IsDeleted == true)
            {
                throw new Exception("Product is not existed!!");
            }
            if (productDetail.Price == null || productDetail.Price < 0)
            {
                throw new Exception("Product Price must be a positive decimal number!!");
            }
            if (productDetail.StoredQuantity == null || productDetail.StoredQuantity < 0)
            {
                throw new Exception("Stored Quantity must be a positive integer!!");
            }
        }

        public async Task DeleteProductDetailAsync(string id)
        {
            ProductDetail productDetail = await work.ProductDetails.GetAsync(id);
            if (productDetail == null)
            {
                throw new Exception("Product Detail is not existed!!");
            }
            work.ProductDetails.Delete(productDetail);
            await work.Save();
        }
    }
}
