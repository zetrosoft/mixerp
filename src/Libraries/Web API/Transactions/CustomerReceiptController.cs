using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MixERP.Net.ApplicationState.Cache;
using MixERP.Net.Common.Extensions;
using MixERP.Net.EntityParser;
using Newtonsoft.Json;
using PetaPoco;

namespace MixERP.Net.Api.Transactions
{
    /// <summary>
    ///     Provides a direct HTTP access to perform various tasks such as adding, editing, and removing Customer Receipts.
    /// </summary>
    [RoutePrefix("api/v1.5/transactions/customer-receipt")]
    public class CustomerReceiptController : ApiController
    {
        /// <summary>
        ///     The CustomerReceipt data context.
        /// </summary>
        private readonly MixERP.Net.Schemas.Transactions.Data.CustomerReceipt CustomerReceiptContext;

        public CustomerReceiptController()
        {
            this.LoginId = AppUsers.GetCurrent().View.LoginId.ToLong();
            this.UserId = AppUsers.GetCurrent().View.UserId.ToInt();
            this.OfficeId = AppUsers.GetCurrent().View.OfficeId.ToInt();
            this.Catalog = AppUsers.GetCurrentUserDB();

            this.CustomerReceiptContext = new MixERP.Net.Schemas.Transactions.Data.CustomerReceipt
            {
                Catalog = this.Catalog,
                LoginId = this.LoginId
            };
        }

        public long LoginId { get; }
        public int UserId { get; private set; }
        public int OfficeId { get; private set; }
        public string Catalog { get; }

        /// <summary>
        ///     Counts the number of customer receipts.
        /// </summary>
        /// <returns>Returns the count of the customer receipts.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("count")]
        [Route("~/api/transactions/customer-receipt/count")]
        public long Count()
        {
            try
            {
                return this.CustomerReceiptContext.Count();
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        ///     Returns an instance of customer receipt.
        /// </summary>
        /// <param name="receiptId">Enter ReceiptId to search for.</param>
        /// <returns></returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("{receiptId}")]
        [Route("~/api/transactions/customer-receipt/{receiptId}")]
        public MixERP.Net.Entities.Transactions.CustomerReceipt Get(long receiptId)
        {
            try
            {
                return this.CustomerReceiptContext.Get(receiptId);
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        ///     Creates a paginated collection containing 25 customer receipts on each page, sorted by the property ReceiptId.
        /// </summary>
        /// <returns>Returns the first page from the collection.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("")]
        [Route("~/api/transactions/customer-receipt")]
        public IEnumerable<MixERP.Net.Entities.Transactions.CustomerReceipt> GetPagedResult()
        {
            try
            {
                return this.CustomerReceiptContext.GetPagedResult();
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        ///     Creates a paginated collection containing 25 customer receipts on each page, sorted by the property ReceiptId.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the resultset.</param>
        /// <returns>Returns the requested page from the collection.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("page/{pageNumber}")]
        [Route("~/api/transactions/customer-receipt/page/{pageNumber}")]
        public IEnumerable<MixERP.Net.Entities.Transactions.CustomerReceipt> GetPagedResult(long pageNumber)
        {
            try
            {
                return this.CustomerReceiptContext.GetPagedResult(pageNumber);
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        ///     Creates a filtered and paginated collection containing 25 customer receipts on each page, sorted by the property ReceiptId.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the resultset.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns the requested page from the collection using the supplied filters.</returns>
        [AcceptVerbs("POST")]
        [Route("get-where/{pageNumber}")]
        [Route("~/api/transactions/customer-receipt/get-where/{pageNumber}")]
        public IEnumerable<MixERP.Net.Entities.Transactions.CustomerReceipt> GetWhere(long pageNumber, [FromBody]dynamic filters)
        {
            try
            {
                List<EntityParser.Filter> f = JsonConvert.DeserializeObject<List<EntityParser.Filter>>(filters);
                return this.CustomerReceiptContext.GetWhere(pageNumber, f);
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        ///     Displayfield is a lightweight key/value collection of customer receipts.
        /// </summary>
        /// <returns>Returns an enumerable key/value collection of customer receipts.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("display-fields")]
        [Route("~/api/transactions/customer-receipt/display-fields")]
        public IEnumerable<DisplayField> GetDisplayFields()
        {
            try
            {
                return this.CustomerReceiptContext.GetDisplayFields();
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        ///     A custom field is a user defined field for customer receipts.
        /// </summary>
        /// <returns>Returns an enumerable custom field collection of customer receipts.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("custom-fields")]
        [Route("~/api/transactions/customer-receipt/custom-fields")]
        public IEnumerable<PetaPoco.CustomField> GetCustomFields()
        {
            try
            {
                return this.CustomerReceiptContext.GetCustomFields();
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        ///     Adds your instance of Account class.
        /// </summary>
        /// <param name="customerReceipt">Your instance of customer receipts class to add.</param>
        [AcceptVerbs("POST")]
        [Route("add/{customerReceipt}")]
        [Route("~/api/transactions/customer-receipt/add/{customerReceipt}")]
        public void Add(MixERP.Net.Entities.Transactions.CustomerReceipt customerReceipt)
        {
            if (customerReceipt == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.MethodNotAllowed));
            }

            try
            {
                this.CustomerReceiptContext.Add(customerReceipt);
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        ///     Edits existing record with your instance of Account class.
        /// </summary>
        /// <param name="customerReceipt">Your instance of Account class to edit.</param>
        /// <param name="receiptId">Enter the value for ReceiptId in order to find and edit the existing record.</param>
        [AcceptVerbs("PUT")]
        [Route("edit/{receiptId}/{customerReceipt}")]
        [Route("~/api/transactions/customer-receipt/edit/{receiptId}/{customerReceipt}")]
        public void Edit(long receiptId, MixERP.Net.Entities.Transactions.CustomerReceipt customerReceipt)
        {
            if (customerReceipt == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.MethodNotAllowed));
            }

            try
            {
                this.CustomerReceiptContext.Update(customerReceipt, receiptId);
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        ///     Deletes an existing instance of Account class via ReceiptId.
        /// </summary>
        /// <param name="receiptId">Enter the value for ReceiptId in order to find and delete the existing record.</param>
        [AcceptVerbs("DELETE")]
        [Route("delete/{receiptId}")]
        [Route("~/api/transactions/customer-receipt/delete/{receiptId}")]
        public void Delete(long receiptId)
        {
            try
            {
                this.CustomerReceiptContext.Delete(receiptId);
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
        }


    }
}