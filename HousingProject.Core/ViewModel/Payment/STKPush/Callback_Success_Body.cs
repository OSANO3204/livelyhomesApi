using System.Collections.Generic;

public class STKCallback
{
    public BodyModel Body { get; set; }
}

public class BodyModel
{
    public StkCallbackModel stkCallback { get; set; }
}

public class StkCallbackModel
{
    public string MerchantRequestID { get; set; }
    public string CheckoutRequestID { get; set; }
    public int ResultCode { get; set; }
    public string ResultDesc { get; set; }
    public CallbackMetadataModel CallbackMetadata { get; set; }
}

public class CallbackMetadataModel
{
    public List<CallbackItemModel> Item { get; set; }
}

public class CallbackItemModel
{
    public string Name { get; set; }
    public object Value { get; set; }
}
