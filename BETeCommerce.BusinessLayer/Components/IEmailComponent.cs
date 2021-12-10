using BETeCommerce.BunessEntities.Requests;
using BETeCommerce.BunessEntities.Responses;

namespace BETeCommerce.BusinessLayer.Components
{
    public interface IEmailComponent
    {
        CheckoutResponse SendEmail(CheckoutItemsRequest request);
    }
}
