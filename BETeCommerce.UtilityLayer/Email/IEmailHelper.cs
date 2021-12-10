using BETeCommerce.BunessEntities.DataEntities;
using BETeCommerce.BunessEntities.Requests;

namespace BETeCommerce.UtilityLayer.Email
{
    public interface IEmailHelper
    {
        EmailDataDto SendEmail(CheckoutItemsRequest request);
    }
}
