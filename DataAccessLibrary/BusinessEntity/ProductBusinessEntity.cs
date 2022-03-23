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

        public async Task<IEnumerable<Product>> GetProductsAsync(
            string categoryId,
            string masterProductId,
            string searchByName,
            SortType? sortByName,
            int page,
            int pageSize
            )
        {
            IEnumerable<Product> products = await work.Products.GetAllAsync();
            products = from product in products
                       where product.IsDeleted == false
                       select product;

            if (!string.IsNullOrEmpty(categoryId))
            {
                products = from product in products
                           where product.CategoryId.Equals(categoryId)
                           select product;
            }

            if (!string.IsNullOrEmpty(masterProductId))
            {
                products = from product in products
                           where !string.IsNullOrEmpty(product.MasterProductId) && product.MasterProductId.Equals(masterProductId)
                           select product;
            }

            if (!string.IsNullOrEmpty(searchByName))
            {
                products = from product in products
                           where product.Name.ToLower().Contains(searchByName.ToLower())
                           select product;
            }

            if (sortByName.HasValue)
            {
                products = GsmsUtils.Sort(products, p => p.Name, sortByName.Value);
            }

            products = GsmsUtils.Paging(products, page, pageSize);

            foreach (Product product in products)
            {
                if (product.Category != null)
                {
                    product.Category.Products = null;
                }
                product.ImportOrderDetails = null;
                product.ReceiptDetails = null;

                product.ProductDetails = (from productDetail in await work.ProductDetails.GetAllAsync()
                                          where productDetail.ProductId.Equals(product.Id)
                                          select productDetail).ToList();
            }

            return products;
        }

        public async Task<Product> GetProductAsync(string id)
        {
            Product product = await work.Products.GetAsync(id);
            if (product == null || (product != null && product.IsDeleted == true))
            {
                return null;
            }
            IEnumerable<ProductDetail> productDetails = await work.ProductDetails.GetAllAsync();
            productDetails = from productDetail in productDetails
                             where productDetail.ProductId.Equals(id)
                             select productDetail;
            product.ProductDetails = productDetails.ToList();
            return product;
        }

        public async Task<Product> AddProductAsync(Product newProduct)
        {
            await CheckProduct(newProduct);
            newProduct.Id = GsmsUtils.CreateGuiId();
            newProduct.IsDeleted = false;
            if (newProduct.ProductDetails != null && newProduct.ProductDetails.Any())
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
                    if (productDetail.ExpiringDate != null && productDetail.ExpiringDate.Value.CompareTo(DateTime.Now) <= 0)
                    {
                        throw new Exception("Expiring Date must be further than today!!");
                    }
                    if (productDetail.ManufacturingDate != null && productDetail.ManufacturingDate.Value.CompareTo(DateTime.Now) >= 0)
                    {
                        throw new Exception("Manufaturing Date must be ealier than today!!");
                    }
                    await UpdateProductDetailStatus(productDetail);
                    productDetail.Id = GsmsUtils.CreateGuiId();
                    productDetail.Product = null;
                }
            }
            IEnumerable<ProductDetail> productDetails = newProduct.ProductDetails;

            await work.Products.AddAsync(newProduct);
            await work.Save();

            newProduct.ProductDetails = productDetails.ToList();
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
            product.ImageUrl = updatedProduct.ImageUrl;

            if (updatedProduct.ProductDetails != null && updatedProduct.ProductDetails.Any())
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
                    if (productDetail.ExpiringDate != null && productDetail.ExpiringDate.Value.CompareTo(DateTime.Now) <= 0)
                    {
                        throw new Exception("Expiring Date must be further than today!!");
                    }
                    if (productDetail.ManufacturingDate != null && productDetail.ManufacturingDate.Value.CompareTo(DateTime.Now) >= 0)
                    {
                        throw new Exception("Manufaturing Date must be ealier than today!!");
                    }
                    await UpdateProductDetailStatus(productDetail);
                    //productDetail.Id = GsmsUtils.CreateGuiId();
                    work.ProductDetails.Update(productDetail);
                    //productDetail.Product = null;
                }
                //product.ProductDetails = null;
            }
            work.Products.Update(product);
            await work.Save();
            product.ProductDetails = (from productDetail in await work.ProductDetails.GetAllAsync()
                                      where productDetail.ProductId.Equals(product.Id)
                                      select productDetail).ToList();
            product.ReceiptDetails = null;
            return product;
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
                                 select detail;

                IEnumerable<ReceiptDetail> receiptCount = new List<ReceiptDetail>();
                foreach (ReceiptDetail receiptDetail in receiptDetails)
                {
                    Receipt receipt = await work.Receipts.GetAsync(receiptDetail.ReceiptId);
                    if (receipt != null && receipt.CreatedDate.HasValue && receipt.CreatedDate.Value.Month == DateTime.Now.Month)
                    {
                        receiptCount.Append(receiptDetail);
                    }
                }

                if (receiptCount.Count() > 50)
                {
                    productDetail.Status = Status.BEST_SELLER;
                }
                else
                {
                    productDetail.Status = Status.AVAILABLE;
                }
            }
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
            await work.Save();
        }
    }
}
