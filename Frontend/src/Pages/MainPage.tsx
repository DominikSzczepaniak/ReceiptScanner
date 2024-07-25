import ReceiptCard from "../components/ReceiptCard";
import Sidebar from "../components/Sidebar"

function MainPage() {

  return (
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
  )
}

export default MainPage;
