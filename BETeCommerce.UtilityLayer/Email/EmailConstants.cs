
namespace BETeCommerce.UtilityLayer.Email
{
    public class EmailConstants
    {
        public static string SmtpHost { get; } = "smtp.gmail.com";
        public static int SmtpPort { get; } = 587;

        public static string EmailSubject { get; } = "BET Shop Checkout Details";

        public static string OrderNumberKey { get; } = "@@orderNumber@@";
        public static string TotalKey { get; } = "@@total@@";
        public static string TableRowsContentKey { get; } = "@@tableRowsContent@@";

        public static string FullEmailTemplate { get; } = @"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset ='UTF-8'>
                <meta name ='x-apple-disable-message-reformatting'>
                <title ></title>
                <style>
                    table, td, div, h1, p {font-family: Arial, sans-serif;}
                table, td {border:2px solid #000000 !important;}
                </style>
            </head>
            <div>
                <h3>OrderNumber: <strong>@@orderNumber@@</strong>
                <h2>Total: @@total@@</h2>
                <table style='border-collapse: collapse;'>
                  <thead>
                    <tr>
                      <th>Product</th>
                      <th>Quantity</th>
                      <th>Price</th>
                    </tr>
                  </thead>
                  <tbody>
                       @@tableRowsContent@@
                  </tbody>
                </table>
            </div>
        </html>
";

        public static string TableRow { get; } = @"
             <tr>
              <td style'padding-top: 12px;padding-bottom: 12px;text-align: left;background-color: #04AA6D;'>{0}</td>
              <td style'padding-top: 12px;padding-bottom: 12px;text-align: left;background-color: #04AA6D;'>{1}</td>
              <td style'padding-top: 12px;padding-bottom: 12px;text-align: left;background-color: #04AA6D;'>{2}</td>
            </ tr >
        ";
    }
}
