import ReceiptCard from "../components/ReceiptCard";
import { serverLink } from "../settings";

function MainPage() {
  if(sessionStorage.getItem("userid") == null){
    window.location.href = "/login";
  }

  async function getLastFourReceipts(){
    let userID = sessionStorage.getItem("userid");
    //since you cant get there if your userid is null we can assume its correct

    try {
        const response = await fetch(`${serverLink}/Receipt/receiptList/${userID}`, {
            method: 'GET', 
            headers: {
                'Content-Type': 'application/json',
            },
        });

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        const data = await response.json();

        if (response.ok) {
            // return first 4 elements of data
            return data.slice(0, 4);
        } else {
            // show error message
            alert('Invalid username or password');
        }
        return null;
    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
        alert('Failed to login. Please try again later.');
        return null;
    }
  }

  async function generateReceiptCards(){
    let cards = [];
    let receipts = await getLastFourReceipts();
    for(let i = 0; i < receipts.length; i++){
      let receipt = receipts[i];
      cards.push(<ReceiptCard ShopName={receipt.shopName} Date={receipt.date} Total={receipt.total} Items={receipt.items} />);
    }
    return cards;
  }

  let cards;
  generateReceiptCards().then((c) => {
    cards = c;
  }) //generate from this

  return (
    <>
      <div className="flex wrap overflow-hidden flex-wrap">
        <ReceiptCard ShopName="ShopName" Date="Date" Total={0} Items={[{Name: "Name", Price: 0, Amount: "Amount"}]} />
        <ReceiptCard ShopName="ShopName" Date="Date" Total={0} Items={[{Name: "Name", Price: 0, Amount: "Amount"}]} />
        <ReceiptCard ShopName="ShopName" Date="Date" Total={0} Items={[{Name: "Name", Price: 0, Amount: "Amount"}]} />
        <ReceiptCard ShopName="ShopName" Date="Date" Total={0} Items={[{Name: "Name", Price: 0, Amount: "Amount"}]} />
        <ReceiptCard ShopName="ShopName" Date="Date" Total={0} Items={[{Name: "Name", Price: 0, Amount: "Amount"}]} />
        <ReceiptCard ShopName="ShopName" Date="Date" Total={0} Items={[{Name: "Name", Price: 0, Amount: "Amount"}]} />
        <ReceiptCard ShopName="ShopName" Date="Date" Total={0} Items={[{Name: "Name", Price: 0, Amount: "Amount"}]} />
        <ReceiptCard ShopName="ShopName" Date="Date" Total={0} Items={[{Name: "Name", Price: 0, Amount: "Amount"}]} />
        <ReceiptCard ShopName="ShopName" Date="Date" Total={0} Items={[{Name: "Name", Price: 0, Amount: "Amount"}]} />
        <ReceiptCard ShopName="ShopName" Date="Date" Total={0} Items={[{Name: "Name", Price: 0, Amount: "Amount"}]} />
        <ReceiptCard ShopName="ShopName" Date="Date" Total={0} Items={[{Name: "Name", Price: 0, Amount: "Amount"}]} />
        <ReceiptCard ShopName="ShopName" Date="Date" Total={0} Items={[{Name: "Name", Price: 0, Amount: "Amount"}]} />
      </div>
    </>
  )
}

export default MainPage;
