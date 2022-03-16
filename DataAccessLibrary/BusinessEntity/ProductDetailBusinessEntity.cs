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
            }
            else if (sortByStoredQuantity.HasValue)
            {
                productDetails = GsmsUtils.Sort(productDetails, d => d.StoredQuantity, sortByStoredQuantity.Value);
            }
            else if (sortByManufacturingDate.HasValue)
            {
                productDetails = GsmsUtils.Sort(productDetails, d => d.ManufacturingDate, sortByManufacturingDate.Value);
            }
            else if (sortByExpiringDate.HasValue)
            {
                productDetails = GsmsUtils.Sort(productDetails, d => d.ExpiringDate, sortByExpiringDate.Value);
            }

            productDetails = GsmsUtils.Paging(productDetails, page, pageSize);
            foreach (ProductDetail productDetail in productDetails)
            {
                if (productDetail.Product != null)
                {
                    productDetail.Product.ImportOrderDetails = null;
                    productDetail.Product.ReceiptDetails = null;
                    if (productDetail.Product.Category != null)
                    {
                        productDetail.Product.Category.Products = null;

                    }
                }
            }

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
            await CheckProductDetail(newProductDetail);
            newProductDetail.Id = GsmsUtils.CreateGuiId();
            newProductDetail.Product = null;
            await UpdateProductDetailStatus(newProductDetail);
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
            await CheckProductDetail(updatedProductDetail);
            productDetail.ProductId = updatedProductDetail.ProductId;
            productDetail.Price = updatedProductDetail.Price;
            productDetail.Status = updatedProductDetail.Status;
            productDetail.StoredQuantity = updatedProductDetail.StoredQuantity;
            await UpdateProductDetailStatus(productDetail);
            //productDetail.Product = null;
            work.ProductDetails.Update(productDetail);
            await work.Save();
            return productDetail;
        }

        private async Task UpdateProductDetailStatus(ProductDetail productDetail)
        {
            if (productDetail.StoredQuantity == 0)
            {
                productDetail.Status = Status.OUT_OF_STOCK;
            }
            else if (productDetail.StoredQuantity < 10)
            {
                productDetail.Status = Status.ALMOST_OUT_OF_STOCK;
            }
            else
            {
                IEnumerable<ReceiptDetail> receiptDetails = await work.ReceiptDetails.GetAllAsync();
                receiptDetails = from detail in receiptDetails
                                 where detail.ProductId.Equals(productDetail.ProductId)
                                      && detail.CreatedDate.Value.Month == DateTime.Now.Month
                                 select detail;
                if (receiptDetails.Count() > 50)
                {
                    productDetail.Status = Status.BEST_SELLER;
                }
                else
                {
                    productDetail.Status = Status.AVAILABLE;
                }
            }
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
