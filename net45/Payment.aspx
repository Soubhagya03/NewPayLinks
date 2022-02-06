<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="RazorpaySampleApp.Payment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Razorpay .Net Sample App</title>
</head>
<body>    
    <p id="pLink"></p>
<script src="https://checkout.razorpay.com/v1/checkout.js"></script>
<script>
    document.getElementById("pLink").innerHTML = "<a href ="+"<%=paymentLink%>"+">"+"<%=paymentLink%>"+"</a>";
    
    </script>
</body>
</html>
