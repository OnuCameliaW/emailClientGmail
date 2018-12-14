namespace SendEmailClient
{
    using System;
    using System.Collections.Generic;

    public class EmailBodyDto : DotLiquid.Drop
    {
        public IList<ItemDto> Items { get; set; }

        public long OrderNumber { get; set; }

        public string PaymentNumber { get; set; }

        public int ReceiptNumber { get; set; }

        public int ReceiptSubtotal { get; set; }

        public int ReceiptTotal { get; set; }

        public int SalesTax { get; set; }

        public DateTime TransactionDate { get; set; }

        public int TransactionNumber { get; set; }
    }
}