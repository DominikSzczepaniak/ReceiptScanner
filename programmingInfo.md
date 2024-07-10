Main thing we use is going to be Rest API. User at the beggining has to login/register. Then he has option to either browse his current receipts or add a new one. 

1. Adding a new one: user is asked to upload a photo or receipt, which is later send to ReceiptReader, and then it's added to database as default.

So we need to have tables:
Product: 
ID,
Name, 
Quantity/Weight,
Price,
OwnerID
Category (optional, need some AI to classify)

Receipt:
ID,
Date,
Shopname
OwnerID

ReceiptToProducts:
Multi-multi table for connecting Receipts ID with Product ID's to have all Receipt Products.

We also need a table for user
ID,
Username,
Password

2. Browsing current receipts/products is just looking up database for current user ID. 

